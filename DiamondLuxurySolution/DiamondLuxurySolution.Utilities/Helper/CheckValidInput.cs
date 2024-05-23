using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Utilities.Helper
{
    public class CheckValidInput
    {
        public static bool ContainsLetters(string input)
        {
            Regex regex = new Regex("[a-zA-Z]");
            return regex.IsMatch(input);
        }
        public static bool ValidLenghPhoneNumber(string phoneNumber)
        {
            var result = phoneNumber.Length >= 10;
            return result ;
        }

        public static bool IsValidEmail(string emailAddress)
        {
            var emailValidation = new EmailAddressAttribute();
            return emailValidation.IsValid(emailAddress);
        }

    }
}
