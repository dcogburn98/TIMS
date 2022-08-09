using System;
using System.Drawing;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.Planogram.Shelving
{
    class PegboardHook
    {
        public Point pegLocation;
        public Item hangingItem;

        public enum HookType
        {
            PlainHook,
            LoopHook,
            PlainHookScanTag,
            LoopHookScanTag
        }
        public HookType hookType;

        public PegboardHook(HookType hookType)
        {
            this.hookType = hookType;
        }
    }
}
