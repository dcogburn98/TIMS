﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Sockets;
using System.Security.Cryptography;
using System.ServiceModel;
using System.Threading.Tasks;

using Microsoft.Data.Sqlite;
using Microsoft.Extensions.Logging;

using TIMSServerModel;
using TIMSServer.WebServer.WooCommerce;

using WooCommerceNET;
using WooCommerceNET.WooCommerce.v3;
using WooCommerceNET.WooCommerce.v3.Extension;

using Quartz;
using Quartz.Impl;

namespace TIMSServer
{
    class Program
    {
        public static int alertLevel = 3;
        public static string regName = string.Empty;
        public int skippedDays = 0;

        public static SqliteConnection sqlite_conn;

        static async Task Main()
        {
            sqlite_conn = new SqliteConnection(
              @"Data Source=database.db; 
                Pooling = true;");
            //Password = 3nCryqtEdT!MSPa$$w0rdFoRrev!tAc0m;

            OpenConnection();
            CloseConnection();
            CreateDatabase();
            TIMSServiceModel.Init();

            string externalIpString = new WebClient().DownloadString("http://icanhazip.com").Replace("\\r\\n", "").Replace("\\n", "").Trim();
            IPAddress externalIp = IPAddress.Parse(externalIpString);

            #region Client Server Initialization
            Console.WriteLine("Starting TIMS client service model (Step 1)...");
            ServiceHost host = new ServiceHost(typeof(TIMSServiceModel));
            host.Open();
            Console.WriteLine("Server is open for connections from TIMS clients.");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine("Internal IP: " + GetLocalIPAddress());
            Console.WriteLine("External IP: " + externalIp.ToString());
            Console.WriteLine();
            Console.ForegroundColor = ConsoleColor.White;
            #endregion

            #region Web Server Initialization
            
            Console.WriteLine("Starting TIMS web server (Step 2)...");
            ServiceHost webhost = new ServiceHost(typeof(TIMSWebServerModel));
                webhost.Open();
            TIMSWebServerModel.Init();
            Console.Write("Web server now serving web pages on ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(externalIp.ToString());
            Console.ForegroundColor = ConsoleColor.White;
            #endregion

            #region EOD Scheduling
            "Scheduling EOD".LogServer(LogLevel.Info);
            string EODTime = "12am";
            DateTime EODStartTime = DateTime.Parse(EODTime).AddDays(1);
            if (EODStartTime - DateTime.Now > TimeSpan.FromDays(1))
                EODStartTime = EODStartTime.AddDays(-1);

            Console.Write("\nScheduling End of Day task.\nEOD currently set to run at ");
            Console.ForegroundColor = ConsoleColor.Green;
            Console.WriteLine(EODStartTime.ToString());
            Console.ForegroundColor = ConsoleColor.White;
            Console.WriteLine("You can change this in the store settings in the TIMS interface.");
            "EOD currently set to run at".LogServer(LogLevel.Info);

            EOD.instance = new TIMSServiceModel();
            IJobDetail job = JobBuilder.Create<EOD>()
                .WithIdentity("EOD", "Daily")
                .Build();

            StdSchedulerFactory factory = new StdSchedulerFactory();
            IScheduler TaskScheduler = await factory.GetScheduler();

            ITrigger EODtrigger = TriggerBuilder.Create()
                .WithIdentity("EODTimeTrigger", "DailyTimeTriggers")
                .StartAt(EODStartTime)
                .WithSimpleSchedule(x => x
                    .WithIntervalInHours(24)
                    .RepeatForever())
                .Build();

            // Tell Quartz to schedule the job using our trigger
            await TaskScheduler.ScheduleJob(job, EODtrigger);
            await TaskScheduler.Start();
            #endregion

            #region WooCommerce
            List<Webhook> webhooks = await WooCommerceHandler.WC.Webhook.GetAll();
            foreach (Webhook wh in webhooks)
            {
                if (wh.name.ToUpper().Contains("TIMS"))
                {
                    wh.delivery_url = "http://" + externalIp.ToString();
                    wh.status = "active";
                    await WooCommerceHandler.WC.Webhook.Update((ulong)wh.id, wh);
                }
                
            }
            //List<WooCommerce.NET.WordPress.v2.Posts> posts = WooCommerceHandler.WP.Post.GetAll().Result;
            //foreach (WooCommerce.NET.WordPress.v2.Posts post in posts)
            //{
            //    Console.WriteLine(post.title);
            //}
            //WooCommerceHandler.MatchProducts();
            
            #endregion

            while (true)
            {
                Console.Write("TIMS@" + GetLocalIPAddress() + ">");
                string input = Console.ReadLine();
                string[] split = input.Split(' ');
                switch (split[0].ToLower())
                {
                    case "exit":
                    {
                        Console.WriteLine("Please type 'exit' again to confirm termination. Type anything else to cancel exit.");
                        if (Console.ReadLine().ToLower() == "exit")
                        {
                            Console.WriteLine("Terminating TIMS client server model...");
                            host.Close();
                            Console.WriteLine("Shutting down micro web server model...");
                            webhost.Close();
                            Console.WriteLine("Shutting down TIMS server application...");
                            Environment.Exit(0);
                        }
                        Console.WriteLine("Exit aborted.");
                        break;
                    }
                    case "reg":
                    {
                        if (split.Length == 3)
                        {
                            if (split[1].ToLower() == "-t")
                            {
                                string terminalName = split[2].ToUpper();
                                if (IsAlphanumeric(terminalName))
                                {
                                    Console.WriteLine("The next unknown terminal to connect to the server will be registered with nickname \"" + terminalName + "\".");
                                    regName = terminalName;
                                }
                                else
                                    Console.WriteLine("Terminal nickname must be alphanumeric (no symbols or special characters).");
                            }
                            else
                                Console.WriteLine("Invalid useage of register command.");
                        } //reg -t Term01
                        else if (split.Length == 5)
                        {
                            if (split[1].ToLower() == "-t")
                            {
                                if (split[2].ToLower() == "-ip")
                                {
                                    if (IPAddress.TryParse(split[3].ToLower(), out IPAddress ip))
                                    {
                                        string terminalName = split[4].ToUpper();
                                        if (IsAlphanumeric(terminalName))
                                        {
                                            OpenConnection();

                                            SqliteCommand command = sqlite_conn.CreateCommand();
                                            command.CommandText =
                                                "SELECT NICKNAME FROM DEVICES WHERE IPADDRESS = $ADDR OR NICKNAME = $NAME";
                                            command.Parameters.Add(new SqliteParameter("$ADDR", ip.ToString()));
                                            command.Parameters.Add(new SqliteParameter("$NAME", terminalName));
                                            SqliteDataReader reader = command.ExecuteReader();
                                            if (reader.HasRows)
                                            {
                                                Console.WriteLine("Terminal nickname or IP address is already registered in your server.");
                                                CloseConnection();
                                                break;
                                            }
                                            else
                                            {
                                                reader.Close();
                                                command.CommandText =
                                                "INSERT INTO DEVICES (DEVICETYPE, IPADDRESS, NICKNAME) VALUES ('TERMINAL', $ADDR, $NAME)";
                                                if (command.ExecuteNonQuery() > 0)
                                                    Console.WriteLine("Registered " + ip.ToString() + " with nickname " + terminalName);
                                                else
                                                    Console.WriteLine("Error registering device.");

                                                CloseConnection();
                                            }
                                        }
                                        else
                                            Console.WriteLine("Terminal nickname must be alphanumeric (no symbols or special characters).");
                                    }
                                    else
                                        Console.WriteLine("IP Address was malformed.");
                                }
                                else
                                    Console.WriteLine("Invalid useage of register command.");
                            }
                            else
                                Console.WriteLine("Invalid useage of register command.");
                        } //reg -t -ip 192.168.254.75 Term01
                        else
                            Console.WriteLine("Invalid useage of register command.");
                        break;
                    }
                    case "dereg":
                    {
                        if (split.Length == 3)
                        {
                            if (split[1] == "-ip")
                            {
                                if (IPAddress.TryParse(split[2], out IPAddress ip))
                                {
                                    OpenConnection();

                                    SqliteCommand command = sqlite_conn.CreateCommand();
                                    command.CommandText =
                                        "SELECT NICKNAME FROM DEVICES WHERE IPADDRESS = $ADDR";
                                    command.Parameters.Add(new SqliteParameter("$ADDR", ip.ToString()));
                                    SqliteDataReader reader = command.ExecuteReader();
                                    if (!reader.HasRows)
                                    {
                                        Console.WriteLine("IP address is not registered in your server.");
                                        CloseConnection();
                                        break;
                                    }
                                    else
                                    {
                                        reader.Close();
                                        command.CommandText =
                                            "DELETE FROM DEVICES WHERE IPADDRESS = $ADDR";
                                        if (command.ExecuteNonQuery() > 0)
                                            Console.WriteLine("Successfully unregistered ip address from your server.");
                                        else
                                            Console.WriteLine("Error unregistering IP address.");
                                    }
                                }
                                else
                                {
                                    Console.WriteLine("Invalid IP address.");
                                }
                            }
                            else
                                Console.WriteLine("Invalid useage of the deregister command.");
                        }
                        else
                            Console.WriteLine("Invalid useage of the deregister command.");
                        break;
                    }
                    default:
                    {
                        Console.WriteLine("Unknown command");
                        break;
                    }
                }
            }

            
        }

        public static void OpenConnection()
        {
            sqlite_conn.Open();
        }

        public static void CloseConnection()
        {
            sqlite_conn.Close();
        }

        public static void CreateDatabase()
        {
            OpenConnection();

            SqliteCommand command = sqlite_conn.CreateCommand();
            if (!TableExists("AccountTransactions"))
            {
                command.CommandText =
                    @"CREATE TABLE ""AccountTransactions"" (
	                ""UID""	INTEGER NOT NULL UNIQUE,
	                ""TransactionID""	INTEGER NOT NULL,
	                ""Date""	TEXT NOT NULL,
	                ""Memo""	TEXT,
	                ""CreditAccount""	INTEGER NOT NULL,
	                ""DebitAccount""	INTEGER NOT NULL,
	                ""Amount""	REAL NOT NULL,
	                ""ReferenceNumber""	INTEGER,
                    ""Void"" INTEGER,
	                PRIMARY KEY(""UID"" AUTOINCREMENT)
                    )";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("Accounts"))
            {
                command.CommandText =
                @"CREATE TABLE ""Accounts"" (
                ""ID""    INTEGER NOT NULL UNIQUE,
                ""Type""  TEXT NOT NULL,
	            ""Name""  TEXT NOT NULL,
	            ""Description""   TEXT NOT NULL,
	            ""CurrentBalance""    REAL NOT NULL,
	            PRIMARY KEY(""ID"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();

                #region Default Account Entries
                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('1', 'Asset', 'Inventory', 'Physical Inventory', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('2', 'Asset', 'Checking Account', 'Checking Account', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('3', 'Equity', 'Owner Investment', 'Investments by company owners', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('4', 'Expense', 'Electricity', 'Electricity Bill', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('5', 'Income', 'Cash Sales', 'Sales of Inventory', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('6', 'Liability', 'Credit Cards', 'Credit Card Balance', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('7', 'Liability', 'Sales Tax Payable', 'Sales Tax Owed', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('8', 'Expense', 'COGS', 'Cost of Goods Sold', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('9', 'Income', 'Charge Sales', 'Sales of Inventory on Credit', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('10', 'Income', 'Card Sales', 'Sales of Inventory on Card Sales', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('11', 'Asset', 'Accounts Receivable', 'Accounts Receivable', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('12', 'Asset', 'Savings Account', 'Savings Account', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('13', 'Asset', 'Petty Cash', 'Petty Cash', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('14', 'Asset', 'Cash Drawers', 'Cash Drawers', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('15', 'Equity', 'Retained Earnings', 'Retained Earnings', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('16', 'Income', 'Interest Income', 'Interest Income', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('17', 'Income', 'Reimbursed Expenses', 'Reimbursed Expenses', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('18', 'Income', 'Discounts', 'Discounts', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('19', 'Liability', 'Accounts Payable', 'Accounts Payable', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('20', 'Asset', 'Imbalance', 'Imbalance', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('21', 'Income', 'Positive Adjustment', 'Positive Adjustment', '0.0');";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Accounts (ID, Type, Name, Description, CurrentBalance) 
                VALUES ('22', 'Expense', 'Negative Adjustment', 'Negative Adjustment', '0.0');";
                command.ExecuteNonQuery();
                #endregion
            }
            else
            {

            }

            if (!TableExists("Barcodes"))
            {
                command.CommandText =
                @"CREATE TABLE ""Barcodes"" (
                ""ID""    INTEGER NOT NULL,
	            ""BarcodeType""   TEXT NOT NULL,
	            ""BarcodeValue""  TEXT NOT NULL,
	            ""ScannedItemNumber"" TEXT NOT NULL,
	            ""ScannedProductLine""    TEXT NOT NULL,
	            ""ScannedQuantity""   REAL NOT NULL,
	            PRIMARY KEY(""ID"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("Brands"))
            {
                command.CommandText =
                @"CREATE TABLE ""Brands"" (
                ""ID"" INTEGER NOT NULL,
                ""BrandName""    TEXT NOT NULL,
	            PRIMARY KEY(""ID"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO BRANDS (BRANDNAME) VALUES ('Default')";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("Categories"))
            {
                command.CommandText =
                @"CREATE TABLE ""Categories"" (
                ""ID"" INTEGER NOT NULL,
                ""CategoryName""    TEXT NOT NULL,
	            PRIMARY KEY(""ID"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO CATEGORIES (CATEGORYNAME) VALUES ('Default')";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("CheckinItems"))
            {
                command.CommandText =
                @"CREATE TABLE ""CheckinItems"" (
                ""CheckinNumber"" INTEGER NOT NULL,
	            ""ProductLine""   TEXT NOT NULL,
	            ""ItemNumber""    TEXT NOT NULL,
	            ""OrderedQty""    REAL NOT NULL,
	            ""ShippedQty""    REAL NOT NULL,
	            ""ReceivedQty""   REAL NOT NULL,
	            ""DamagedQty""    REAL NOT NULL
                )";
                command.ExecuteNonQuery();

                
            }
            else
            {

            }

            if (!TableExists("Checkins"))
            {
                command.CommandText =
                @"CREATE TABLE ""Checkins"" (
                ""CheckinNumber"" INTEGER NOT NULL,
	            ""PONumbers"" TEXT NOT NULL,
	            ""OpenEditing""   INTEGER,
	            PRIMARY KEY(""CheckinNumber"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("CustomerBalances"))
            {
                command.CommandText =
                @"CREATE TABLE ""CustomerBalances"" (
                ""GUID""  INTEGER NOT NULL,
	            ""CustomerNumber""    INTEGER NOT NULL,
	            ""InvoiceNumber"" INTEGER NOT NULL,
	            ""DaysDue""   INTEGER NOT NULL,
	            ""DateOfOrigin""  TEXT NOT NULL,
	            ""Balance""   REAL NOT NULL,
	            PRIMARY KEY(""GUID"")
                ); ";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("Customers"))
            {
                command.CommandText =
                    @"CREATE TABLE ""Customers""(
                    ""CustomerName""  TEXT NOT NULL,
                    ""CustomerNumber""    TEXT NOT NULL,
                    ""InStorePricingProfile""    TEXT NOT NULL,
                    ""OnlinePricingProfile""    TEXT NOT NULL,
                    ""DefaultInStorePriceSheet""    TEXT NOT NULL,
                    ""DefaultOnlinePriceSheet""    TEXT NOT NULL,
                    ""CanCharge"" INTEGER NOT NULL,
                    ""CreditLimit""   REAL NOT NULL,
                    ""AccountBalance""    REAL NOT NULL,
                    ""PhoneNumber""   TEXT NOT NULL,
                    ""FaxNumber"" TEXT NOT NULL,
                    ""BillingAddress""    TEXT NOT NULL,
                    ""ShippingAddress""   TEXT NOT NULL,
                    ""InvoiceMessage""    TEXT NOT NULL,
                    ""Website""   TEXT NOT NULL,
                    ""Email"" TEXT NOT NULL,
                    ""AssignedRep""   TEXT NOT NULL,
                    ""BusinessCategory""  TEXT NOT NULL,
                    ""DateAdded"" TEXT NOT NULL,
                    ""DateOfLastSale""    TEXT NOT NULL,
                    ""DateOfLastROA"" TEXT NOT NULL,
                    ""PreferredLanguage"" TEXT NOT NULL,
                    ""AuthorizedBuyers""  TEXT NOT NULL,
                    ""DefaultTaxTable""   TEXT NOT NULL,
                    ""DeliveryTaxTable""  TEXT NOT NULL,
                    ""PrimaryTaxStatus""  TEXT NOT NULL,
                    ""SecondaryTaxStatus""    TEXT NOT NULL,
                    ""PrimaryTaxExemptionNumber"" TEXT NOT NULL,
                    ""SecondaryTaxExemptionNumber""   TEXT NOT NULL,
                    ""PrimaryTaxExemptionExpiration"" TEXT NOT NULL,
                    ""SecondaryTaxExemptionExpiration""   TEXT NOT NULL,
                    ""PrintCatalogNotesOnInvoice""    INTEGER NOT NULL,
                    ""PrintBalanceOnInvoice"" INTEGER NOT NULL,
                    ""EmailInvoices"" INTEGER NOT NULL,
                    ""AllowBackorders""   INTEGER NOT NULL,
                    ""AllowSpecialOrders""    INTEGER NOT NULL,
                    ""ExemptFromInvoiceSurcharges""   INTEGER NOT NULL,
                    ""ExtraInvoiceCopies""    INTEGER NOT NULL,
                    ""PORequiredThresholdAmount"" REAL NOT NULL,
                    ""BillingType""   TEXT NOT NULL,
                    ""DefaultToDeliver""  INTEGER NOT NULL,
                    ""DeliveryRoute"" TEXT NOT NULL,
                    ""TravelTime""    INTEGER NOT NULL,
                    ""TravelDistance""    INTEGER NOT NULL,
                    ""MinimumSaleFreeDelivery""   REAL NOT NULL,
                    ""DeliveryCharge""    REAL NOT NULL,
                    ""StatementType"" TEXT NOT NULL,
                    ""PercentDiscount""   REAL NOT NULL,
                    ""PaidByForDiscount"" INTEGER NOT NULL,
                    ""DueDate""   TEXT NOT NULL,
                    ""ExtraStatementCopies""  INTEGER NOT NULL,
                    ""SendInvoicesEvery_Days""    INTEGER NOT NULL,
                    ""SendAccountSummaryEvery_Days""  INTEGER NOT NULL,
                    ""EmailStatements""   INTEGER NOT NULL,
                    ""StatementMailingAddress""   TEXT NOT NULL,
                    ""LastPaymentAmount"" REAL NOT NULL,
                    ""LastPaymentDate""   TEXT NOT NULL,
                    ""HighestAmountOwed"" REAL NOT NULL,
                    ""HighestAmountOwedDate"" TEXT NOT NULL,
                    ""HighestAmountPaid"" REAL NOT NULL,
                    ""HighestAmountPaidDate"" TEXT NOT NULL,
                    ""LastStatementAmount""   REAL NOT NULL,
                    ""TotalDue""  REAL NOT NULL,
                    ""Due30Days"" REAL NOT NULL,
                    ""Due60Days"" REAL NOT NULL,
                    ""Due90Days"" REAL NOT NULL,
                    ""FurtherDue""    REAL NOT NULL,
                    ""ServiceCharge"" REAL NOT NULL,
                    ""EnableTIMSRelations""   INTEGER NOT NULL,
                    ""RelationshipKey""   TEXT NOT NULL,
                    ""AutomaticallySendPriceUpdates"" INTEGER NOT NULL,
                    ""AutomaticallySendMedia""    INTEGER NOT NULL,
                    PRIMARY KEY(""CustomerNumber"")
                    )";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Customers (
                    ""CustomerName"", ""CustomerNumber"", ""InStorePricingProfile"", ""OnlinePricingProfile"", ""DefaultInStorePriceSheet"", 
                    ""DefaultOnlinePriceSheet"", ""CanCharge"", ""CreditLimit"", ""AccountBalance"",
                    ""PhoneNumber"", ""FaxNumber"", ""BillingAddress"", ""ShippingAddress"", ""InvoiceMessage"", ""Website"",
                    ""Email"", ""AssignedRep"", ""BusinessCategory"", ""DateAdded"", ""DateOfLastSale"", ""DateOfLastROA"",
                    ""PreferredLanguage"", ""AuthorizedBuyers"", ""DefaultTaxTable"", ""DeliveryTaxTable"", ""PrimaryTaxStatus"",
                    ""SecondaryTaxStatus"", ""PrimaryTaxExemptionNumber"", ""SecondaryTaxExemptionNumber"", ""PrimaryTaxExemptionExpiration"",
                    ""SecondaryTaxExemptionExpiration"", ""PrintCatalogNotesOnInvoice"", ""PrintBalanceOnInvoice"", ""EmailInvoices"",
                    ""AllowBackorders"", ""AllowSpecialOrders"", ""ExemptFromInvoiceSurcharges"", ""ExtraInvoiceCopies"",
                    ""PORequiredThresholdAmount"", ""BillingType"", ""DefaultToDeliver"", ""DeliveryRoute"", ""TravelTime"",
                    ""TravelDistance"", ""MinimumSaleFreeDelivery"", ""DeliveryCharge"", ""StatementType"", ""PercentDiscount"",
                    ""PaidByForDiscount"", ""DueDate"", ""ExtraStatementCopies"", ""SendInvoicesEvery_Days"", ""SendAccountSummaryEvery_Days"",
                    ""EmailStatements"", ""StatementMailingAddress"", ""LastPaymentAmount"", ""LastPaymentDate"", ""HighestAmountOwed"",
                    ""HighestAmountOwedDate"", ""HighestAmountPaid"", ""HighestAmountPaidDate"", ""LastStatementAmount"", ""TotalDue"",
                    ""Due30Days"", ""Due60Days"", ""Due90Days"", ""FurtherDue"", ""ServiceCharge"", ""EnableTIMSRelations"",
                    ""RelationshipKey"", ""AutomaticallySendPriceUpdates"", ""AutomaticallySendMedia"") 
                VALUES (
                    'Cash Sale', --Customer Name
                    '0', --Customer Number
                    '0', --In Store Pricing Profile
                    '0', --Online Pricing Profile
                    'Green', --Default In Store Price Sheet
                    'Green', --Default Online Price Sheet
                    '0', --Can Charge
                    '0', --Credit Limit
                    '0', --Account Balance
                    '870-279-7192', --Phone Number
                    '870-286-2463', --Fax Number
                    '1002 N Walters Ave, Dierks, AR, 71833, USA', --Billing Address
                    '206 Main Ave, Dierks, AR, 71833, USA', --Shipping Address
                    'Thank you for choosing TIMS!', --Invoice Message
                    'http://www.fishnmunition.com', --Website
                    'blake.cogburn@fishnmunition.com', --Email
                    '', --Assigned Representative
                    '', --Business Category
                    '10/10/2022', --Date Added
                    '10/10/2022', --Date of Last Sale
                    '10/10/2022', --Date of Last ROA
                    'English', --Preferred Language
                    '', --Authorized Buyers
                    'Default', --Default Tax Table
                    'Default', --Delivery Tax Table
                    'Non-Exempt', --Primary Tax Status
                    'Non-Exempt', --Secondary Tax Status
                    '', --Primary Tax Exempt Number
                    '', --Secondary Tax Exempt Number
                    '', --Primary Tax Exemption Expiration
                    '', --Secondary Tax Exemption Expiration
                    '1', --Print Catalog Notes on Invoices
                    '0', --Print Balance on Invoices
                    '0', --Email Invoices
                    '1', --Allow Backorders
                    '1', --Allow Special Orders
                    '0', --Exempt from Invoice Surcharges
                    '1', --Extra Copies of Invoices
                    '0', --PO Required Threshold Amount
                    'Cash Only', --Billing Type
                    '0', --Default to Deliver
                    '', --Delivery Route
                    '0', --Travel Time
                    '0', --Travel Distance
                    '0', --Free Delivery Minimum Amount
                    '0', --Delivery Charge
                    'Balance Forward', --Statement Type
                    '0', --Percent Discount if Paid in Full
                    '0', --Days to Pay for Discount
                    '0', --Due Date
                    '0', --Extra Statement Copies
                    '0', --Send Invoices Every * Days
                    '0', --Send Account Summary Every * Days
                    '0', --Email Statements
                    '', --Statement Mailing Address
                    '0', --Last Payment Amount
                    '', --Last Payment Date
                    '0', --Highest Amount Owed
                    '', --Highest Amount Owed Date
                    '0', --Highest Amount Paid
                    '', --Highest Amount Paid Date
                    '0', --Last Statement Amount
                    '0', --Total Due
                    '0', --Due 30 Days
                    '0', --Due 60 Days
                    '0', --Due 90 Days
                    '0', --Further Due
                    '0', --Service Charge
                    '0', --Enable TIMS Relations
                    '', --TIMS Relations Key
                    '0', --Automatic Price Updates
                    '0' --Automatic Media Updates
                );";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Customers (
                    ""CustomerName"", ""CustomerNumber"", ""InStorePricingProfile"", ""OnlinePricingProfile"", ""DefaultInStorePriceSheet"", 
                    ""DefaultOnlinePriceSheet"", ""CanCharge"", ""CreditLimit"", ""AccountBalance"",
                    ""PhoneNumber"", ""FaxNumber"", ""BillingAddress"", ""ShippingAddress"", ""InvoiceMessage"", ""Website"",
                    ""Email"", ""AssignedRep"", ""BusinessCategory"", ""DateAdded"", ""DateOfLastSale"", ""DateOfLastROA"",
                    ""PreferredLanguage"", ""AuthorizedBuyers"", ""DefaultTaxTable"", ""DeliveryTaxTable"", ""PrimaryTaxStatus"",
                    ""SecondaryTaxStatus"", ""PrimaryTaxExemptionNumber"", ""SecondaryTaxExemptionNumber"", ""PrimaryTaxExemptionExpiration"",
                    ""SecondaryTaxExemptionExpiration"", ""PrintCatalogNotesOnInvoice"", ""PrintBalanceOnInvoice"", ""EmailInvoices"",
                    ""AllowBackorders"", ""AllowSpecialOrders"", ""ExemptFromInvoiceSurcharges"", ""ExtraInvoiceCopies"",
                    ""PORequiredThresholdAmount"", ""BillingType"", ""DefaultToDeliver"", ""DeliveryRoute"", ""TravelTime"",
                    ""TravelDistance"", ""MinimumSaleFreeDelivery"", ""DeliveryCharge"", ""StatementType"", ""PercentDiscount"",
                    ""PaidByForDiscount"", ""DueDate"", ""ExtraStatementCopies"", ""SendInvoicesEvery_Days"", ""SendAccountSummaryEvery_Days"",
                    ""EmailStatements"", ""StatementMailingAddress"", ""LastPaymentAmount"", ""LastPaymentDate"", ""HighestAmountOwed"",
                    ""HighestAmountOwedDate"", ""HighestAmountPaid"", ""HighestAmountPaidDate"", ""LastStatementAmount"", ""TotalDue"",
                    ""Due30Days"", ""Due60Days"", ""Due90Days"", ""FurtherDue"", ""ServiceCharge"", ""EnableTIMSRelations"",
                    ""RelationshipKey"", ""AutomaticallySendPriceUpdates"", ""AutomaticallySendMedia"") 
                VALUES (
                    'Default Customer', --Customer Name
                    '9999', --Customer Number
                    '0', --In Store Pricing Profile
                    '0', --Online Pricing Profile
                    'Green', --Default In Store Price Sheet
                    'Green', --Default Online Price Sheet
                    '0', --Can Charge
                    '0', --Credit Limit
                    '0', --Account Balance
                    '0', --Phone Number
                    '0', --Fax Number
                    'Address, City, State, ZIP, Country', --Billing Address
                    'Address, City, State, ZIP, Country', --Shipping Address
                    '', --Invoice Message
                    '', --Website
                    '', --Email
                    '', --Assigned Representative
                    '', --Business Category
                    '10/10/2022', --Date Added
                    '10/10/2022', --Date of Last Sale
                    '10/10/2022', --Date of Last ROA
                    '', --Preferred Language
                    '', --Authorized Buyers
                    'Default', --Default Tax Table
                    'Default', --Delivery Tax Table
                    'Non-Exempt', --Primary Tax Status
                    'Non-Exempt', --Secondary Tax Status
                    '', --Primary Tax Exempt Number
                    '', --Secondary Tax Exempt Number
                    '', --Primary Tax Exemption Expiration
                    '', --Secondary Tax Exemption Expiration
                    '1', --Print Catalog Notes on Invoices
                    '0', --Print Balance on Invoices
                    '0', --Email Invoices
                    '1', --Allow Backorders
                    '1', --Allow Special Orders
                    '0', --Exempt from Invoice Surcharges
                    '0', --Extra Copies of Invoices
                    '0', --PO Required Threshold Amount
                    'Cash Only', --Billing Type
                    '0', --Default to Deliver
                    '', --Delivery Route
                    '0', --Travel Time
                    '0', --Travel Distance
                    '0', --Free Delivery Minimum Amount
                    '0', --Delivery Charge
                    'Balance Forward', --Statement Type
                    '0', --Percent Discount if Paid in Full
                    '0', --Days to Pay for Discount
                    '0', --Due Date
                    '0', --Extra Statement Copies
                    '0', --Send Invoices Every * Days
                    '0', --Send Account Summary Every * Days
                    '0', --Email Statements
                    '', --Statement Mailing Address
                    '0', --Last Payment Amount
                    '', --Last Payment Date
                    '0', --Highest Amount Owed
                    '', --Highest Amount Owed Date
                    '0', --Highest Amount Paid
                    '', --Highest Amount Paid Date
                    '0', --Last Statement Amount
                    '0', --Total Due
                    '0', --Due 30 Days
                    '0', --Due 60 Days
                    '0', --Due 90 Days
                    '0', --Further Due
                    '0', --Service Charge
                    '0', --Enable TIMS Relations
                    '', --TIMS Relations Key
                    '0', --Automatic Price Updates
                    '0' --Automatic Media Updates
                );";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("Departments"))
            {
                command.CommandText =
                @"CREATE TABLE ""Departments"" (
                ""ID"" INTEGER NOT NULL,
                ""Department""    TEXT NOT NULL,
	            PRIMARY KEY(""ID"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO DEPARTMENTS (DEPARTMENT) VALUES ('Default')";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("Devices"))
            {
                command.CommandText =
                @"CREATE TABLE ""Devices"" (
                ""ID""    INTEGER NOT NULL,
	            ""DeviceType""    INTEGER NOT NULL,
	            ""IPAddress"" TEXT NOT NULL,
                ""Nickname""	TEXT NOT NULL,
                PRIMARY KEY(""ID"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();

                command.CommandText =
                    "INSERT INTO DEVICES (DEVICETYPE, IPADDRESS, NICKNAME) VALUES ('TERMINAL', '::1', 'SERVER')";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("DeviceAssignments"))
            {
                command.CommandText =
                @"CREATE TABLE ""DeviceAssignments"" (
                ""AssignmentID""  INTEGER NOT NULL,
	            ""DeviceID""    INTEGER NOT NULL,
                ""TerminalID""  INTEGER NOT NULL,
	            PRIMARY KEY(""AssignmentID"" AUTOINCREMENT)
                ); ";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("Employees"))
            {
                command.CommandText =
                @"CREATE TABLE ""Employees"" (
                ""EmployeeNumber""    INTEGER NOT NULL UNIQUE,
                ""FullName""  TEXT NOT NULL,
	            ""Username""  TEXT NOT NULL,
	            ""Position""  TEXT NOT NULL,
	            ""BirthDate"" TEXT NOT NULL,
	            ""HireDate""  TEXT NOT NULL,
	            ""TerminationDate""   TEXT NOT NULL,
	            ""Permissions""   TEXT NOT NULL,
	            ""StartupScreen"" INTEGER NOT NULL,
	            ""Commissioned""  INTEGER NOT NULL,
	            ""CommissionRate""    REAL,
	            ""Waged"" INTEGER NOT NULL,
	            ""HourlyWage""    REAL,
	            ""PayPeriod"" INTEGER NOT NULL,
	            ""PasswordHash""  BLOB NOT NULL,
	            PRIMARY KEY(""EmployeeNumber"")
                )";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Employees (""EmployeeNumber"", ""FullName"", ""Username"", ""Position"", ""BirthDate"", ""HireDate"", ""TerminationDate"", ""Permissions"", ""StartupScreen"", ""Commissioned"", ""CommissionRate"", ""Waged"", ""HourlyWage"", ""PayPeriod"", ""PasswordHash"") 
                VALUES ('0', 'Administrator', 'admin', 'Administrator', '01/01/1979', '01/01/1979', '', 'ALL', '0', '0', '', '0', '', '0', X'5feceb66ffc86f38d952786c6d696c79c2dbc239dd4e91b46729d73a27fb57e9');";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("GlobalProperties"))
            {
                command.CommandText =
                @"CREATE TABLE ""GlobalProperties"" (
                ""ID""    INTEGER NOT NULL UNIQUE,
                ""Key""   TEXT NOT NULL UNIQUE,
                ""Value"" TEXT NOT NULL,
	            PRIMARY KEY(""ID"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();
                string APIKey = GetRandomAlphanumericString(32);
                
                command.CommandText =
              @"INSERT INTO ""main"".""GlobalProperties"" (""Key"", ""Value"") VALUES ('Store Name', '');
                INSERT INTO ""main"".""GlobalProperties"" (""Key"", ""Value"") VALUES ('Store Number', '');
                INSERT INTO ""main"".""GlobalProperties"" (""Key"", ""Value"") VALUES ('Store Physical Address', '');
                INSERT INTO ""main"".""GlobalProperties"" (""Key"", ""Value"") VALUES ('Store Mailing Address', '');
                INSERT INTO ""main"".""GlobalProperties"" (""Key"", ""Value"") VALUES ('Store Phone Number', '');
                INSERT INTO ""main"".""GlobalProperties"" (""Key"", ""Value"") VALUES ('Store Fax Number', '');
                INSERT INTO ""main"".""GlobalProperties"" (""Key"", ""Value"") VALUES ('Store Website Number', '');
                INSERT INTO ""main"".""GlobalProperties"" (""Key"", ""Value"") VALUES ('Store Email Number', '');
                INSERT INTO ""main"".""GlobalProperties"" (""Key"", ""Value"") VALUES ('Integrated Card Payments', '0');
                INSERT INTO ""main"".""GlobalProperties"" (""Key"", ""Value"") VALUES ('Server Relationship Key', '" + APIKey + @"');
                INSERT INTO ""main"".""GlobalProperties"" (""Key"", ""Value"") VALUES ('Charge Override Password', 'paul');
                INSERT INTO ""main"".""GlobalProperties"" (""Key"", ""Value"") VALUES ('Current Transaction Number', '1');
                INSERT INTO ""main"".""GlobalProperties"" (""Key"", ""Value"") VALUES ('Current Batch Number', '1');
                INSERT INTO ""main"".""GlobalProperties"" (""Key"", ""Value"") VALUES ('Signature Minimum', '100');";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("InvoiceItems"))
            {
                command.CommandText =
                @"CREATE TABLE ""InvoiceItems"" (
                ""InvoiceNumber"" INTEGER NOT NULL,
	            ""ItemNumber""    TEXT NOT NULL,
	            ""ProductLine""   TEXT NOT NULL,
	            ""ItemDescription""   TEXT,
	            ""Price"" REAL NOT NULL,
	            ""ListPrice"" REAL NOT NULL,
	            ""Quantity""  REAL NOT NULL,
	            ""Total"" REAL NOT NULL,
	            ""PriceCode"" TEXT NOT NULL,
	            ""Serialized""    INTEGER NOT NULL,
	            ""SerialNumber""  TEXT,
	            ""AgeRestricted"" INTEGER NOT NULL,
	            ""MinimumAge""    INTEGER,
	            ""Taxed"" INTEGER NOT NULL,
	            ""InvoiceCodes""  TEXT,
	            ""GUID""  TEXT NOT NULL UNIQUE,
                ""Cost""  REAL,
	            PRIMARY KEY(""GUID"")
                )";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("Invoices"))
            {
                command.CommandText =
                @"CREATE TABLE ""Invoices"" (
                ""InvoiceNumber"" INTEGER NOT NULL UNIQUE,
                ""Subtotal""  REAL NOT NULL,
	            ""TaxableTotal""  REAL NOT NULL,
	            ""TaxRate""   REAL NOT NULL,
	            ""TaxAmount"" REAL NOT NULL,
	            ""Total"" REAL NOT NULL,
	            ""TotalPayments"" REAL NOT NULL,
	            ""AgeRestricted"" INTEGER NOT NULL,
	            ""CustomerBirthdate"" TEXT,
	            ""Attention"" TEXT,
	            ""PO""    TEXT,
	            ""Message""   TEXT,
	            ""SavedInvoice""  INTEGER NOT NULL,
	            ""SavedInvoiceTime""  TEXT,
	            ""InvoiceCreationTime""   TEXT,
	            ""InvoiceFinalizedTime""  TEXT,
	            ""Finalized"" INTEGER NOT NULL,
	            ""Voided""    INTEGER NOT NULL,
	            ""CustomerNumber""    INTEGER NOT NULL,
	            ""EmployeeNumber""    INTEGER NOT NULL,
	            ""Cost""  REAL,
	            ""Profit""    REAL,
	            PRIMARY KEY(""InvoiceNumber"")
                )";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("Items"))
            {
                command.CommandText =
                @"CREATE TABLE ""Items"" (
                ""ProductLine""   TEXT NOT NULL,
	            ""ItemNumber""    TEXT NOT NULL,
	            ""ItemName""  TEXT,
	            ""LongDescription""   TEXT,
	            ""Supplier""  TEXT,
	            ""GroupCode"" INTEGER,
	            ""VelocityCode""  INTEGER,
	            ""PreviousYearVelocityCode""  INTEGER,
	            ""ItemsPerContainer"" INTEGER,
	            ""StandardPackage""   INTEGER,
	            ""DateStocked""   TEXT,
	            ""DateLastReceipt""   TEXT,
	            ""Minimum""   REAL,
	            ""Maximum""   REAL,
	            ""OnHandQuantity""    REAL,
	            ""WIPQuantity""   REAL,
	            ""OnOrderQuantity""   REAL,
	            ""BackorderQuantity"" REAL,
	            ""DaysOnOrder""   INTEGER,
	            ""DaysOnBackOrder""   INTEGER,
	            ""ListPrice"" REAL,
	            ""RedPrice""  REAL,
	            ""YellowPrice""   REAL,
	            ""GreenPrice""    REAL,
	            ""PinkPrice"" REAL,
	            ""BluePrice"" REAL,
	            ""ReplacementCost""   REAL,
	            ""AverageCost""   REAL,
	            ""Taxed"" INTEGER,
	            ""AgeRestricted"" INTEGER,
	            ""MinimumAge""    INTEGER,
	            ""LocationCode""  INTEGER,
	            ""Serialized""    INTEGER,
	            ""Category""  TEXT,
	            ""UPC""   TEXT,
	            ""LastLabelDate"" TEXT,
	            ""LastLabelPrice""    REAL,
                ""DateLastSale""    TEXT,
                ""ManufacturerNumber""  TEXT,
                ""LastSalePrice""   REAL,
                ""Brand""   TEXT,
                ""Department""  TEXT,
                ""Subdepartment""   TEXT,
                ""ImagePaths""  TEXT,
                ""WooCommerceID"" INTEGER
                )";
                command.ExecuteNonQuery();

                command.CommandText =
                    @"INSERT INTO Items (""ProductLine"", ""ItemNumber"", ""ItemName"", ""LongDescription"", ""Supplier"", ""GroupCode"", ""VelocityCode"", ""PreviousYearVelocityCode"", ""ItemsPerContainer"", ""StandardPackage"", ""DateStocked"", ""DateLastReceipt"", ""Minimum"", ""Maximum"", ""OnHandQuantity"", ""WIPQuantity"", ""OnOrderQuantity"", ""BackorderQuantity"", ""DaysOnOrder"", ""DaysOnBackOrder"", ""ListPrice"", ""RedPrice"", ""YellowPrice"", ""GreenPrice"", ""PinkPrice"", ""BluePrice"", ""ReplacementCost"", ""AverageCost"", ""Taxed"", ""AgeRestricted"", ""MinimumAge"", ""LocationCode"", ""Serialized"", ""Category"", ""UPC"", ""LastLabelDate"", ""LastLabelPrice"", ""DateLastSale"", ""ManufacturerNumber"", ""LastSalePrice"", ""Brand"", ""Department"", ""Subdepartment"", ""ImagePaths"", ""WooCommerceID"") 
                    VALUES ('XXX', 'XXX', 'XXX', 'XXX', 'Default', '0', '0', '0', '0', '0', '6/11/2022 7:43:28 PM', '6/11/2022 7:43:28 PM', '0.0', '0.0', '0.0', '0.0', '0.0', '0.0', '0', '0', '0.0', '0.0', '0.0', '0.0', '0.0', '0.0', '0.0', '0.0', '1', '0', '0', '0', '0', 'Etc', 'xxx', '01/01/0001 12:00 AM', '0', '01/01/0001 12:00 AM', 'XXX', '0', 'Default', 'Default', 'Default', '', '0');";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("Media"))
            {
                command.CommandText =
                @"CREATE TABLE ""Media"" (
                ""ID""          INTEGER NOT NULL UNIQUE,
                ""Key""         TEXT NOT NULL UNIQUE,
                ""MediaType""   TEXT NOT NULL,
                ""Value""       BLOB,
	            PRIMARY KEY(""ID"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO ""main"".""Media"" (""Key"", ""MediaType"", ""Value"") 
                    VALUES ('Company Logo', 'Image', '');";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("Messages"))
            {
                command.CommandText =
                    @"CREATE TABLE ""Messages"" (
	                ""ID""	TEXT NOT NULL UNIQUE,
	                ""Subject""	TEXT NOT NULL,
	                ""Body""	TEXT NOT NULL,
	                ""Read""	INTEGER NOT NULL,
	                ""SendDate""	TEXT NOT NULL,
	                ""ReadDate""	TEXT NOT NULL,
	                ""Sender""	INTEGER NOT NULL,
                    ""Recipient"" INTEGER NOT NULL,
	                PRIMARY KEY(""ID"")
                    )";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("Payments"))
            {
                command.CommandText =
                @"CREATE TABLE ""Payments"" (
                ""InvoiceNumber"" INTEGER NOT NULL,
	            ""ID""    BLOB NOT NULL UNIQUE,
                ""PaymentType""   TEXT NOT NULL,
	            ""PaymentAmount"" REAL NOT NULL,
                ""CardReaderErrorMessage"" TEXT NOT NULL,
                ""IngenicoResponse"" TEXT, 
                ""IngenicoRequest"" TEXT, 
	            PRIMARY KEY(""ID"")
                )";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("ProductLines"))
            {
                command.CommandText =
                @"CREATE TABLE ""ProductLines"" (
                ""ProductLine""   TEXT NOT NULL,
	            ""ID""    INTEGER NOT NULL,
	            ""DefaultGroupCode""  INTEGER,
	            PRIMARY KEY(""ID"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO ProductLines (""ProductLine"", ""ID"", ""DefaultGroupCode"") VALUES ('XXX', '14', '');";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("PricingProfiles"))
            {
                command.CommandText =
                @"CREATE TABLE ""PricingProfiles"" (
                ""ProfileID""  INTEGER NOT NULL,
	            ""ProfileName"" TEXT NOT NULL,
	            PRIMARY KEY(""ProfileID"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();

                command.CommandText =
                    @"INSERT INTO PRICINGPROFILES (PROFILEID, PROFILENAME) VALUES ('0', 'Default Profile')";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("PricingProfileElements"))
            {
                command.CommandText =
                @"CREATE TABLE ""PricingProfileElements"" (
                ""ElementID""  INTEGER NOT NULL,
	            ""ProfileID"" INTEGER NOT NULL,
                ""Priority"" INTEGER NOT NULL,
                ""GroupCode"" TEXT,
                ""Department"" TEXT,
                ""SubDepartment"" TEXT,
                ""ProductLine"" TEXT,
                ""ItemNumber"" TEXT,
                ""PriceSheet"" TEXT NOT NULL,
                ""Margin"" REAL NOT NULL,
                ""BeginDate"" TEXT,
                ""EndDate"" TEXT,
	            PRIMARY KEY(""ElementID"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO PRICINGPROFILEELEMENTS 
                    (ELEMENTID, PROFILEID, PRIORITY, GROUPCODE, DEPARTMENT, SUBDEPARTMENT, PRODUCTLINE, ITEMNUMBER, PRICESHEET, MARGIN, BEGINDATE, ENDDATE)
                VALUES (
                    '0',
                    '0',
                    '1',
                    '',
                    '',
                    '',
                    '',
                    '',
                    'Cost',
                    '25',
                    '',
                    '')";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("PurchaseOrderItems"))
            {
                command.CommandText =
                @"CREATE TABLE ""PurchaseOrderItems"" (
                ""PONumber""  INTEGER NOT NULL,
	            ""ID""    BLOB NOT NULL UNIQUE,
                ""ItemNumber""    TEXT NOT NULL,
	            ""ProductLine""   TEXT NOT NULL,
	            ""Quantity""  REAL NOT NULL,
	            ""Cost""  REAL NOT NULL,
	            ""Price"" REAL NOT NULL,
	            PRIMARY KEY(""ID"")
                )";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("PurchaseOrders"))
            {
                command.CommandText =
                @"CREATE TABLE ""PurchaseOrders"" (
                ""PONumber""  INTEGER NOT NULL,
	            ""TotalCost"" REAL NOT NULL,
	            ""TotalItems""    REAL NOT NULL,
	            ""AssignedCheckIn""   INTEGER,
	            ""Supplier""  TEXT,
	            ""Finalized"" INTEGER NOT NULL,
	            ""ShippingCost""  REAL,
	            PRIMARY KEY(""PONumber"")
                )";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("Reports"))
            {
                command.CommandText =
                @"CREATE TABLE ""Reports"" (
                ""ReportName""    TEXT NOT NULL,
	            ""ReportShortCode""   TEXT NOT NULL,
	            ""DataSource""    TEXT NOT NULL,
	            ""Conditions""    TEXT NOT NULL,
	            ""Fields""    TEXT NOT NULL,
	            ""Totals""    TEXT NOT NULL,
	            PRIMARY KEY(""ReportShortCode"")
                )";
                command.ExecuteNonQuery();

                #region Default Reports
                command.CommandText =
                    @"INSERT INTO Reports (""ReportName"", ""ReportShortCode"", ""DataSource"", ""Conditions"", ""Fields"", ""Totals"") 
                    VALUES ('Invoice Sales', 
                    'COM150', 
                    'Invoices', 
                    'InvoiceNumber > 1', 
                    'InvoiceNumber|Subtotal|TaxAmount|Total|TotalPayments|CustomerNumber|EmployeeNumber', 
                    'Subtotal|TaxAmount|Total|TotalPayments');";
                command.ExecuteNonQuery();

                command.CommandText =
                    @"INSERT INTO Reports (""ReportName"", ""ReportShortCode"", ""DataSource"", ""Conditions"", ""Fields"", ""Totals"") 
                    VALUES ('On-Hand Inventory Low Report', 
                    'STLO', 
                    'Items', 
                    'OnHandQuantity <= Minimum|Maximum > 0', 
                    'ProductLine|ItemNumber|ItemName|Minimum|Maximum|OnHandQuantity', 
                    '');";
                command.ExecuteNonQuery();

                command.CommandText =
                    @"INSERT INTO Reports (""ReportName"", ""ReportShortCode"", ""DataSource"", ""Conditions"", ""Fields"", ""Totals"") 
                    VALUES ('Invoice Profit and Revenue Date Span Report', 
                    'COM160', 
                    'Invoices', 
                    'InvoiceFinalizedTime <= 6/1/2022|InvoiceFinalizedTime >= 5/1/2022', 
                    'InvoiceNumber|Subtotal|TaxAmount|Total|CustomerNumber|EmployeeNumber', 
                    'Subtotal|TaxAmount|Total');";
                command.ExecuteNonQuery();

                command.CommandText = //Daily Invoices Report
                @"INSERT INTO Reports (""ReportName"", ""ReportShortCode"", ""DataSource"", ""Conditions"", ""Fields"", ""Totals"") 
                VALUES ('Daily Sales and Transactions Report', 
                'COM170', 
                'Invoices', 
                'InvoiceFinalizedTime >= DateTime.Today|InvoiceFinalizedTime <= DateTime.Today', 
                'InvoiceNumber|Subtotal|TaxableTotal|TaxAmount|Total|TotalPayments', 
                'Subtotal|TaxableTotal|TaxAmount|Total|TotalPayments');";
                command.ExecuteNonQuery();

                command.CommandText =
                    @"INSERT INTO Reports (""ReportName"", ""ReportShortCode"", ""DataSource"", ""Conditions"", ""Fields"", ""Totals"") 
                    VALUES ('Daily Sales and Transactions Report', 
                    'COM180', 
                    'Invoices', 
                    'InvoiceFinalizedTime >= DateTime.Today|InvoiceFinalizedTime <= DateTime.Today', 
                    'InvoiceNumber|Subtotal|TaxableTotal|TaxAmount|Total|TotalPayments|Cost|Profit', 
                    'Subtotal|TaxAmount|Total|TotalPayments|Cost|Profit');";
                command.ExecuteNonQuery();

                command.CommandText = //Stock Level Discrepancies Report
                    @"INSERT INTO REPORTS (
                    ReportName, ReportShortCode, DataSource, 
                    Conditions, Fields, Totals) 
                    VALUES(
                    'Stock Level Discrepancies Report', 'STK110', 'Items',
                    'OnHandQuantity < 0',
                    'ProductLine|ItemNumber|OnHandQuantity|WIPQuantity|OnOrderQuantity|DateLastSale|Minimum|GreenPrice',
                    ''); ";
                command.ExecuteNonQuery();
                #endregion
            }
            else
            {

            }

            if (!TableExists("SerialNumbers"))
            {
                command.CommandText =
                @"CREATE TABLE ""SerialNumbers"" (
                ""ItemNumber""    TEXT NOT NULL,
	            ""ProductLine""   TEXT NOT NULL,
	            ""SerialNumber""  TEXT NOT NULL,
	            ""ID""    REAL NOT NULL,
	            PRIMARY KEY(""ID"")
                )";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("ServerRelationships"))
            {
                command.CommandText =
                @"CREATE TABLE ""ServerRelationships"" (
                ""ID""    INTEGER NOT NULL,
	            ""DeviceType""    INTEGER NOT NULL,
	            ""IPAddress"" TEXT NOT NULL,
                ""Relationship""	TEXT NOT NULL,
                PRIMARY KEY(""ID"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("ShortcutMenus"))
            {
                command.CommandText =
                @"CREATE TABLE ""ShortcutMenus"" (
                ""ID""    INTEGER NOT NULL UNIQUE,
                ""MenuName""  TEXT NOT NULL,
	            ""MenuItems"" TEXT NOT NULL,
	            ""ParentMenu""    TEXT NOT NULL,
	            PRIMARY KEY(""ID"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("Suppliers"))
            {
                command.CommandText =
                @"CREATE TABLE ""Suppliers"" (
                ""Supplier""  TEXT NOT NULL,
	            ""LastOrderDate"" TEXT,
	            PRIMARY KEY(""Supplier"")
                )";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            if (!TableExists("Subdepartments"))
            {
                command.CommandText =
                @"CREATE TABLE ""Subdepartments"" (
                ""ID"" INTEGER NOT NULL,
                ""Department""    TEXT NOT NULL,
                ""ParentDepartment"" TEXT NOT NULL,
	            PRIMARY KEY(""ID"" AUTOINCREMENT)
                )";
                command.ExecuteNonQuery();

                command.CommandText = "INSERT INTO SUBDEPARTMENTS (DEPARTMENT, PARENTDEPARTMENT) VALUES ('Default', 'Default')";
                command.ExecuteNonQuery();
            }
            else
            {

            }

            CloseConnection();
        }

        public static bool TableExists(string tableName)
        {
            if (sqlite_conn.State == System.Data.ConnectionState.Open)
            {
                SqliteCommand command = sqlite_conn.CreateCommand();
                command.CommandText = "SELECT name FROM sqlite_master WHERE type='table' AND name='" + tableName + "';";
                SqliteDataReader reader = command.ExecuteReader();
                if (reader.HasRows)
                {
                    return true;
                }
                return false;
            }
            else
            {
                throw new System.ArgumentException("Data.ConnectionState must be open");
            }
        }

        public void AddColumnToTable(string tableName, string fieldName, string dataType, string defaultValue = "")
        {
            OpenConnection();
            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = "SELECT " + fieldName + " FROM " + tableName + " LIMIT 1";
            try
            {
                command.ExecuteReader();
            }
            catch
            {
                command.CommandText = "ALTER TABLE " + tableName + " ADD " + fieldName + " " + dataType + ";";
                command.ExecuteNonQuery();
                command.CommandText = "UPDATE " + tableName + " SET " + fieldName + " = " + defaultValue;
                command.ExecuteNonQuery();
            }
            CloseConnection();
        }

        public static bool IsAlphanumeric(string str)
        {
            foreach (char c in str)
                if (!Char.IsLetterOrDigit(c))
                    return false;
            return true;
        }

        public static string SqlCheckEmployee(string input)
        {
            if (!int.TryParse(input, out int _))
                input = "'" + input + "'";
            string value = null;
            OpenConnection();
            SqliteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT FULLNAME " +
                "FROM EMPLOYEES " +
                "WHERE USERNAME = " + input + " " +
                "OR EMPLOYEENUMBER = " + input;

            SqliteDataReader rdr = sqlite_cmd.ExecuteReader();
            while (rdr.Read())
            {
                value = $"{rdr.GetString(0)}";
            }

            CloseConnection();

            return value;
        }

        public static string GetLocalIPAddress()
        {
            IPHostEntry host = Dns.GetHostEntry(Dns.GetHostName());
            foreach (var ip in host.AddressList)
            {
                if (ip.AddressFamily == AddressFamily.InterNetwork)
                {
                    return ip.ToString();
                }
            }
            throw new Exception("No network adapters with an IPv4 address in the system!");
        }

        public static string GetRandomAlphanumericString(int length)
        {
            const string alphanumericCharacters =
                "ABCDEFGHIJKLMNOPQRSTUVWXYZ" +
                "abcdefghijklmnopqrstuvwxyz" +
                "0123456789";
            return GetRandomString(length, alphanumericCharacters);
        }

        public static string GetRandomString(int length, IEnumerable<char> characterSet)
        {
            if (length < 0)
                throw new ArgumentException("length must not be negative", "length");
            if (length > int.MaxValue / 8) // 250 million chars ought to be enough for anybody
                throw new ArgumentException("length is too big", "length");
            if (characterSet == null)
                throw new ArgumentNullException("characterSet");
            var characterArray = characterSet.Distinct().ToArray();
            
            if (characterArray.Length == 0)
                throw new ArgumentException("characterSet must not be empty", "characterSet");

            var bytes = new byte[length * 8];
            var result = new char[length];
            using (var cryptoProvider = new RNGCryptoServiceProvider())
            {
                cryptoProvider.GetBytes(bytes);
            }
            for (int i = 0; i < length; i++)
            {
                ulong value = BitConverter.ToUInt64(bytes, i * 8);
                result[i] = characterArray[value % (uint)characterArray.Length];
            }
            return new string(result);
        }
    
        public static string GetProperty(string key)
        {
            string value;
            OpenConnection();
            SqliteCommand command = sqlite_conn.CreateCommand();
            command.CommandText = "SELECT VALUE FROM GLOBALPROPERTIES WHERE KEY = $KEY";
            command.Parameters.Add(new SqliteParameter("$KEY", key));
            SqliteDataReader reader = command.ExecuteReader();
            if (!reader.HasRows)
                return "";
            else
            {
                reader.Read();
                value = reader.GetString(0);
            }
            reader.Close();
            CloseConnection();
            return value;
        }
    }
}