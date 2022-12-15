using System;
using System.Collections.Generic;
using System.Linq;
using System.ServiceModel;
using System.Text;
using System.Threading.Tasks;

using TIMSServerModel;

namespace TIMSCentralServer
{
    [ServiceContract]
    internal interface ITIMSCentralServerModel
    {
        [OperationContract]
        bool RequestProductLineRegistration(string productLine, string description, string[] brands, string key);

        [OperationContract]
        bool AddBrandToProductLine(string productLine, string brand, string key);

        [OperationContract]
        bool CheckProductLine(string productLine, string key);

        [OperationContract]
        List<Item> CheckItemNumber(string itemNumber, string key);

        [OperationContract]
        byte[] RetrieveItemImages(Item item, string key);

        [OperationContract]
        byte[] RetrieveItemModel(Item item, string key);

        [OperationContract]
        bool CheckKey(string key);
    }
}
