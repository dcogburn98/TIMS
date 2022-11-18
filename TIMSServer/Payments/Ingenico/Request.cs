using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServer.Payments.Ingenico
{
    internal class Request
    {
        public enum ACI_TYPES
        {
            /// <summary>
            /// Payment Authorization and Capture (CCxT)
            /// </summary>
            AC,
            /// <summary>
            /// Return Authorization and Capture Hold
            /// </summary>
            AH,
            /// <summary>
            /// CryptoCurrency
            /// </summary>
            CC,
            /// <summary>
            /// Debt Repayment / Consumer Loan
            /// </summary>
            D,
            /// <summary>
            /// High-risk Securities
            /// </summary>
            HR,
            /// <summary>
            /// Quasi Cash
            /// </summary>
            Q
        }
        public enum ACI_EXT_TYPES
        {
            /// <summary>
            /// Estimated Amount
            /// </summary>
            AE,
            /// <summary>
            /// Final Authorization
            /// </summary>
            AF,
            /// <summary>
            /// Open Authorization
            /// </summary>
            AO,
            /// <summary>
            /// Auto-substantiation
            /// </summary>
            AS,
            /// <summary>
            /// Incremental Authorization
            /// </summary>
            IA,
            /// <summary>
            /// Installment Payment
            /// </summary>
            IP,
            /// <summary>
            /// Partial Shipment
            /// </summary>
            PS,
            /// <summary>
            /// Re-authorization
            /// </summary>
            RA,
            /// <summary>
            /// Recurring Billing
            /// </summary>
            RB,
            /// <summary>
            /// Subscription/Standing Authorization
            /// </summary>
            SA
        }


        /// <summary>
        /// The ACCOUNT_NBR field contains the account number to be acted upon during the transaction.
        /// </summary>
        public int ACCOUNT_NBR;

        /// <summary>
        /// The Authorization Characteristics Indicator (ACI) field is used to identify specific characteristics
        /// of the transaction for the Networks.The table below describes the possible values.
        /// </summary>
        public ACI_TYPES ACIType;

        /// <summary>
        /// The Authorization Characteristics Indicator Extension (ACI_EXT) field is used to identify 
        /// additional characteristics of the transaction for the Networks.The table below describes the possible values.
        /// </summary>
        public ACI_EXT_TYPES ACI_EXT;

        /// <summary>
        /// The ACTION_CODE field indicates that the account should be
        /// updated by Visa or MasterCard.This action should be taken against an ORIG_AUTH_GUID
        /// during an Automatic Account Updater Service transaction.
        /// </summary>
        public bool ACTION_CODE;

        /// <summary>
        /// The ADDENDA_X fields, where X is a number between 0 and 9, are used in the ACH
        ///environment to attach an addendum to a transaction.
        /// </summary>
        public string[] ADDENDA;

        /// <summary>
        /// The ADDRESS field contains the street address associated with the account number. This field,
        /// along with ZIP_CODE, is part of the credit card address verification as the numeric value
        /// contained in this field will be validated against the value recorded at the issuing bank for the
        /// account when doing a credit card authorization.The response for this verification is found in
        /// the AUTH_AVS field. EPX supports all AVS formats supported by the card networks.
        /// </summary>
        public string ADDRESS;

        /// <summary>
        /// The AMOUNT field contains the positive dollar amount of funds to be moved during the
        /// transaction.The AMOUNT should be the amount of goods and service including any
        /// reference field amount; for example, TIP_AMT, TAX_AMT.
        /// AMOUNT should not include cashback amounts.
        /// </summary>
        public decimal AMOUNT;


    }
}
