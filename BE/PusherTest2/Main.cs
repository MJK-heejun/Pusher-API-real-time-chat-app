using System.Collections.Generic;
using System.Net;
using System.ServiceModel;
using System.ServiceModel.Activation;
using System.ServiceModel.Web;
using System.IO;
using System.Text;
using MyChat.Controller;

namespace MyChat
{

    [ServiceContract]
    [AspNetCompatibilityRequirements(RequirementsMode = AspNetCompatibilityRequirementsMode.Allowed)]
    public class Main
    {
        #region -- Methods --     
        

        [WebInvoke(UriTemplate = "channel", Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public List<string> GetChannelList()
        {
            MainController controller = new MainController();            
            return controller.GetChannelList();
        }

        [WebInvoke(UriTemplate = "channel/{newChannelName}", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        public void AddChannel(string newChannelName)
        {
            MainController controller = new MainController();
            controller.AddNewChannel(newChannelName);
        }

        [WebInvoke(UriTemplate = "channel/delete", Method = "POST", ResponseFormat = WebMessageFormat.Json)]
        public void DeleteChannel()
        {
            throw new WebFaultException<string>("not implemented", HttpStatusCode.NotImplemented);
        }


        [System.Web.Mvc.HttpGet]
        [WebInvoke(UriTemplate = "message/channel/{channelName}", Method = "GET", ResponseFormat = WebMessageFormat.Json)]
        public Stream GetOldMessage(string channelName)
        {
            MainController controller = new MainController();
            string result = controller.GetPreviousMessage(channelName);
            WebOperationContext.Current.OutgoingResponse.ContentType = "application/json; charset=utf-8";
            return new MemoryStream(Encoding.UTF8.GetBytes(result.ToString()));
        }

        [WebInvoke(UriTemplate = "message", Method = "POST", RequestFormat = WebMessageFormat.Json, ResponseFormat = WebMessageFormat.Json)]
        public void SendMessage(Stream data)
        {
            string rawData = new StreamReader(data).ReadToEnd();
            MainController controller = new MainController();
            controller.SendMessage(rawData);
        }

        [OperationContract]
        [WebInvoke(Method = "OPTIONS", UriTemplate = "*")]
        void GetOptions() { }

        #endregion // -- Methods --
    }
}