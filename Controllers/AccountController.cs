using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using FacturaOnline.Models;
using FacturaOnline.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FacturaOnline.Controllers
{
    public class AccountController : Controller
    {

        private readonly UserManager<Partner> userManager;
        private readonly SignInManager<Partner> signInManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private static readonly HttpClient client = new HttpClient();

        public AccountController(UserManager<Partner> userManager,
                                SignInManager<Partner> signInManager,
                                RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.roleManager = roleManager;
        }

        [HttpGet]
        public IActionResult GetInfoAnaf()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ShowInfoUser()
        {
            var current_user = userManager.GetUserAsync(User);
            return View();
        }


        [HttpGet]
        [ProducesResponseType(200, Type = typeof(String))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ReturnInfoAnaf(GetInfoAnafViewModel model)
        {
            model.data = DateTime.Today.ToString("yyyy-MM-dd");
            HttpResponseMessage result = await client.PostAsJsonAsync("https://webservicesp.anaf.ro/PlatitorTvaRest/api/v3/ws/tva",
                                                        new List<GetInfoAnafViewModel>() { model });
            HttpContent content = result.Content;
            string resultContent = await content.ReadAsStringAsync();
            InfoAnafModel modelInfoAnaf = new InfoAnafModel();

            if (resultContent != null)
            {
                dynamic json = JValue.Parse(resultContent);
                dynamic requestedFirm = json.found[0];
                modelInfoAnaf = new InfoAnafModel
                {
                    cui = requestedFirm.cui,
                    Address = requestedFirm.adresa,
                    FirmName = requestedFirm.denumire
                };
            } else
            {
                return RedirectToAction("ErrorRegister");
            }

            return View(modelInfoAnaf);
        }


        [HttpPost]
        public async Task<IActionResult> Register(InfoAnafModel model)
        {
            if (!ModelState.IsValid)
                return View("Error");

            var user = new Partner()
            {
                UserName = model.Email,
                Email = model.Email,
                Address = model.Address,
                cui = model.cui,
                FirmName = model.FirmName
            };
            var result = await userManager.CreateAsync(
                user, model.Password);

             if (!await roleManager.RoleExistsAsync("Partner"))
                 await roleManager.CreateAsync(new IdentityRole { Name = "Partner" });


            await userManager.AddToRoleAsync(user, "Partner");

            if (result.Succeeded)
                return View("RegistrationConfirmation");

            foreach (var error in result.Errors)
                ModelState.AddModelError("error", error.Description);
            return View(model);
        }

        [HttpGet]
        public IActionResult Login(string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model, string returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            if (ModelState.IsValid)
            {
                var result =
                    await
                        signInManager.PasswordSignInAsync(model.Email, model.Password, model.RememberMe,
                            lockoutOnFailure: false);

                if (result.Succeeded)
                    return RedirectToAction("Dashboard", "Home");
           
                ModelState.AddModelError(string.Empty, "Invalid login attempt.");
                return View(model);

            }
            return View(model);
        }

        [HttpGet]
        public async Task<IActionResult> LogOff()
        {
            await signInManager.SignOutAsync();
            return RedirectToAction("StartPage", "Home");
        }
    }

}
