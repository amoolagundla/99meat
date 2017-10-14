using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using _99meat.Models;
using System.Security.Claims;
using System.Data.SqlClient;
using _99meat.ViewModel;

namespace _99meat.Controllers
{
    [Authorize]
    public class OrdersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();



        public OrdersController()
        {
        }
        // GET: api/Orders
        public IQueryable<Models.Order> GetOrders()
        {
            return db.Orders;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Orders/GetDeliveryOrders/{email?}")]
        public List<OrderViewModel> GetDeliveryOrders(string email = null)
        {
            var userInfo = db.Database.SqlQuery<OrderViewModel>("GetDeliveryOrders @email", new SqlParameter("@email", email == "null" ? (object)DBNull.Value : email)).ToList<OrderViewModel>();

            return userInfo;
        }

        [HttpGet]
        [Route("api/Orders/GetOrders/{email?}")]
        public List<OrderViewModel> GetOrders(string email = null)
        {
            var userInfo = db.Database.SqlQuery<OrderViewModel>("GetOrders @email", new SqlParameter("@email", email == "null" ? (object)DBNull.Value : email)).ToList<OrderViewModel>();

            return userInfo;
        }

        [HttpGet]
        [Route("api/Orders/GetUserOrders/{email?}")]
        public List<UserOrderViewModel> GetUserOrders(string email = null)
        {
            var userInfo = db.Database.SqlQuery<UserOrderViewModel>("GetUserOrders @email", new SqlParameter("@email", email == "null" ? (object)DBNull.Value : email)).ToList<UserOrderViewModel>();

            return userInfo;
        }

        [AllowAnonymous]
        [HttpGet]
        [Route("api/Orders/GetUserOrderDetails/{email?}")]
        public List<UserOrderDetailViewModel> GetUserOrderDetails(string email = null)
        {
            email = email == "null" ? string.Empty : email;
            var userInfo = db.Database.SqlQuery<UserOrderDetailViewModel>("GetUserOrderDetails @Id", new SqlParameter("@Id", (email == null || string.IsNullOrEmpty(email)) ? (object)DBNull.Value : email)).ToList<UserOrderDetailViewModel>();
            return userInfo;
        }
        [AllowAnonymous]
        [HttpGet]
        [Route("api/Orders/getDistance/{startAddress}/{endAdress}/{id}")]
        public async Task<string> getDistance(string startAddress, string endAdress,int id)
        {
            var returnstring = string.Empty;
            var pushToken = new SendNotification();
            var orderDistance=  await pushToken.GetDistance(startAddress, endAdress);
            var distance = string.Empty;
            var duration = string.Empty;
            var fav = new Favourite();
          
            fav.ProductId = id;
            fav.UserId = 12345;
            fav.Email = startAddress + "," + endAdress ;
            db.Favourites.Add(fav);
            await db.SaveChangesAsync();
            if (orderDistance.rows.FirstOrDefault().elements.FirstOrDefault().distance.text.ToString().Contains("mi"))
            {
                distance = orderDistance.rows.FirstOrDefault().elements.FirstOrDefault().distance.text.ToString().Replace("mi", "").TrimEnd().ToString();
            }
            else
            {
                distance = orderDistance.rows.FirstOrDefault().elements.FirstOrDefault().distance.text.ToString().Replace("ft", "").TrimEnd().ToString();
            }

            if (orderDistance.rows.FirstOrDefault().elements.FirstOrDefault().duration_in_traffic.text.ToString().Contains("min"))
            {
                duration = orderDistance.rows.FirstOrDefault().elements.FirstOrDefault().duration_in_traffic.text.ToString().Replace("min", "").Replace("s","").TrimEnd().ToString();
            }
            else
            {
                duration = orderDistance.rows.FirstOrDefault().elements.FirstOrDefault().duration_in_traffic.text.ToString().Replace("mins", "").TrimEnd().ToString();
            }

            if (!string.IsNullOrEmpty(distance))
            {
                var dist = Convert.ToInt32(duration);              
               
                if(dist <= 1)
                {
                    returnstring = "near";
                    var ddd = await UpdateOrder(id, 4, distance);
                }
                else
                {
                    var dd = await UpdateOrder(id, 2, duration);
                }
            }
            
            return returnstring;
        }

        [AllowAnonymous]
        [HttpPost]
        [Route("api/Orders/CheckLocation/")]
        public async Task<string> CheckLocation(latLong latlong)
        {
            var returnstring = string.Empty;
            var pushToken = new SendNotification();
            var distance = string.Empty;
            var duration = string.Empty;
            var orderDistance = await pushToken.reverseGeocoding(latlong.lat , latlong.lon);
           
            if(orderDistance.results[2].address_components[1].long_name.ToLower().ToString().Equals("delaware"))
            {
                returnstring = orderDistance.results[2].formatted_address.ToString();
            }
            return returnstring;
        }
        [HttpGet]
        [Route("api/Orders/UpdateOrder/{id}/{status}/{time}")]
        public async Task<IHttpActionResult> UpdateOrder(int id, int status,string time)
        {
            Models.Order order = await db.Orders.FindAsync(id);
            var previousOrderStatus = order.OrderStatus;
            var obj = (_99meat.Models.OrderStatus)status;
            order.OrderStatus = obj.ToString();
            db.Entry(order).State = System.Data.Entity.EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                string orderStatus = string.Empty;
                var pushToken = new SendNotification();
                var token = await db.PushTokens.Where(x => x.Email == order.UserId).FirstOrDefaultAsync();
                var userInfo = await db.Database.SqlQuery<AspNetUser>("GetUserByEmail @email", new SqlParameter("@email", order.UserId)).FirstOrDefaultAsync();
                if (previousOrderStatus != OrderStatus.StartedToDestination.ToString() && status == (int)OrderStatus.StartedToDestination)
                {
                    await pushToken.SendPushNotification("Order Status", string.Format("Your order will be arriving shortly in {0} mins", time), token?.token, userInfo.PhoneNumber);
                }
                else
                {
                    if (status == (int)_99meat.Models.OrderStatus.ReadyToPickup)
                    {
                        await pushToken.SendPushNotification("Order Status", "Your Order is Ready To PickUp", token?.token, userInfo.PhoneNumber);
                    }
                    else if (status == (int)_99meat.Models.OrderStatus.Arrived)
                    {
                        await pushToken.SendPushNotification("Order Status", string.Format("Your order is here", time), token?.token, userInfo.PhoneNumber);
                    }
                }
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);

        }

