﻿using System;
using System.Collections.Generic;
using System.Text;

namespace TIMSServer
{
    [Serializable]
    public class Invoice
    {
        public int invoiceNumber;
        public Customer customer;
        public Employee employee;

        public InvoiceItem[] items;
        public float subtotal;
        public float taxRate;
        public float taxAmount;
        public float total;
        public Payment[] payments;

        public bool containsAgeRestrictedItem;
        public DateTime customerBirthdate;
        public string attentionLine;
        public string PONumber;
        public string invoiceMessage;

        public bool savedInvoice;
        public DateTime savedInvoiceTime;
    }
}