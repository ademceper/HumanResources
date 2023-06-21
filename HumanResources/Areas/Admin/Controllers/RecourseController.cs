using HumanResources.Areas.Admin.Models;
using HumanResources.Data;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace HumanResources.Areas.Admin.Controllers;
[Area("Admin")]
public class RecourseController : Controller
{
    private readonly string entityName = "Ürün";

    private readonly AppDbContext context;
    private readonly IConfiguration configuration;


    public RecourseController(
        AppDbContext context,
        IConfiguration configuration

        )
    {
        this.context = context;
        this.configuration = configuration;

    }

    public IActionResult Index()
    {
        var model = context.Recourses.OrderBy(p => p.Name).ToList();

        return View(model);
    }
    public async Task<IActionResult> TableData(Guid? id, DataTableParameters parameters)
    {
        var query = context.Recourses;

        var result = new DataTableResult
        {
            data = await query
                .Skip(parameters.Start)
                .Take(parameters.Length)
                .Select(p => new
                {
                    p.Id,
                    p.Name,
                    p.Number,
                  p.Email,
                  dateCreated = p.DateCreated.ToShortDateString(),
                }).ToListAsync(),
            draw = parameters.Draw,
            recordsFiltered = await query.CountAsync(),
            recordsTotal = await query.CountAsync()
        };

        return Json(result);
    }
    public IActionResult Delete(Guid id)
    {
        var model = context.Recourses.Find(id);
        context.Recourses.Remove(model);
        context.SaveChanges();
        TempData["success"] = "Ürün silme işlemi başarıyla tamamlanmıştır";
        return RedirectToAction(nameof(Index));
    }
}



