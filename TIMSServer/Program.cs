using System;
using System.Data.SQLite;
using System.ServiceModel;
using System.Net;
using System.Net.Http;
using ESCPOS_NET;
using ESCPOS_NET.Emitters;
using System.Collections.Generic;
using Microsoft.Web.Administration;
using System.IO;

namespace TIMSServer
{
    class Program
    {
        private static readonly HttpClient client = new HttpClient();
        public static int alertLevel = 3;
        public static bool waitingForRegister;

        /* Legacy Code
        static void Main()
        {
            Customer cust = new Customer();
            cust.customerName = "Kirby & Kirby";
            cust.customerNumber = "11600";
            Console.WriteLine(DateTime.Now + "Initializing database connection...");
            Database.InitializeDatabase();
            Console.WriteLine("Testing database connection open and close...");
            Database.OpenConnection();
            Database.CloseConnection();
            Console.WriteLine("Success");
            Console.WriteLine("Starting server...");
            System.Threading.Thread serverThread = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(Server.ServerLoop));
            serverThread.Start();
            while (!Server.serverRunning) { }
            Console.WriteLine("Ready");

            while (true)
            {
                Console.Write("\nTIMS@" + Environment.MachineName + ">");
                string input = Console.ReadLine();
                FileStream inputLogger = File.Open("inputHistory.txt", FileMode.OpenOrCreate);
                if (input == "prev")
                {
                    byte[] data = new byte[inputLogger.Length];
                    inputLogger.Read(data, 0, data.Length);
                    string[] inputSplit = System.Text.Encoding.ASCII.GetString(data).Split('\n');
                    input = inputSplit[inputSplit.Length - 2];
                }
                else
                {
                    inputLogger.Seek(0, SeekOrigin.End);
                    inputLogger.Write(System.Text.Encoding.ASCII.GetBytes(input + "\n"), (int)inputLogger.Seek(0, SeekOrigin.End), System.Text.Encoding.ASCII.GetBytes(input + "\n").Length);
                }
                inputLogger.Close();
                string[] split = input.Split(' ');
                switch (split[0])
                {
                    case "exit":
                        {
                            Console.WriteLine("Type 'exit' again to stop the server, otherwise press enter.");
                            if (Console.ReadLine().ToLower() == "exit")
                                Environment.Exit(0);
                            else
                                Console.WriteLine("Exit aborted.");
                            break;
                        }
                    case "sql":
                        {
                            if (split.Length == 1)
                            {
                                Database.OpenConnection();
                                Console.WriteLine("Connection string: " + Database.sqlite_conn.ConnectionString);
                                Console.WriteLine("Server version: " + Database.sqlite_conn.ServerVersion);
                                Console.WriteLine("Server location: " + Database.sqlite_conn.FileName);
                                Database.CloseConnection();
                            }
                            else if (split.Length == 2)
                            {
                                switch (split[1].ToLower())
                                {
                                    case "command":
                                        {
                                            Console.WriteLine("Usage: sql command [SQLite Command]");
                                            break;
                                        }
                                    case "list":
                                        {
                                            Console.WriteLine("Usage: sql list [customers/items/invoices]");
                                            break;
                                        }
                                    case "help":
                                        {
                                            Console.WriteLine("Usage: sql [help/list]");
                                            break;
                                        }
                                    default:
                                        {
                                            Console.WriteLine("Usage: sql [help/list]");
                                            break;
                                        }
                                }
                            }
                            else if (split.Length > 2)
                            {
                                switch (split[1].ToLower())
                                {
                                    case "list":
                                        {
                                            switch (split[2].ToLower())
                                            {
                                                case "invoices":
                                                    {
                                                        break;
                                                    }
                                                case "customers":
                                                    {
                                                        Console.WriteLine("Hold on a minute...");
                                                        break;
                                                    }
                                                case "items":
                                                    {
                                                        break;
                                                    }
                                                default:
                                                    {
                                                        Console.WriteLine("Usage: sql list [customers/items/invoices]");
                                                        break;
                                                    }
                                            }
                                            break;
                                        }
                                    case "command":
                                        {
                                            string SplitTemp = input.Split(' ', (char)3)[2];
                                            Database.QueueCommand("server " + SplitTemp);
                                            while (Database.ResponseQueue.Count == 0) { }
                                            Response response = Database.ResponseQueue.Dequeue();
                                            if (response == null)
                                            {
                                                Console.WriteLine("SQL Error! Aborting command input.");
                                                break;
                                            }
                                            string[] rows = response.responseText.Split('\n');

                                            int i = 0;
                                            foreach (string col in rows)
                                            {
                                                if (i == 0)
                                                {
                                                    Console.WriteLine("Origin: " + rows[0]);
                                                    i++;
                                                    continue;
                                                }
                                                string[] cols = col.Split('`');
                                                foreach (string c in cols)
                                                    Console.Write("[{0}]", c);
                                                i++;
                                                Console.Write("\n");
                                            }
                                            break;
                                        }
                                    case "help":
                                        {
                                            Console.WriteLine("Usage: sql [help/list]");
                                            break;
                                        }
                                    default:
                                        {
                                            Console.WriteLine("Usage: sql [help/list]");
                                            break;
                                        }
                                }
                            }
                            break;
                        }
                    case "invitems":
                        {
                            if (split.Length == 1)
                            {
                                Console.WriteLine("Usage: invitems [invoice_number]");
                                Console.WriteLine("Returns a list of items on an invoice matching [invoice_number].");
                            }
                            break;
                        }
                    case "invsearch":
                        {
                            if (split.Length == 1)
                            {
                                Console.WriteLine("Usage: invsearch [customer_number] [invoice_date]");
                                Console.WriteLine("Returns invoice numbers matching [customer_number] and [invoice_date].");
                                Console.WriteLine("[Invoice_date] must be formed as mm/dd/yyyy");
                            }
                            break;
                        }
                    case "help":
                        {
                            Console.WriteLine("Supported commands:");
                            Console.WriteLine("exit, sql, help, invitems, invsearch");
                            break;
                        }
                    case "sha2":
                        {
                            if (split.Length != 2)
                                break;
                            SHA256 hasher = SHA256.Create();
                            string hash = System.Text.Encoding.ASCII.GetString(hasher.ComputeHash(System.Text.Encoding.ASCII.GetBytes(split[1])));
                            Console.WriteLine(hash);
                            break;
                        }
                    default:
                        {
                            Console.WriteLine("Unknown command \"" + split[0] + "\". Type 'help' for more information.");
                            break;
                        }
                }
            }
        }

        public static void ReturnToInput()
        {
            Console.Write("\nTIMS@" + Environment.MachineName + ">");
        }

        static void Panic(object o)
        {
            while (true)
                Alert();
        }

        public static void Alert()
        {
            if (alertLevel == 1)
            {
                Console.Beep(1000, 150);
                Console.Beep(1000, 150);
                Console.Beep(1000, 150);
                System.Threading.Thread.Sleep(10000);
            }

            if (alertLevel == 2)
            {
                Console.Beep(1400, 500);
                Console.Beep(1400, 500);
                Console.Beep(1400, 500);
                System.Threading.Thread.Sleep(5000);
            }

            if (alertLevel == 3)
            {
                Console.Beep(1800, 800);
                System.Threading.Thread.Sleep(40);
                Console.Beep(1800, 800);
                System.Threading.Thread.Sleep(40);
                Console.Beep(1800, 800);
                System.Threading.Thread.Sleep(800);
            }
        }
    */

