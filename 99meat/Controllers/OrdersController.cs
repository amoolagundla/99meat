﻿using System;
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
            var userInfo = db.Database.SqlQuery<OrderViewModel>("GetOrders @email", new SqlParameter("@email", email == null ? (object)DBNull.Value : email)).ToList<OrderViewModel>();

            return userInfo;
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
        public async Task<IHttpActionResult> PostOrder(ViewModel.Order order)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Models.Order _order = new Models.Order();
            _order.AddressId = order.AddressId.ToString();
            _order.OrderTotal = order.Cart.ToList().Sum(x => x.Quantity * x.UnitPrice);
            _order.UserId = User.Identity.Name.ToString();
            _order.OrderStatus = "Order Placed";

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
            await db.SaveChangesAsync();

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