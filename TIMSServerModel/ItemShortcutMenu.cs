using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    public class ItemShortcutMenu
    {
        public string menuName;
        public List<Item> menuItems;
        public string parentMenu;

        public ItemShortcutMenu()
        {
            menuItems = new List<Item>();
        }
    }
}
