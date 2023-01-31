using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

using Newtonsoft.Json.Linq;

using TIMSServerModel;

using WooCommerceNET;
using WooCommerceNET.WooCommerce.v2;
//using WooCommerceNET.WooCommerce.v3;
using WooCommerceNET.WooCommerce.v3.Extension;
using WooCommerce.NET.WordPress.v2;

namespace TIMSServer.WebServer.WooCommerce
{
    public class WooCommerceHandler
    {
        public static string WCKey = "ck_c7d09d95ae1a49362dbfcfc98f305dc7b9d6fc10";
        public static string WCSecret = "cs_63a9a37bf5845070569ec374a2edb692eb130733";
        public static RestAPI WCRest = new RestAPI("https://www.revitacom.com/wp-json/wc/v3/", WCKey, WCSecret);
        public static WCObject WC = new WCObject(WCRest);

        public static RestAPI WPRest = new RestAPI(
            "https://revitacom.com/wp-json/jwt-auth/v1/token/", 
            "revitacominc@gmail.com", "P!GFfdpK89");
        public static WPObject WP = new WPObject(WPRest);
        public static TIMSServiceModel instance = new TIMSServiceModel();

        public static async Task MatchProducts()
        {
            Console.WriteLine("Matching items in WooCommerce webshop to items in the TIMS database. Temporarily locking the database.");
            List<string> suppliers = instance.RetrieveSuppliers();
            List<Item> items = new List<Item>();
            foreach (string supplier in suppliers)
            {
                foreach (Item item in instance.RetrieveItemsFromSupplier(supplier, TIMSServiceModel.BypassKey).Data)
                {
                    items.Add(item);
                }
            }
            Console.WriteLine("Unlocking database. Continuing item matching in background.");

            List<Product> WCProducts = new List<Product>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("per_page", "100");
            int pageNumber = 1;
            dic.Add("page", pageNumber.ToString());
            bool endWhile = false;
            while (!endWhile)
            {
                List<Product> productsTemp = await WC.Product.GetAll(dic);
                if (productsTemp.Count > 0)
                {
                    WCProducts.AddRange(productsTemp);
                    pageNumber++;
                    dic["page"] = pageNumber.ToString();
                }
                else
                {
                    endWhile = true;
                }
            }

            List<ProductCategory> WCCategories = new List<ProductCategory>();
            pageNumber = 1;
            dic["page"] = pageNumber.ToString();
            endWhile = false;
            while (!endWhile)
            {
                List<ProductCategory> catTemp = await WC.Category.GetAll(dic);
                if (catTemp.Count > 0)
                {
                    WCCategories.AddRange(catTemp);
                    pageNumber++;
                    dic["page"] = pageNumber.ToString();
                }
                else
                {
                    endWhile = true;
                }
            }

            int catIndex = 0;
            Console.WriteLine("Synchronizing Departments");
            List<string> departments = instance.RetrieveProductDepartments(TIMSServiceModel.BypassKey).Data;
            
            foreach (string department in departments)
            {
                ProductCategory newCat;
                try
                {
                    newCat = await WC.Category.Add(new ProductCategory() { name = department });
                    WCCategories.Add(newCat);
                }
                catch
                {
                    newCat = WCCategories.First(el => el.name == department);
                }
                List<string> subdepartments = instance.RetrieveProductSubdepartments(department, TIMSServiceModel.BypassKey).Data;
                int subIndex = 0;
                foreach (string subdepartment in subdepartments)
                {
                    Console.SetCursorPosition(0, Console.CursorTop);
                    Console.Write((int)(((decimal)catIndex / (decimal)departments.Count) * 100) + "% Complete | Subdepartments: " + (int)(((decimal)subIndex / (decimal)subdepartments.Count) * 100) + "% from " + department);
                    try
                    {
                        WCCategories.Add(await WC.Category.Add(new ProductCategory() { name = subdepartment, parent = newCat.id }));
                    }
                    catch (System.Net.WebException ex)
                    {
                        JObject json = JObject.Parse(ex.Message);
                        //Console.WriteLine(json.ToString(Newtonsoft.Json.Formatting.Indented));
                        int i = 0;
                    }
                    subIndex++;
                }
                catIndex++;
            }
            Console.WriteLine("Department synchronization complete. Starting item synchronization.");

            int index = 0;
            foreach (Item item in items)
            {
                Console.SetCursorPosition(0, Console.CursorTop);
                Console.Write($"Item {index}/{items.Count}");
                if (item.itemNumber.ToLower() == "xxx")
                    continue;

                Product p = new Product();
                p.sku = item.productLine.ToUpper() + item.itemNumber.ToUpper();
                p.description = item.longDescription;
                p.price = item.greenPrice;
                p.stock_quantity = (int)item.onHandQty;
                p.manage_stock = true;
                p.name = item.itemName;
                p.short_description = item.itemName;
                ProductCategory c = WCCategories.First(el => el.name.ToLower() == "default" && el.parent != 0);
                p.categories = new List<ProductCategoryLine>() { new ProductCategoryLine() { 
                    id = WCCategories.FirstOrDefault(el => el.name == item.subDepartment.Replace("&","&amp;"))?.id ?? c.id } };
                p.images = item.itemPicturePaths.Count > 0 ? new List<ProductImage>() { 
                    new ProductImage() { src = item.itemPicturePaths[0] } } : null;

                try
                {
                    WCProducts.Add(await WC.Product.Add(p));
                }
                catch (System.Net.WebException ex)
                {
                    JObject json = JObject.Parse(ex.Message);
                    //Console.WriteLine(json.ToString(Newtonsoft.Json.Formatting.Indented));
                    if (((string)json["message"]).Contains("duplicated SKU"))
                    {
                        try
                        {
                            await WC.Product.Update(ulong.Parse((string)json["data"]["resource_id"]), p);
                            //Console.WriteLine("Updated.");
                        }
                        catch
                        {
                            //Console.WriteLine("Skipped updating.");
                        }
                    }
                }
                
                index++;
            }
        }
    
