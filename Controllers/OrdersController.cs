using Coffeeshop.Models;
using Coffeeshop.Models.Interfaces; // Đảm bảo namespace này đúng cho Interfaces của bạn
using CoffeeShop.Models.Interfaces;
using Microsoft.AspNetCore.Authorization;
// using CoffeeShop.Models.Interfaces; // Kiểm tra nếu bạn có namespace khác cho Interfaces
using Microsoft.AspNetCore.Mvc;
using System.Linq; // Cần cho .Any()
using System.Security.Claims; // Cần cho User.FindFirstValue(ClaimTypes.NameIdentifier)

namespace CoffeeShop.Controllers // Namespace của Controller
{
    [Authorize] // Yêu cầu người dùng phải đăng nhập để truy cập controller này
    public class OrdersController : Controller
    {
        // Sử dụng readonly và quy ước đặt tên _camelCase cho private fields
        private readonly IOrderRepository _orderRepository;
        private readonly IShoppingCartRepository _shoppingCartRepository;

        // Constructor với tên tham số đã được sửa
        public OrdersController(IOrderRepository orderRepository,
                                IShoppingCartRepository shoppingCartRepository)
        {
            _orderRepository = orderRepository;
            _shoppingCartRepository = shoppingCartRepository;
        }

        // Action GET cho trang Checkout
        public IActionResult Checkout()
        {
            // Truyền một đối tượng Order mới (rỗng) vào View.
            // Điều này giúp các Tag Helper như asp-for hoạt động tốt nhất,
            // và bạn cũng có thể điền trước một số thông tin nếu muốn.
            var order = new Order();

            // Ví dụ: Nếu người dùng đã đăng nhập, bạn có thể điền trước Email
            // (nếu bạn có lưu Email trong Claims hoặc có cách khác để lấy)
            // if (User.Identity != null && User.Identity.IsAuthenticated)
            // {
            //     order.Email = User.FindFirstValue(ClaimTypes.Email);
            //     // Điền các thông tin khác như FirstName, LastName nếu có từ User Profile
            // }

            return View(order);
        }

        // Action POST để xử lý việc đặt hàng
        [HttpPost]
        [ValidateAntiForgeryToken] // Thêm để chống CSRF attacks
        public IActionResult Checkout(Order order) // 'order' này được binding từ form
        {
            // Đặt breakpoint ở đây để kiểm tra các giá trị của 'order'
            // (order.FirstName, order.LastName, ...) sau khi model binding.
            // Điều này rất quan trọng để xác nhận dữ liệu từ View Checkout.cshtml đã được gửi đúng.

            // Kiểm tra xem giỏ hàng có sản phẩm không
            var shoppingCartItems = _shoppingCartRepository.GetAllShoppingCartItems();
            if (!shoppingCartItems.Any())
            {
                // Thêm lỗi vào ModelState nếu giỏ hàng trống
                ModelState.AddModelError("", "Your shopping cart is empty. Please add some products before checking out.");
            }

            // Kiểm tra ModelState (bao gồm cả lỗi giỏ hàng trống nếu có)
            if (!ModelState.IsValid)
            {
                // Nếu ModelState không hợp lệ (ví dụ: trường bắt buộc chưa nhập, định dạng sai),
                // quay lại View Checkout với đối tượng 'order' hiện tại.
                // Điều này giúp hiển thị các thông báo lỗi validation
                // và giữ lại những dữ liệu người dùng đã nhập trên form.
                return View(order);
            }

            // Nếu bạn muốn lưu UserId của người dùng đã đăng nhập:
            // 1. Đảm bảo model 'Order.cs' có một thuộc tính để lưu UserId, ví dụ:
            //    public string? UserId { get; set; }
            // 2. Gán UserId cho đơn hàng:
            // order.UserId = User.FindFirstValue(ClaimTypes.NameIdentifier);


            // Nếu tất cả đều hợp lệ, tiến hành đặt hàng
            _orderRepository.PlaceOrder(order); // Truyền đối tượng order đã được bind dữ liệu

            // Xóa giỏ hàng và cập nhật session
            _shoppingCartRepository.ClearCart();
            HttpContext.Session.SetInt32("CartCount", 0);

            // Chuyển hướng đến trang xác nhận đặt hàng thành công
            return RedirectToAction("CheckoutComplete");
        }

        // Action cho trang xác nhận đặt hàng thành công
        public IActionResult CheckoutComplete()
        {
            // Bạn có thể truyền thông báo thành công đến View
            ViewBag.CheckoutCompleteMessage = "Thank you for your order! We've received it and will process it shortly.";
            return View();
        }
    }
}