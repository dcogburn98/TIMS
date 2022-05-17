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
            All,
            Invoice,
            ViewItemMaintenance,
            EditItemMaintenance,
            ViewCustomerInformation,
            EditCustomerInformation
        }
        public List<EmployeePermissions> employeePermissions = new List<EmployeePermissions>();

        public enum StartupScreens
        {
            Dashboard,
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
            Monthly
        }

        public bool isCommisioned;
        public float commisionRate;
        public bool waged;
        public float wage;

        public Employee()
        {

        }

        public Employee(int employeeNo, string fullName)
        {
            this.employeeNumber = employeeNo;
            this.fullName = fullName;
        }
    }
}
