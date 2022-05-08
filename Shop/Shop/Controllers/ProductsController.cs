using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Shop.Data;
using Shop.Data.Entities;
using Shop.Helpers;
using Shop.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Shop.Controllers
{
    [Authorize(Roles = "Admin")]
    public class ProductsController: Controller 
    {
        private readonly DataContext _context;
        private readonly ICombosHelper _combosHelper;
        private readonly IBlopHelper _blopHelper;

        public ProductsController(DataContext context, ICombosHelper combosHelper, IBlopHelper blobHelper)
        {
            _context = context;
            _combosHelper = combosHelper;
            _blopHelper = blobHelper;
        }

        [HttpGet]
        public async Task<IActionResult> Index()
        {
            return View(await _context.Products
                .Include(p => p.ProductImages)
                .Include(p => p.ProductCategories)
                .ThenInclude(pc => pc.Category)
                .ToListAsync());
        }

        [HttpGet]
        public async Task<IActionResult> Create()
        {
            CreateProductViewModel model = new()
            {
                Categories = await _combosHelper.GetComboCategoriesAsync(),
            };

            return View(model);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CreateProductViewModel model)
        {
            if (ModelState.IsValid)
            {
                Guid imageId = Guid.Empty;
                if (model.ImageFile != null)
                {
                    imageId = await _blopHelper.UploadBlobAsync(model.ImageFile, "products");
                }

                Product product = new()
                {
                    Description = model.Description,
                    Name = model.Name,
                    Price = model.Price,
                    Stock = model.Stock,
                };

                product.ProductCategories = new List<ProductCategory>()
        {
            new ProductCategory
            {
                Category = await _context.Categories.FindAsync(model.CategoryId)
            }
        };

                if (imageId != Guid.Empty)
                {
                    product.ProductImages = new List<ProductImage>()
            {
                new ProductImage { ImageId = imageId }
            };
                }

                try
                {
                    _context.Add(product);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateException dbUpdateException)
                {
                    if (dbUpdateException.InnerException.Message.Contains("duplicate"))
                    {
                        ModelState.AddModelError(string.Empty, "Ya existe un producto con el mismo nombre.");
                    }
                    else
                    {
                        ModelState.AddModelError(string.Empty, dbUpdateException.InnerException.Message);
                    }
                }
                catch (Exception exception)
                {
                    ModelState.AddModelError(string.Empty, exception.Message);
                }
            }

            model.Categories = await _combosHelper.GetComboCategoriesAsync();
            return View(model);
        }


    }
}
