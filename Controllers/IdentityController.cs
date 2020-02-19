using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AspNetIdentityStepByStep.Models;
using AspNetIdentityStepByStep.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AspNetIdentityStepByStep.Controllers
{
    public class IdentityController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly SignInManager<ApplicationUser> _signInManager;



        public IdentityController(UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        [HttpGet]
        public async Task<IActionResult> Signin()
        {
            var model = new SigninViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Signin(SigninViewModel model)
        {
            if (ModelState.IsValid)
            {

                var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe, false);
                if (result.Succeeded)
                {
                    return RedirectToAction("Index", "Home");
                }
                ModelState.AddModelError("Error", "Login Failed!");
            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> Signup()
        {
            var model = new SignupViewModel();
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Signup(SignupViewModel model)
        {
            if (ModelState.IsValid)
            {
                if (_userManager.FindByEmailAsync(model.Email).Result == null)
                {
                    var newUser = new ApplicationUser
                    {
                        Email = model.Email,
                        UserName = model.Email,
                        Name=model.Name
                    };
                    var result = await _userManager.CreateAsync(newUser, model.Password);
                    if (result.Succeeded)
                    {
                        return RedirectToAction(nameof(SignIn));
                    }

                    ModelState.AddModelError("Error", string.Join("", result.Errors.Select(a => a.Description)));
                    return View(model);
                }
                else
                {
                    ModelState.AddModelError("Error", $"{model.Email} has been already taken.");
                    return View(model);
                }
            }
            else
            {

            }
            return View(model);
        }

        public void AccessDenied()
        {

        }
    }
}