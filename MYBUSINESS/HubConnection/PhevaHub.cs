using Microsoft.AspNet.SignalR;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Mvc;

namespace MYBUSINESS.HubConnection
{
    public class PhevaHub : Hub
    {
        //public void SendProductUpdate(string productDetails)
        //{
        //    // This method will send the product updates to all connected clients
        //    Clients.All.ReceiveProductUpdate(productDetails);
        //} 
        //public void GetMessage()
        //{
        //    // This method will send the product updates to all connected clients
        //    Clients.All.ReceiveMessageDet("");
        //}
        public void getCustomerProductDetails(string ok)
        {
            Clients.All.getCustomerProductDetails(ok);
        }
    }
}
