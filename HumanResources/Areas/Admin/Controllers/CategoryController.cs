using HumanResources.Areas.Admin.Models;
using HumanResources.Data;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SixLabors.ImageSharp.Formats.Jpeg;
using System.Data;
using System.Globalization;
using System.Security.Claims;

namespace HumanResources.Areas.Admin.Controllers;

[Area("Admin")]
public class CategoryController : Controller
{

    private readonly string entityName = "Ürün";

    private readonly AppDbContext context;
    private readonly IConfiguration configuration;


    public CategoryController(
        AppDbContext context,
        IConfiguration configuration

        )
    {
        this.context = context;
        this.configuration = configuration;

    }

    public IActionResult Index()
    {
        var model = context.Categories.OrderBy(p => p.Name).ToList();

        return View(model);
    }
    public async Task<IActionResult> TableData(Guid? id, DataTableParameters parameters)
    {
        var query = context.Categories;

        var result = new DataTableResult
        {
            data = await query
                .Skip(parameters.Start)
                .Take(parameters.Length)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
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

    public async Task<IActionResult> Create(Category model)
    {
        model.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        context.Categories.Add(model);
        context.SaveChanges();
        TempData["success"] = "Ürün ekleme işlemi başarıyla tamamlanmıştır";
        return RedirectToAction(nameof(Index));

    }
    public IActionResult Edit(Guid id)
    {
        var model = context.Categories.Find(id);
        return View(model);
    }
    [Authorize(Roles = "Administrators")]
    [HttpPost]
    public async Task<IActionResult> Edit(Category model)
    {



      
        model.UserId = Guid.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);

        context.Categories.Add(model);
        try
        {
            context.Categories.Update(model);
            context.SaveChanges();
            TempData["success"] = "Ürün güncelleme işlemi başarıyla tamamlanmıştır";
            return RedirectToAction(nameof(Index));
        }
        catch (DbUpdateException)
        {
            TempData["error"] = "Aynı isimli bir başka kayıt olduğundan kayıt işlemi tamamlanamıyor!";
            return View(model);
        }




    }
    [Authorize(Roles = "Administrators")]
    public IActionResult Delete(Guid id)
    {
        var model = context.Categories.Find(id);
        context.Categories.Remove(model);
        context.SaveChanges();
        TempData["success"] = "Ürün silme işlemi başarıyla tamamlanmıştır";
        return RedirectToAction(nameof(Index));
    }
}
