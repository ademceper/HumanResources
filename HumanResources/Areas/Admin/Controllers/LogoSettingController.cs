using HumanResources.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Data;
using System.Globalization;
using System.Security.Claims;

namespace HumanResources.Areas.Admin.Controllers
{
    [Area("Admin")]
    public class LogoSettingController : Controller

    {


        private readonly string entityName = "Ürün";

        private readonly AppDbContext context;
        private readonly IConfiguration configuration;


        public LogoSettingController(
            AppDbContext context,
            IConfiguration configuration

            )
        {
            this.context = context;
            this.configuration = configuration;

        }


        public IActionResult Index()
        {
            ViewBag.WebSetting = context.LogoSettings.OrderBy(p => p.WebLogo).ToList();
            ViewBag.AdminSetting = context.LogoSettings.OrderBy(p => p.WebLogo).ToList();

            return View();
        }

        public IActionResult Edit(Guid id)
        {
            var model = context.LogoSettings.Find(id);
            return View(model);
        }
        [Authorize(Roles = "Administrators")]
        [HttpPost]
        public async Task<IActionResult> Edit(LogoSettings model)
        {



            if (model.AdminFile is not null)
            {
                using var image = await Image.LoadAsync(model.AdminFile.OpenReadStream());


                image.Mutate(p => p.Resize(new ResizeOptions
                {
                    Size = new Size(500, 740),
                    Mode = ResizeMode.Crop
                }));

                model.AdminLogo = image.ToBase64String(JpegFormat.Instance);

            }





            context.LogoSettings.Add(model);

            context.LogoSettings.Update(model);
            context.SaveChanges();
            TempData["success"] = "Ürün güncelleme işlemi başarıyla tamamlanmıştır";
            return RedirectToAction(nameof(Index));



        }
    }



    }





    

