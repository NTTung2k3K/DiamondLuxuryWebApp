using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.AdminCrewApp.Models
{
    public static class StaffSessionHelper
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

    }
}
