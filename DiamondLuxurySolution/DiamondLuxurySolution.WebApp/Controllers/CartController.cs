using DiamondLuxurySolution.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Controllers
{
    public class CartController : Controller
    {
		[HttpPost]
		public IActionResult AddToCart([FromBody] CartItem cartItem)
		{

            if (cartItem == null)
            {
                return Json(new { success = false, message = "Có lỗi xảy ra" });
            }
            try
			{
				

				CartSessionHelper.AddToCart(cartItem);
				return Json(new { success = true, message = "Đã thêm vào giỏ hàng" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Lỗi: " + ex.Message });
			}
		}
		[HttpPost]
		public IActionResult RemoveFromCart([FromBody] CartRemoveItemRequestModel request)
		{
			try
			{
				CartSessionHelper.RemoveFromCart(request);
				return Json(new { success = true, message = "Item removed from cart successfully!" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Error removing item from cart: " + ex.Message });
			}
		}

		[HttpPost]
		public IActionResult UpdateQuantity( [FromBody] CartUpdateItemRequestModel request)
		{
			try
			{
				CartSessionHelper.UpdateQuantity(request);
				return Json(new { success = true, message = "Item quantity updated successfully!" });
			}
			catch (Exception ex)
			{
				return Json(new { success = false, message = "Error updating item quantity: " + ex.Message });
			}
		}

		public IActionResult View()
        {
            var cart = CartSessionHelper.GetCart();
            return View(cart);
        }

    }
}
