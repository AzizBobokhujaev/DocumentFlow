using Api.DbContext;
using Api.Models;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Api.Controllers;

public class UserController : Controller
{
    private readonly DocumentFlowDbContext _dbContext;
    private readonly SignInManager<User> _signInManager;
    private readonly UserManager<User> _userManager;

    public UserController(DocumentFlowDbContext dbContext, SignInManager<User> signInManager, 
        UserManager<User> userManager)
    {
        _dbContext = dbContext;
        _signInManager = signInManager;
        _userManager = userManager;
    }
    
    
    [HttpGet]
    public IActionResult Login()
    {
        return View();
    }
        
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginVm model)
    {
        var result = await _signInManager.PasswordSignInAsync(model.UserName, model.Password, false, false);
        if (!result.Succeeded)
        {
            ModelState.AddModelError("LoginFail", "Неверный логин или пароль");
            return View(model);
        }

        return RedirectToAction("Index", "Home");
    }
    
    // ------------------------------------------------Register---------------------------------------------------------
    [HttpGet]
    public IActionResult Register()
    {
        return View();
    }
        
    [HttpPost]
    public async Task<IActionResult> Register(RegisterVm model)
    {
        if (!ModelState.IsValid)
        {
            return View(model);
        }

        var result = await _userManager.CreateAsync(new User
        {
            UserName = model.UserName,
            Email = model.Email,
            PhoneNumber=model.PhoneNumber,
                
        },model.Password);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return View(model);
        }
        else
        {
            await _userManager.AddToRoleAsync(await _userManager.FindByEmailAsync(model.Email), "User");
        }
        return RedirectToAction("Login");
    }
    
    // --------------------------------------------ChangePassword-------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> ChangePassword(int id)
    {
        var account = await _dbContext.Users.FindAsync(id);
        if (account == null)
        {
            return RedirectToAction("UsersList");
        }

        var changePasswordVm = new ChangePasswordVm
        {
            Id = account.Id
        };
        return View(changePasswordVm);
    }
    
    [HttpPost]
    public async Task<IActionResult> ChangePassword(ChangePasswordVm model)
    {
        var user = await _userManager.FindByIdAsync(model.Id.ToString());
        if (user == null)
        {
            return NotFound();
        }
    
        var token = await _userManager.GeneratePasswordResetTokenAsync(user);
        var result = await _userManager.ResetPasswordAsync(user, token, model.NewPassword);
        if (!result.Succeeded)
        {
            foreach (var error in result.Errors)
            {
                ModelState.AddModelError(error.Code, error.Description);
            }
            return BadRequest(ModelState);
        }

        return RedirectToAction("UsersList");
    }
    
    //--------------------------------------------------UsersList-------------------------------------------------------

    [HttpGet]
    public async Task<IActionResult> UsersList(string searchString, int pageNumber = 1)
    {
        const int pageSize = 5;
        var users =  _dbContext.Users.Where(user => user.UserName != "Admin").AsQueryable();
        
        if (!string.IsNullOrEmpty(searchString))
        {
            var text = searchString.ToUpper();
            users = users.Where(b => b.UserName.ToUpper().Contains(text) || 
                                     b.PhoneNumber.ToUpper().Contains(text));
        }
        
        var count = users.Count();
        var items = await users.Skip((pageNumber - 1) * pageSize).Take(pageSize).ToListAsync();
        
        var pagingInfo = new PagingInfo
        {
            CurrentPage = pageNumber,
            ItemsPerPage = pageSize,
            TotalItems = count
        };
        
        var model = new UsersListViewModel
        {
            Users = items,
            PagingInfo = pagingInfo,
            SearchString = searchString
        };
        return View(model);
    }
    
    //--------------------------------------------------Logout----------------------------------------------------------
    
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
    
    //-------------------------------------------------Edit_User--------------------------------------------------------
    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var account = await _dbContext.Users.FindAsync(id);
        if (account == null)
        {
            return RedirectToAction("UsersList");
        }

        var editUserVm = new EditUserVm()
        {
            Id = account.Id,
            UserName = account.UserName,
            PhoneNumber = account.PhoneNumber
        };
        return View(editUserVm);
    }
    
    [HttpPost]
    public async Task<IActionResult> Edit(EditUserVm model)
    {
        var user = await _userManager.FindByIdAsync(model.Id.ToString());
        if (user == null)
        {
            return NotFound();
        }

        user.UserName = model.UserName;
        user.PhoneNumber = model.PhoneNumber;

        _dbContext.Users.Update(user);
        await _dbContext.SaveChangesAsync();

        return RedirectToAction("UsersList");
    }
}