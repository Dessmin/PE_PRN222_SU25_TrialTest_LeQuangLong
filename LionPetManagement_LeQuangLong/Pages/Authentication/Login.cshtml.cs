using BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace LionPetManagement_LeQuangLong.Pages.Authentication
{
    public class LoginModel : PageModel
    {
        private readonly IAuthService _authService;

        public LoginModel(IAuthService authService)
        {
            _authService = authService;
        }

        [BindProperty]
        [Required(ErrorMessage = "Email is required.")]
        [Display(Name = "Email")]
        public string Email { get; set; } = string.Empty;

        [BindProperty]
        [Required(ErrorMessage = "Password is required.")]
        [Display(Name = "Password")]
        public string Password { get; set; } = string.Empty;

        public IActionResult OnGet()
        {
            return CheckLogin();
        }

        public async Task<IActionResult> OnPostAsync()
        {
            var loginCheck = CheckLogin();
            if (loginCheck is RedirectToPageResult)
            {
                return loginCheck;
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }
            var user = await _authService.LoginAsync(Email, Password);
            if (user == null)
            {
                ModelState.AddModelError(string.Empty, "Invalid Email or Password!");
                TempData["Message"] = "Invalid Email or Password!";

                await HttpContext.SignOutAsync(CookieAuthenticationDefaults.AuthenticationScheme);
                return Page();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, user.FullName),
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Role, user.RoleId.ToString())
            };

            var identity = new ClaimsIdentity(claims,
                CookieAuthenticationDefaults.AuthenticationScheme);
            await HttpContext.SignInAsync
                (CookieAuthenticationDefaults.AuthenticationScheme,
                new ClaimsPrincipal(identity));

            Response.Cookies.Append("UserName", user.FullName);

            return RedirectToPage("/LionProfilePage/Index");
        }

        private IActionResult CheckLogin()
        {
            if (User.Identity!.IsAuthenticated)
            {
                return RedirectToPage("/LionProfilePage/Index");
            }
            return Page();
        }
    }
}
