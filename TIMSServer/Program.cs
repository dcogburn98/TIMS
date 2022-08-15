using System;
using System.Data.SQLite;
using System.ServiceModel;
using Microsoft.Web.Administration;

namespace TIMSServer
{
    class Program
    {
        public static int alertLevel = 3;

        /*
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
            sqlite_conn = new SQLiteConnection("Data Source=database.db; Version = 3; New = True; Pooling = true; Max Pool Size = 100; Compress = True; ");
            OpenConnection();
            CloseConnection();

            using (ServiceHost host = new ServiceHost(typeof(TIMSServiceModel)))
            {
                host.Open();

                ServerManager server = new ServerManager();
                Site site = server.Sites.Add("TIMSServerManager", "TIMSServerManager", 8080);
                if (site != null)
                {
                    //stop the site
                    site.Stop();
                    //start the site
                    site.Start();
                }

                Console.WriteLine("Server is open for connections.");
                Console.WriteLine("Press a key to close.");
                Console.ReadKey();
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