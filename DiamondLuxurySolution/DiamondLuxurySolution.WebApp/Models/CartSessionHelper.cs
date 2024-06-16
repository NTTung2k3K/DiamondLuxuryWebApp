using Newtonsoft.Json;

namespace DiamondLuxurySolution.WebApp.Models
{
    public class CartItem
    {
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public int Quantity { get; set; }
        public int? Ni { get; set; }
    }

    public static class CartSessionHelper
    {
        private static IHttpContextAccessor _httpContextAccessor;

        public static void Configure(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        public static void SetObjectAsJson(string key, object value)
        {
            _httpContextAccessor.HttpContext.Session.SetString(key, JsonConvert.SerializeObject(value));
        }

        public static T GetObjectFromJson<T>(string key)
        {
            var jsonString = _httpContextAccessor.HttpContext.Session.GetString(key);
            return jsonString == null ? default(T) : JsonConvert.DeserializeObject<T>(jsonString);
        }

        public static void AddToCart(CartItem item)
        {
            var cart = GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var existingItem = cart.FirstOrDefault(i => i.ProductId == item.ProductId && i.Ni == item.Ni);

            if (existingItem != null)
            {
                existingItem.Quantity += item.Quantity;
            }
            else
            {
                cart.Add(item);
            }

            SetObjectAsJson("Cart", cart);
        }

        public static void RemoveFromCart(string productId, int? ni)
        {
            var cart = GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var itemToRemove = cart.FirstOrDefault(i => i.ProductId == productId && i.Ni == ni);

            if (itemToRemove != null)
            {
                cart.Remove(itemToRemove);
                SetObjectAsJson("Cart", cart);
            }
        }

        public static void UpdateQuantity(string productId, int? ni, int quantity)
        {
            var cart = GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
            var itemToUpdate = cart.FirstOrDefault(i => i.ProductId == productId && i.Ni == ni);

            if (itemToUpdate != null)
            {
                if (quantity > 0)
                {
                    itemToUpdate.Quantity = quantity;
                }
                else
                {
                    cart.Remove(itemToUpdate);
                }

                SetObjectAsJson("Cart", cart);
            }
        }

        public static List<CartItem> GetCart()
        {
            return GetObjectFromJson<List<CartItem>>("Cart") ?? new List<CartItem>();
        }
    }
}
