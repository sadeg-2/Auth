using Auth.Web.Data;
using Auth.Web.Models;
using Auth.Web.ViewModel;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System.Linq;

namespace Auth.Web.Controllers
{
    public class UserController : Controller
    {
        private ApplicationDbContext db;
        private UserManager<User> userManager;

        public UserController(ApplicationDbContext db, UserManager<User> userManager)
        {
            this.db = db;
            this.userManager = userManager;
        }


        public IActionResult Index()
        {
            var users = db.Users.Where(x => !x.IsDeleted).
                Select(x => new UserViewModel() { 
                    Id = x.Id ,
                    CreatedAt = x.CreatedAt ,
                    Email = x.Email ,
                    PhoneNumber = x.PhoneNumber ,
                    UserName = x.UserName ,
                }).ToList();

            return View(users);
        }

        [HttpGet]
        public IActionResult Create() {
            // email , phone , password

            return View();
        }

        [HttpPost]
        public IActionResult Create(CreateUserViewModel input)
        {
            // email , phone , password

            return View();
        }
    }
}
