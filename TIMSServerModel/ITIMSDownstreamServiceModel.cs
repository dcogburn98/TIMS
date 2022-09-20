using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Serialization;
using System.ServiceModel;
using System.Text;

namespace TIMSServerModel
{
    [ServiceContract]
    public interface ITIMSDownstreamServiceModel
    {
        [OperationContract]
        bool AuthKeyMatch(AuthKey key);
        [OperationContract]
        Item GetCurrentDownstreamItem(Item item);
        [OperationContract]
        void UpdateDownstreamItem(Item item);
    }
}
