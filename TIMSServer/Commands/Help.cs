using System;
using System.Collections.Generic;
using System.Text;

namespace TIMSServer.Commands
{
    class Help : Command
    {
        public Help()
        {
            name = "help";
            aliases = new string[] { "hlp" };
            basicHelp = "Provides helpful information about certain commands or usage of commands.";
            description = "Provides helpful information about certain commands or usage of commands.";
            usage = "help [command]";
        }

        public override void callCommand(string[] args)
        {
            Console.WriteLine("It works!");
        }
    }
}
