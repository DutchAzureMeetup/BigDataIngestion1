using System.Web.Http;
using Microsoft.AspNet.SignalR;

namespace ServiceBusUI.Controllers
{
    public class MessageController : ApiController
    {
        public void Post([FromBody]string value)
        {
            var context = GlobalHost.ConnectionManager.GetHubContext<MessageHub>();

            context.Clients.All.newMessage("hello world");
        }
    }
}
