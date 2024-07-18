using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.Utilities.Helper
{
    public class Settings
    {
        public class FirebaseSettings
        {
            public string AuthApiKey { get; set; }
            public string Email { get; set; }
            public string Password { get; set; }
            public string Bucket { get; set; }
        }

        public class EmailSettings
        {
            public string Password { get; set; }
            public string Email { get; set; }
        }

    }
}
