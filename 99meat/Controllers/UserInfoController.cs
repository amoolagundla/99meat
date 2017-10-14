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
             var userInfo = await db.Database.SqlQuery<AspNetUser>("GetUserByEmail @email", new SqlParameter("@email", User.Identity.Name) ).FirstOrDefaultAsync();

            var useroles = await db.Database.SqlQuery<UserRoles>("GetUserRoles @email", new SqlParameter("@email", userInfo.Email)).ToListAsync();

            var orders = new UserInfoViewModelWithAddresses()
            {
                Id = userInfo.Id,
                Email = userInfo.Email,
                FirstName = userInfo.FirstName,
                 LastName =userInfo.LastName,
                 PhoneNumber=userInfo.PhoneNumber,
                Addresses = await db.Database.SqlQuery<Address>("GetAddressesEmail @email", new SqlParameter("@email", userInfo.Email)).ToListAsync(),
                Orders= await db.Database.SqlQuery<ViewModel.OrderViewModel>("GetOrders @email", new SqlParameter("@email", userInfo.Email)).ToListAsync(),
                Roles= useroles.Select(x=>x.Roles).ToList(),
                minOrder =20
               

        };
            if (userInfo == null)
            {
                return NotFound();
            }

            return Ok(orders);
        }

        // GET: api/Products/5
        [ResponseType(typeof(UserInfoViewModelWithAddresses))]
        [HttpPost]
        public async Task<IHttpActionResult> SaveProfile(UserInfoViewModelWithAddresses UserInfo)
        {
            var userInfo = db.Database.SqlQuery<int>("SaveProfile @Email,@FirstName,@LastName,@PhoneNumber"
                                                  , new SqlParameter("@Email", User.Identity.Name)
                                                   , new SqlParameter("@FirstName", UserInfo.FirstName)
                                                    , new SqlParameter("@LastName", UserInfo.LastName)
                                                     , new SqlParameter("@PhoneNumber", UserInfo.PhoneNumber)
                                                  ).Single<int>();
            return Ok(userInfo);
        }

    }
}
