using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using SLE_System.Models;
using System.Net;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Caveret.Data;
using Microsoft.Data.SqlClient;
using Microsoft.IdentityModel.Protocols;


namespace Caveret.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<IdentityUser> _userManager;
        private readonly SignInManager<IdentityUser> _signInManager;
        


        public AccountController(UserManager<IdentityUser> userManager,
                              SignInManager<IdentityUser> signInManager
                              
            )
        {
            
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View();
        }
        
        [HttpPost]
        

        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            
            if (ModelState.IsValid)
            {
                
                
                

                var user = new IdentityUser
                {
                    UserName = model.Email,
                    Email = model.Email,
                };

                var result = await _userManager.CreateAsync(user, model.Password);
                
                if (result.Succeeded)
                {
                    if (user.Email == "admin@gmail.com") {
                      await  _userManager.AddToRoleAsync(user, "Admin");
                    }
                    await _signInManager.SignInAsync(user, isPersistent: false);

                    return RedirectToAction("index", "Home");
                }

                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError("", error.Description);
                }

                ModelState.AddModelError(string.Empty, "Invalid Login Attempt");

            }
            return View(model);
        }


        [HttpGet]
        [AllowAnonymous]
        public IActionResult Login()
        {

            

            return View();
        }
        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> Login(LoginViewModel user)
        {

           
                     
           

            int flag = 0;
                //if (flag == 0)
                //{
              

                //var admin = new IdentityUser
                //    {
                //        UserName = "admin@gmail.com",
                //        Email = "admin@gmail.com",
                //    };
                //var result1 = await _userManager.CreateAsync(admin, "Ab!123");
                
                //if (result1.Succeeded)
                //    {
                //    var roleres=await _userManager.AddToRoleAsync(admin, "Admin");
                //    await _signInManager.SignInAsync(admin, isPersistent: false);

                //}

                //flag = 1;
                //}
                if (ModelState.IsValid)
            {
                var result = await _signInManager.PasswordSignInAsync(user.Email, user.Password, user.RememberMe, false);
                
                    if (result.Succeeded)
                {
                    
                    return RedirectToAction("Index", "Home");
                }
                //if (user.Email == "admin@gmail.com")
                //{
                //    if (user.Password == "Ab!123")
                //    {
                //        // await _signInManager.SignOutAsync();
                //        //await HttpContext.SignInAsync("Cookies",new ClaimsPrincipal(new ClaimsIdentity(new[] { new Claim(ClaimTypes.Role,"Admin")},"Cookies")));
                //        //var roleresult = _userManager.AddToRoleAsync, "Admin");
                //       // _userManager.AddToRoleAsync(user, "Admin");
                //        return RedirectToAction("Index", "Home");
                //       // return Json(new { succeeded = true });
                //    }


                //}
                else
                { ModelState.AddModelError(string.Empty, "Invalid Login Attempt"); }
                    

            }
            return View(user);
        }

        [AllowAnonymous]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();

            return RedirectToAction("Login");
        }
    }
}
