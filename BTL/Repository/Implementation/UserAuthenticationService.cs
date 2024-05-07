using BTL.Models;
using BTL.Models.Domain;
using BTL.Models.DTO;
using BTL.Repository.Abstract;
using Microsoft.AspNetCore.Identity;
using System.Security.Claims;
using System.Text;

namespace BTL.Repository.Implementation
{
    public class UserAuthenticationService: IUserAuthenticationService
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly SignInManager<ApplicationUser> signInManager;
		private static Dictionary<string, int> loginAttempts = new Dictionary<string, int>();
		private static Dictionary<string, DateTime> lockedUsers = new Dictionary<string, DateTime>();
		public UserAuthenticationService(UserManager<ApplicationUser> userManager, 
            SignInManager<ApplicationUser> signInManager, RoleManager<IdentityRole> roleManager)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.signInManager = signInManager; 

        }

        public async Task<Status> RegisterAsync(RegistrationModel model)
        {
            var status = new Status();
            var userExists = await userManager.FindByNameAsync(model.Username);
            if (userExists != null)
            {
                status.StatusCode = 0;
                status.Message = "User already exist";
                return status;
            }
            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                FirstName = model.FirstName,
                LastName=model.LastName,
                EmailConfirmed=true,
                PhoneNumberConfirmed=true,
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
            {
                status.StatusCode = 0;
                status.Message = "User creation failed";
                return status;
            }

            if (!await roleManager.RoleExistsAsync(model.Role))
                await roleManager.CreateAsync(new IdentityRole(model.Role));
            

            if (await roleManager.RoleExistsAsync(model.Role))
            {
                await userManager.AddToRoleAsync(user, model.Role);
            }

            status.StatusCode = 1;
            status.Message = "You have registered successfully";
            return status;
        }


		public async Task<Status> LoginAsync(LoginModel model)
		{
			var status = new Status();
			var user = await userManager.FindByNameAsync(model.Username);
			if (user == null)
			{
				status.StatusCode = 0;
				status.Message = "Invalid username";
				return status;
			}

			// Check if user is locked out
			if (lockedUsers.ContainsKey(user.UserName))
			{
				var lockoutTime = lockedUsers[user.UserName];
				if (DateTime.Now < lockoutTime.AddMinutes(0.5))
				{
					status.StatusCode = 0;
					status.Message = "User is locked out. Please try again later.";
					return status;
				}
				// If the lockout time has passed, remove the user from the locked users list
				lockedUsers.Remove(user.UserName);
			}

			if (!await userManager.CheckPasswordAsync(user, model.Password))
			{
				// Increment login attempts
				if (!loginAttempts.ContainsKey(user.UserName))
				{
					loginAttempts[user.UserName] = 1;
				}
				else
				{
					loginAttempts[user.UserName]++;
				}

				status.StatusCode = 0;
				status.Message = "Invalid Password";

				// Check if login attempts exceed 5, if so, lock the user out
				if (loginAttempts[user.UserName] >= 3)
				{
					lockedUsers[user.UserName] = DateTime.Now;
					loginAttempts.Remove(user.UserName);
					status.Message = "Invalid Password. User is locked out. Please try again later.";
				}
				return status;
			}

			// Successful login, reset login attempts
			if (loginAttempts.ContainsKey(user.UserName))
			{
				loginAttempts.Remove(user.UserName);
			}

			var signInResult = await signInManager.PasswordSignInAsync(user, model.Password, false, true);
			if (signInResult.Succeeded)
			{
				var userRoles = await userManager.GetRolesAsync(user);
				var authClaims = new List<Claim>
		{
			new Claim(ClaimTypes.Name, user.UserName),
		};

				foreach (var userRole in userRoles)
				{
					authClaims.Add(new Claim(ClaimTypes.Role, userRole));
				}
				status.StatusCode = 1;
				status.Message = "Logged in successfully";
			}
			else if (signInResult.IsLockedOut)
			{
				status.StatusCode = 0;
				status.Message = "User is locked out";
			}
			else
			{
				status.StatusCode = 0;
				status.Message = "Error on logging in";
			}

			return status;
		}

		public async Task LogoutAsync()
        {
           await signInManager.SignOutAsync();
           
        }

        public async Task<Status> ChangePasswordAsync(ChangePasswordModel model,string username)
        {
            var status = new Status();
            
            var user = await userManager.FindByNameAsync(username);
            if (user == null)
            {
                status.Message = "User does not exist";
                status.StatusCode = 0;
                return status;
            }
            var result = await userManager.ChangePasswordAsync(user, model.CurrentPassword, model.NewPassword);
            if (result.Succeeded)
            {
                status.Message = "Password has updated successfully";
                status.StatusCode = 1;
            }
            else
            {
                status.Message = "Some error occcured";
                status.StatusCode = 0;
            }
            return status;

        }
    }
}
