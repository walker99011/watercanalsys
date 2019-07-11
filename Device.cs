using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PaymentSystemWatercanal
{
    public class Device
    {
        [Key]
        public int DeviceId { get; set; }
        public int PersonalAccount { get; set; }
        public int Meter { get; set; }
        public int PreviousStats { get; set; }
        public DateTime Date { get; set; }
    }
}