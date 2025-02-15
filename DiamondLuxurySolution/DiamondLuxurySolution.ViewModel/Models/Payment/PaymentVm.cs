﻿using DiamondLuxurySolution.Data.Entities;
using System;
﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Payment
{
    public class PaymentVm
    {
        public Guid PaymentId { get; set; }
        public string PaymentMethod { get; set; } =null!;

        public string? Description { get; set; }
        public bool Status { get; set; }
    }
}
