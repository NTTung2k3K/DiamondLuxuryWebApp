﻿using DiamondLuxurySolution.ViewModel.Common;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DiamondLuxurySolution.ViewModel.Models.KnowledgeNews
{
    public class ViewKnowledgeNewsRequest : PagingRequestBase
    {
        public string? KeyWord { get; set; }
    }
}