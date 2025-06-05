using System;
using System.Collections.Generic; // Cần cho List

namespace Coffeeshop.Models
{
    public class Order
    {
        // Khóa chính của Order là 'Id'
        public int Id { get; set; }
        public string? FirstName { get; set; }
        public string? LastName { get; set; }
        public string? Email { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public decimal OrderTotal { get; set; }
        public DateTime OrderPlaced { get; set; }

        // Khởi tạo OrderDetails để tránh lỗi null khi thêm chi tiết đơn hàng
        // và làm cho nó không còn nullable (List<OrderDetail>? -> List<OrderDetail>)
        public List<OrderDetail> OrderDetails { get; set; } = new List<OrderDetail>();
    }
}