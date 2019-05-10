using FacturaOnline.Models;
using FacturaOnline.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;
using System.Net.Http;

namespace FacturaOnline.Controllers
{
    public class CustomerController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Partner> _userManager;
        private List<Customer> CurrentUser_Customers;
        private static readonly HttpClient client = new HttpClient();

        public CustomerController(AppDbContext context, UserManager<Partner> userManager)
        {
            _context = context;
            _userManager = userManager;
           
        }


        //GET: Customers
        [HttpGet]
        public IActionResult Index()
        {
            var id = _userManager.GetUserId(User);
            CurrentUser_Customers = new List<Customer>();
            IEnumerable<Customer> Customers = _context.Customers.ToList();
           foreach(Customer customer in Customers) {
                if (customer.IdPartner.Equals(id)) {
                    CurrentUser_Customers.Add(customer);
                }
            }
            CustomerViewModel customerModel = new CustomerViewModel
            {
                Customers = CurrentUser_Customers
            };
            return View(customerModel);
        }

        //POST: Create Customer
        [HttpPost]
        public async Task<IActionResult> CreateCustomer(CustomerViewModel model)
        {
            var IdPartner = _userManager.GetUserId(User);
            Customer customer = new Customer()
            {
                FirmName = model.Customer.FirmName,
                Address = model.Customer.Address,
                Email = model.Customer.Email,
                RepName = model.Customer.RepName,
                IdPartner = IdPartner
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }

        public async Task<IActionResult> CreateCustomerAnaf(InfoAnafModel model)
        {
            var IdPartner = _userManager.GetUserId(User);
            Customer customer = new Customer()
            {
                FirmName = model.FirmName,
                Address = model.Address,
                Email = model.Email,
                IdPartner = IdPartner
            };

            _context.Customers.Add(customer);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        [HttpGet]
        [ProducesResponseType(200, Type = typeof(String))]
        [ProducesResponseType(400)]
        public async Task<IActionResult> ReturnInfoAnaf(CustomerViewModel model)
        {
            model.data = DateTime.Today.ToString("yyyy-MM-dd");
            GetInfoAnafViewModel info = new GetInfoAnafViewModel
            {
                cui = model.cui,
                data = model.data
            };
            HttpResponseMessage result = await client.PostAsJsonAsync("https://webservicesp.anaf.ro/PlatitorTvaRest/api/v3/ws/tva",
                                                        new List<GetInfoAnafViewModel>() { info });
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
            }
            else
            {
                return RedirectToAction("ErrorRegister");
            }

            return View(modelInfoAnaf);
        }


        // GET: Customer/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FindAsync(id);
            if (customer == null)
            {
                return NotFound();
            }
            return View(customer);
        }

        // GET: Customer/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var customer = await _context.Customers.FirstOrDefaultAsync(m => m.Id == id);

            if (customer == null)
            {
                return NotFound();
            }

            return View(customer);
        }

        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var customerUpdate = await _context.Customers.FirstOrDefaultAsync(customer => customer.Id == id);
            if (await TryUpdateModelAsync<Customer>(
                customerUpdate,
                "",
                customer => customer.FirmName, customer => customer.Address, customer => customer.Email))
            {
                try
                {
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException /* ex */)
                {
                    //Log the error (uncomment ex variable name and write a log.)
                    ModelState.AddModelError("", "Unable to save changes. " +
                        "Try again, and if the problem persists, " +
                        "see your system administrator.");
                }
            }
            return View(customerUpdate);
        }



        // GET: Students/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var student = await _context.Customers
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (student == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(student);
        }

        // POST: Students/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var student = await _context.Customers.FindAsync(id);
            if (student == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Customers.Remove(student);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }



    }
}
