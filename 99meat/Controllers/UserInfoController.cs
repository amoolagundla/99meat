using _99meat.Models;
using System;
using System.Collections.Generic;
using System.Data.SqlClient;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using static _99meat.Controllers.AccountController;

namespace _99meat.Controllers
{
    [Authorize]
    [Route("api/UserInfo")]
    public class UserInfoController : ApiController
    {
        private ApplicationDbContext db = new ApplicationDbContext();

        // GET: api/Products/5
        [ResponseType(typeof(UserInfoViewModelWithAddresses))]
        [HttpGet]
        public async Task<IHttpActionResult> GetAll()
        {
            var userInfo = db.Database.SqlQuery<AspNetUser>("GetUserByEmail @email", new SqlParameter("@email", User.Identity.Name) ).FirstOrDefault();
            var orders = new UserInfoViewModelWithAddresses()
            {
                Id = userInfo.Id,
                 Email=userInfo.Email,
                  
                Addresses = db.Database.SqlQuery<Address>("GetAddressesEmail @email", new SqlParameter("@email", userInfo.Email)).ToListAsync().Result,
                Orders=db.Database.SqlQuery<Order>("GetOrderByUserId @Id", new SqlParameter("@Id", userInfo.Id)).ToListAsync().Result

            };
            if (userInfo == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }
    }
}