        // GET: api/Orders/5
        [ResponseType(typeof(Models.Order))]
        public async Task<IHttpActionResult> GetOrder(int id)
        {
            Models.Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            return Ok(order);
        }

        // PUT: api/Orders/5
        [ResponseType(typeof(void))]
        public async Task<IHttpActionResult> PutOrder(int id, Models.Order order)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != order.Id)
            {
                return BadRequest();
            }

            db.Entry(order).State = System.Data.Entity.EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!OrderExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        // POST: api/Orders
        [ResponseType(typeof(Models.Order))]
        public async Task<IHttpActionResult> PostOrder(ViewModel.OrderView order)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Models.Order _order = new Models.Order();
            _order.AddressId = order.AddressId.ToString();
            _order.OrderTotal = order.Cart.ToList().Sum(x => x.Quantity * x.Product.Price);
            _order.UserId = User.Identity.Name.ToString();
            _order.OrderStatus = OrderStatus.OrderPlaced.ToString();

            _order.DeliveryTime = DateTime.Parse(order.DeliveryTime).TimeOfDay.ToString();
            _order.modPayment = order.PaymentMethod.ToString();
            _order.OrderDate = DateTime.Parse(order.DeliveryDate);
            _order.OrderItems = new List<OrderDetail>();
            foreach (var cat in order.Cart)
            {
                _order.OrderItems.Add(new OrderDetail()
                {
                    Amount = cat.Product.Price * cat.Quantity,
                    ProductId = cat.Product.Id,
                    Quanity = cat.Quantity,
                    UnitPrice = decimal.Parse(cat.Product.Price.ToString()),
                    spicyLevel = cat.Product.SpicyLevel,
                    instructions = cat.Product.Instructions
                });
            }
            db.Orders.Add(_order);
            try
            {
                 db.SaveChanges();
                var pushToken = new SendNotification();
                var token = await db.PushTokens.Where(x => x.Email == "sys@gmail.com").FirstOrDefaultAsync();
                if (token != null)
                {
                    await pushToken.SendPushNotification("Order Status", "You have a new order", token.token);
                }
            }
            catch (Exception ex)
            {
                throw;
            }
            return CreatedAtRoute("DefaultApi", new { id = _order.Id }, order);
        }

        // DELETE: api/Orders/5
        [ResponseType(typeof(Models.Order))]
        public async Task<IHttpActionResult> DeleteOrder(int id)
        {
            Models.Order order = await db.Orders.FindAsync(id);
            if (order == null)
            {
                return NotFound();
            }

            db.Orders.Remove(order);
            await db.SaveChangesAsync();

            return Ok(order);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool OrderExists(int id)
        {
            return db.Orders.Count(e => e.Id == id) > 0;
        }
    }
}