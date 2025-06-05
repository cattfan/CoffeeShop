using CoffeeShop.Models;

namespace Coffeeshop.Models
{
    public class OrderDetail
    {
        public int OrderDetailId { get; set; } // Khóa chính của OrderDetail

        public int ProductId { get; set; }     // Khóa ngoại đến Product
        public Product? Product { get; set; }   // Thuộc tính điều hướng đến Product

        // OrderId là khóa ngoại, sẽ tham chiếu đến Order.Id
        public int OrderId { get; set; }
        public Order? Order { get; set; }       // Thuộc tính điều hướng đến Order

        public int Quantity { get; set; }
        public decimal Price { get; set; }
    }
}