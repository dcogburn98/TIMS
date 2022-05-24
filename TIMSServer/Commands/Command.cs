using System.Collections.Generic;

namespace TIMSServer.Commands
{
    public abstract class Command
    {
        public string name;
        public string[] aliases;
        public string description;
        public string basicHelp;
        public string usage;

        public abstract void callCommand(string[] args);
    }


    public class Commands
    {
        public static List<Command> availableCommands = new List<Command>();

        public static void InitCommands()
        {
            availableCommands.Add(new Help());
        }
    }
}
