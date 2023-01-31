using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel.WooCommerce.Products
{
    public class Product
    {
        public int id;
        public string name;
        public string slug;
        public string permalink;

        public DateTime dateCreated;
        public DateTime dateCreatedGMT;
        public DateTime dateModified;
        public DateTime dateModifiedGMT;

        public string type;
        public string status;
        public bool featured;
        public string catalogVisibility;
        public string description;
        public string shortDescription;
        public string sku;

        public decimal price;
        public decimal regularPrice;
        public decimal salePrice;
        public DateTime dateOnSaleFrom;
        public DateTime dateOnSaleFromGMT;
        public DateTime dateOnSaleTo;
        public DateTime dateOnSaleToGMT;
        public string priceHTML;
        public bool onSale;

        public bool purchasable;
        public int totalSales;
        public bool isVirtual;
        public bool downloadable;
        public List<Download> downloads;
        public int downloadLimit;
        public int downloadExpiry;

        public string externalURL;
        public string buttonText;

        public string taxStatus;
        public string taxClass;

        public bool manageStock;
        public int stockQuantity;
        public string stockStatus;
        public string backorders;
        public bool backordersAllowed;
        public bool backordered;
        public bool soldIndividually;

        public string weight;
        public Dimension dimensions;

        public bool shippingRequired;
        public bool shippingTaxable;
        public string shippingClass;
        public int shippingClassID;

        public bool reviewsAllowed;
        public string averageRating;
        public int ratingCount;

        public List<int> relatedIDs;
        public List<int> upsellIDs;
        public List<int> crossSellIDs;
        public int parentID;

        public string purchaseNote;

        public List<Category> categories;
        public List<Tag> tags;
        public List<Image> images;
        public List<Attribute> attributes;
        public List<DefaultAttribute> defaultAttributes;
        public List<int> variations;
        public List<int> groupedProducts;

        public int menuOrder;
        public List<Metadata> metadata;

    }
}
