using System;
using System.Collections.Generic;
using System.Linq;
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
            printer?.Write(e.Initialize());
            printer?.Write(e.Enable());
            List<byte[]> ReceiptPreamble = new List<byte[]>() {
                e.CenterAlign(),
                e.PrintLine(),
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
                e.PrintLine("Order: " + inv.invoiceNumber + " | Date: " + inv.invoiceCreationTime.ToString("MM/dd/yyyy h:mm tt")),
                e.PrintLine(),
                e.PrintLine()
            };
            List<byte[]> ReceiptBody = new List<byte[]>() {
                e.SetStyles(PrintStyle.FontB),
                e.PrintLine("1   TRITON LOW-NOISE IN-LINE MICROPHONE PREAMP"),
                e.PrintLine("    TRFETHEAD/FETHEAD                        89.95         89.95"),
                e.PrintLine("----------------------------------------------------------------"),
                e.RightAlign()
            };
            List<byte[]> ReceiptTail = new List<byte[]>() {
                e.PrintLine("SUBTOTAL         " + inv.subtotal.ToString("C")),
                e.PrintLine("Total Order:         " + inv.taxAmount.ToString("C") + " @ " + inv.taxRate.ToString("P")),
                e.PrintLine("Total Payment:         89.95"),
                e.PrintLine(),
                e.LeftAlign(),
                e.SetStyles(PrintStyle.Bold | PrintStyle.FontB),
                e.PrintLine("SOLD TO:                        SHIP TO:"),
                e.SetStyles(PrintStyle.FontB),
                e.PrintLine("  LUKE PAIREEPINART               LUKE PAIREEPINART"),
                e.PrintLine("  123 FAKE ST.                    123 FAKE ST."),
                e.PrintLine("  DECATUR, IL 12345               DECATUR, IL 12345"),
                e.PrintLine("  (123)456-7890                   (123)456-7890"),
                e.PrintLine("  CUST: 87654321"),
                e.PrintLine(),
                e.PrintLine()
                };
            printer?.Write(ReceiptPreamble.ToArray());
            printer?.Write(ReceiptBody.ToArray());
            printer?.Write(ReceiptTail.ToArray());
            printer?.Write(e.FeedLines(2));
            printer?.Write(e.FullCut());
        }
    }
}
