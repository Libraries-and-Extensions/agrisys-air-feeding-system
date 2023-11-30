#nullable disable

using System.ComponentModel.DataAnnotations;
using AgrisysAirFeedingSystem.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace AgrisysAirFeedingSystem.Areas.Administration.Pages.Accounts;

public class RegisterUserModel : AdminPageModel
{
    private readonly IUserEmailStore<IdentityUser> _emailStore;
    private readonly ILogger<RegisterUserModel> _logger;
    private readonly SignInManager<IdentityUser> _signInManager;
    private readonly UserManager<IdentityUser> _userManager;
    private readonly IUserStore<IdentityUser> _userStore;

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

    [BindProperty] public InputModel Input { get; set; }

    public string ReturnUrl { get; set; }


    public async Task OnGetAsync(string returnUrl = null)
    {
        ReturnUrl = returnUrl;
    }

    public async Task<IActionResult> OnPostAsync(string returnUrl = null)
    {
        returnUrl ??= Url.Content("~/Administration/Accounts/RegisterUser?createdUser=" + Input.Email);
        
        //check if input is valid
        if (ModelState.IsValid)
        {
            var result = await RegisterUser(Input);
            if (result.Succeeded)
            {
                _logger.LogInformation("User created a new account with password.");
                return LocalRedirect(returnUrl);
            }

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
        }

        // If we got this far, something failed, redisplay form
        return Page();
    }

    //This method is used to create an instance of IdentityUser. It is used to create a new user.
    //Handles if the identity class is abstract or has a parameterless constructor.
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

    private async Task<IdentityResult> RegisterUser(InputModel input)
    {
        var user = CreateUser();
        user.UserName = input.DisplayName;
        user.Email = input.Email;
        /*await _userStore.SetUserNameAsync(user, input.DisplayName, CancellationToken.None);
        await _emailStore.SetEmailAsync(user, Input.Email, CancellationToken.None);*/
        await _userManager.CreateAsync(user, Input.Password);
        return await _userManager.AddToRoleAsync(user, Input.Role.ToString());
    }

    private IUserEmailStore<IdentityUser> GetEmailStore()
    {
        return (IUserEmailStore<IdentityUser>)_userStore;
    }

    public enum Roles
    {
        Admin,
        Manager,
        User
    }
    
    public class InputModel
    {
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 6)]
        [Display(Name = "DisplayName")]
        public string DisplayName { get; set; }

        [Required]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; }

        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} and at max {1} characters long.",
            MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        public string Password { get; set; }

        [DataType(DataType.Password)]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }

        [Required]
        [Display(Name = "Role")]
        public Roles Role { get; set; }
    }
}