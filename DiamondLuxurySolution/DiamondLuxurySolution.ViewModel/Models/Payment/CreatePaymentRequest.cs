﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.Payment
{
    public class CreatePaymentRequest
    {
        public string PaymentMethod { get; set; } = null!;

        public string? Description { get; set; }

        public string? Message { get; set; }

        public bool Status { get; set; }

        public string orderId {  get; set; }
    }
}