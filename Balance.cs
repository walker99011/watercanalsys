using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.ComponentModel.DataAnnotations;

namespace PaymentSystemWatercanal
{
    public class Balance
    {
        [Key]
        public int BalanceId { get; set; }
        public int PersonalAccount { get; set; }
        public int BalanceSum { get; set; }
        public DateTime Date { get; set; }
        public int Type { get; set; }
    }
}