        public static SQLiteConnection sqlite_conn;

        static void Main()
        {
            sqlite_conn = new SQLiteConnection("Data Source=database.db; Version = 3; New = True; Pooling = true; Max Pool Size = 100; Compress = True; Password = 3nCryqtEdT!MSPa$$w0rdFoRrev!tAc0m");

            OpenConnection();
            CloseConnection();
            CreateDatabase();

            using (ServiceHost host = new ServiceHost(typeof(TIMSServiceModel)))
            {
                host.Open();
                #region Web Host
                //string path = @"C:\\Users\\Blake\\source\\repos\\dcogburn98\\TIMS\\TIMSServerManager\\";
                //string name = "TIMSServer";

                //char[] invalid = SiteCollection.InvalidSiteNameCharacters();
                //if (name.IndexOfAny(invalid) > -1)
                //{
                //    Console.WriteLine("Invalid site name: {0}", name);
                //}

                //if (!System.IO.Directory.Exists(path))
                //{
                //    System.IO.Directory.CreateDirectory(path);
                //}

                //ServerManager manager = new ServerManager();

                //if (manager.Sites.Count > 0)
                //    manager.Sites.Clear();
                //Site managerSite = manager.Sites.Add(name + "Manager", "http", "*:9842:", path);
                ////Site shoppingSite = manager.Sites.Add(name + "Website", "http", "*:8080:*", path);

                //managerSite.Applications.Clear();
                //Application managerApplication = managerSite.Applications.Add("/", "C:\\Users\\Blake\\source\\repos\\dcogburn98\\TIMS\\TIMSServerManager\\");
                ////managerSite.SetAttributeValue("defaultDocument", "default.aspx");

                //managerSite.ServerAutoStart = false;
                //manager.CommitChanges();
                //Console.WriteLine("Site " + name + " added to ApplicationHost.config file.");
                //managerSite.Start();
                #endregion

                //GetTiers();
                // Ethernet or WiFi (This uses an Immediate Printer, no live paper status events, but is easier to use)
                string hostnameOrIp = "192.168.254.75";
                int port = 9100;
                ICommandEmitter e = new EPSON();
                BasePrinter printer = new NetworkPrinter(new NetworkPrinterSettings()
                    { ConnectionString = $"{hostnameOrIp}:{port}", PrinterName = "TestPrinter" });
                //printer = null; //Uncomment to disable printer
                printer?.Write(e.Initialize());
                printer?.Write(e.Enable());
                byte[][] Receipt() => new byte[][] {
                    e.CenterAlign(),
                    e.PrintLine(),
                    e.SetBarcodeHeightInDots(360),
                    e.SetBarWidth(BarWidth.Default),
                    e.SetBarLabelPosition(BarLabelPrintPosition.None),
                    e.PrintBarcode(BarcodeType.ITF, "0123456789"),
                    e.PrintLine(),
                    e.PrintLine("B&H PHOTO & VIDEO"),
                    e.PrintLine("420 NINTH AVE."),
                    e.PrintLine("NEW YORK, NY 10001"),
                    e.PrintLine("(212) 502-6380 - (800)947-9975"),
                    e.SetStyles(PrintStyle.Underline),
                    e.PrintLine("www.bhphotovideo.com"),
                    e.SetStyles(PrintStyle.None),
                    e.PrintLine(),
                    e.LeftAlign(),
                    e.PrintLine("Order: 123456789        Date: 02/01/19"),
                    e.PrintLine(),
                    e.PrintLine(),
                    e.SetStyles(PrintStyle.FontB),
                    e.PrintLine("1   TRITON LOW-NOISE IN-LINE MICROPHONE PREAMP"),
                    e.PrintLine("    TRFETHEAD/FETHEAD                        89.95         89.95"),
                    e.PrintLine("----------------------------------------------------------------"),
                    e.RightAlign(),
                    e.PrintLine("SUBTOTAL         89.95"),
                    e.PrintLine("Total Order:         89.95"),
                    e.PrintLine("Total Payment:         89.95"),
                    e.PrintLine(),
                    e.LeftAlign(),
                    e.SetStyles(PrintStyle.Bold | PrintStyle.FontB),
                    e.PrintLine("SOLD TO:                        SHIP TO:"),
                    e.SetStyles(PrintStyle.FontB),
                    e.PrintLine("  LUKE PAIREEPINART               LUKE PAIREEPINART"),
                    e.PrintLine("  123 FAKE ST.                    123 FAKE ST."),
                    e.PrintLine("  DECATUR, IL 12345               DECATUR, IL 12345"),
                    e.PrintLine("  (123)456-7890                   (123)456-7890"),
                    e.PrintLine("  CUST: 87654321"),
                    e.PrintLine(),
                    e.PrintLine()
                };
                printer?.Write(Receipt());
                printer?.Write(e.FeedLines(2));
                printer?.Write(e.FullCut());

                Console.WriteLine("Server is open for connections.");
                Console.WriteLine(host.Description.Endpoints[0].Address.ToString());
                Console.WriteLine("Press a key to close.");
                while (true)
                {
                    Console.Write("TIMS@" + host.Description.Endpoints[0].Address.ToString() + ">");
                    string input = Console.ReadLine();
                    string[] split = input.Split(' ');
                    switch (split[0].ToLower())
                    {
                        case "exit":
                            {
                                Environment.Exit(0);
                                break;
                            }
                        case "reg":
                            {
                                if (split.Length == 3)
                                {
                                    if (split[1].ToLower() == "-t")
                                    {

                                    }
                                    else
                                    {
                                        Console.WriteLine("Invalid useage of register command.");
                                    }
                                }
                                else if (split.Length == 5)
                                {

                                }
                                else
                                {
                                    Console.WriteLine("Invalid useage of register command.");
                                }
                                break;
                            }
                        default:
                            {
                                Console.WriteLine("Invalid input...");
                                break;
                            }
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

            SQLiteCommand command = sqlite_conn.CreateCommand();
            if (!TableExists(sqlite_conn, "AccountTransactions"))
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
	                PRIMARY KEY(""UID"" AUTOINCREMENT)
                    )";
                command.ExecuteNonQuery();
            }

            if (!TableExists(sqlite_conn, "Accounts"))
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
            }

            if (!TableExists(sqlite_conn, "Barcodes"))
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

            if (!TableExists(sqlite_conn, "CheckinItems"))
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
                #endregion
            }

