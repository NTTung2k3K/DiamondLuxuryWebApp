<<<<<<< HEAD
﻿using DiamondLuxurySolution.Data.Entities;
using System;
=======
﻿using System;
>>>>>>> cc97b0c5ca626e3469d8c24450e54774d316c713
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Payment
{
    public class PaymentVm
    {
        public Guid PaymentId { get; set; }
<<<<<<< HEAD

        public string PaymentMethod { get; set; } = null!;

        public string? Description { get; set; }

        public string? Message { get; set; }

        public bool Status { get; set; }
=======
        public string PaymentMethod { get; set; }
        public string Description { get; set; }
>>>>>>> cc97b0c5ca626e3469d8c24450e54774d316c713
    }
}
