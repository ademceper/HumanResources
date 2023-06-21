using HumanResources.Data;
using HumanResources.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics;
using System.Security.Claims;

namespace HumanResources.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;
    private readonly AppDbContext context;
    public HomeController(
        ILogger<HomeController> logger, AppDbContext context)
    {
        _logger = logger;
        this.context = context;
    }
    public async Task<IActionResult> Index()
    {
        ViewBag.Adverts = await context.Adverts.OrderByDescending(p => p.Salary).Take(15).ToListAsync();
        return View();
    }

    public async Task<IActionResult> FullAdverts()
    {
        ViewBag.Adverts = await context.Adverts.OrderByDescending(p => p.Salary).Take(15).ToListAsync();
        return View();
    }
    public IActionResult Privacy()
    {
        return View();
    }
    public IActionResult About()
    {
        return View();
    }
    public async Task<IActionResult> Advert(Guid id)
    {
        var advert = await context.Adverts.SingleOrDefaultAsync(p => p.Id == id);


        return View(advert);

    }

    public IActionResult Recourse()
    {
        return View();
    }

      
    
    [HttpPost]
    public async Task<IActionResult> Recourse(RecourseViewModel model)
    {
        var Recourse = new Recourse
        {
          Name=model.Name!,
          Email=model.Email!,
          Number=model.Number!,
           
         
        };

        await context.Recourses.AddAsync(Recourse);
        await context.SaveChangesAsync();
        TempData["success"] = "Yorumunuz tarafımıza ulaşmıştır.";
        return RedirectToAction(nameof(Index));
    }
    public async Task<IActionResult> Search(string keyword, int? page)
    {

        if (keyword == null)
        {
            return RedirectToAction("Index");

        }
        ViewBag.Keyword = keyword;
        var model = (await context.Adverts.Where(p => p.Appellation.Contains(keyword)).ToListAsync());
        return View(model);
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }

}