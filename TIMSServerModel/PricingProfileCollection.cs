using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    public class PricingProfileCollection
    {
        public List<PricingProfile> Profiles;
        public PricingProfileElement.PriceSheets defaultPriceSheet;

        public PricingProfileCollection()
        {
            Profiles = new List<PricingProfile>();
        }

        public decimal CalculatePrice(Item item)
        {
            foreach (PricingProfile profile in Profiles)
            {
                if (profile.CalculateItemPrice(item))
                {
                    return item.calculatedPrice;
                }
            }

            switch (defaultPriceSheet)
            {
                case PricingProfileElement.PriceSheets.Red:
                    return item.redPrice;
                case PricingProfileElement.PriceSheets.Yellow:
                    return item.yellowPrice;
                case PricingProfileElement.PriceSheets.Green:
                    return item.greenPrice;
                case PricingProfileElement.PriceSheets.Pink:
                    return item.pinkPrice;
                case PricingProfileElement.PriceSheets.Blue:
                    return item.bluePrice;
                case PricingProfileElement.PriceSheets.Cost:
                    return item.replacementCost;
            }

            return 0;
        }

        public override string ToString()
        {
            string collection = string.Empty;
            foreach (PricingProfile profile in Profiles)
            {
                collection += profile.ProfileID.ToString() + ",";
            }
            collection = collection.Trim(',');
            return collection;
        }
    }
}
