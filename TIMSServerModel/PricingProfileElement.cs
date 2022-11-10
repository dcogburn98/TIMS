using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    public class PricingProfileElement
    {
        public enum PriceSheets
        {
            Red,
            Yellow,
            Green,
            Pink,
            Blue,
            Cost
        }

        public int profileID;
        public int elementID;
        public int priority;
        public string groupCode;
        public string department;
        public string subDepartment;
        public string productLine;
        public string itemNumber;
        public PriceSheets priceSheet;
        public decimal margin;
        public DateTime? beginDate;
        public DateTime? endDate;

        public bool CalculateItemPrice(Item item)
        {
            if (groupCode != string.Empty)
                if (item.groupCode != int.Parse(groupCode))
                {
                    item.calculatedPrice = item.greenPrice;
                    return false;
                }

            if (department != string.Empty)
                if (item.department.ToUpper() != department.ToUpper())
                {
                    item.calculatedPrice = item.greenPrice;
                    return false;
                }

            if (subDepartment != string.Empty)
                if (item.subDepartment.ToUpper() != subDepartment.ToUpper())
                {
                    item.calculatedPrice = item.greenPrice;
                    return false;
                }

            if (productLine != string.Empty)
                if (item.productLine.ToUpper() != productLine.ToUpper())
                {
                    item.calculatedPrice = item.greenPrice;
                    return false;
                }

            string fixedIN = "";
            foreach (char c in itemNumber)
                if (char.IsLetterOrDigit(c))
                    fixedIN += c.ToString().ToUpper();

            string fixedIIN = "";
            foreach (char c in item.itemNumber)
                if (char.IsLetterOrDigit(c))
                    fixedIIN += c.ToString().ToUpper();

            if (itemNumber != string.Empty)
                if (!fixedIIN.StartsWith(fixedIN))
                {
                    item.calculatedPrice = item.greenPrice;
                    return false;
                }

            if (beginDate != null)
                if (DateTime.Now < beginDate)
                {
                    item.calculatedPrice = item.greenPrice;
                    return false;
                }

            if (endDate != null)
                if (DateTime.Now >= endDate)
                {
                    item.calculatedPrice = item.greenPrice;
                    return false;
                }

            switch (priceSheet)
            {
                case PriceSheets.Red:
                    item.calculatedPrice = item.redPrice / (1 - (margin / 100));
                    break;
                case PriceSheets.Yellow:
                    item.calculatedPrice = item.yellowPrice / (1 - (margin / 100));
                    break;
                case PriceSheets.Green:
                    item.calculatedPrice = item.greenPrice / (1 - (margin / 100));
                    break;
                case PriceSheets.Pink:
                    item.calculatedPrice = item.pinkPrice / (1 - (margin / 100));
                    break;
                case PriceSheets.Blue:
                    item.calculatedPrice = item.bluePrice / (1 - (margin / 100));
                    break;
                case PriceSheets.Cost:
                    item.calculatedPrice = item.replacementCost / (1 - (margin / 100));
                    break;
            }

            return true;
        }
    }
}
