using HumanResources.Areas.Admin.Models;
using HumanResources.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using SixLabors.ImageSharp.Processing;
using System.Data;
using System.Globalization;
using System.Security.Claims;

namespace HumanResources.Areas.Admin.Controllers;

[Area("Admin")]
public class AdvertController : Controller
{
 
    private readonly string entityName = "Ürün";

    private readonly AppDbContext context;
    private readonly IConfiguration configuration;
  

    public AdvertController(
        AppDbContext context,
        IConfiguration configuration
       
        )
    {
        this.context = context;
        this.configuration = configuration;
        
    }

    public IActionResult Index()
    {
        var model = context.Adverts.OrderBy(p => p.Appellation).ToList();

        return View(model);
    }
    public async Task<IActionResult> TableData(Guid? id, DataTableParameters parameters)
    {
        var query = context.Adverts;

        var result = new DataTableResult
        {
            data = await query
                .Skip(parameters.Start)
                .Take(parameters.Length)
                .Select(p => new
                {
                    p.Id,
                    p.Appellation,
                    p.Logo,
                    p.Flag,
                   
                    p.Salary,
                    categoryName = p.Category!.Name,
                    UserName = p.CreatorUser!.Name,                  
                    dateCreated = p.DateCreated.ToLocalTime().ToShortDateString(),
                }).ToListAsync(),
            draw = parameters.Draw,
            recordsFiltered = await query.CountAsync(),
            recordsTotal = await query.CountAsync()
        };

        return Json(result);
    }
    public async Task<IActionResult> Create()
    {
        ViewBag.Categories = new SelectList(await context.Categories.OrderBy(p => p.Name).ToListAsync(), "Id", "Name");

        return View();
    }
    [HttpPost]
    
    public async Task<IActionResult> Create(Advert model)
    {
        ViewBag.Categories = new SelectList(await context.Categories.OrderBy(p => p.Name).ToListAsync(), "Id", "Name");


        if (model.LogoFile is not null)
        {
            using var image = await Image.LoadAsync(model.LogoFile.OpenReadStream());


            image.Mutate(p => p.Resize(new ResizeOptions
            {
                Size = new Size(200, 200),
                Mode = ResizeMode.Crop
            }));

            model.Logo = image.ToBase64String(JpegFormat.Instance);

        }
        if (model.FlagFile is not null)
        {
            using var image = await Image.LoadAsync(model.FlagFile.OpenReadStream());


            image.Mutate(p => p.Resize(new ResizeOptions
            {
                Size = new Size(200, 200),
                Mode = ResizeMode.Crop
            }));

            model.Flag = image.ToBase64String(JpegFormat.Instance);

        }
        model.Salary = decimal.Parse(model.SalaryText, CultureInfo.CreateSpecificCulture("tr-TR"));



        try
        {

            model.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        context.Adverts.Add(model);
        context.SaveChanges();
        TempData["success"] = "Ürün ekleme işlemi başarıyla tamamlanmıştır";
        return RedirectToAction(nameof(Index));

        }
        catch (DbUpdateException)
        {
            TempData["error"] = "Şirket Logosu Ve Bayrak Alanı boş Bırakılamaz";
            return View(model);
        }



    }
    public async Task< IActionResult >Edit(Guid id)
    {
        ViewBag.Categories = new SelectList(await context.Categories.OrderBy(p => p.Name).ToListAsync(), "Id", "Name");

        var model = context.Adverts.Find(id);
        model.SalaryText = model.Salary.ToString("n2", CultureInfo.CreateSpecificCulture("tr-TR"));
        return View(model);
    }
    [Authorize(Roles = "Administrators")]
    [HttpPost]
    public async Task<IActionResult> Edit(Advert model)
    {



        if (model.LogoFile is not null)
        {
            using var image = await Image.LoadAsync(model.LogoFile.OpenReadStream());


            image.Mutate(p => p.Resize(new ResizeOptions
            {
                Size = new Size(250, 250),
                Mode = ResizeMode.Crop
            }));

            model.Logo = image.ToBase64String(JpegFormat.Instance);

        }
        if (model.FlagFile is not null)
        {
            using var image = await Image.LoadAsync(model.FlagFile.OpenReadStream());


            image.Mutate(p => p.Resize(new ResizeOptions
            {
                Size = new Size(250, 250),
                Mode = ResizeMode.Crop
            }));

            model.Flag = image.ToBase64String(JpegFormat.Instance);

        }

        model.Salary = decimal.Parse(model.SalaryText, CultureInfo.CreateSpecificCulture("tr-TR"));

        model.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        context.Adverts.Add(model);

        context.Adverts.Update(model);
        context.SaveChanges();
        TempData["success"] = "Ürün güncelleme işlemi başarıyla tamamlanmıştır";
        return RedirectToAction(nameof(Index));





    }
    [Authorize(Roles = "Administrators")]
    public IActionResult Delete(Guid id)
    {
        var model = context.Adverts.Find(id);
        context.Adverts.Remove(model);
        context.SaveChanges();
        TempData["success"] = "Ürün silme işlemi başarıyla tamamlanmıştır";
        return RedirectToAction(nameof(Index));
    }
}

