using System;

namespace StringSplit
{
    internal class Program
    {
        static void Main(string[] args)
        {
            string splitTest = 
@"<html>
    <head>
    </head>
    <body>
        <p>Item $Number</p>
        <p>$$ItemNumber$$</P>
    </body>
</html>";
            string[] split = splitTest.Split('$');
            int stringCount = split.Length;
        }
    }
}