using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.WooCommerce.Orders
{
    public class Order
    {
        public int id;
        public int parentID;
        public string number;
        public string orderKey;
        public string createdVia;
        public string version;
        public string status;
        public string currency;

        public DateTime dateCreated;
        public DateTime dateCreatedGMT;
        public DateTime dateModified;
        public DateTime dateModifiedGMT;

        public decimal discountTotal;
        public decimal discountTax;
        public decimal shippingTotal;
        public decimal shippingTax;
        public decimal cartTax;
        public decimal total;
        public decimal totalTax;
        public bool pricesIncludeTax;

        public int customerID;
        public IPAddress customerIPAddress;
        public string customerUserAgent;
        public string customerNote;
        public BillingAddress billing;
        public ShippingAddress shipping;
        public string paymentMethod;
        public string paymentMethodTitle;
        public string transactionID;

        public DateTime datePaid;
        public DateTime datePaidGMT;
        public DateTime dateCompleted;
        public DateTime dateCompletedGMT;

        public string cartHash;
        public List<Metadata> metadata;
        public List<LineItem> lineItems;
        public List<TaxLine> taxLines;
        public List<ShippingLine> shippingLines;
        public List<FeeLine> feeLines;
        public List<CouponLine> couponLines;
        public List<Refund> refunds;
        public bool setPaid;
    }
}
