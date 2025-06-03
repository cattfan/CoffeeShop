using Coffeeshop.Data;
using Coffeeshop.Models;
using Coffeeshop.Models.Interfaces;
using CoffeeShop.Models.Interfaces;

namespace CoffeeShop.Models.Services
{
    public class OrderRepository: IOrderRepository
    {
        private CoffeeshopDbContext dBcontext;
        private IShoppingCartRepository shoppingCartRepository;
        public OrderRepository(CoffeeshopDbContext dBcontext, IShoppingCartRepository shoppingCartRepository)
        {
            this.dBcontext = dBcontext;
            this.shoppingCartRepository = shoppingCartRepository;
        }
        public void PlaceOrder(Order order)
        {
            var shoppingCartItems = shoppingCartRepository.GetAllShoppingCartItems();
            order.OrderDetails = new List<OrderDetail>();
            foreach (var item in shoppingCartItems)
            {
                var orderDetail = new OrderDetail
                {
                    Quantity = item.Qty,
                    ProductId = item.Product.Id,
                    Price = item.Product.Price,
                };
                order.OrderDetails.Add(orderDetail);
            }
            order.OrderPlaced = DateTime.Now;
            order.OrderTotal = shoppingCartRepository.GetShoppingCartTotal();
            dBcontext.Orders.Add(order);
            dBcontext.SaveChanges();
        }
    }
}