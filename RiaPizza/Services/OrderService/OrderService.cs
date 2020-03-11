using Microsoft.EntityFrameworkCore;
using RiaPizza.Data;
using RiaPizza.DTOs.Order;
using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.OrderService
{
    public class OrderService : IOrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }
        public async Task<Order> GetOrder(int id)
        {
            var order = await _context.Orders.
                Include(s => s.OrderBy).
                Include(s => s.OrderDeliveryAddress).
                Include(s => s.OrderItems).
                    ThenInclude(s => s.Dish).
                SingleOrDefaultAsync(s => s.OrderId == id);
            return order;
        }
        public async Task<List<OrderItem>> GetOrderItems(int id)
        {
            var orderItems = await _context.OrderItems.Where(s => s.OrderId == id).Include(s=>s.Dish).ToListAsync();
            return orderItems;
        }
        public async Task<int> CreateOrder(Order order)
        {
            try
            {
                order.OrderCode = await GenerateOrderCode();
                order.IsCompleted = false;
                order.OrderStatus = "Pending";
                order.OrderDateTime = DateTime.Now;
                await _context.Orders.AddAsync(order);
                await _context.SaveChangesAsync();

                return order.OrderId;
            }
            catch (Exception ex)
            {
                return 0;
            }
        }
        private async Task<string> GenerateOrderCode()
        {
            try
            {
                var previousOrder = await _context.Orders.MaxAsync(s => s.OrderId);
                var code = "00" + previousOrder;
                return code;
            }
            catch (Exception ex)
            {
                return "001";
            }
        }
        public async Task AddOrder(Order addOrder,OrderBy addOrderBy,OrderDeliveryAddress addOrderDeliveryAddress,List<OrderItem> addOrderItems)
        {

           await _context.Orders.AddAsync(addOrder);
           await _context.SaveChangesAsync();
           var OrderId= _context.Orders.Max(x => x.OrderId);

            addOrderBy.OrderId = OrderId;
            addOrderDeliveryAddress.OrderId = OrderId;
            foreach (var item in addOrderItems)
            {
                item.OrderId = OrderId;
            }

           await  AddOrderBy(addOrderBy);
           await  AddOrderDeliveryAddress(addOrderDeliveryAddress);
           await  AddOrderItem(addOrderItems);
        }
        private async Task AddOrderBy(OrderBy addOrderBy) {
           await _context.OrderBy.AddAsync(addOrderBy);
           await _context.SaveChangesAsync();
        }
        private async Task AddOrderDeliveryAddress(OrderDeliveryAddress addOrderDeliveryAddress) {
           await  _context.OrderDeliveryAddresses.AddAsync(addOrderDeliveryAddress);
           await  _context.SaveChangesAsync();
        }
        private async Task AddOrderItem(List<OrderItem> addOrderItem) {
            foreach (var item in addOrderItem)
            {
               await _context.OrderItems.AddAsync(item);
                await _context.SaveChangesAsync();
            }
        }
        public async Task<List<Order>> AllOrders()
        {
            List<Order> allOrders = await _context.Orders.Include(s=>s.OrderBy).ToListAsync();
            return allOrders;
        }
        public async Task<int> TodayOrdersCount() {
            var todayOrderCount = await _context.Orders.Where(s=>s.OrderDateTime.Year == DateTime.Now.Year && s.OrderDateTime.Month == DateTime.Now.Month && s.OrderDateTime.Day == DateTime.Now.Day).CountAsync();
            return todayOrderCount;
        }
        public async Task<List<Order>> TodayOrders() {
            var todayOrders = await _context.Orders.Where(s => s.OrderDateTime.Year == DateTime.Now.Year && s.OrderDateTime.Month == DateTime.Now.Month && s.OrderDateTime.Day == DateTime.Now.Day)
                .Include(s => s.OrderBy).ToListAsync();
            return todayOrders;
        }
        public async Task<int> PendingCount() {
            var pendingCount = await _context.Orders.Where(x => x.OrderStatus == "Pending").CountAsync();
            return pendingCount;
        }
        public async Task<List<Order>> PendingOrders() { 
            var pendingOrders= await _context.Orders.Include(s=>s.OrderBy).Where(x => x.OrderStatus == "Pending").ToListAsync();
            return pendingOrders;
        }
        public async Task<int> TodayDeliveredCount() {
            var todayDeliveredCount = await _context.Orders.Where(s => s.OrderDateTime.Year == DateTime.Now.Year && s.OrderDateTime.Month == DateTime.Now.Month && s.OrderDateTime.Day == DateTime.Now.Day && s.OrderStatus == "Delivered").CountAsync();
            return todayDeliveredCount;
        }
        public async Task<List<Order>> TodayDeliveredOrders() {
            var todayDeliveredOrders = await _context.Orders.Where(s => s.OrderDateTime.Year == DateTime.Now.Year && s.OrderDateTime.Month == DateTime.Now.Month && s.OrderDateTime.Day == DateTime.Now.Day && s.OrderStatus == "Delivered")
                .Include(s => s.OrderBy).ToListAsync();
            return todayDeliveredOrders;
        }
        public async Task<int> TodaySale() {
           var todaySale=await _context.Orders.Where(s => s.OrderDateTime.Year == DateTime.Now.Year && s.OrderDateTime.Month == DateTime.Now.Month && s.OrderDateTime.Day == DateTime.Now.Day && s.IsCompleted == true)
                .Select(s=>s.TotalBill).SumAsync();
            return todaySale;
        }
        public async Task<List<Order>> TotalTodaySales()
        {
            var totalSales=await _context.Orders.Where(s=>s.OrderDateTime.Year==DateTime.Now.Year && s.OrderDateTime.Month==DateTime.Now.Month && s.OrderDateTime.Day==DateTime.Now.Day && s.OrderStatus== "Confirmed")
                .Include(s => s.OrderBy).ToListAsync();
            return totalSales;
        }
        public async Task<List<Order>> InCompletedOrders()
        {
            var uncompletedOrders = await _context.Orders.Where(x=>x.IsCompleted==false).Include(s => s.OrderBy).ToListAsync();
            return uncompletedOrders;
        }
        public async Task ChangeStatus(int id, string status)
        {
            var order = await _context.Orders.FindAsync(id);
            if (status == "Delivered")
            {
                order.IsPaymentConfirmed = true;
                order.IsCompleted = true;
            }
            order.OrderStatus = status;
            _context.Orders.Update(order);
            await _context.SaveChangesAsync();
        }
        public async Task EditOrder(Order editOrder, OrderBy editOrderBy, OrderDeliveryAddress editOrderDeliveryAddress, List<OrderItem> addOrderItems)
        {
            var EditOrder = await _context.Orders.FindAsync(editOrder.OrderId);
            editOrderBy.OrderId = EditOrder.OrderId;
            EditOrder.RecievingTime =      editOrder.RecievingTime;
            EditOrder.OrderCode =          editOrder.OrderCode;
            EditOrder.Remarks =            editOrder.Remarks;
            EditOrder.TotalBill =          editOrder.TotalBill;
            EditOrder.PaymentMethod =      editOrder.PaymentMethod;
            EditOrder.IsPaymentConfirmed = editOrder.IsPaymentConfirmed;
            EditOrder.OrderStatus =        editOrder.OrderStatus;
            EditOrder.IsCompleted =        editOrder.IsCompleted;


            _context.Orders.Update(EditOrder);
           await _context.SaveChangesAsync();
        }
        private async Task UpdateOrderBy(OrderBy editOrderBy) {
            var EditOrderBy = await _context.OrderBy.FindAsync(editOrderBy.OrderId);

            EditOrderBy.Name =    editOrderBy.Name;
            EditOrderBy.Email =   editOrderBy.Email;
            EditOrderBy.Contact = editOrderBy.Contact;
            EditOrderBy.Company = editOrderBy.Company;

            _context.OrderBy.Update(EditOrderBy);
           await _context.SaveChangesAsync();
        }
        private async Task UpdateOrderDeliveryAddress(OrderDeliveryAddress editOrderDeliveryAddress) {
            var EditOrderDeliveryAddress = await _context.OrderDeliveryAddresses
                                                 .FindAsync(editOrderDeliveryAddress.OrderId);

            EditOrderDeliveryAddress.City =       editOrderDeliveryAddress.City;
            EditOrderDeliveryAddress.Floor =   editOrderDeliveryAddress.Floor;
            EditOrderDeliveryAddress.Address =    editOrderDeliveryAddress.Address;
            EditOrderDeliveryAddress.PostalCode = editOrderDeliveryAddress.PostalCode;

            _context.OrderDeliveryAddresses.Update(EditOrderDeliveryAddress);
           await _context.SaveChangesAsync();
        }
        public async Task<List<Order>> SearchByDate(string _dateFrom, string _dateTo)
        {
            DateTime dateFrom, dateTo;
            var orders = new List<Order>();
            if (_dateFrom != null && _dateTo == null)
            {
                dateFrom = Convert.ToDateTime(_dateFrom);
                orders = await _context.Orders.Where(s => s.OrderDateTime >= dateFrom).Include(s => s.OrderBy).ToListAsync();
            }
            else if (_dateFrom == null && _dateTo != null)
            {
                dateTo = Convert.ToDateTime(_dateTo);
                orders = await _context.Orders.Where(s => s.OrderDateTime <= dateTo).Include(s => s.OrderBy).ToListAsync();
            }
            else if (_dateFrom != null && _dateTo != null)
            {
                dateFrom = Convert.ToDateTime(_dateFrom);
                dateTo = Convert.ToDateTime(_dateTo);
                orders = await _context.Orders.Where(s => s.OrderDateTime >= dateFrom && s.OrderDateTime <= dateTo).Include(s => s.OrderBy).ToListAsync();
            }
            return orders;
        }
        public async Task<List<Order>> SearchByStatus(string status)
        {
            var orderStatus = new List<Order>();
            orderStatus = await _context.Orders.Where(s => s.OrderStatus == status).Include(s => s.OrderBy).ToListAsync();
            return orderStatus;
        }
        public async Task<bool> IsCompleted(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            return order.IsCompleted;
        }
        public async Task<string> GetOrderCode(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            return order.OrderCode;
        }
        public async Task<OrderDetailsDto> GetOrderDetails(int id)
        {
            var order = await GetOrder(id);
            order.OrderItems.ToList().ForEach(s => s.Order = null);
            order.OrderBy.Order = null;
            order.OrderDeliveryAddress.Order = null;
            var orderDetails = new OrderDetailsDto
            {
                Address = order.OrderDeliveryAddress,
                OrderBy = order.OrderBy,
                OrderItems = order.OrderItems
            };
            return orderDetails;
        }
        public async Task<List<Order>> GetUserOrders(int id)
        {
            var orders = await _context.Orders.Where(s => s.UserId == id).Include(s=>s.OrderItems).ThenInclude(s=>s.Dish).ToListAsync();
            orders.ForEach(s => s.OrderItems.ToList().ForEach(s => s.Order = null));
            return orders;
        }
        public async Task DeleteOrder(int id)
        {
            Order order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
        }
    }     
}          
