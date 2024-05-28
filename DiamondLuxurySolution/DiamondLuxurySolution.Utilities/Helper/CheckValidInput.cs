using Azure.Core;
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
      
        public static bool ContainsNumbers(string input)
        {
            Regex regex = new Regex("[a-zA-Z]");
            return regex.IsMatch(input);
        }
        public static bool ValidPhoneNumber(string phoneNumber)
        {
            var result = Regex.IsMatch(phoneNumber, "^(09|03|07|08|05)[0-9]{8,9}$");
            return result ;
        }
        //^ [A-Z0-9._%+-]+@ [A-Z0-9.-]+. [A-Z] {2,6}$(chuỗi check email - ghi thêm cho dui)
        public static bool ValidIdentCitizenIdentityCard(string citizenCard)
        {
            var result = Regex.IsMatch(citizenCard, "^(0)[0-9]{11}$");
            return result;
        }

        public static bool IsValidEmail(string emailAddress)
        {
            var emailValidation = new EmailAddressAttribute();
            return emailValidation.IsValid(emailAddress);
        }

    }
}
