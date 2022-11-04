using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    public class PricingProfile
    {
        public List<PricingProfileElement> Elements = new List<PricingProfileElement>();
        public string ProfileName;
        public int ProfileID;

        public bool CalculateItemPrice(Item item)
        {
            foreach (PricingProfileElement el in Elements)
            {
                if (el.CalculateItemPrice(item))
                {
                    return true;
                }
            }
            return false;
        }
    }
}
