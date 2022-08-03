using System.ServiceModel;
using TIMSServerModel;

namespace TIMS.Server
{
    class Communication
    {
        private static ChannelFactory<ITIMSServiceModel> channelFactory = new
            ChannelFactory<ITIMSServiceModel>("TIMSServerEndpoint");

        private static ITIMSServiceModel proxy = channelFactory.CreateChannel();

        public static string CheckEmployee(string input)
        {
            return proxy.CheckEmployee(input);
        }
    }
}
