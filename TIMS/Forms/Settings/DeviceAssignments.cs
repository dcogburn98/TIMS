using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Management;
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
                invoicePrintersLB.Items.Add(device);
            }
            foreach (Device device in Communication.RetrieveDevices().Where(el => el.Type == Device.DeviceType.LineDisplay))
            {
                lineDisplaysLB.Items.Add(device);
            }
            foreach (Device device in Communication.RetrieveDevices().Where(el => el.Type == Device.DeviceType.CardReader))
            {
                cardReadersLB.Items.Add(device);
            }

            terminalsLB.SelectedIndex = 0;
            selectedTerminal = (Device)terminalsLB.SelectedItem;
        }

        private void terminalsLB_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedTerminal = (Device)terminalsLB.SelectedItem;
            receiptPrintersLB.Refresh();
            invoicePrintersLB.Refresh();
            lineDisplaysLB.Refresh();
            cardReadersLB.Refresh();
        }

        #region Receipt Printer Methods
        private void addReceiptPrinter_Click(object sender, EventArgs e)
        {
            AddDevice adder = new AddDevice("Receipt Printer");
            if (adder.ShowDialog() == DialogResult.OK)
            {
                receiptPrintersLB.Items.Clear();
                foreach (Device device in Communication.RetrieveDevices().Where(el => el.Type == Device.DeviceType.ThermalPrinter))
                {
                    receiptPrintersLB.Items.Add(device);
                }
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
            int termID = selectedTerminal.ID;
            selectedTerminal = Communication.RetrieveTerminals().First(el => el.ID == termID);
            receiptPrintersLB.Items.Clear();
            foreach (Device device in Communication.RetrieveDevices().Where(el => el.Type == Device.DeviceType.ThermalPrinter))
            {
                receiptPrintersLB.Items.Add(device);
            }
        }

        private void receiptPrintersLB_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font f = e.Font;
            Device device = (Device)receiptPrintersLB.Items[e.Index];
            if (selectedTerminal.AssignedDevices.Where(el => el.ID == device.ID).Count() > 0)
                f = new Font(e.Font, FontStyle.Bold);
            e.DrawBackground();
            e.Graphics.DrawString(((ListBox)(sender)).Items[e.Index].ToString(), f, new SolidBrush(e.ForeColor), e.Bounds);
            e.DrawFocusRectangle();
        }

        private void deleteReceiptPrinter_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Are you sure you want to remove this device?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Device device = (Device)receiptPrintersLB.Items[receiptPrintersLB.SelectedIndex];
                foreach (Device term in Communication.RetrieveTerminals())
                    if (term.AssignedDevices.Where(el => el.ID == device.ID).Count() > 0)
                        Communication.RemoveDeviceAssignment(term, device);
                Communication.DeleteDevice(device);

                receiptPrintersLB.Items.Clear();
                foreach (Device dev in Communication.RetrieveDevices().Where(el => el.Type == Device.DeviceType.ThermalPrinter))
                {
                    receiptPrintersLB.Items.Add(dev);
                }
            }
        }

        #endregion

        #region Invoice Printer Methods
        private void addInvoicePrinter_Click(object sender, EventArgs e)
        {
            AddDevice adder = new AddDevice("Printer");
            if (adder.ShowDialog() == DialogResult.OK)
            {
                invoicePrintersLB.Items.Clear();
                foreach (Device device in Communication.RetrieveDevices().Where(el => el.Type == Device.DeviceType.ConventionalPrinter))
                {
                    invoicePrintersLB.Items.Add(device.Nickname + " (" + device.address + ")");
                }
            }
        }

        private void chooseInvoicePrinter_Click(object sender, EventArgs e)
        {

        }

        private void invoicePrintersLB_DrawItem(object sender, DrawItemEventArgs e)
        {

        }

        private void deleteInvoicePrinter_Click(object sender, EventArgs e)
        {

        }
        #endregion

        #region Card Reader Methods
        private void addCardReader_Click(object sender, EventArgs e)
        {
            AddDevice adder = new AddDevice("Card Reader");
            if (adder.ShowDialog() == DialogResult.OK)
            {
                cardReadersLB.Items.Clear();
                foreach (Device device in Communication.RetrieveDevices().Where(el => el.Type == Device.DeviceType.CardReader))
                {
                    cardReadersLB.Items.Add(device);
                }
            }
        }

        private void chooseCardReader_Click(object sender, EventArgs e)
        {
            if (terminalsLB.SelectedIndex == -1 || cardReadersLB.SelectedIndex == -1)
                return;

            foreach (Device device in selectedTerminal.AssignedDevices)
            {
                if (device.Type == Device.DeviceType.CardReader)
                    Communication.RemoveDeviceAssignment(selectedTerminal, device);
            }

            Communication.AssignDevice(selectedTerminal, (Device)cardReadersLB.SelectedItem);
            int termID = selectedTerminal.ID;
            selectedTerminal = Communication.RetrieveTerminals().First(el => el.ID == termID);
            cardReadersLB.Items.Clear();
            foreach (Device device in Communication.RetrieveDevices().Where(el => el.Type == Device.DeviceType.CardReader))
            {
                cardReadersLB.Items.Add(device);
            }
        }

        private void cardReadersLB_DrawItem(object sender, DrawItemEventArgs e)
        {
            Font f = e.Font;
            Device device = (Device)cardReadersLB.Items[e.Index];
            if (selectedTerminal.AssignedDevices.Where(el => el.ID == device.ID).Count() > 0)
                f = new Font(e.Font, FontStyle.Bold);
            e.DrawBackground();
            e.Graphics.DrawString(((ListBox)(sender)).Items[e.Index].ToString(), f, new SolidBrush(e.ForeColor), e.Bounds);
            e.DrawFocusRectangle();
        }

        private void deleteCardReader_Click(object sender, EventArgs e)
        {
            if (cardReadersLB.SelectedIndex < 0)
                return;

            if (MessageBox.Show("Are you sure you want to remove this device?", "Warning", MessageBoxButtons.YesNo) == DialogResult.Yes)
            {
                Device device = (Device)cardReadersLB.Items[cardReadersLB.SelectedIndex];
                foreach (Device term in Communication.RetrieveTerminals())
                    if (term.AssignedDevices.Where(el => el.ID == device.ID).Count() > 0)
                        Communication.RemoveDeviceAssignment(term, device);
                Communication.DeleteDevice(device);

                cardReadersLB.Items.Clear();
                foreach (Device dev in Communication.RetrieveDevices().Where(el => el.Type == Device.DeviceType.CardReader))
                {
                    cardReadersLB.Items.Add(dev);
                }
            }
        }
        #endregion

        private void addLineDisplay_Click(object sender, EventArgs e)
        {
        }
    }
}
