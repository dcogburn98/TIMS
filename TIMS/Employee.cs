using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace TIMS
{
    public class Employee
    {
        public int employeeNumber;
        public string fullName;
        public string username;
        public string position;
        public DateTime birthDate;
        public DateTime hireDate;
        public DateTime terminationDate;

        public enum EmployeeType
        {
            Salesperson,
            Manager,
            Administrator,
            WarehousePicker,
            WarehouseShipper,
            WarehouseReceiver
        }
        public EmployeeType employeeType;

        public enum EmployeePermissions
        {
            Invoice = 1,
            ViewItemMaintenance = 2,
            EditItemMaintenance = 4,
            ViewCustomerInformation = 8,
            EditCustomerInformation = 16
        }
        public EmployeePermissions employeePermissions;

        public enum StartupScreens
        {
            Invoicing,
            EmployeeManagement,
            InventoryManagement,
            Inbox,
        }
        public StartupScreens startupScreen;

        public enum PaySchedules
        {
            Weekly,
            Biweekly,
            Monthly,
            Yearly
        }

        public bool isCommisioned;
        public float commisionRate;
        public bool waged;
        public float wage;

        public Employee(int employeeNo, string fullName)
        {
            this.employeeNumber = employeeNo;
            this.fullName = fullName;
        }
    }
}
