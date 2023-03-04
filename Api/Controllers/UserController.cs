using Api.DbContext;
using Api.Models.Entities;
using Api.Models.ViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

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
    
    [HttpGet]
    public IActionResult ChangePassword()
    {
        return View();
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

        return RedirectToAction("Login", "User");
    }
}