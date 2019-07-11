using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Data.Entity;

namespace PaymentSystemWatercanal
{
    public class Context : DbContext
    {
        public Context()
            : base("WaterCanal")
        { }
        public DbSet<Device> Devices { get; set; }
        public DbSet<User> Users { get; set; }
        public DbSet<Meter> Meters { get; set; }
        public DbSet<Balance> Balances { get; set; }
    }
}