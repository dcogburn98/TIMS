using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSLauncher
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool serverOnly = false;
            bool clientOnly = false;
            if (args.Length > 0)
            {
                if (args[0].ToLower() == "server")
                    serverOnly = true;
                else if (args[0].ToLower() == "client")
                    clientOnly = true;
            }

            Console.WriteLine("Thank you for choosing TIMS!");
            for (int i = 0; i != 6; i++)
            {
                switch (i % 4)
                {
                    case 0:
                        Console.Clear();
                        Console.Write("Initializing server.");
                        break;
                    case 1:
                        Console.Write(".");
                        break;
                    case 2:
                        Console.Write(".");
                        break;
                    case 3:
                        Console.Write(".");
                        break;
                }
                System.Threading.Thread.Sleep(500);
            }
            try
            {
                Console.WriteLine("Launching server");
                if (!clientOnly)
                    Process.Start("TIMSServer.exe");

                try
                {
                    Console.WriteLine("Launching client");
                    if (!serverOnly)
                        Process.Start("TIMS.exe");

                }
                catch (Win32Exception)
                {
                    Console.ForegroundColor = ConsoleColor.Red;
                    Console.WriteLine("The application file for TIMS client was not found. Please make sure the launcher program is contained in the same directory as the TIMS installation.");
                    Console.ForegroundColor = ConsoleColor.White;
                    Console.Write("Launcher directory: ");
                    Console.ForegroundColor = ConsoleColor.Yellow;
                    Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
                    Console.WriteLine("For help, please consult the TIMS user manual or contact Revitacom support at revitacom.com.");
                    while (true) { }
                }
            }
            catch (Win32Exception)
            {
                Console.ForegroundColor = ConsoleColor.Red;
                Console.WriteLine("The application file for TIMS server was not found. Please make sure the launcher program is contained in the same directory as the TIMS installation.");
                Console.ForegroundColor = ConsoleColor.White;
                Console.Write("Launcher directory: ");
                Console.ForegroundColor = ConsoleColor.Yellow;
                Console.WriteLine(AppDomain.CurrentDomain.BaseDirectory);
                Console.WriteLine("For help, please consult the TIMS user manual or contact Revitacom support at revitacom.com.");
                while (true) { }
            }

            Console.WriteLine("TIMS services successfully started! Please wait for loading to complete. Thanks for using TIMS!");
            System.Threading.Thread.Sleep(5000);
        }
    }
}
