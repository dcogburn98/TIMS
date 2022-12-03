using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;
using ESCPOS_NET;
using ESCPOS_NET.Emitters;

using TIMSServerModel;

namespace TIMSServer
{
    internal class ReceiptPrinter
    {
        public List<BasePrinter> printers;

        public void InitializePrinters()
        {

        }

        public static void PrintReceipt(Invoice inv, Device dev, TIMSServiceModel instance)
        {
            string[] storeaddr = instance.RetrieveProperty("Store Physical Address", TIMSServiceModel.BypassKey).Data.Split(',');
            ICommandEmitter e = new EPSON();
            BasePrinter printer = new NetworkPrinter(new NetworkPrinterSettings()
            { ConnectionString = dev.address.Address.ToString() + ":" + dev.address.Port.ToString(), PrinterName = dev.Nickname });
            
            List<byte[]> ReceiptPreamble = new List<byte[]>() {
                e.CenterAlign(),
                e.PrintLine(inv.invoiceNumber.ToString()),
                e.SetBarcodeHeightInDots(360),
                e.SetBarWidth(BarWidth.Default),
                e.SetBarLabelPosition(BarLabelPrintPosition.None),
                e.PrintBarcode(BarcodeType.CODE39, inv.invoiceNumber.ToString()),
                e.PrintLine(),
                e.PrintLine(instance.RetrieveProperty("Store Name", TIMSServiceModel.BypassKey).Data),
                e.PrintLine(storeaddr[0]),
                e.PrintLine(storeaddr[1] + ", " + storeaddr[2] + " " + storeaddr[3] + ", " + storeaddr[4]),
                e.PrintLine(instance.RetrieveProperty("Store Phone Number", TIMSServiceModel.BypassKey).Data),
                e.SetStyles(PrintStyle.Underline),
                e.PrintLine(instance.RetrieveProperty("Store Website", TIMSServiceModel.BypassKey).Data),
                e.SetStyles(PrintStyle.None),
                e.PrintLine(),
                e.LeftAlign(),
                e.PrintLine("Order: " + inv.invoiceNumber + " | Date: " + inv.invoiceFinalizedTime.ToString("MM/dd/yyyy h:mm tt")),
                e.PrintLine("PO#:   " + inv.PONumber),
                e.PrintLine("Attn:  " + inv.attentionLine),
                e.PrintLine(),
                e.PrintLine(),
                e.PrintLine("QTY---------------DESCRIPTION--------------PRICE"),
            };
            
            List<byte[]> ReceiptBody = new List<byte[]>() { e.SetStyles(PrintStyle.FontB) };
            foreach (InvoiceItem item in inv.items)
            {
                ReceiptBody.Add(e.LeftAlign());
                ReceiptBody.Add(e.PrintLine(item.quantity + "      " + item.itemNumber + " -- " + item.itemName));
                ReceiptBody.Add(e.RightAlign());
                ReceiptBody.Add(e.PrintLine(item.total.ToString("C").Trim('$') + " @ " + item.price.ToString("C").Trim('$') + "/each"));
                ReceiptBody.Add(e.PrintLine("----------------------------------------------------------------"));
            }

            List<byte[]> ReceiptTail = new List<byte[]>() {
                e.RightAlign(),
                e.PrintLine("SUBTOTAL      "),
                e.PrintLine(inv.subtotal.ToString("C").Trim('$')),

                e.PrintLine("TAX " + "(" + inv.taxRate.ToString("P") + ")      "),
                e.PrintLine(Math.Round(inv.taxAmount, 2).ToString("C").Trim('$')),

                e.PrintLine("TOTAL      "),
                e.PrintLine(inv.total.ToString("C").Trim('$')),
                e.PrintLine(),
                e.PrintLine("----------------------------PAYMENTS----------------------------"),
                e.PrintLine(),
            };

            foreach (Payment pay in inv.payments)
            {
                ReceiptTail.Add(e.CenterAlign());
                ReceiptTail.Add(e.PrintLine("PAYMENT TYPE"));
                ReceiptTail.Add(e.RightAlign());
                ReceiptTail.Add(e.PrintLine(Enum.GetName(typeof(Payment.PaymentTypes), pay.paymentType)));
                ReceiptTail.Add(e.CenterAlign());
                ReceiptTail.Add(e.PrintLine("PAYMENT AMOUNT"));
                ReceiptTail.Add(e.RightAlign());
                ReceiptTail.Add(e.PrintLine(pay.paymentAmount.ToString("C").Trim('$')));
                if (pay.paymentType == Payment.PaymentTypes.PaymentCard && pay.cardResponse != null)
                {
                    ReceiptTail.Add(e.CenterAlign());
                    ReceiptTail.Add(e.PrintLine("CARD DETAILS"));
                    ReceiptTail.Add(e.LeftAlign());
                    ReceiptTail.Add(e.PrintLine("MASKED CARD #: " + pay.cardResponse?.MaskedCardNumber));
                    ReceiptTail.Add(e.PrintLine("NAME ON CARD:  " + pay.cardResponse?.FirstName + " " + pay.cardResponse.LastName));
                    ReceiptTail.Add(e.PrintLine("APP LABEL:     " + pay.cardResponse?.AppLabel));
                    ReceiptTail.Add(e.PrintLine("TVR:           " + pay.cardResponse?.TVR));
                    ReceiptTail.Add(e.PrintLine("AID:           " + pay.cardResponse?.AID));
                    ReceiptTail.Add(e.PrintLine("TSI:           " + pay.cardResponse?.TSI));
                    if (pay.cardResponse?.Signature != null)
                    {
                        ReceiptTail.Add(e.CenterAlign());
                        ReceiptTail.Add(e.PrintImage(Convert.FromBase64String(pay.cardResponse?.Signature), false));
                    }
                }
                ReceiptTail.Add(e.PrintLine("----------------------------------------------------------------"));
            }

            ReceiptTail.AddRange(new byte[][] {
                e.CenterAlign(),
                e.PrintLine("TOTAL PAYMENTS"),
                e.RightAlign(),
                e.PrintLine(inv.totalPayments.ToString()),
                e.PrintLine(),
                e.CenterAlign(),
                e.SetStyles(PrintStyle.Bold | PrintStyle.FontB),
                e.PrintLine("----------------------------------------------------------------"),
                e.PrintLine("CUSTOMER INFO"),
                e.PrintLine("----------------------------------------------------------------"),
                e.SetStyles(PrintStyle.FontB),
                e.LeftAlign(),
                e.PrintLine("CUSTOMER NUMBER: " + inv.customer.customerNumber),
                e.PrintLine(inv.customer.customerName),
                e.PrintLine(inv.customer.billingAddress.Split(',')[0]),
                e.PrintLine(inv.customer.billingAddress.Split(',')[1].Trim() + inv.customer.billingAddress.Split(',')[2].Trim()),
                e.PrintLine(inv.customer.phoneNumber),
                e.PrintLine(),
                e.SetStyles(PrintStyle.None),
                e.CenterAlign(),
                e.PrintLine("THANKS FOR SHOPPING WITH US!"),
                e.LeftAlign(),
                e.Print("TIMS - Total Inventory Management System"),
                e.PrintLine(),
                e.PrintLine()
            });

            printer.Write(e.Initialize());
            printer.Write(e.Enable());
            printer.Write(e.FeedLines(1));
            printer.Write(ReceiptPreamble.ToArray());
            printer.Write(ReceiptBody.ToArray());
            printer.Write(ReceiptTail.ToArray());
            printer.Write(e.PartialCutAfterFeed(3));
        }
    }
}
