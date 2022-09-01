using System;
using System.Collections.Generic;
using System.Data.SQLite;

namespace TIMSServerLegacy
{
    public class Database
    {

        public static Queue<string> ModifyCommandQueue = new Queue<string>();
        public static Queue<string> ReadCommandQueue = new Queue<string>();
        public static Queue<Response> ResponseQueue = new Queue<Response>();
        public static int tickSpeed = 100;

        public static SQLiteConnection sqlite_conn;

        public static void InitializeDatabase()
        {
            sqlite_conn = new SQLiteConnection("Data Source=database.db; Version = 3; New = True; Compress = True; ");
            System.Threading.Thread queueMaintainer = new System.Threading.Thread(new System.Threading.ParameterizedThreadStart(MainDatabaseLoop));
            queueMaintainer.Start();
        }

        public static void MainDatabaseLoop(object o)
        {
            while (true)
            {
                //Console.Beep(1200, 250);
                System.Threading.Thread.Sleep(tickSpeed);
                if (ModifyCommandQueue.Count > 0)
                    ExecuteNextCommand();
                if (ReadCommandQueue.Count > 0)
                {
                    Response r = ExecuteNextReadCommand();
                    if (r == null)
                        continue;
                    else
                        ResponseQueue.Enqueue(r);
                }
                if (ResponseQueue.Count > 0)
                    continue; //Send responses to appropriate terminals
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

        public static void QueueCommand(string Command)
        {
            if (Command.Split(' ').Length < 2)
                ModifyCommandQueue.Enqueue(Command);
            else if (Command.Split(' ')[1].ToLower() == "select")
                ReadCommandQueue.Enqueue(Command);
            else
                ModifyCommandQueue.Enqueue(Command);
        }

        public static void ExecuteNextCommand()
        {
            OpenConnection();
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            string[] com_split = ModifyCommandQueue.Dequeue().Split(new char[] { ' ' }, 2);
            if (com_split.Length < 2)
            {
                Console.WriteLine("Unknown input from sender: {0}", com_split.ToString());
                ResponseQueue.Enqueue(new Response("UnknownSender", new List<List<string>>() { new List<string>() { "Failure" } }));
                CloseConnection();
                return;
            }
            sqlite_cmd.CommandText = com_split[1];

            try
            {
                sqlite_cmd.ExecuteNonQuery();
                ResponseQueue.Enqueue(new Response(com_split[0], new List<List<string>>() { new List<string>() { "Success" } }));
            }
            catch (SQLiteException e)
            {
                ResponseQueue.Enqueue(new Response(com_split[0], new List<List<string>>() { new List<string>() { "Failure" } }));
                Console.WriteLine("SQL Error: " + e.Message);
            }
            CloseConnection();
        }

        public static Response ExecuteNextReadCommand()
        {
            if (ReadCommandQueue.Count < 1)
                return null;

            List<List<string>> data;

            OpenConnection();
            SQLiteDataReader sqlite_datareader;
            SQLiteCommand sqlite_cmd;
            sqlite_cmd = sqlite_conn.CreateCommand();
            string[] com_split = ReadCommandQueue.Dequeue().Split(new char[] { ' ' }, 2);
            sqlite_cmd.CommandText = com_split[1];

            try
            {
                sqlite_datareader = sqlite_cmd.ExecuteReader();
            }
            catch (SQLiteException e)
            {
                Console.WriteLine("SQL Error: " + e.Message);
                CloseConnection();
                ResponseQueue.Enqueue(new Response(com_split[0], new List<List<string>> { new List<string> { e.Message } }));
                return null;
            }

            data = new List<List<string>>();
            while (sqlite_datareader.Read())
            {
                List<string> temp = new List<string>();
                for (int i = 0; i != sqlite_datareader.FieldCount; i++)
                {
                    temp.Add(sqlite_datareader.GetValue(i).ToString());
                }
                data.Add(temp);
            }

            CloseConnection();
            return new Response(com_split[0], data);
        }
    
        
    }
}
