using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMS
{
    public class EmployeePermissions
    {
        public static List<Permission> Permissions = new List<Permission>() 
        { 
            new Permission("ChargeAccounts", "Charge to Accounts",                                          0x0001, 0x0001),
            new Permission("CashSales", "Perform Cash Sales",                                               0x0002, 0x0001), 
            new Permission("CreateHeldInvoices", "Put Invoices on Hold",                                    0x0004, 0x0001),
            new Permission("ViewHeldInvoices", "View Invoices on Hold",                                     0x0008, 0x0001),

            new Permission("OpenHeldInvoices", "Open Invoices on Hold",                                     0x0010, 0x0001),
            new Permission("ViewTodaysOwnInvoices", "View Invoices Made by Own Self Today",                 0x0020, 0x0001),
            new Permission("EditTodaysOwnInvoices", "Make Changes to Invoices Made by Own Self Today",      0x0040, 0x0001),
            new Permission("ViewTodaysInvoices", "View Invoices made by Anybody Today",                     0x0080, 0x0001),

            new Permission("EditTodaysInvoices", "Made Changes to Invoices Made by Anybody Today",          0x0100, 0x0001),
            
        };
        public enum Permissionss : ulong
        {
            ChargeAccounts =        0x0001, //Allows charging of items to capable accounts
            CashSales =             0x0002, //Allows sales of items with cash, check, or payment cards
            CreateHeldInvoices =    0x0004, //Allows invoices to be put on hold under capable accounts
            ViewHeldInvoices =      0x0008, //Allows viewing of held invoices

            OpenHeldInvoices =      0x0010, //Allows retrieval and opening of held invoices
            ViewTodaysOwnInvoices,          //Allows viewing of invoices created on current day by the employee
            ViewTodaysInvoices =    0x0020, //Allows viewing of all invoices created on current day
            ViewAllInvoices =       0x0040, //Allows viewing of all invoices
            VoidInvoices =          0x0080, //Allows voiding of capable invoices

            DeleteHeldInvoices =    0x0100, //Allows deletion of held invoices
            ViewCustomers =         0x0400, //Allows viewing of customers in the database
            AddCustomers =          0x0800, //Allows addition of customers to the database

            EditCustomers =         0x1000, //Allows editing of customer information
            DeleteCustomers =       0x2000, //Allows deletion of customers from the database
            ViewOwnPermissions,             //Allows viewing of permissions of the current employee
            ViewPermissions =       0x4000, //Allows viewing of permissions of all employees
            EditPermissions =       0x8000, //Allows editing of permissions

            ViewEmployeeTime =      0x10000, //Allows viewing of all employees' times
            SubmitEmployeeTime =    0x20000, //Allows submission of clock-in,clock-out times for the current employee
            EditOwnEmployeeTime =   0x40000, //Allows editing of current employee times
            EditEmployeeTime =      0x80000, //Allows editing of all employee times

            ChangeInvoiceEmployeeNumber =  0x100000, //Allows changing employee number on POS screen

        }
    }

    public class Permission
    {
        public string name;
        public string friendlyName;
        public ulong code;
        public int group;

        // Group Codes:
        // 0 = General
        // 1 = Invoicing
        // 2 = Inventory
        // 3 = Employees
        // 4 = Customers
        // 5 = Extensions
        public Permission(string name, string friendlyName, ulong code, int group = 0)
        {
            this.name = name;
            this.friendlyName = friendlyName;
            this.code = code;
            this.group = group;
        }


    }
}
