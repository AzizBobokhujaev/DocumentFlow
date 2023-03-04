using Api.DbContext;
using Api.FileRootService;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

public class OrderController : Controller
{
    private readonly DocumentFlowDbContext _context;
    private readonly UserManager<User> _userManager;
    private readonly IFileService _fileService;

    public OrderController(DocumentFlowDbContext context, UserManager<User> userManager, IFileService fileService)
    {
        _context = context;
        _userManager = userManager;
        _fileService = fileService;
    }

    public IActionResult Create()
    {
        var model = new CreateOrderVm();
        ViewBag.Users = _context.Users.ToList();

        return View(model);
    }

    // POST: Order/Create
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateOrderVm model, int[] Users)
    {
        if (ModelState.IsValid)
        {
            var filePath = await _fileService.AddFileAsync(model.DocumentNumber, "files", model.DecreeFile);
            var order = new Order
            {
                DocumentNumber = model.DocumentNumber,
                Title = model.Title,
                Deadline = model.Deadline,
                Status = Status.InProgress.Name,
                CreatedAt = DateTime.Now,
                DecreeFilePath = filePath,
                Users = new List<User>()
            };

            foreach (var userId in Users)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    order.Users.Add(user);
                }
            }

            ViewBag.Users = _context.Users.ToList();
            await _context.AddAsync(order);
            await _context.SaveChangesAsync();
            return RedirectToAction("Index");
        }

        model.Users = _context.Users.Select(u => new SelectListItem
        {
            Text = u.UserName,
            Value = u.Id.ToString()
        }).ToList();

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> Index()
    {
        var orders = await _context.Orders.ToListAsync();
        return View(orders);
    }
}