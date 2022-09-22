using System;
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

namespace TIMS.Forms.Settings
{
    public partial class DeviceAssignments : Form
    {
        public Device selectedTerminal;
        public DeviceAssignments()
        {
            InitializeComponent();

            foreach (Device term in Communication.RetrieveTerminals())
            {
                int i = terminalsLB.Items.Add(term);
            }
            
            foreach (Device device in Communication.RetrieveDevices().Where(el => el.Type == Device.DeviceType.ThermalPrinter))
            {
                receiptPrintersLB.Items.Add(device);
            }
            foreach (Device device in Communication.RetrieveDevices().Where(el => el.Type == Device.DeviceType.ConventionalPrinter))
            {
                receiptPrintersLB.Items.Add(device);
            }
            foreach (Device device in Communication.RetrieveDevices().Where(el => el.Type == Device.DeviceType.LineDisplay))
            {
                receiptPrintersLB.Items.Add(device);
            }
            foreach (Device device in Communication.RetrieveDevices().Where(el => el.Type == Device.DeviceType.CardReader))
            {
                receiptPrintersLB.Items.Add(device);
            }

            terminalsLB.SelectedIndex = 0;
            selectedTerminal = (Device)terminalsLB.SelectedItem;
        }

        private void addReceiptPrinter_Click(object sender, EventArgs e)
        {
            AddDevice adder = new AddDevice("Receipt Printer");
            if (adder.ShowDialog() == DialogResult.OK)
            {
                receiptPrintersLB.Items.Clear();
                foreach (Device device in Communication.RetrieveDevices().Where(el => el.Type == Device.DeviceType.ThermalPrinter))
                {
                    receiptPrintersLB.Items.Add(device.Nickname + " (" + device.address + ")");
                }
            }
        }

        private void receiptPrinterUSB_CheckedChanged(object sender, EventArgs e)
        {
            if (receiptPrinterUSB.Checked)
            {
                receiptPrintersLB.Enabled = false;
                chooseReceiptPrinter.Enabled = false;
            }
            else
            {
                receiptPrintersLB.Enabled = true;
                chooseReceiptPrinter.Enabled = true;
            }
        }

        private void chooseReceiptPrinter_Click(object sender, EventArgs e)
        {
            if (terminalsLB.SelectedIndex == -1 || receiptPrintersLB.SelectedIndex == -1)
                return;

            foreach (Device device in selectedTerminal.AssignedDevices)
            {
                if (device.Type == Device.DeviceType.ThermalPrinter)
                    Communication.RemoveDeviceAssignment(selectedTerminal, device);
            }

            Communication.AssignDevice(selectedTerminal, (Device)receiptPrintersLB.SelectedItem);
            receiptPrintersLB.Items.Clear();
            foreach (Device device in Communication.RetrieveDevices().Where(el => el.Type == Device.DeviceType.ThermalPrinter))
            {
                receiptPrintersLB.Items.Add(device);
            }
        }

        private void receiptPrintersLB_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font f = e.Font;
            if (selectedTerminal.AssignedDevices.Contains(receiptPrintersLB.Items[e.Index])) //TODO: Your condition to make text bold
                f = new Font(e.Font, FontStyle.Bold);
            e.DrawBackground();
            e.Graphics.DrawString(((ListBox)(sender)).Items[e.Index].ToString(), f, new SolidBrush(e.ForeColor), e.Bounds);
            e.DrawFocusRectangle();
        }
    }
}
