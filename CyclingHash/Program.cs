using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Security.Cryptography;

namespace CyclingHash
{
    class Program
    {
        static byte privateKey = 17;
        static SHA256 hasher = SHA256.Create();

        static void Main(string[] args)
        {
            byte[] serverHash = new byte[] { 94, 136, 72, 152, 218, 40, 4, 113, 81, 208, 229, 111, 141, 198, 41, 39, 115, 96, 61, 13, 106, 171, 189, 214, 42, 17, 239, 114, 29, 21, 66, 216 };
            
            byte[] clientHash = hasher.ComputeHash(Encoding.ASCII.GetBytes(Console.ReadLine()));

            if (Enumerable.SequenceEqual(serverHash, clientHash))
            {
                Console.WriteLine("Correct password");
                foreach (byte b in serverHash)
                    Console.Write(b + " ");
                Console.WriteLine();
                foreach (byte b in clientHash)
                    Console.Write(b + " ");
            }
            else
            {
                Console.WriteLine("Incorrect password");
                foreach (byte b in serverHash)
                    Console.Write(b + " ");
                Console.WriteLine();
                foreach (byte b in clientHash)
                    Console.Write(b + " ");
            }

            while (true)
            {
                Console.ReadLine();

                serverHash = NextHash(serverHash);
                clientHash = NextHash(clientHash);

                if (Enumerable.SequenceEqual(serverHash, clientHash))
                {
                    Console.WriteLine("Correct password");
                    foreach (byte b in serverHash)
                        Console.Write(b + " ");
                    Console.WriteLine();
                    foreach (byte b in clientHash)
                        Console.Write(b + " ");
                }
                else
                {
                    Console.WriteLine("Incorrect password");
                    foreach (byte b in serverHash)
                        Console.Write(b + " ");
                    Console.WriteLine();
                    foreach (byte b in clientHash)
                        Console.Write(b + " ");
                }
            }
        }

        static byte[] NextHash(byte[] currentHash)
        {
            for (int i = 0; i != currentHash.Length - 1; i++)
            {
                if (currentHash[i] + privateKey > 255)
                    currentHash[i] = 0;
                else
                    currentHash[i] += privateKey;
            }
            return hasher.ComputeHash(currentHash);
        }
    }
}
