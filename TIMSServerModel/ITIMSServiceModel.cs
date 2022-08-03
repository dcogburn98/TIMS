using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TIMSServerModel
{
    // NOTE: You can use the "Rename" command on the "Refactor" menu to change the interface name "IEmployeeModel" in both code and config file together.
    [ServiceContract]
    public interface ITIMSServiceModel
    {
        [OperationContract]
        string CheckEmployee(string input);
    }
}
