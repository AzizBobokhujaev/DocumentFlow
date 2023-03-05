using Api.DbContext;
using Api.FileRootService;
using Api.Models;
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
            var fileName = string.Concat($"{model.DocumentNumber}-", DateTime.Now.ToString("dd/MM/yy/HH/mm/ss"));
            var filePath = await _fileService.AddFileAsync(fileName, "files",
                model.DecreeFile);
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
    public async Task<IActionResult> Index(string searchString, int pageNumber = 1)
    {
        const int pageSize = 5;

        var books = _context.Orders.AsSplitQuery();

        if (!string.IsNullOrEmpty(searchString))
        {
            books = books.Where(b => b.DocumentNumber.Contains(searchString) || 
                                     b.Title.Contains(searchString) ||
                                     b.Users.Any(category => category.UserName.Contains(searchString)));
        }

        var count = books.Count();
        var items = await books.Skip((pageNumber - 1) * pageSize).Take(pageSize)
            .OrderByDescending(order => order.CreatedAt).ToListAsync();

        var pagingInfo = new PagingInfo
        {
            CurrentPage = pageNumber,
            ItemsPerPage = pageSize,
            TotalItems = count
        };

        var model = new OrdersListViewModel
        {
            Orders = items,
            PagingInfo = pagingInfo,
            SearchString = searchString
        };

        return View(model);
    }

    [HttpGet]
    public async Task<IActionResult> GetInProgressOrders(string searchString,int pageNumber = 1)
    {
        const int pageSize = 5;

        var orders = _context.Orders.Where(order => order.Status == Status.InProgress.Name)
            .OrderByDescending(order => order.CreatedAt)
            .AsQueryable();
     
        
        if (!string.IsNullOrEmpty(searchString))
        {
            orders = orders.Where(b => b.DocumentNumber.Contains(searchString) || 
                                       b.Title.Contains(searchString) ||
                                       b.Users.Any(category => category.UserName.Contains(searchString)));
        }

        var count = orders.Count();
        var items = await orders.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        var pagingInfo = new PagingInfo
        {
            CurrentPage = pageNumber,
            ItemsPerPage = pageSize,
            TotalItems = count
        };

        var model = new OrdersListViewModel
        {
            Orders = items,
            PagingInfo = pagingInfo,
            SearchString = searchString
        };

        return View(model);
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetDoneOrders(string searchString,int pageNumber = 1)
    {
        const int pageSize = 5;

        var orders = _context.Orders.Where(order => order.Status == Status.Done.Name)
            .OrderByDescending(order => order.CreatedAt)
            .AsQueryable();
     
        
        if (!string.IsNullOrEmpty(searchString))
        {
            orders = orders.Where(b => b.DocumentNumber.Contains(searchString) || 
                                       b.Title.Contains(searchString) ||
                                       b.Users.Any(category => category.UserName.Contains(searchString)));
        }

        var count = orders.Count();
        var items = await orders.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        var pagingInfo = new PagingInfo
        {
            CurrentPage = pageNumber,
            ItemsPerPage = pageSize,
            TotalItems = count
        };

        var model = new OrdersListViewModel
        {
            Orders = items,
            PagingInfo = pagingInfo,
            SearchString = searchString
        };

        return View(model);
    }
    
    
    [HttpGet]
    public async Task<IActionResult> GetNotDoneOrders(string searchString,int pageNumber = 1)
    {
        const int pageSize = 5;

        var orders = _context.Orders.Where(order => order.Status == Status.NotDone.Name)
            .OrderByDescending(order => order.CreatedAt)
            .AsQueryable();
     
        
        if (!string.IsNullOrEmpty(searchString))
        {
            orders = orders.Where(b => b.DocumentNumber.Contains(searchString) || 
                                       b.Title.Contains(searchString) ||
                                       b.Users.Any(category => category.UserName.Contains(searchString)));
        }

        var count = orders.Count();
        var items = await orders.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

        var pagingInfo = new PagingInfo
        {
            CurrentPage = pageNumber,
            ItemsPerPage = pageSize,
            TotalItems = count
        };

        var model = new OrdersListViewModel
        {
            Orders = items,
            PagingInfo = pagingInfo,
            SearchString = searchString
        };

        return View(model);
    }

    public async Task<IActionResult> SetExecutionFile(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        var model = new SetExecutionFileVm
        {
            Id = id
        };
        return View(model);
    }

    [HttpPost]
    public async Task<IActionResult> SetExecutionFile(SetExecutionFileVm model)
    {
        if (!ModelState.IsValid) return RedirectToAction("Index");
        var order = await _context.Orders.FindAsync(model.Id);
        var fileName = string.Concat($"{order!.DocumentNumber}-", DateTime.Now.ToString("dd/MM/yy/HH/mm/ss"));
        var filePath = await _fileService.AddFileAsync(fileName, "response-files",
            model.ExecutionFile);

        order.ExecutionFilePath = filePath;
        order.ExecutionFileCreatedAt = DateTime.Now;
        order.Status = Status.Done.Name;
        _context.Update(order);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
}