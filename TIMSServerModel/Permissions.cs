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
        public string FriendlyName;
        public string Description;

        public Permission(string Name, string FriendlyName, string Description)
        {
            this.Name = Name;
            this.FriendlyName = FriendlyName;
            this.Description = Description;
            Permissions.Add(this);
        }

        public static List<Permission> Parse(string PermissionName)
        {
            if (PermissionName == "*")
                return Permissions;
            else
            {
                List<Permission> ParsedPermissions = Permissions;
                string[] split = PermissionName.Split('.');
                for (int i = 0; i != split.Length; i++)
                {
                    if (split[i] == "*")
                        break;
                    else
                    {

                    }
                }
                return ParsedPermissions;
            }
        }

        public string ToString(bool FriendlyName)
        {
            if (FriendlyName)
                return this.FriendlyName;
            else
                return Name;
        }
        public override string ToString()
        {
            return Name; 
        }
    }
    class Permissions
    {
        public static Permission EditGlobalProperties = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission InstallPlugins = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission RemovePlugins = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");

        public static Permission CreateInvoices = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission SellAgeRestrictedItems = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission UseAdvancedInvoicing = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission ProcessReturns = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission ProcessReturnsWithoutInvoice = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");

        public static Permission AddReceiptPrinters = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission AddCardReaders = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission AddLineDisplays = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission AddConventionalPrinters = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission DeleteReceiptPrinters = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission DeleteCardReaders = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission DeleteLineDisplays = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission DeleteConventionalPrinters = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission AssignReceiptPrinters = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission AssignCardReaders = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission AssignLineDisplays = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission AssignConventionalPrinters = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");

        public static Permission CreateCustomers = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission DeleteCustomers = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission EditCustomers = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission AssignPricingProfiles = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");

        public static Permission CreatePricingProfiles = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission DeletePricingProfiles = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission EditPricingProfiles = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");

        public static Permission AddEmployees = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission DeleteEmployees = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission EditEmployees = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission ChangeEmployeeHireStatus = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
        public static Permission EditEmployeeWageDetails = new Permission("properties.edit", "Edit Global Properties", "Allow editing of global system properties.");
    }
}
