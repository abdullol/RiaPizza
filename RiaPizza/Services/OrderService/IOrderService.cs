using RiaPizza.DTOs.Order;
using RiaPizza.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace RiaPizza.Services.OrderService
{
    public interface IOrderService
    {
        Task<int> CreateOrder(Order order);
        Task AddOrder(Order addOrder, OrderBy addOrderBy, OrderDeliveryAddress addOrderDeliveryAddress, List<OrderItem> addOrderItems);
        Task EditOrder(Order editOrder, OrderBy editOrderBy, OrderDeliveryAddress editOrderDeliveryAddress, List<OrderItem> addOrderItems);
        Task<List<Order>> TodayOrders();
        Task<int> TodayOrdersCount();
        Task<int> PendingCount();
        Task<List<Order>> PendingOrders();
        Task<int> TodayDeliveredCount();
        Task<List<Order>> TodayDeliveredOrders();
        Task<float> TodaySale();
        Task<List<Order>> InCompletedOrders();
        Task<List<Order>> AllOrders();
        Task ChangeStatus(int id, string status);
        Task<List<Order>> SearchByDate(string _dateFrom, string _dateTo);
        Task<List<Order>> SearchByStatus(string status);
        Task<List<Order>> SearchByNumber(string number);
        Task<List<Order>> SearchByDish(int DishId);
        Task<List<Order>> SearchByDishCat(int DishCatId);
        Task<List<Order>> SearchByAddress(string address);
        Task<List<Order>> SearchByUser(int userId);
        Task<List<Order>> Filter(string DF, string DT, string status, string number, string address, int DishId, int DishCatId, int userId, string payment);
        Task<Order> GetOrder(int id);
        Task<List<OrderItem>> GetOrderItems(int id);
        Task<bool> IsCompleted(int id);
        Task<string> GetOrderCode(int id);
        Task<Order> GetOrderDetails(int id);
        Task<List<Order>> GetUserOrders(int id);
        Task<List<Order>> GetUserOrdersFromCodes(List<string> codes);
        Task<List<Order>> TotalTodaySales();
        Task DeleteOrder(int id);
    }
}
