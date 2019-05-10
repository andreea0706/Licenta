using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FacturaOnline.Models;
using FacturaOnline.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

// For more information on enabling MVC for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace FacturaOnline.Controllers
{
    public class ProductController : Controller
    {
        private readonly AppDbContext _context;
        private readonly UserManager<Partner> _userManager;
        private List<Product> CurrentUser_Products;
        private List<Category> CurrentUser_Categories;

        public ProductController(AppDbContext context, UserManager<Partner> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        // GET: Products
        public IActionResult Index()
        {

            var id = _userManager.GetUserId(User);
            CurrentUser_Products = new List<Product>();
            CurrentUser_Categories = new List<Category>();
            IEnumerable<Product> Products = _context.Products.ToList();
            IEnumerable<Category> Categories = _context.Categories.ToList();

            foreach (Product product in Products)
            {
                if (product.IdPartner.Equals(id))
                {
                    CurrentUser_Products.Add(product);
                }
            }

            foreach (Category category in Categories)
            {
                if (category.IdPartner.Equals(id))
                {
                    CurrentUser_Categories.Add(category);
                }
            }

            ProductViewModel productModel = new ProductViewModel
            {
                Products = CurrentUser_Products,
                Categories = CurrentUser_Categories
            };

            return View(productModel);
        }

        //POST: Create Category
        [HttpPost]
        public async Task<IActionResult> CreateCategory(ProductViewModel model)
        {
            var IdPartner = _userManager.GetUserId(User);
            Category category = new Category()
            {

                Name = model.Category.Name,
                Description = model.Category.Description,
                IdPartner = IdPartner
            };    

            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }



        //POST: Create Product
        [HttpPost]
        public async Task<IActionResult> CreateProduct(ProductViewModel model)
        {
            var cat =  await _context.Categories.FirstOrDefaultAsync(m => m.Id == model.Category.Id);
            string catName = cat.Name;

            var IdPartner = _userManager.GetUserId(User);
            Product product = new Product()
            {
                Name = model.Product.Name,
                Description = model.Product.Description,
                Currency = model.Product.Currency,
                Price = model.Product.Price,
                HasVAT = model.Product.HasVAT,
                MeasureUnit = model.Product.MeasureUnit,
                Categ = catName,
                IdPartner = IdPartner
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            return RedirectToAction("Index");
        }


        // GET: Product/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }

        // POST: Edit
        [HttpPost, ActionName("Edit")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> EditPost(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }
            var productUpdate = await _context.Products.FirstOrDefaultAsync(product => product.Id == id);
            if (await TryUpdateModelAsync<Product>(
                productUpdate,
                "",
                product => product.Name, 
                product => product.Description,
                product => product.Categ,
                product => product.HasVAT,
                product => product.Price))
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
            return View(productUpdate);
        }


        // GET: Product/Delete/5
        public async Task<IActionResult> Delete(int? id, bool? saveChangesError = false)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            if (saveChangesError.GetValueOrDefault())
            {
                ViewData["ErrorMessage"] =
                    "Delete failed. Try again, and if the problem persists " +
                    "see your system administrator.";
            }

            return View(product);
        }

        // POST: Product/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return RedirectToAction(nameof(Index));
            }

            try
            {
                _context.Products.Remove(product);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            catch (DbUpdateException /* ex */)
            {
                //Log the error (uncomment ex variable name and write a log.)
                return RedirectToAction(nameof(Delete), new { id = id, saveChangesError = true });
            }
        }


        // GET: Product/Details/5
        [HttpGet]
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var product = await _context.Products.FirstOrDefaultAsync(m => m.Id == id);

            if (product == null)
            {
                return NotFound();
            }

            return View(product);
        }






    }
}
