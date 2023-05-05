using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using System.Text;
using System.Threading.Tasks;

using ESCPOS_NET;
using ESCPOS_NET.Emitters;

using Microsoft.Extensions.Logging;

using Newtonsoft.Json;

using TIMSServerModel;

namespace TIMSServer
{
    internal class ReceiptPrinter
    {
        public static List<BasePrinter> printers = new List<BasePrinter>();
        public static ICommandEmitter e;
        public static TIMSServiceModel instance;
        public static bool monitor;

        private static bool _hasEnabledStatusMonitoring = false;

        public static void Init(TIMSServiceModel instancee)
        {
            instance = instancee;
#if DEBUG
            var logLevel = Microsoft.Extensions.Logging.LogLevel.Trace;
#else
            var loglevel = Microsoft.Extensions.Logging.LogLevel.Information;
#endif
            var factory = LoggerFactory.Create(b => b.AddConsole().SetMinimumLevel(logLevel));
            var logger = factory.CreateLogger<Program>();
            ESCPOS_NET.Logging.Logger = logger;

            monitor = true;
            e = new EPSON();
            foreach (Device dev in instancee.RetrieveDevices().Where(el => el.Type == Device.DeviceType.ThermalPrinter))
            {
                AddPrinter(dev);
            }
        }
        public static void PrintReceipt(Invoice inv, Device dev, string signature = null)
        {
            Console.WriteLine("Print Receipt called in receipt printer class");
            byte[] signatureImg = null;
            if (signature != null)
            {
                try
                {
                    signatureImg = Convert.FromBase64String(signature);
                }
                catch
                {
                    signatureImg = null;
                }
            }
            BasePrinter printer;
            printer = printers.FirstOrDefault(el => el.PrinterName == dev.Nickname);
            if (printer == default(BasePrinter))
            {
                AddPrinter(dev);
                printer = printers.FirstOrDefault(el => el.PrinterName == dev.Nickname);
            }

            //MemoryStream imgStream = new MemoryStream(instance.RetrieveCompanyLogo());
            //Image img = Image.FromStream(imgStream);
            //decimal ratio = img.Width / img.Height;
            //img = ResizeImage(img, 450, (int)(450 / ratio));
            //imgStream.Position = 0;
            //img.Save(imgStream, ImageFormat.Png);

            System.Threading.Thread.Sleep(500);
            printer.Write(e.Initialize());
            printer.Write(e.Enable());
            //            Console.WriteLine($@"Printer Details:
            //Printer Name: {printer.PrinterName}
            //Printer Online: {printer.Status.IsPrinterOnline}
            //Is In Error: {printer.Status.IsInErrorState}
            //Autocutter Error: {printer.Status.DidAutocutterErrorOccur}
            //Recoverable Autocutter Error: {printer.Status.DidRecoverableNonAutocutterErrorOccur}
            //Cover Open: {printer.Status.IsCoverOpen}
            //Paper Out: {printer.Status.IsPaperOut}
            //Recoverable Error: {printer.Status.DidRecoverableErrorOccur}
            //Cash Drawer Open: {printer.Status.IsCashDrawerOpen}
            //Feed Button Pressed: {printer.Status.IsPaperFeedButtonPushed}
            //Waiting for Online Recovery: {printer.Status.IsWaitingForOnlineRecovery}
            //Paper Low: {printer.Status.IsPaperLow}
            //Paper Currently Feeding: {printer.Status.IsPaperCurrentlyFeeding}");

            string[] storeaddr = instance.RetrieveProperty("Store Physical Address", TIMSServiceModel.BypassKey).Data.Split(',');
            string addressString = "";
            foreach (string addrPart in storeaddr)
            {
                addressString += addrPart + ", ";
            }
            addressString = addressString.Trim().Trim(',');
            List<byte[]> ReceiptPreamble = new List<byte[]>() {
                e.CenterAlign(),
                //e.PrintImage(imgStream.ToArray(), false, true),
                e.PrintLine(inv.invoiceNumber.ToString()),
                e.SetBarcodeHeightInDots(360),
                e.SetBarWidth(BarWidth.Default),
                e.SetBarLabelPosition(BarLabelPrintPosition.None),
                e.PrintBarcode(BarcodeType.CODE39, inv.invoiceNumber.ToString()),
                e.PrintLine(),
                e.PrintLine(instance.RetrieveProperty("Store Name", TIMSServiceModel.BypassKey).Data),
                e.PrintLine(storeaddr[0]),
                e.PrintLine(addressString),
                e.PrintLine(instance.RetrieveProperty("Store Phone Number", TIMSServiceModel.BypassKey).Data),
                e.SetStyles(PrintStyle.Underline),
                e.PrintLine(instance.RetrieveProperty("Store Website", TIMSServiceModel.BypassKey).Data),
                e.SetStyles(PrintStyle.None),
                e.PrintLine(),
                e.LeftAlign(),
                e.PrintLine("Order: " + inv.invoiceNumber + " | Date: " + inv.invoiceFinalizedTime.ToString("MM/dd/yyyy h:mm:ss tt")),
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
                ReceiptBody.Add(e.PrintLine(item.total.ToString("C").Trim('(').Trim(')').Trim('$') + " @ " + item.price.ToString("C").Trim('(').Trim(')').Trim('$') + "/each"));
                ReceiptBody.Add(e.PrintLine("----------------------------------------------------------------"));
            }

            List<byte[]> ReceiptTail = new List<byte[]>() {
                e.RightAlign(),
                e.PrintLine("SUBTOTAL      "),
                e.PrintLine(inv.subtotal.ToString("C").Trim('(').Trim(')').Trim('$')),

                e.PrintLine("TAX " + "(" + inv.taxRate.ToString("P") + ")      "),
                e.PrintLine(Math.Round(inv.taxAmount, 2).ToString("C").Trim('(').Trim(')').Trim('$')),

                e.PrintLine("TOTAL      "),
                e.PrintLine(inv.total.ToString("C").Trim('(').Trim(')').Trim('$')),
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
                ReceiptTail.Add(e.PrintLine(pay.paymentAmount.ToString("C").Trim('(').Trim(')').Trim('$')));
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
                    ReceiptTail.Add(e.PrintLine());
                    if (pay.cardResponse?.Signature != null)
                    {
                        MemoryStream imgStr = new MemoryStream(Convert.FromBase64String(pay.cardResponse?.Signature));
                        Image img = Image.FromStream(imgStr);
                        Bitmap newimg = ResizeImage(img, 800, 60);
                        MemoryStream newimgStr = new MemoryStream();
                        img.Save(newimgStr, ImageFormat.Png);
                        ReceiptTail.Add(e.CenterAlign());
                        ReceiptTail.Add(e.PrintImage(newimgStr.ToArray(), true, true));
                        ReceiptTail.Add(e.SetStyles(PrintStyle.Bold));
                        ReceiptTail.Add(e.PrintLine("  ____________________________________________________________  "));
                        ReceiptTail.Add(e.PrintLine("                           Signature                            "));
                        ReceiptTail.Add(e.FeedLines(2));
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
                e.PrintLine(inv.customer.phoneNumber) });

            Console.WriteLine("5");
            ReceiptTail.AddRange(new byte[][] {
                //e.PrintLine("  ____________________________________________________________  "),
                //e.PrintLine("                       Customer Signature                       "),
                e.SetStyles(PrintStyle.None),
                e.CenterAlign(),
                e.PrintLine(),
                e.PrintLine(),
                e.PrintLine(),
                e.PrintLine("THANKS FOR SHOPPING WITH US!"),
                e.LeftAlign(),
                e.Print("TIMS - Total Inventory Management System")
            });

            Console.WriteLine("Writing to printer");
            List<byte[]> Receipt = new List<byte[]>();
            Setup(monitor, printer);
            printer.Write(e.CashDrawerOpenPin2());
            printer.Write(e.FeedLines(1));
            printer.Write(ReceiptPreamble.ToArray());
            printer.Write(ReceiptBody.ToArray());
            printer.Write(ReceiptTail.ToArray());
            printer.Write(e.FeedLines(6));
            printer.Write(e.PartialCut());

            
            //printer.Write(Receipt.ToArray());
            //printer.Write(e.PrintLine());
            //printer.Write(e.PrintLine());
            Setup(monitor, printer);
        }

