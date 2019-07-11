using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Services;
using System.Data.Entity;
using System.Web.Script.Serialization;
//using System.LINQ;

namespace PaymentSystemWatercanal
{
    /// <summary>
    /// Сводное описание для PaymentSystem
    /// </summary>
    [WebService(Namespace = "http://tempuri.org/")]
    [WebServiceBinding(ConformsTo = WsiProfiles.BasicProfile1_1)]
    [System.ComponentModel.ToolboxItem(false)]
    // Чтобы разрешить вызывать веб-службу из скрипта с помощью ASP.NET AJAX, раскомментируйте следующую строку. 
    [System.Web.Script.Services.ScriptService]
    public class PaymentSystem : System.Web.Services.WebService
    {
        [WebMethod]
        public void GetUser(int id)
        {
            using (Context db = new Context())
            {
                User user = db.Users.Find(id);
                if (user == null)
                {
                    Context.Response.Write("401");
                }
                else
                {
                    JavaScriptSerializer js = new JavaScriptSerializer();
                    Context.Response.Write("200 " + js.Serialize(user));
                }
            }
        }

        [WebMethod]
        public void GetDevice(int id, int meter)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(js.Serialize(GetLastDevice(id,meter)));
        }

        [WebMethod]
        public void GetBalance (int id)
        {
            JavaScriptSerializer js = new JavaScriptSerializer();
            Context.Response.Write(200 + js.Serialize(GetLastBalance(id)));
        }

        [WebMethod]
        public void SetDeviceStats (string inpId, string inpStats, string inpMeter) // изменение показаний => снятие денег
        {
            DateTime currentDate = DateTime.Today;
            int id, stats, meter, prevStats;
            Int32.TryParse(inpId,out id);
            Int32.TryParse(inpStats, out stats);
            Int32.TryParse(inpMeter, out meter);
            Device prevDevice = GetLastDevice(id, meter);
            if (prevDevice == null)
                prevStats = 0;
            else
                prevStats = prevDevice.PreviousStats;
            using (Context db = new Context())
            {               
                Device device = new Device { PersonalAccount = id, Meter = meter, PreviousStats = stats + prevStats, Date = currentDate };
                db.Devices.Add(device);
                db.SaveChanges();
            }
            SetBalanceStats(id, stats, meter, -stats);
        }

        [WebMethod]
        public void SetBalanceRefill (string inpId, string inpBalance) // пополнение
        {
            DateTime currentDate = DateTime.Today;
            int id, newBalance;
            Int32.TryParse(inpId, out id);
            Int32.TryParse(inpBalance, out newBalance);
            using (Context db = new Context())
            {
                Balance balance = GetLastBalance(id);
                balance.BalanceSum += newBalance;
                balance.Date = currentDate;
                balance.Type = 1;
                db.Balances.Add(balance);
                db.SaveChanges();
            }
        } 

        public void SetBalanceStats(int id, int stats, int meter, int balance) // снятие денег
        {
            DateTime date = DateTime.Today;
            Balance prevBalance = GetLastBalance(id);
            Meter priceMeter = GetMeter(meter);
            using (Context db = new Context())
            {
                Balance newBalance = new Balance { PersonalAccount = id, BalanceSum = prevBalance.BalanceSum + balance * priceMeter.Price, Date = date, Type = 2 };
                db.Balances.Add(newBalance);
                db.SaveChanges();
            }
        } 

        public Device GetLastDevice(int id, int meter)
        {
            using (Context db = new Context())
            {
                int reqId = db.Devices.Where(b => b.PersonalAccount == id && b.Meter == meter)
                    .Max(b => b.DeviceId);
                Device reqDevice = db.Devices.FirstOrDefault(b => b.DeviceId == reqId);
                return reqDevice;
            }
        }

        public Balance GetLastBalance(int id)
        {
            using (Context db = new Context())
            {
                int reqId = db.Balances.Where(b => b.PersonalAccount == id)
                    .Max(b => b.BalanceId);
                Balance reqBalance = db.Balances.FirstOrDefault(b => b.BalanceId == reqId);
                return reqBalance;
            }
        }

        public Meter GetMeter(int id)
        {
            using (Context db = new Context())
            {
                Meter meter = db.Meters.Find(id);
                return meter;
            }
        }

        /* <!-- Первоначальное добавление данных -->
        [WebMethod]
        public void Add()
        {
            using (Context db = new Context())
            {
                DateTime date1 = new DateTime(2019, 07, 01);
                Balance balance1 = new Balance { PersonalAccount = 1, BalanceSum = 0, Date = date1, Type = 1 };
                Balance balance2 = new Balance { PersonalAccount = 2, BalanceSum = 0, Date = date1, Type = 1 };
                Balance balance3 = new Balance { PersonalAccount = 3, BalanceSum = 0, Date = date1, Type = 1 };
                //Balance balance4 = new Balance { PersonalAccount = 1, BalanceSum = -500, Date = date1, Type = 1 };
                //Balance balance5 = new Balance { PersonalAccount = 1, BalanceSum = -1000, Date = date1, Type = 1 };
                db.Balances.AddRange(new List<Balance> { balance1, balance2, balance3 });
                db.SaveChanges();
                User user1 = new User { Name = "Михаил", Surname = "Попов", Patronymic = "Евгеньевич" };
                User user2 = new User { Name = "Антон", Surname = "Лукьянченко", Patronymic = "Владимирович" };
                User user3 = new User { Name = "Маргарита", Surname = "Корыстова", Patronymic = "Алексеевна" };
                db.Users.AddRange(new List<User> { user1, user2, user3 });
                db.SaveChanges();
                Meter meter1 = new Meter { MeterId = 0, Price = 10 };
                Meter meter2 = new Meter { MeterId = 1, Price = 20 };
                db.Meters.AddRange(new List<Meter> { meter1, meter2 });
                db.SaveChanges();
                Device device1 = new Device { PersonalAccount = 1, Meter = 1, PreviousStats = 0, Date = date1 };
                Device device2 = new Device { PersonalAccount = 2, Meter = 1, PreviousStats = 0, Date = date1 };
                Device device3 = new Device { PersonalAccount = 3, Meter = 1, PreviousStats = 0, Date = date1 };
                db.Devices.AddRange(new List<Device> { device1, device2, device3 });
                db.SaveChanges();
            }
        }
        //*/
    }
}
