using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Caveret.Models;
using Microsoft.AspNetCore.Authorization;

namespace Caveret.Controllers
{
    public class AdminController : Controller
    {
        
        private readonly RoleManager<IdentityRole> rolemanager;
        private readonly UserManager<IdentityUser> userManager;

        public AdminController(RoleManager<IdentityRole> rolemanager, UserManager<IdentityUser> userManager) {
            this.rolemanager = rolemanager;
            this.userManager = userManager;
            

        }
        public IActionResult Index()
        {
            if (!User.IsInRole("IT"))
            {

                return View("../Home/Unauthorize");
            }

            return View();
        }
        public IActionResult Create()
        
        {
            if (!User.IsInRole("IT"))
            {

                return View("../Home/Unauthorize");
            }
            return View();
        }

        [HttpPost]
        [Authorize(Roles = "IT")]
        public async Task <IActionResult> Create(ProjectRole rolee) {
            var roleExist = await rolemanager.RoleExistsAsync(rolee.RoleName);
            if (!roleExist) {
                

                var result = await rolemanager.CreateAsync(new IdentityRole(rolee.RoleName));
               

            }

            return View();
        }
    }
}
