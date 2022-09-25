using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMSServerModel
{
    public class Permission
    {
        public static List<Permission> Permissions;

        public string Name;
        public string Description;
        public Permission Parent;
        public List<Permission> Children;
        public enum Test
        {
            Test1,
            Test2
        }

        public Permission(string name)
        {

        }

        public override string ToString()
        {
            return Name; 
        }
    }
    class Permissions
    {

        public bool EditGlobalProperties =          false;
        public bool InstallPlugins =                false;
        public bool RemovePlugins =                 false;

        public bool CreateInvoices =                false;
        public bool SellAgeRestrictedItems =        false;
        public bool UseComplexInvoicing =           false;
        public bool ProcessReturns =                false;
        public bool ProcessReturnsWithoutInvoice =  false;

        public bool AddReceiptPrinters =            false;
        public bool AddCardReaders =                false;
        public bool AddLineDisplays =               false;
        public bool AddConventionalPrinters =       false;
        public bool DeleteReceiptPrinters =         false;
        public bool DeleteCardReaders =             false;
        public bool DeleteLineDisplays =            false;
        public bool DeleteConventionalPrinters =    false;
        public bool AssignReceiptPrinters =         false;
        public bool AssignCardReaders =             false;
        public bool AssignLineDisplays =            false;
        public bool AssignConventionalPrinters =    false;

        public bool CreateCustomers =               false;
        public bool DeleteCustomers =               false;
        public bool EditCustomers =                 false;
        public bool AssignPricingProfiles =         false;

        public bool CreatePricingProfiles =         false;
        public bool DeletePricingProfiles =         false;
        public bool EditPricingProfiles =           false;

        public bool AddEmployees =                  false;
        public bool DeleteEmployees =               false;
        public bool EditEmployees =                 false;
        public bool ChangeEmployeeHireStatus =      false;
        public bool EditEmployeeWageDetails =       false;

        public override string ToString()
        {
            return base.ToString();
        }
    }
}
