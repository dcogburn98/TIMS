using System;
using System.Collections.Generic;
using System.Net;
using System.Net.Sockets;
using System.Text;
using System.Threading;

namespace TIMSServer
{
    public class Server
    {
        public static IPAddress localAddr = IPAddress.Loopback;
        public static int port = 13000;
        public static bool serverRunning;
        public static List<Thread> comThreads = new List<Thread>();
        public static List<Originator> terminals = new List<Originator>();

        public static void ServerLoop(object o)
        {
            TcpListener server = null;
            try
            {
                server = new TcpListener(localAddr, port);
                server.Start();
                serverRunning = true;
                byte[] bytes = new byte[65536];
                string data = null;

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    IPAddress IP = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                    if (terminals.Find(el => IPAddress.Equals(el.IP, IP) == true) == null)
                    {
                        Console.WriteLine("\nConnection request received from " + IP.ToString());
                        Console.WriteLine("Waiting for handshake data from terminal...");
                    }
                    else
                    {
                        continue;
                    }
                    Program.ReturnToInput();

                    data = null;
                    NetworkStream stream = client.GetStream();
                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.ASCII.GetString(bytes, 0, i);

                        string response = string.Empty;
                        string[] rcvd = data.Split('\0');
                        string termID = rcvd[0];


                        if (rcvd[1].ToLower() == "login")
                        {
                            //Console.WriteLine("Connection attempt from " + IP.ToString());
                            Database.QueueCommand(termID + " select employee_no from employees where username = \"" + rcvd[2] + "\"");
                            while (response == string.Empty)
                            {
                                if (Database.ResponseQueue.Count < 1)
                                    continue;
                                if (Database.ResponseQueue.Peek().responseText.Split('\0')[0] != termID)
                                    continue;
                                else
                                    response = Database.ResponseQueue.Dequeue().responseText;
                            }
                            Program.ReturnToInput();
                        }
                        else
                        {
                            response = "Invalid command format.";
                        }


                        byte[] msg = Encoding.ASCII.GetBytes(response);
                        stream.Write(msg, 0, msg.Length);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Server failure... Please save the following exception information for troubleshooting purposes.");
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                Console.WriteLine("Server failure... Server is haulted and server thread has aborted.");
                Console.WriteLine("Please attempt to restart the server or contact your network administrator.");
                server.Stop();
                Program.ReturnToInput();
            }
        }

        public static void TerminalListener(object o)
        {
            TcpListener server = null;
            try
            {
                server = new TcpListener(localAddr, port);
                server.Start();
                serverRunning = true;
                byte[] bytes = new byte[65536];
                string data = null;

                while (true)
                {
                    TcpClient client = server.AcceptTcpClient();
                    IPAddress IP = ((IPEndPoint)client.Client.RemoteEndPoint).Address;
                    if (terminals.Find(el => IPAddress.Equals(el.IP, IP) == true) == null)
                    {
                        Console.WriteLine("\nConnection request received from " + IP.ToString());
                        Console.WriteLine("Waiting for handshake data from terminal...");
                    }
                    else
                    {
                        continue;
                    }
                    Program.ReturnToInput();

                    data = null;
                    NetworkStream stream = client.GetStream();
                    int i;

                    // Loop to receive all the data sent by the client.
                    while ((i = stream.Read(bytes, 0, bytes.Length)) != 0)
                    {
                        data = Encoding.ASCII.GetString(bytes, 0, i);

                        //Database.QueueCommand(data);
                        //while (Database.ResponseQueue.Count < 1) { }
                        //byte[] msg = Database.ResponseQueue.Dequeue().responseData;
                        string response = string.Empty;
                        string[] rcvd = data.Split(' ');

                        if (rcvd.Length != 4)
                        {
                            Console.WriteLine("Connection refused.");
                            response = "Connection refused.";
                            Program.ReturnToInput();
                        }
                        else
                        {
                            terminals.Add(new Originator(rcvd[0], IP, rcvd[2]));
                            response = "Connection request approved, terminal added to originator threads.";
                        }

                        byte[] msg = Encoding.ASCII.GetBytes(response);
                        stream.Write(msg, 0, msg.Length);
                    }

                    // Shutdown and end connection
                    client.Close();
                }
            }
            catch (SocketException e)
            {
                Console.WriteLine("Server failure... Please save the following exception information for troubleshooting purposes.");
                Console.WriteLine("SocketException: {0}", e);
            }
            finally
            {
                Console.WriteLine("Server failure... Server is haulted and server thread has aborted.");
                Console.WriteLine("Please attempt to restart the server or contact your network administrator.");
                server.Stop();
                Program.ReturnToInput();
            }
        }
    }
}
