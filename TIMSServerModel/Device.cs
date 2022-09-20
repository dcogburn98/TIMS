using System;
using System.Runtime.Serialization;
using System.Net;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    [DataContract]
    public class Device
    {
        public enum DeviceType
        {
            Terminal,
            ThermalPrinter,
            CardReader,
            ConventionalPrinter,
            SignaturePad,
            LineDisplay,
            IPCamera,
            ConveyorBelt,
            BarcodeScanner,
            Scale,
            CustomerDisplay,
            LabelPrinter
        }
        [DataMember]
        public DeviceType Type;
        [DataMember]
        public string Nickname;
        [DataMember]
        public IPAddress address;
        [DataMember]
        public List<Device> AssignedDevices = new List<Device>();
    }
}
