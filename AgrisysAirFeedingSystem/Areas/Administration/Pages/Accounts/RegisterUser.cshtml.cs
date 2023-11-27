#nullable disable

using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

namespace AgrisysAirFeedingSystem.Areas.Administration.Pages.Accounts
{
    public class RegisterUserModel : PageModel
    {
        private readonly SignInManager<IdentityUser> _signInManager;
        private readonly UserManager<IdentityUser> _userManager;
        private readonly IUserStore<IdentityUser> _userStore;
        private readonly IUserEmailStore<IdentityUser> _emailStore;
        private readonly ILogger<RegisterUserModel> _logger;

        public RegisterUserModel(
            UserManager<IdentityUser> userManager,
            IUserStore<IdentityUser> userStore,
            SignInManager<IdentityUser> signInManager,
            ILogger<RegisterUserModel> logger)
        {
            _userManager = userManager;
            _userStore = userStore;
            _emailStore = GetEmailStore();
            _signInManager = signInManager;
            _logger = logger;
        }

        [BindProperty]
        public InputModel Input { get; set; }

        public string ReturnUrl { get; set; }

        public class InputModel
        {
            [Required]
            [EmailAddress]
            [Display(Name = "Email")]
            public string Email { get; set; }

            [Required]
            [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.", MinimumLength = 6)]
            [DataType(DataType.Password)]
            [Display(Name = "Password")]
            public string Password { get; set; }

            [DataType(DataType.Password)]
            [Display(Name = "Confirm password")]
            [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
            public string ConfirmPassword { get; set; }
            
            [Required]
            [Display(Name = "Role")]
            public string Role
            {
                get { return _role; }
                //Da de tre muligheder for role i vores program er Admin, Manager, og User har jeg lavet et tjek
                //her i propertyens set-metode for at fange den så tidligt som muligt.
                set {
                    if (value is "Admin" or "Manager" or "User")
                        _role = value;
                } 
            }
            //private property ifm tjek om selected value stemmer overens med de tilgængelige værdier ovenfor
            private string _role;
    }


        public async Task OnGetAsync(string returnUrl = null)
        {
            ReturnUrl = returnUrl;
        }

        public async Task<IActionResult> OnPostAsync(string returnUrl = null)
        {
                
            returnUrl ??= Url.Content("~/Administration/Accounts/RegisterUser?createdUser=" + Input.Email);
            //Role skal ikke kunne være null ved normal brug, men ved ændring i DOM'en. Derfor vises ingen specifik fejl,
            //hvorimod siden bare loader igen.
            if (ModelState.IsValid && Input.Role != null)
            {
                var user = CreateUser();

                await _userStore.SetUserNameAsync(user, Input.Email, CancellationToken.None);
                await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);
                await _userManager.CreateAsync(user, Input.Password);
                var result = await _userManager.AddToRoleAsync(user, Input.Role);

                if (result.Succeeded)
                {
                    _logger.LogInformation("User created a new account with password.");

                    //await _signInManager.SignInAsync(user, isPersistent: false);
                    return LocalRedirect(returnUrl);
                }
                foreach (var error in result.Errors)
                {
                    ModelState.AddModelError(string.Empty, error.Description);
                }
            }

            // If we got this far, something failed, redisplay form
            return Page();
        }

        private IdentityUser CreateUser()
        {
            try
            {
                return Activator.CreateInstance<IdentityUser>();
            }
            catch
            {
                throw new InvalidOperationException($"Can't create an instance of '{nameof(IdentityUser)}'. " +
                    $"Ensure that '{nameof(IdentityUser)}' is not an abstract class and has a parameterless constructor, or alternatively " +
                    $"override the register page in /Areas/Identity/Pages/Account/Register.cshtml");
            }
        }
        private IUserEmailStore<IdentityUser> GetEmailStore()
        {
            return (IUserEmailStore<IdentityUser>)_userStore;
        }
    }
}