        private static void StatusChanged(object sender, EventArgs ps)
        {
            var status = (PrinterStatusEventArgs)ps;
            if (status == null) { Console.WriteLine("Status was null - unable to read status from printer."); return; }
            Console.WriteLine($"Printer Online Status: {status.IsPrinterOnline}");
            Console.WriteLine(JsonConvert.SerializeObject(status));
        }
        private static void Setup(bool enableStatusBackMonitoring, BasePrinter printer)
        {
            if (printer != null)
            {
                // Only register status monitoring once.
                if (!_hasEnabledStatusMonitoring)
                {
                    printer.StatusChanged += StatusChanged;
                    _hasEnabledStatusMonitoring = true;
                }
                printer?.Write(e.Initialize());
                printer?.Write(e.Enable());
                if (enableStatusBackMonitoring)
                {
                    printer.Write(e.EnableAutomaticStatusBack());
                }
            }
        }

        private static Bitmap ResizeImage(Image image, int width, int height)
        {
            var destRect = new Rectangle(0, 0, width, height);
            var destImage = new Bitmap(width, height);

            destImage.SetResolution(image.HorizontalResolution, image.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(image, destRect, 0, 0, image.Width, image.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            return destImage;
        }

        internal static void AddPrinter(Device device)
        {
            string ip = device.address.Address.ToString();
            string port = device.address.Port.ToString();
            NetworkPrinter printer = new NetworkPrinter(settings: new NetworkPrinterSettings() { ConnectionString = $"{ip}:{port}", PrinterName = device.Nickname });

            if (monitor)
            {
                printer.Write(e.Initialize());
                printer.Write(e.Enable());
                printer.Write(e.EnableAutomaticStatusBack());
            }
            printers.Add(printer);
            Setup(monitor, printer);
        }
    
        internal static void RemovePrinter(Device device)
        {
            BasePrinter printer = printers.FirstOrDefault(el => el.PrinterName == device.Nickname);
            if (printer == default(BasePrinter))
                return;

            printer.Dispose();
            printers.Remove(printer);
        }
    }
}
