﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PaymentSystemWatercanal
{
    public class Meter
    {
        [Key]
        public int MeterId { get; set; }
        public int Price { get; set; }
    }
}