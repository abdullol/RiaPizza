using Microsoft.AspNetCore.SignalR;
using Microsoft.AspNetCore.Session;
using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.NotifyOrder
{
    public class NotifyHub : Hub
    {
        public async Task NotifyOrder(Order order)
        {
            await Clients.All.SendAsync("notifyOrder", order);
        }
    }
}
