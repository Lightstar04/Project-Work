using InventoryManagement.Domain.Entities;
using InventoryManagement.ViewModels.AccountViewModels;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace InventoryManagement.Controllers
{
    public class AccountController : Controller
    {
        private readonly UserManager<AppUser> _userManager;
        private readonly SignInManager<AppUser> _signInManager;

        public AccountController(UserManager<AppUser> userManager, SignInManager<AppUser> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        [HttpPost]
        public async Task<IActionResult> Register(RegisterViewModel viewModel)
        {
            if(!ModelState.IsValid)
            {
                return View(viewModel);
            }

            var user = new AppUser
            {
                UserName = viewModel.Email,
                Email = viewModel.Email,
                DisplayName = viewModel.DisplayName,
                Theme = "light"
            };

            var response = await _userManager.CreateAsync(user, viewModel.Password);
            if(response.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return Redirect("/");
            }

            foreach(var error in response.Errors)
            {
                ViewData["ErrorMessage"] = error.Description;
            }

            return View(viewModel);
        }

        public IActionResult Login(string returnUrl = null)
        {
            return View(new LoginViewModel
            {
                ReturnUrl = returnUrl
            });
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            var result = await _signInManager.PasswordSignInAsync(model.Email, model.Password,
                                                                  model.RememberMe, lockoutOnFailure: false);

            if (result.Succeeded)
            {
                return Redirect(model.ReturnUrl ?? "/");
            }

            ViewData["ErrorMessage"] = $"Invalid login attempt";
            
            return View(model);
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return Redirect("/");
        }
    }
}
