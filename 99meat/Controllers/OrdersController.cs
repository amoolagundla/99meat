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
using static _99meat.Controllers.AccountController;
using System.Data.SqlClient;
using _99meat.ViewModel;

namespace _99meat.Controllers
{
    [Authorize]
    public class OrdersController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Orders
        public IQueryable<Models.Order> GetOrders()
        {
            return db.Orders;
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
        [HttpGet]
        [Route("api/Orders/GetUserOrderDetails/{email?}")]
        public List<UserOrderDetailViewModel> GetUserOrderDetails(string email = null)
        {
            var userInfo = db.Database.SqlQuery<UserOrderDetailViewModel>("GetUserOrderDetails @Id", new SqlParameter("@Id", email == null ? (object)DBNull.Value : email)).ToList<UserOrderDetailViewModel>();
            return userInfo;
        }

        [HttpGet]
        [Route("api/Orders/UpdateOrder/{id}/{status}")]
        public async Task<IHttpActionResult> UpdateOrder(int id ,int status)
        {
            Models.Order order = await db.Orders.FindAsync(id);
            var obj =  (_99meat.Models.OrderStatus)status;
            order.OrderStatus = obj.ToString();
            db.Entry(order).State = System.Data.Entity.EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
                var pushToken = new SendNotification();
                var token = await db.PushTokens.Where(x => x.Email == order.UserId).FirstOrDefaultAsync();
                if(order.OrderStatus.ToString().Equals(OrderStatus.OrderCooked.ToString()))
                await pushToken.SendPushNotification("Order Status", "Yum Yum! your food is ready to pick up", token.token);
                else
                    await pushToken.SendPushNotification("Order Status", "Your order has been picked up", token.token);
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
            _order.OrderTotal = order.Cart.ToList().Sum(x => x.Quantity * x.UnitPrice);
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
                    Amount = cat.Amount,
                    ProductId = cat.Product.Id,
                    Quanity = cat.Quantity,
                    UnitPrice = decimal.Parse(cat.Product.Price.ToString())
                });
            }
            db.Orders.Add(_order);
            try
            {
                await db.SaveChangesAsync();
                var pushToken = new SendNotification();
                var token = await db.PushTokens.Where(x => x.Email == "sys@gmail.com").FirstOrDefaultAsync();
                await pushToken.SendPushNotification("Order Status", "You have a new order", token.token);
            }
            catch (DbUpdateConcurrencyException)
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