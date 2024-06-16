using DiamondLuxurySolution.WebApp.Models;
using Microsoft.AspNetCore.Mvc;

namespace DiamondLuxurySolution.WebApp.Controllers
{
    public class CartController : Controller
    {
        public IActionResult AddToCart(string productId, string productName, int quantity, int? ni)
        {
            var item = new CartItem
            {
                ProductId = productId,
                ProductName = productName,
                Quantity = quantity,
                Ni = ni
            };

            CartSessionHelper.AddToCart(item);

            return RedirectToAction("Index");
        }

        public IActionResult RemoveFromCart(string productId, int? ni)
        {
            CartSessionHelper.RemoveFromCart(productId, ni);
            return RedirectToAction("Index");
        }

        public IActionResult UpdateQuantity(string productId, int? ni, int quantity)
        {
            CartSessionHelper.UpdateQuantity(productId, ni, quantity);
            return RedirectToAction("Index");
        }

        public IActionResult Cart()
        {
            var cart = CartSessionHelper.GetCart();
            return View(cart);
        }

    }
}
