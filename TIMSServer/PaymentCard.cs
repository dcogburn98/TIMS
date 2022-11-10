using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
//using PaymentEngine.xTransaction;

using TIMSServerModel;

namespace TIMSServer
{
    class PaymentCard
    {
        //public static void LoadStandardSettings(Request Request)
        //{
        //    Request.xKey = "revitacomdevc9743d6ba73347ca86d5392dc8c56853"; // Credential
        //    Request.xVersion = "4.5.4"; //API Version
        //    Request.xSoftwareName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name; //Name of your software
        //    Request.xSoftwareVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();//My.Application.Info.Version.ToString //Version of your software
        //}

        //public static void LoadDeviceSettings(Request Request)
        //{
        //    #region Device Type
        //    //Request.Settings.Device_Name = "";
        //    //Request.Settings.Device_Name = "Verifone_Vx805" 'Non-EMV
        //    //Request.Settings.Device_Name = "Verifone_Vx805.4" 'EMV

        //    //Request.Settings.Device_Name = "Verifone_Mx915" 'Non-EMV
        //    Request.Settings.Device_Name = "Verifone_Mx915.4";

        //    //Request.Settings.Device_Name = "Verifone_Mx925" 'Non-EMV
        //    //Request.Settings.Device_Name = "Verifone_Mx925.4" 'EMV
        //    #endregion

        //    #region USB/Serial Setup
        //    //======================================================================
        //    //Settings for Serial/USB Devices
        //    //======================================================================
        //    Request.Settings.Device_COM_Port = "";
        //    //Ex: COM9 
        //    Request.Settings.Device_COM_Baud = "";
        //    //Ex: 115200
        //    Request.Settings.Device_COM_Parity = "";
        //    //Ex: N
        //    Request.Settings.Device_COM_DataBits = "";
        //    //Ex: 8
        //    #endregion

        //    #region LAN/IP Setup
        //    //======================================================================
        //    //Settings for IP Devices
        //    //======================================================================
        //    Request.Settings.Device_IP_Address = "192.168.254.67";
        //    Request.Settings.Device_IP_Port = "10760";
        //    #endregion

        //    //======================================================================
        //    //Set Custom Device Timeout (Default = 120000)
        //    //======================================================================
        //    Request.Settings.Device_Timeout = 120000;
        //    //120 Seconds
        //}

        //public static Request ProcessOutOfScopeAsync(Invoice inv, decimal paymentAmount)
        //{
        //    Request MyRequest = new Request();

        //    //Display Line-Items
        //    //ShowItems();

        //    LoadStandardSettings(MyRequest);
        //    LoadDeviceSettings(MyRequest);
        //    MyRequest.xCommand = "cc:sale";
        //    MyRequest.xInvoice = inv.invoiceNumber.ToString(); //We recommend using invoice numbers to improve duplicate transaction handling
        //    MyRequest.xAmount = paymentAmount;

        //    //TIP: For the ability to control and show custom forms on a device please contact support.

        //    //If Printer settings are set receipts will be automatically printed, alternatively the response variables (see below) can be used
        //    MyRequest.Settings.Printer_Name = ""; //Ex: EPSON TM-T20"
        //    MyRequest.Settings.Receipt_Merchant_Disabled = false;
        //    MyRequest.Settings.Receipt_Customer_Disabled = false;

        //    //If Merchant's system is touch enabled, set to True to display an on screen key pad
        //    MyRequest.ShowKeyPad = false;

        //    //To disable keyed entry, set to False
        //    MyRequest.EnableKeyedEntry = true;

        //    //To return control to the calling application after any device error, set to True
        //    MyRequest.ExitFormOnDeviceError = true;

        //    //To enable silent mode and prevent the payment screen from showing, set to True
        //    MyRequest.EnableSilentMode = false;

        //    MyRequest.RequireAVS = false; //Only affects keyed transactions
        //    MyRequest.RequireCVV = false; //Only affects keyed transactions
        //    MyRequest.EnableDeviceInsertSwipeTap = true;
        //    MyRequest.EnableDevicePin = false;
        //    MyRequest.EnableDeviceSignature = false;
        //    MyRequest.ExitFormIfApproved = true;
        //    MyRequest.ExitFormIfNotApproved = true;
        //    return MyRequest;
        //}
    }
}
