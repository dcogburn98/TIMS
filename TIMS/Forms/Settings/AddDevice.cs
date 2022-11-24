using System;
using System.Net;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

using TIMSServerModel;
using TIMS.Server;

using ESCPOS_NET;
using ESCPOS_NET.Emitters;

namespace TIMS.Forms.Settings
{
    public partial class AddDevice : Form
    {
        public AddDevice(string deviceType)
        {
            InitializeComponent();

            deviceTypeField.Text = deviceType;
        }

        private void AddDevice_Load(object sender, EventArgs e)
        {
            if (deviceTypeField.Text == "Card Reader")
                label5.Text = "Default port is 6200";
            if (deviceTypeField.Text == "Receipt Printer")
                label5.Text = "Default port is 9100";
        }

        private void saveButton_Click(object sender, EventArgs e)
        {
            int port;
            if (!IPAddress.TryParse(ipAddressField.Text, out IPAddress ip))
            {
                MessageBox.Show("IP address is malformed.\nPlease enter a correctly formed IP address.");
                return;
            }
            if (nicknameField.Text == string.Empty || !Program.IsStringAlphaNumeric(nicknameField.Text))
            {
                MessageBox.Show("Please enter a valid nickname for the device.\nOnly numbers and letters are allowed nicknames.");
                return;
            }
            if (portField.Text != string.Empty)
            {
                foreach (char c in portField.Text)
                {
                    if (!Char.IsDigit(c))
                    {
                        MessageBox.Show("Please enter a valid port number.\nLeave blank for common default port for device.");
                        return;
                    }
                }
                port = int.Parse(portField.Text);
                if (port < 1 || port > 65536)
                {
                    MessageBox.Show("Please enter a valid port number.\nLeave blank for common default port for device.");
                    return;
                }
            }
            else
            {
                switch (deviceTypeField.Text)
                {
                    case ("Receipt Printer"):
                        port = 9100;
                        break;
                    case ("Printer"):
                        port = 9100;
                        break;
                    case ("Card Reader"):
                        port = 6200;
                        break;
                    case ("Line Display"):
                        port = 9100;
                        break;
                    default:
                        port = 9100;
                        break;
                }
            }

            Device device = new Device();
            device.address = new IPEndPoint(ip, port);
            device.Nickname = nicknameField.Text;
            if (deviceTypeField.Text == "Receipt Printer")
                device.Type = Device.DeviceType.ThermalPrinter;
            else if (deviceTypeField.Text == "Printer")
                device.Type = Device.DeviceType.ConventionalPrinter;
            else if (deviceTypeField.Text == "Card Reader")
                device.Type = Device.DeviceType.CardReader;
            else if (deviceTypeField.Text == "Line Display")
                device.Type = Device.DeviceType.LineDisplay;
            else
                device.Type = Device.DeviceType.Other;

            Communication.RegisterDevice(device);
            DialogResult = DialogResult.OK;
            Close();
        }

        private void cancelButton_Click(object sender, EventArgs e)
        {
            DialogResult = DialogResult.Cancel;
            Close();
        }

        private void testButton_Click(object sender, EventArgs e)
        {
            string port;
            switch (deviceTypeField.Text)
            {
                case ("Receipt Printer"):
                    port = "9100";
                    break;
                case ("Printer"):
                    port = "9100";
                    break;
                case ("Card Reader"):
                    port = "6200";
                    break;
                case ("Line Display"):
                    port = "9100";
                    break;
                default:
                    port = "9100";
                    break;
            }

            if (!IPAddress.TryParse(ipAddressField.Text, out IPAddress ip))
            {
                MessageBox.Show("Invalid IP Address.");
                return;
            }
            if (portField.Text != string.Empty)
            {
                foreach (char c in portField.Text)
                    if (!Char.IsNumber(c))
                    {
                        MessageBox.Show("Invalid port number");
                        return;
                    }

                port = portField.Text;
            }

            if (deviceTypeField.Text == "Receipt Printer")
            {
                ICommandEmitter ee = new EPSON();
                BasePrinter printer = new NetworkPrinter(new NetworkPrinterSettings()
                { ConnectionString = ip + ":" + port, PrinterName = "TestPrinter" });
                printer?.Write(ee.Initialize());
                printer?.Write(ee.Enable());
                byte[][] Receipt() => new byte[][] {
                    ee.CenterAlign(),
                    ee.PrintLine(),
                    ee.SetBarcodeHeightInDots(360),
                    ee.SetBarWidth(BarWidth.Default),
                    ee.SetBarLabelPosition(BarLabelPrintPosition.None),
                    ee.PrintBarcode(BarcodeType.ITF, "0123456789"),
                    ee.PrintLine(),
                    ee.PrintLine("B&H PHOTO & VIDEO"),
                    ee.PrintLine("420 NINTH AVE."),
                    ee.PrintLine("NEW YORK, NY 10001"),
                    ee.PrintLine("(212) 502-6380 - (800)947-9975"),
                    ee.SetStyles(PrintStyle.Underline),
                    ee.PrintLine("www.bhphotovideo.com"),
                    ee.SetStyles(PrintStyle.None),
                    ee.PrintLine(),
                    ee.LeftAlign(),
                    ee.PrintLine("Order: 123456789        Date: 02/01/19"),
                    ee.PrintLine(),
                    ee.PrintLine(),
                    ee.SetStyles(PrintStyle.FontB),
                    ee.PrintLine("1   TRITON LOW-NOISE IN-LINE MICROPHONE PREAMP"),
                    ee.PrintLine("    TRFETHEAD/FETHEAD                        89.95         89.95"),
                    ee.PrintLine("----------------------------------------------------------------"),
                    ee.RightAlign(),
                    ee.PrintLine("SUBTOTAL         89.95"),
                    ee.PrintLine("Total Order:         89.95"),
                    ee.PrintLine("Total Payment:         89.95"),
                    ee.PrintLine(),
                    ee.LeftAlign(),
                    ee.SetStyles(PrintStyle.Bold | PrintStyle.FontB),
                    ee.PrintLine("SOLD TO:                        SHIP TO:"),
                    ee.SetStyles(PrintStyle.FontB),
                    ee.PrintLine("  LUKE PAIREEPINART               LUKE PAIREEPINART"),
                    ee.PrintLine("  123 FAKE ST.                    123 FAKE ST."),
                    ee.PrintLine("  DECATUR, IL 12345               DECATUR, IL 12345"),
                    ee.PrintLine("  (123)456-7890                   (123)456-7890"),
                    ee.PrintLine("  CUST: 87654321"),
                    ee.PrintLine(),
                    ee.PrintLine()
                };
                printer?.Write(Receipt());
                printer?.Write(ee.FeedLines(2));
                printer?.Write(ee.FullCut());
            }
            else if (deviceTypeField.Text == "Card Reader")
            {
                //Communication.RequestSignature(new Device() { address = new IPEndPoint(ip, int.Parse(port)) });
            }
            else
            {
                MessageBox.Show("No test yet implemented for devices of this type!");
            }
        }
    }
}