            if (!TableExists(sqlite_conn, "Checkins"))
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

            if (!TableExists(sqlite_conn, "Customers"))
            {
                command.CommandText =
                @"CREATE TABLE ""Customers"" (
                ""CustomerName""  TEXT NOT NULL,
	            ""CustomerNumber""    INTEGER NOT NULL,
	            ""TaxExempt"" INTEGER NOT NULL,
	            ""TaxExemptionNumber""    TEXT NOT NULL,
	            ""PricingProfile""    TEXT NOT NULL,
	            ""PaymentTypes""  TEXT NOT NULL,
	            ""CanCharge"" INTEGER NOT NULL,
	            ""CreditLimit""   REAL NOT NULL,
	            ""AccountBalance""    REAL NOT NULL,
	            ""PhoneNumber""   TEXT NOT NULL,
	            ""FaxNumber"" TEXT NOT NULL,
	            ""BillingAddress""    TEXT NOT NULL,
	            ""ShippingAddress""   TEXT NOT NULL,
	            ""InvoiceMessage""    TEXT,
	            PRIMARY KEY(""CustomerNumber"")
                )";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO Customers (""CustomerName"", ""CustomerNumber"", ""TaxExempt"", ""TaxExemptionNumber"", ""PricingProfile"", ""PaymentTypes"", ""CanCharge"", ""CreditLimit"", ""AccountBalance"", ""PhoneNumber"", ""FaxNumber"", ""BillingAddress"", ""ShippingAddress"", ""InvoiceMessage"") 
                VALUES ('Cash Sale', '0', '0', '0', 'GREEN', 'Cash,Check,PaymentCard,Venmo,CashApp,Paypal', '0', '0.0', '0.0', '870-279-1334', '0', '1002 N Walters Ave, Dierks, AR, 71833, USA', '206 Main Ave, Dierks, AR, 71833, USA', '');";
                command.ExecuteNonQuery();

            }

