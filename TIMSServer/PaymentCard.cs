using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using PaymentEngine.xTransaction;

using TIMSServerModel;

namespace TIMSServer
{
    class PaymentCard
    {
        public static void LoadStandardSettings(Request Request)
        {
            Request.xKey = "revitacomdevc9743d6ba73347ca86d5392dc8c56853"; // Credential
            Request.xVersion = "4.5.4"; //API Version
            Request.xSoftwareName = System.Reflection.Assembly.GetEntryAssembly().GetName().Name; //Name of your software
            Request.xSoftwareVersion = System.Reflection.Assembly.GetEntryAssembly().GetName().Version.ToString();//My.Application.Info.Version.ToString //Version of your software
        }

        public static void LoadDeviceSettings(Request Request)
        {
            #region Device Type
            Request.Settings.Device_Name = "";
            //Request.Settings.Device_Name = "Verifone_Vx805" 'Non-EMV
            //Request.Settings.Device_Name = "Verifone_Vx805.4" 'EMV

            //Request.Settings.Device_Name = "Verifone_Mx915" 'Non-EMV
            //Request.Settings.Device_Name = "Verifone_Mx915.4" 'EMV

            //Request.Settings.Device_Name = "Verifone_Mx925" 'Non-EMV
            //Request.Settings.Device_Name = "Verifone_Mx925.4" 'EMV
            #endregion

            #region USB/Serial Setup
            //======================================================================
            //Settings for Serial/USB Devices
            //======================================================================
            Request.Settings.Device_COM_Port = "";
            //Ex: COM9 
            Request.Settings.Device_COM_Baud = "";
            //Ex: 115200
            Request.Settings.Device_COM_Parity = "";
            //Ex: N
            Request.Settings.Device_COM_DataBits = "";
            //Ex: 8
            #endregion

            #region LAN/IP Setup
            //======================================================================
            //Settings for IP Devices
            //======================================================================
            Request.Settings.Device_IP_Address = "";
            Request.Settings.Device_IP_Port = "";
            #endregion

            //======================================================================
            //Set Custom Device Timeout (Default = 120000)
            //======================================================================
            Request.Settings.Device_Timeout = 120000;
            //120 Seconds
        }

        public static Response ProcessOutOfScope(Invoice inv, decimal paymentAmount)
        {
            Request MyRequest = new Request();

            //Display Line-Items
            //ShowItems();

            LoadStandardSettings(MyRequest);
            LoadDeviceSettings(MyRequest);
            MyRequest.xCommand = "cc:sale";
            MyRequest.xInvoice = inv.invoiceNumber.ToString(); //We recommend using invoice numbers to improve duplicate transaction handling
            MyRequest.xAmount = paymentAmount;

            //TIP: For the ability to control and show custom forms on a device please contact support.

            //If Printer settings are set receipts will be automatically printed, alternatively the response variables (see below) can be used
            MyRequest.Settings.Printer_Name = ""; //Ex: EPSON TM-T20"
            MyRequest.Settings.Receipt_Merchant_Disabled = false;
            MyRequest.Settings.Receipt_Customer_Disabled = false;

            //If Merchant's system is touch enabled, set to True to display an on screen key pad
            MyRequest.ShowKeyPad = false;

            //To disable keyed entry, set to False
            MyRequest.EnableKeyedEntry = true;

            //To return control to the calling application after any device error, set to True
            MyRequest.ExitFormOnDeviceError = false;

            //To enable silent mode and prevent the payment screen from showing, set to True
            MyRequest.EnableSilentMode = false;

            //To allow cashier to have option of selecting a Stored Account Number, set to True
            //Use Case: 
            //   If application stores account numbers in a PCI compliant fashion, this feature will allow the cashier to select the Account on file.
            //Message Format: 
            //   Placeholder {0} will insert the masked card number (4xxxxxxxxxxx1111)
            //   Placeholder {1} will insert the last 4 of the card number (1111)
            //   Placeholders are optional
            //   The "&" will create a keyboard shortcut using ALT + the letter following the ampersand.
            //======================================================================
            MyRequest.EnableStoredAccount = false;
            //MyRequest.StoredAccount_Message = "Use {0} (ALT + &S)" 'Sample Message 1
            //MyRequest.StoredAccount_Message = "Use Stored Card [ALT + &S]" 'Sample Message 2
            //MyRequest.StoredAccount_Message = "{0} [ALT + &S]" 'Sample Message 3
            //MyRequest.StoredAccount_Message = "ALT + &S Use Stored Card {1}" 'Sample Message 4
            //MyRequest.xCardNum = "4444333322221111"
            //MyRequest.xExp = "1229"


            bool MyRequire_AVS = false; //Only affects keyed transactions
            bool MyRequire_CVV = false; //Only affects keyed transactions
            bool MyEnableDeviceInsertSwipeTap = true;
            bool MyRequire_Pin = false; //TIP: Skip the signature prompt for small transactions by setting the Require_Signature parameter to false for those transactions.
            bool MyRequire_Signature = false;
            bool MyExitFormIfApproved = true; //Return control to the calling application if transaction is approved
            bool MyExitFormIfNotApproved = false; //Return control to the calling application if transaction is not approved
            Response MyResponse = MyRequest.Manual(MyRequire_AVS, MyRequire_CVV, MyEnableDeviceInsertSwipeTap, MyRequire_Pin, MyRequire_Signature, MyExitFormIfApproved, MyExitFormIfNotApproved, false);
            if (MyResponse.xResult == "A")
            {
                //Signature can be obtained via a parameter in the Manual function above (MyRequire_Signature = True) or in a separate command as shown below.
                if (MyResponse.xSignaturerRequired == true && string.IsNullOrEmpty(MyResponse.xSignature))
                {
                    MyResponse.xSignature = MyRequest.GetSignature();
                }

                //Prompt for Email Address on a device
                string MyEmailAddress = MyRequest.Device_PromptForEmail();

                //Prompt for Phone Number on a device. Customer will be prompted to opt-in to receive promotions via text message. If they opt-in the phone number will be returned.
                string MyPhoneNumber_JSON = MyRequest.Device_PromptForPhone_JSON();
                string MyPhoneNumber_XML = MyRequest.Device_PromptForPhone_XML();

                //Prompt for Zip Code on a device
                string MyZipCode = MyRequest.Device_PromptForZip();

                //Receipt is split into separate parts to allow placement of each part on existing receipt
                //Merchant Receipt
                //MessageBox.Show(MyResponse.xReceiptHeader + MyResponse.xReceiptBody1 + MyResponse.xReceiptBody2_CVM + MyResponse.xReceiptBody3 + MyResponse.xReceiptFooterMerchant);
                //Customer Receipt (xReceiptBody2_CVM does not need to be included on the cusotmer receipt)
                //MessageBox.Show(MyResponse.xReceiptHeader + MyResponse.xReceiptBody1 + MyResponse.xReceiptBody3 + MyResponse.xReceiptFooterCustomer);
                return MyResponse;
            }
            //Formatted Response
            //MessageBox.Show(MyResponse.FormattedResponse());
            return MyResponse;
        }
    }
}