        public static async Task AddProduct(Item item)
        {
            List<ProductCategory> WCCategories = new List<ProductCategory>();
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("per_page", "100");
            int pageNumber = 1;
            dic.Add("page", pageNumber.ToString());
            bool endWhile = false;
            while (!endWhile)
            {
                List<ProductCategory> catTemp = await WC.Category.GetAll(dic);
                if (catTemp.Count > 0)
                {
                    WCCategories.AddRange(catTemp);
                    pageNumber++;
                    dic["page"] = pageNumber.ToString();
                }
                else
                {
                    endWhile = true;
                }
            }

            Product p = new Product();
            p.sku = item.productLine.ToUpper() + item.itemNumber.ToUpper();
            p.description = item.longDescription;
            p.price = item.greenPrice;
            p.stock_quantity = (int)item.onHandQty;
            p.manage_stock = true;
            p.name = item.itemName;
            p.short_description = item.itemName;
            ProductCategory c = WCCategories.First(el => el.name.ToLower() == "default" && el.parent != 0);
            p.categories = new List<ProductCategoryLine>() { new ProductCategoryLine() { 
                id = WCCategories.FirstOrDefault(el => el.name == item.subDepartment.Replace("&", "&amp;"))?.id ?? c.id } };
            p.images = item.itemPicturePaths.Count > 0 ? new List<ProductImage>() { 
                new ProductImage() { src = item.itemPicturePaths[0] } } : null;

            try
            {
                await WC.Product.Add(p);
            }
            catch (System.Net.WebException ex)
            {
            }
        }
    
        public static async Task GetMediaImages()
        {
            
        }

