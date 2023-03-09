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
    private const int DoneStatusId = 1;
    private const int NotDoneStatusId = 2;
    private const int InProgressStatusId = 3;

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
            var status = _context.Statuses.FirstOrDefault(status1 => status1.Id == InProgressStatusId);
            var fileName = string.Concat($"{model.DecreeName}-", DateTime.Now.ToString("dd/MM/yy/HH/mm/ss"));
            var filePath = await _fileService.AddFileAsync(fileName, "files",
                model.DecreeFile);
            var order = new Order
            {
                DecreeName = model.DecreeName,
                ImportDate = model.ImportDate,
                DocumentNumber = model.DocumentNumber,
                Title = model.Title,
                Deadline = model.Deadline,
                Status = status!,
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

        var books = _context.Orders.OrderByDescending(order => order.CreatedAt).AsSplitQuery();

        if (!string.IsNullOrEmpty(searchString))
        {
            var text = searchString.ToUpper();
            books = books.Where(b => b.DocumentNumber.ToUpper().Contains(text) || 
                                     b.Title.ToUpper().Contains(text) ||
                                     b.Users.Any(category => category.UserName.ToUpper().Contains(text)));
        }

        var count = books.Count();
        var items = await books.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();

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

        var orders = _context.Orders.Where(order => order.Status.Id == InProgressStatusId)
            .OrderByDescending(order => order.CreatedAt)
            .AsQueryable();
     
        
        if (!string.IsNullOrEmpty(searchString))
        {
            var text = searchString.ToUpper();
            orders = orders.Where(b => b.DocumentNumber.ToUpper().Contains(text) || 
                                     b.Title.ToUpper().Contains(text) ||
                                     b.Users.Any(category => category.UserName.ToUpper().Contains(text)));
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

        var orders = _context.Orders.Where(order => order.Status.Id == DoneStatusId)
            .OrderByDescending(order => order.CreatedAt)
            .AsQueryable();
     
        
        if (!string.IsNullOrEmpty(searchString))
        {
            var text = searchString.ToUpper();
            orders = orders.Where(b => b.DocumentNumber.ToUpper().Contains(text) || 
                                       b.Title.ToUpper().Contains(text) ||
                                       b.Users.Any(category => category.UserName.ToUpper().Contains(text)));
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
    public async Task<IActionResult> GetNotDoneOrders(string searchString, int pageNumber = 1)
    {
        const int pageSize = 5;

        var orders = _context.Orders.Where(order => order.Status.Id == NotDoneStatusId)
            .OrderByDescending(order => order.CreatedAt)
            .AsQueryable();
     
        
        if (!string.IsNullOrEmpty(searchString))
        {
            var text = searchString.ToUpper();
            orders = orders.Where(b => b.DocumentNumber.ToUpper().Contains(text) || 
                                       b.Title.ToUpper().Contains(text) ||
                                       b.Users.Any(category => category.UserName.ToUpper().Contains(text)));
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
        var fileName = string.Concat($"{order!.ExecutionDocumentName}-", DateTime.Now.ToString("dd/MM/yy/HH/mm/ss"));
        var filePath = await _fileService.AddFileAsync(fileName, "response-files",
            model.ExecutionFile);

        order.ExecutionFilePath = filePath;
        order.ExecutionDocumentName = model.ExecutionDocumentName;
        order.ExecutionFileCreatedAt = DateTime.Now;
        order.StatusId = 1;
        _context.Update(order);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }
    
    public async Task<IActionResult> Edit(int id)
    {
        var order = await _context.Orders.FindAsync(id);

        if (order == null)
        {
            return NotFound();
        }

        var users = _context.Users.ToList();
        
        var model = new EditOrderVm
        {
            Id = order.Id,
            Title = order.Title,
            Deadline = order.Deadline,
            StatusId = order.StatusId,
            ExecutionFilePath = order.ExecutionFilePath,
            ExecutionDocumentName = order.ExecutionDocumentName,
            Users = users.Select(u => new SelectListItem
            {
                Text = u.UserName,
                Value = u.Id.ToString(),
                Selected = order.Users.Any(o => o.Id == u.Id)
            }).ToList()

        };

        ViewBag.Statuses = _context.Statuses.ToList();

        return View(model);
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, EditOrderVm model)
    {
        if (id != model.Id)
        {
            return NotFound();
        }

        var order = await _context.Orders
            .Include(o => o.Users)
            .FirstOrDefaultAsync(o => o.Id == id);

        if (order == null)
        {
            return NotFound();
        }

        if (ModelState.IsValid)
        {
            order.Title = model.Title;
            if (model.StatusId != order.StatusId)
            {
                order.StatusId = model.StatusId;
            }
            else if (order.StatusId == model.StatusId && (model.Deadline != order.Deadline || model.Deadline > DateTime.Now))
            {
                order.StatusId = InProgressStatusId;
            }
            order.Deadline = model.Deadline;

            if (model.ResponseFile != null && order.ExecutionFilePath is null)
            {
                var fileName = string.Concat($"{order.ExecutionDocumentName}-response-", DateTime.Now.ToString("dd/MM/yy/HH/mm/ss"));
                var filePath = await _fileService.AddFileAsync(fileName, "response-files",
                    model.ResponseFile);
                order.ExecutionFilePath = filePath;
                order.ExecutionDocumentName = model.ExecutionDocumentName;
                order.ExecutionFileCreatedAt = DateTime.Now;
                order.StatusId = DoneStatusId;
            }

            order.Users.Clear();

            foreach (var userId in model.UserIds)
            {
                var user = await _context.Users.FindAsync(userId);
                if (user != null)
                {
                    order.Users.Add(user);
                }
            }
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
            
        }

        model.Users = _context.Users.Select(u => new SelectListItem
        {
            Text = u.UserName,
            Value = u.Id.ToString(),
            Selected = model.UserIds.Contains(u.Id)
        }).ToList();

        return View(model);
    }
    
    public async Task<IActionResult> Delete(int? id)
    {
        if (id == null)
        {
            return NotFound();
        }

        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        var model = new DeleteOrderVm { Id = order.Id };
        return View(model);
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var order = await _context.Orders.FindAsync(id);
        if (order == null)
        {
            return NotFound();
        }

        _context.Orders.Remove(order);
        await _context.SaveChangesAsync();
        return RedirectToAction("Index");
    }

    [HttpGet]
    public async Task<IActionResult> GetMyDoneOrders(string searchString, int pageNumber = 1)
    {
        const int pageSize = 5;

        var orders = _context.Orders.Where(order => order.Users.Any(user => user.UserName == User.Identity!.Name)
                                                    && order.StatusId == DoneStatusId)
            .OrderByDescending(order => order.CreatedAt).AsQueryable();


        if (!string.IsNullOrEmpty(searchString))
        {
            var text = searchString.ToUpper();
            orders = orders.Where(b => b.DocumentNumber.ToUpper().Contains(text) || 
                                       b.Title.ToUpper().Contains(text) ||
                                       b.Users.Any(category => category.UserName.ToUpper().Contains(text)));
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
    public async Task<IActionResult> GetMyNotDoneOrders(string searchString, int pageNumber = 1)
    {
        const int pageSize = 5;

        var orders = _context.Orders.Where(order => order.Users.Any(user => user.UserName == User.Identity!.Name)
                                                    && order.StatusId == NotDoneStatusId)
            .OrderByDescending(order => order.CreatedAt).AsQueryable();


        if (!string.IsNullOrEmpty(searchString))
        {
            var text = searchString.ToUpper();
            orders = orders.Where(b => b.DocumentNumber.ToUpper().Contains(text) || 
                                       b.Title.ToUpper().Contains(text) ||
                                       b.Users.Any(category => category.UserName.ToUpper().Contains(text)));
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
    public async Task<IActionResult> GetMyInProgressOrders(string searchString, int pageNumber = 1)
    {
        const int pageSize = 5;

        var orders = _context.Orders.Where(order => order.Users.Any(user => user.UserName == User.Identity!.Name)
                                                    && order.StatusId == InProgressStatusId)
            .OrderByDescending(order => order.CreatedAt).AsQueryable();


        if (!string.IsNullOrEmpty(searchString))
        {
            var text = searchString.ToUpper();
            orders = orders.Where(b => b.DocumentNumber.ToUpper().Contains(text) || 
                                       b.Title.ToUpper().Contains(text) ||
                                       b.Users.Any(category => category.UserName.ToUpper().Contains(text)));
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
}