            if (!TableExists(sqlite_conn, "Employees"))
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

            if (!TableExists(sqlite_conn, "GlobalProperties"))
            {
                command.CommandText =
                @"CREATE TABLE ""GlobalProperties"" (
                ""ID""    INTEGER NOT NULL UNIQUE,
                ""Key""   TEXT NOT NULL UNIQUE,
                ""Value"" TEXT NOT NULL,
	            PRIMARY KEY(""ID"")
                )";
                command.ExecuteNonQuery();

                command.CommandText =
                @"INSERT INTO ""main"".""GlobalProperties"" (""ID"", ""Key"", ""Value"") VALUES ('1', 'Store Name', 'Fish N Munition');
                INSERT INTO ""main"".""GlobalProperties""(""ID"", ""Key"", ""Value"") VALUES('2', 'Store Address', '206 Main Ave, Dierks, AR, 71833, USA');
                INSERT INTO ""main"".""GlobalProperties""(""ID"", ""Key"", ""Value"") VALUES('3', 'Store Phone Number', '870-279-1334');
                INSERT INTO ""main"".""GlobalProperties""(""ID"", ""Key"", ""Value"") VALUES('4', 'Store Alternate Phone Number', '870-279-7192');
                INSERT INTO ""main"".""GlobalProperties""(""ID"", ""Key"", ""Value"") VALUES('5', 'Tax 1 Rate', '0.1025');
                INSERT INTO ""main"".""GlobalProperties""(""ID"", ""Key"", ""Value"") VALUES('6', 'Tax 2 Rate', '0.0000');
                INSERT INTO ""main"".""GlobalProperties""(""ID"", ""Key"", ""Value"") VALUES('7', 'Apply Tax 1', '1');
                INSERT INTO ""main"".""GlobalProperties""(""ID"", ""Key"", ""Value"") VALUES('8', 'Apply Tax 2', '0');
                INSERT INTO ""main"".""GlobalProperties""(""ID"", ""Key"", ""Value"") VALUES('9', 'Tax 2 Taxes Tax 1', '0');
                INSERT INTO ""main"".""GlobalProperties""(""ID"", ""Key"", ""Value"") VALUES('10', 'Payment Types Available', 'Cash,Charge,PaymentCard,Paypal,Venmo,CashApp,Check');
                INSERT INTO ""main"".""GlobalProperties""(""ID"", ""Key"", ""Value"") VALUES('11', 'Store Number', '000028500');
                INSERT INTO ""main"".""GlobalProperties""(""ID"", ""Key"", ""Value"") VALUES('12', 'Mailing Address', '1002 N Walters Ave, Dierks, AR, 71833, USA'); 
                INSERT INTO ""main"".""GlobalProperties""(""ID"", ""Key"", ""Value"") VALUES('13', 'Integrated Card Payments', '1');";
                command.ExecuteNonQuery();
            }

            if (!TableExists(sqlite_conn, "InvoiceItems"))
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

            if (!TableExists(sqlite_conn, "Invoices"))
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

            if (!TableExists(sqlite_conn, "Items"))
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
	            ""SKU""   TEXT,
	            ""LastLabelDate"" TEXT,
	            ""LastLabelPrice""    REAL
                )";
                command.ExecuteNonQuery();

                command.CommandText =
                    @"INSERT INTO Items (""ProductLine"", ""ItemNumber"", ""ItemName"", ""LongDescription"", ""Supplier"", ""GroupCode"", ""VelocityCode"", ""PreviousYearVelocityCode"", ""ItemsPerContainer"", ""StandardPackage"", ""DateStocked"", ""DateLastReceipt"", ""Minimum"", ""Maximum"", ""OnHandQuantity"", ""WIPQuantity"", ""OnOrderQuantity"", ""BackorderQuantity"", ""DaysOnOrder"", ""DaysOnBackOrder"", ""ListPrice"", ""RedPrice"", ""YellowPrice"", ""GreenPrice"", ""PinkPrice"", ""BluePrice"", ""ReplacementCost"", ""AverageCost"", ""Taxed"", ""AgeRestricted"", ""MinimumAge"", ""LocationCode"", ""Serialized"", ""Category"", ""SKU"", ""LastLabelDate"", ""LastLabelPrice"") 
                    VALUES ('XXX', 'xxx', 'xxx', 'xxx', 'Default', '0', '0', '0', '0', '0', '6/11/2022 7:43:28 PM', '6/11/2022 7:43:28 PM', '0.0', '0.0', '0.0', '0.0', '0.0', '0.0', '0', '0', '0.0', '0.0', '0.0', '0.0', '0.0', '0.0', '0.0', '0.0', '1', '0', '0', '0', '0', 'Etc', 'xxx', '', '');";
                command.ExecuteNonQuery();
            }

            if (!TableExists(sqlite_conn, "Payments"))
            {
                command.CommandText =
                @"CREATE TABLE ""Payments"" (
                ""InvoiceNumber"" INTEGER NOT NULL,
	            ""ID""    BLOB NOT NULL UNIQUE,
                ""PaymentType""   TEXT NOT NULL,
	            ""PaymentAmount"" REAL NOT NULL,
	            PRIMARY KEY(""ID"")
                )";
                command.ExecuteNonQuery();
            }

            if (!TableExists(sqlite_conn, "ProductLines"))
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

            if (!TableExists(sqlite_conn, "PurchaseOrderItems"))
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

            if (!TableExists(sqlite_conn, "PurchaseOrders"))
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

            if (!TableExists(sqlite_conn, "Reports"))
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

                command.CommandText =
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
                #endregion
            }

            if (!TableExists(sqlite_conn, "SerialNumbers"))
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

            if (!TableExists(sqlite_conn, "ShortcutMenus"))
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

            if (!TableExists(sqlite_conn, "Suppliers"))
            {
                command.CommandText =
                @"CREATE TABLE ""Suppliers"" (
                ""Supplier""  TEXT NOT NULL,
	            ""LastOrderDate"" TEXT,
	            PRIMARY KEY(""Supplier"")
                )";
                command.ExecuteNonQuery();
            }

            if (!TableExists(sqlite_conn, "DeviceAssignments"))
            {
                command.CommandText =
                @"CREATE TABLE ""DeviceAssignments"" (
                ""AssignmentID""  INTEGER NOT NULL,
	            ""TerminalIP""    TEXT NOT NULL,
	            ""ReceiptPrinterIP""  TEXT NOT NULL,
	            ""InvoicePrinterIP""  TEXT NOT NULL,
	            ""LineDisplayIP"" TEXT NOT NULL,
	            ""CardReaderIP""  TEXT NOT NULL,
	            PRIMARY KEY(""AssignmentID"" AUTOINCREMENT)
                ); ";
                command.ExecuteNonQuery();
            }

            if (!TableExists(sqlite_conn, "Devices"))
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
            }

            CloseConnection();
        }

        public static bool TableExists(SQLiteConnection openConnection, string tableName)
        {
            var sql =
            "SELECT name FROM sqlite_master WHERE type='table' AND name='" + tableName + "';";
            if (openConnection.State == System.Data.ConnectionState.Open)
            {
                SQLiteCommand command = new SQLiteCommand(sql, openConnection);
                SQLiteDataReader reader = command.ExecuteReader();
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

        public static void GetTiers()
        {
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://psapi.cardknox.com/boarding/v1/GetTierNames");
            httpWebRequest.ContentType = "application/json";
            httpWebRequest.Method = "POST";

            using (StreamWriter streamWriter = new StreamWriter(httpWebRequest.GetRequestStream()))
            {
                string json =
                  @"{
                        ""apiKey"": ""revitacomdevbaa4881ad5524244b867e76344f10645""
                    }";

                streamWriter.Write(json);
            }

            string result = String.Empty;
            HttpWebResponse httpResponse = (HttpWebResponse)httpWebRequest.GetResponse();
            using (StreamReader streamReader = new StreamReader(httpResponse.GetResponseStream()))
            {
                result = streamReader.ReadToEnd();
            }
            Console.WriteLine(result);
        }

        public static string SqlCheckEmployee(string input)
        {
            if (!int.TryParse(input, out int v))
                input = "'" + input + "'";
            string value = null;
            OpenConnection();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();

            sqlite_cmd.CommandText =
                "SELECT FULLNAME " +
                "FROM EMPLOYEES " +
                "WHERE USERNAME = " + input + " " +
                "OR EMPLOYEENUMBER = " + input;

            SQLiteDataReader rdr = sqlite_cmd.ExecuteReader();
            while (rdr.Read())
            {
                value = $"{rdr.GetString(0)}";
            }

            CloseConnection();

            if (value == null)
                return null;
            else
                return value;
        }
    }
}