        public static void ProcessOrder(Order order)
        {
            Invoice inv = new Invoice();
            inv.employee = instance.RetrieveEmployee("0");
            inv.attentionLine = "Online Order";
            inv.PONumber = order.id.ToString();

            foreach (OrderLineItem lineItem in order.line_items)
            {
                InvoiceItem item = new InvoiceItem(instance.RetrieveItem(lineItem.sku.Substring(0, 3), lineItem.sku.Substring(3)));
                item.price = (decimal)lineItem.price;
                item.quantity = (decimal)lineItem.quantity;
                inv.items.Add(item);
                inv.subtotal += Math.Round(item.price * item.quantity);
                inv.taxableTotal += Math.Round(item.price * item.quantity);
                inv.cost += Math.Round(item.cost * item.quantity);
            }
            inv.taxAmount = (decimal)order.total_tax;
            inv.total = (decimal)order.total;

            inv.payments.Add(new Payment() { paymentAmount = inv.total, paymentType = Payment.PaymentTypes.PaymentCard });

            foreach (InvoiceItem invItem in inv.items)
            {
                Item newItem = instance.RetrieveItem(invItem.itemNumber, invItem.productLine);
                newItem.onHandQty -= invItem.quantity;
                newItem.dateLastSale = DateTime.Now;
                newItem.lastSalePrice = invItem.price;
                invItem.cost = newItem.replacementCost;
                inv.cost += invItem.cost * invItem.quantity;
                instance.UpdateItem(newItem);
            }

            inv.finalized = true;
            inv.savedInvoice = false;
            inv.invoiceCreationTime = (DateTime)order.date_created;
            inv.invoiceFinalizedTime = DateTime.Now;
            inv.invoiceNumber = inv.invoiceNumber == 0 ? instance.RetrieveNextInvoiceNumber() : inv.invoiceNumber;
            inv.totalPayments = inv.total;
            inv.profit = inv.subtotal - inv.cost;
            instance.SaveInvoice(inv);

            #region Accounting Transactions

            List<Transaction> salesTransactions = new List<Transaction>();
            List<Transaction> salesTaxTransactions = new List<Transaction>();
            Transaction inventoryTransaction = null;

            if (inv.total != 0)
                foreach (Payment p in inv.payments)
                {
                    if (p.paymentType == Payment.PaymentTypes.PaymentCard)
                    {
                        if (p.paymentAmount > 0)
                        {
                            salesTransactions.Add(new Transaction(2, 10, p.paymentAmount - Math.Round((p.paymentAmount / inv.total) * inv.taxAmount, 2)) //2 - Checking Account  5 - Cash Sales Account
                            {
                                transactionID = instance.RetrieveNextTransactionNumber(),
                                referenceNumber = inv.invoiceNumber,
                                memo = "Subtotal transaction for card sale"
                            });
                            salesTaxTransactions.Add(new Transaction(2, 7, Math.Round((p.paymentAmount / inv.total) * inv.taxAmount, 2)) //2 - Checking Account  7 - Sales Tax Payable Account
                            {
                                transactionID = instance.RetrieveNextTransactionNumber(),
                                referenceNumber = inv.invoiceNumber,
                                memo = "Tax transaction for card sale"
                            });
                        }
                        else
                        {
                            salesTransactions.Add(new Transaction(10, 2, Math.Abs(p.paymentAmount) - Math.Abs(Math.Round((p.paymentAmount / inv.total) * inv.taxAmount, 2))) //2 - Checking Account  5 - Cash Sales Account
                            {
                                transactionID = instance.RetrieveNextTransactionNumber(),
                                referenceNumber = inv.invoiceNumber,
                                memo = "Refund subtotal for card sale"
                            });
                            salesTaxTransactions.Add(new Transaction(7, 2, Math.Abs(Math.Round((p.paymentAmount / inv.total) * inv.taxAmount, 2))) //2 - Checking Account  7 - Sales Tax Payable Account
                            {
                                transactionID = instance.RetrieveNextTransactionNumber(),
                                referenceNumber = inv.invoiceNumber,
                                memo = "Refund tax for card sale"
                            });
                        }
                    }
                    else if (p.paymentType == Payment.PaymentTypes.Charge)
                    {
                        if (p.paymentAmount > 0)
                        {
                            salesTransactions.Add(new Transaction(11, 6, p.paymentAmount - Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2)) //11 - A/R Account  6 - Credit Sales Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Subtotal transaction for charge sale"
                            });
                            salesTaxTransactions.Add(new Transaction(11, 7, Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2)) //11 - A/R Account  7 - Sales Tax Payable Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Tax transaction for charge sale"
                            });
                        }
                        else
                        {
                            salesTransactions.Add(new Transaction(6, 11, Math.Abs(p.paymentAmount) - Math.Abs(Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2))) //11 - A/R Account  6 - Credit Sales Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Credit subtotal for charge sale"
                            });
                            salesTaxTransactions.Add(new Transaction(7, 11, Math.Abs(Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2))) //11 - A/R Account  7 - Sales Tax Payable Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Tax credit for charge sale"
                            });
                        }
                    }
                    else
                    {
                        if (p.paymentAmount > 0)
                        {
                            salesTransactions.Add(new Transaction(14, 5, p.paymentAmount - Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2)) //14 - Cash Drawer Account  5 - Cash Sales Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Subtotal transaction for cash sale"
                            });
                            salesTaxTransactions.Add(new Transaction(14, 7, Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2)) //14 - Cash Drawer Account  7 - Sales Tax Payable Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Tax transaction for cash sale"
                            });
                        }
                        else
                        {
                            salesTransactions.Add(new Transaction(5, 14, Math.Abs(p.paymentAmount) - Math.Abs(Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2))) //14 - Cash Drawer Account  5 - Cash Sales Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Refund subtotal for cash sale"
                            });
                            salesTaxTransactions.Add(new Transaction(7, 14, Math.Abs(Math.Round((p.paymentAmount / invoice.total) * invoice.taxAmount, 2))) //14 - Cash Drawer Account  7 - Sales Tax Payable Account
                            {
                                transactionID = Communication.RetrieveNextTransactionNumber(),
                                referenceNumber = invoice.invoiceNumber,
                                memo = "Refund tax for cash sale"
                            });
                        }
                    }
                }

            if (invoice.cost > 0)
            {
                inventoryTransaction = new Transaction(8, 1, invoice.cost)
                {
                    transactionID = Communication.RetrieveNextTransactionNumber(),
                    referenceNumber = invoice.invoiceNumber,
                    memo = "Inventory transaction"
                };
            }
            else
            {
                inventoryTransaction = new Transaction(1, 8, Math.Abs(invoice.cost))
                {
                    transactionID = Communication.RetrieveNextTransactionNumber(),
                    referenceNumber = invoice.invoiceNumber,
                    memo = "Inventory return transaction"
                };
            }

            if (((salesTaxTransactions.Count < 1 || salesTransactions.Count < 1) && invoice.total != 0) ||
                 (inventoryTransaction == null && invoice.cost != 0))
                MessageBox.Show("There was an error completing transactions for this sale in accounting. Please make sure to record those transactions in the ledger in order to keep records accurate.");
            else
            {
                if (invoice.total != 0)
                    foreach (Transaction t in salesTransactions)
                        Communication.SaveTransaction(t);

                if (invoice.total != 0)
                    foreach (Transaction t in salesTaxTransactions)
                        Communication.SaveTransaction(t);

                if (invoice.cost != 0)
                    Communication.SaveTransaction(inventoryTransaction);
            }

            #endregion
        }
    }
}
