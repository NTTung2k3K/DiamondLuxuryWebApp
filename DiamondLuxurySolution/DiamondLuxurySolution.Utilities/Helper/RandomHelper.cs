using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Utilities.Helper
{
    public static class RandomHelper
    {
        public static string GenerateRandomString(int length)
        {
            const string chars = "ABCDEFGHIJKLMNOPQRSTUVWXYZabcdefghijklmnopqrstuvwxyz0123456789";
            StringBuilder result = new StringBuilder(length);
            Random rd = new Random();

            for (int i = 0; i < length; i++)
            {
                result.Append(chars[rd.Next(chars.Length)]);
            }

            return result.ToString();
        }
    }
}
