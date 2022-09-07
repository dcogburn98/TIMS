using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Net;
using System.Net.NetworkInformation;

using Rssdp;

namespace TIMS.Forms.Settings
{
    public partial class FindDevice : Form
    {
        private static NetworkInterface selectedInterface;

        public FindDevice()
        {
            InitializeComponent();

            foreach (NetworkInterface i in NetworkInterface.GetAllNetworkInterfaces())
            {
                foreach (UnicastIPAddressInformation ip in i.GetIPProperties().UnicastAddresses)
                {
                    if (ip.Address.AddressFamily == System.Net.Sockets.AddressFamily.InterNetwork)
                    {
                        comboBox1.Items.Add(i.Name + " (" + ip.Address.ToString() + ")");
                    }
                }
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            SearchForDevices();
        }

        private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            if (textBox1.Text == string.Empty)
                return;

            textBox1.Text = listBox1.SelectedItem.ToString();
        }

        public async void SearchForDevices()
        {
            button1.Enabled = false;
            listBox1.Items.Clear();
            // This code goes in a method somewhere.
            using (SsdpDeviceLocator deviceLocator = new SsdpDeviceLocator())
            {
                IEnumerable<DiscoveredSsdpDevice> foundDevices = await deviceLocator.SearchAsync(); // Can pass search arguments here (device type, uuid). No arguments means all devices.
                List<DiscoveredSsdpDevice> doctoredDevices = new List<DiscoveredSsdpDevice>();
                foreach (DiscoveredSsdpDevice d in foundDevices)
                {
                    if (GetGateways().Contains(IPAddress.Parse(d.DescriptionLocation.Host)))
                        continue;

                    if (doctoredDevices.Find(el => el.DescriptionLocation.AbsoluteUri == d.DescriptionLocation.AbsoluteUri) == null)
                        doctoredDevices.Add(d);
                }

                foreach (DiscoveredSsdpDevice foundDevice in doctoredDevices)
                {
                    // Can retrieve the full device description easily though.
                    try
                    {
                        SsdpDevice fullDevice = await foundDevice.GetDeviceInfo();
                        listBox1.Items.Add(foundDevice.DescriptionLocation.AbsoluteUri);
                    }
                    catch
                    {
                        //MessageBox.Show("Error");
                    }
                }
            }
            button1.Enabled = true;
        }

        public static IEnumerable<IPAddress> GetGateways()
        {
            foreach (NetworkInterface i in NetworkInterface.GetAllNetworkInterfaces())
            {
                IPInterfaceProperties adapterProperties = i.GetIPProperties();
                foreach (GatewayIPAddressInformation ip in adapterProperties.GatewayAddresses)
                    yield return ip.Address;
            }
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            selectedInterface = NetworkInterface.GetAllNetworkInterfaces()[comboBox1.SelectedIndex];
        }
    }
}
