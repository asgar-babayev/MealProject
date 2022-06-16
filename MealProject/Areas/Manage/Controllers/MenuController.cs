using MealProject.DAL;
using MealProject.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace MealProject.Areas.Manage.Controllers
{
    [Area("Manage"), Authorize]
    public class MenuController : Controller
    {
        private readonly Context _context;
        private readonly IWebHostEnvironment _env;

        public MenuController(Context context, IWebHostEnvironment env)
        {
            _context = context;
            _env = env;
        }

        public IActionResult Index()
        {
            return View(_context.Menus.Include(x => x.Category).ToList());
        }

        public IActionResult Create()
        {
            ViewBag.Categories = _context.Categories.ToList();
            return View();
        }

        [HttpPost, AutoValidateAntiforgeryToken]
        public IActionResult Create(Menu menu)
        {
            if (!ModelState.IsValid) return View(menu);
            if (menu.ImageFile != null)
            {
                if (menu.ImageFile.ContentType != "image/jpeg" && menu.ImageFile.ContentType != "image/png" && menu.ImageFile.ContentType != "image/webp")
                {
                    ModelState.AddModelError("ImageFile", "Image can be only .jpeg or .png");
                    return View(menu);
                }
                if (menu.ImageFile.Length / 1024 > 2000)
                {
                    ModelState.AddModelError("ImageFile", "Image size must be lower than 2mb");
                    return View(menu);
                }

                string filename = menu.ImageFile.FileName;
                if (filename.Length > 64)
                {
                    filename.Substring(filename.Length - 64, 64);
                }
                string newFileName = Guid.NewGuid().ToString() + filename;
                string path = Path.Combine(_env.WebRootPath, "assets", "images", newFileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    menu.ImageFile.CopyTo(stream);
                }
                menu.Image = newFileName;
                _context.Menus.Add(menu);
                _context.SaveChanges();
                return RedirectToAction("Index");
            }
            return View();
        }

        public IActionResult Edit(int id)
        {
            Menu menu = _context.Menus.Include(x => x.Category).FirstOrDefault(x => x.Id == id);
            if (menu == null) return NotFound();
            ViewBag.Categories = _context.Categories.ToList();
            return View(menu);
        }

        [HttpPost]
        public IActionResult Edit(Menu menu)
        {
            var existMenu = _context.Menus.Include(x => x.Category).FirstOrDefault(x => x.Id == menu.Id);
            if (existMenu == null) return NotFound();
            string newFileName = null;

            if (menu.ImageFile != null)
            {
                if (menu.ImageFile.ContentType != "image/jpeg" && menu.ImageFile.ContentType != "image/png" && menu.ImageFile.ContentType != "image/webp")
                {
                    ModelState.AddModelError("ImageFile", "Image can be only .jpeg or .png");
                    return View();
                }
                if (menu.ImageFile.Length > 2097152)
                {
                    ModelState.AddModelError("ImageFile", "Image size must be lower than 2mb");
                    return View();
                }
                string fileName = menu.ImageFile.FileName;
                if (fileName.Length > 64)
                {
                    fileName = fileName.Substring(fileName.Length - 64, 64);
                }
                newFileName = Guid.NewGuid().ToString() + fileName;

                string path = Path.Combine(_env.WebRootPath, "assets", "images", newFileName);
                using (FileStream stream = new FileStream(path, FileMode.Create))
                {
                    menu.ImageFile.CopyTo(stream);
                }
            }
            if (newFileName != null)
            {
                string deletePath = Path.Combine(_env.WebRootPath, "assets", "images", existMenu.Image);

                if (System.IO.File.Exists(deletePath))
                {
                    System.IO.File.Delete(deletePath);
                }

                existMenu.Image = newFileName;
            }

            existMenu.CategoryId = menu.CategoryId;
            existMenu.Name = menu.Name;
            existMenu.Descriprion = menu.Descriprion;
            existMenu.Price = menu.Price;
            _context.SaveChanges();

            return RedirectToAction("index");
        }
        public IActionResult Delete(int id)
        {
            var menu = _context.Menus.FirstOrDefault(x => x.Id == id);
            _context.Menus.Remove(menu);
            _context.SaveChanges();
            return RedirectToAction("index");
        }
    }
}
