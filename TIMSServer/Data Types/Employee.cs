using System;
using System.Collections.Generic;
using System.Text;

namespace TIMSServer
{
    [Serializable]
    public class Employee
    {
        public int employeeNumber;
        public string employeeName;

        public enum EmployeeType
        {
            Salesperson,
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

        public bool isCommisioned;
        public float commisionRate;
    }
}
