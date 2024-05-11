using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Org.BouncyCastle.Crypto.Macs;
using Practice_01.Authentication;
using Practice_01.Models;
using practice1.Services;
using practice1.ViewModels;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Mail;
using System.Security.Claims;
using System.Text;
using System.Web;

namespace Practice_01.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IConfiguration _configuration;
        private readonly IEmailService _emailservice;

        public AuthController(UserManager<ApplicationUser> userManager, IEmailService emailservice, RoleManager<IdentityRole> roleManager, IConfiguration configuration)
        {
            this.userManager = userManager;
            this.roleManager = roleManager;
            _configuration = configuration;
            _emailservice = emailservice;
        }
        [HttpPost]
        [Route("register")]
        public async Task<IActionResult> Register([FromBody] Models.RegisterModel model)
        {
            var test = userManager.Users.ToList();
            //var userExists = await userManager.FindByNameAsync(model.Username);
            var userExists = await userManager.FindByEmailAsync(model.Email);
            if (userExists != null)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });

            ApplicationUser user = new ApplicationUser()
            {
                Email = model.Email,
                SecurityStamp = Guid.NewGuid().ToString(),
                UserName = model.Username,
                //RefreshToken = GenerateRefreshToken()
            };
            var result = await userManager.CreateAsync(user, model.Password);
            if (!result.Succeeded)
                return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed! Please check user details and try again." });
            if (!await roleManager.RoleExistsAsync(Role.Admin))
                await roleManager.CreateAsync(new IdentityRole(Role.Admin));
            if (!await roleManager.RoleExistsAsync(UserRoles.User))
                await roleManager.CreateAsync(new IdentityRole(Role.User));
            if (!await roleManager.RoleExistsAsync(Role.Decorator))
                await roleManager.CreateAsync(new IdentityRole(Role.Decorator));


            //if (!await roleManager.RoleExistsAsync(Role.Decorator.ToString()))
            //    await roleManager.CreateAsync(new IdentityRole(Role.Decorator.ToString()));

            if (!await roleManager.RoleExistsAsync(Role.Caterer))
                await roleManager.CreateAsync(new IdentityRole(Role.Caterer));

            if (model.UserRole == Role.Admin)
            {
                await userManager.AddToRoleAsync(user, Role.Admin);
                MailMessage message1 = new MailMessage();
                message1.To.Add("lysanne.hilll@ethereal.email");
                message1.Subject = "Registration Successfull of Admin";
                string body1 = "<h1>Admin Registered</h1><br /><p>Thank you for registering your account!</p>";

                // Send the email using the email service
                bool success1 = _emailservice.SendAsync(message1, body1);

                // Optionally, handle the result of sending the email
                if (success1)
                {

                    return Ok(new { Status = "Success", Message = "Admin created successfully!" });
                }
                else
                {
                    return BadRequest("user not created ");

                }
            }
            if (model.UserRole == Role.Decorator)
            {
                await userManager.AddToRoleAsync(user, Role.Decorator);
                MailMessage message1 = new MailMessage();
                message1.To.Add("lysanne.hilll@ethereal.email");
                message1.Subject = "Registration of Decorator";
                string body1 = "<h1>Decorator Registeration</h1><br /><p>Decorator Registration Successfully</p>";

                // Send the email using the email service
                bool success1 = _emailservice.SendAsync(message1, body1);
                if (success1)
                {
                    return Ok(new { Status = "Sucess", Message = "Decorator created successfully and pls check youe email !" });
                }
                else
                {
                    return BadRequest("user not created ");
                }
            }
            if (model.UserRole == Role.Caterer)
            {
                await userManager.AddToRoleAsync(user, Role.Caterer);
                MailMessage message1 = new MailMessage();
                message1.To.Add("lysanne.hilll@ethereal.email");
                message1.Subject = "Registration of Caterer";
                string body1 = "<h1>Caterer Registeration</h1><br /><p>Please wait while your registration is pending approval from an administrator and add some basic details.</p>";
                bool success1 = _emailservice.SendAsync(message1, body1);
                if (success1)
                {
                    return Ok(new { Status = "Sucess", Message = "Caterer created successfully and pls check youe email and add some details !" });
                }
                else
                {
                    return BadRequest("user not created");
                }

            }

            if (model.UserRole ==Role.User)



            await userManager.AddToRoleAsync(user, Role.User);
            MailMessage message = new MailMessage();
            message.To.Add("lysanne.hilll@ethereal.email");
            message.Subject = "Successful";
            string body = "<h1>Event Book</h1><br /><p>Event Book Successfully!</p>";

            // Send the email using the email service
            bool success = _emailservice.SendAsync(message, body);

            // Optionally, handle the result of sending the email
            if (success)
            {
                // Email sent successfully, return success response or redirect
                //return RedirectToAction("RegistrationSuccess");
                return Ok(new { Status = "Success", Message = "User created successfully!" });
            }
            else
            {
                return BadRequest("user not created ");

            }

        }

      

        [HttpPost]
        [Route("login")]
        public async Task<IActionResult> Login([FromBody] LoginModel model)
        {
            //var user = await userManager.FindByNameAsync(model.Username);
            //if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            //{
            //    var userRoles = await userManager.GetRolesAsync(user);

            //    var authClaims = new List<Claim>
            //    {
            //     new Claim(ClaimTypes.Name, user.UserName),
            //     new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
            //     };

            //    // Add user roles to claims
            //    foreach (var userRole in userRoles)
            //    {
            //        authClaims.Add(new Claim(ClaimTypes.Role, userRole));
            //    }

            //    // Create JWT token
            //    var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
            //    var token = new JwtSecurityToken(
            //        issuer: _configuration["JWT:ValidIssuer"],
            //        audience: _configuration["JWT:ValidAudience"],
            //        expires: DateTime.Now.AddHours(3),
            //        claims: authClaims,
            //        signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
            //    );

            //    // Convert token to string
            //    var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

            //    // Store token in a cookie
            //    HttpContext.Response.Cookies.Append("access_token", tokenString, new CookieOptions
            //    {
            //        HttpOnly = true,
            //        Secure = true, // Set to true if using HTTPS
            //        SameSite = SameSiteMode.Strict, // Adjust as per your requirement
            //        Expires = DateTime.Now.AddHours(3)
            //    });

            //    return Ok(new
            //    {
            //        token = tokenString,
            //        expiration = token.ValidTo,
            //        user = user.UserName
            //    });
            //}

            //return Unauthorized();
           // var users = await userManager.Users.ToListAsync();
            var users = await userManager.Users.Where(x=>x.Email == model.Email).ToListAsync();
            var user = users.Where(x => x.Email.Contains(model.Email)).FirstOrDefault();
            //var user = await userManager.FindByNameAsync(model.Email);
            if (user != null && await userManager.CheckPasswordAsync(user, model.Password))
            {
                var userRoles = await userManager.GetRolesAsync(user);

                var authClaims = new List<Claim>
                {
                    new Claim(ClaimTypes.Name, user.Email),
                    new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                 };

                // Add user roles to claims
                foreach (var userRole in userRoles)
                {
                    authClaims.Add(new Claim(ClaimTypes.Role, userRole));

                }

                // Create JWT token
                var authSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["JWT:Secret"]));
                var token = new JwtSecurityToken(
                    issuer: _configuration["JWT:ValidIssuer"],
                    audience: _configuration["JWT:ValidAudience"],
                    expires: DateTime.Now.AddHours(3),
                    claims: authClaims,
                    signingCredentials: new SigningCredentials(authSigningKey, SecurityAlgorithms.HmacSha256)
                );

                // Convert token to string
                var tokenString = new JwtSecurityTokenHandler().WriteToken(token);

                // Store token in a cookie
                HttpContext.Response.Cookies.Append("access_token", tokenString, new CookieOptions
                {
                    HttpOnly = true,
                    Secure = true, // Set to true if using HTTPS
                    SameSite = SameSiteMode.Strict, // Adjust as per your requirement
                    Expires = DateTime.Now.AddHours(3)
                });

                return Ok(new
                {
                    token = tokenString,
                    expiration = token.ValidTo,
                    //user = user.Email // Change to email
                    user = new
                    {
                        userID = user.Id,
                        email = user.Email,
                        roles = userRoles
                    }
                });
            }

            return Unauthorized();
        }


        [HttpGet]
        [Route("testemail")]
        public IActionResult TestEmail()
        {
            return StatusCode(StatusCodes.Status200OK, new Response { Status = "sucess", Message = "Your Event booked successfully" });
        }

        [HttpPost]
        [Route("forgot-password")]
        public async Task<IActionResult> Forgotpassword(string email)
        {
            var users = await userManager.Users.ToListAsync();
            var user = users.Where(x => x.Email.Contains(email)).FirstOrDefault();
          //  var user = await userManager.FindByEmailAsync(email);
            if (user != null)
            {
                var token = await userManager.GeneratePasswordResetTokenAsync(user);
                var forgotPassLink = Url.Action(nameof(ResetPassword), "Auth", new { token, email = user.Email }, Request.Scheme);

                // Create a MimeMessage
                var message = new MailMessage();
                if (!string.IsNullOrEmpty(user.Email))
                {
                    message.To.Add(user.Email);
                }// Use the user's email here
                message.Subject = "Forgot Password";

                //create rset pass link
                string resetPasswordLink = $"http://localhost:3000/changepassword/{HttpUtility.UrlEncode(token)}";
                string resetPasswordHtml = $"<a href=\"{resetPasswordLink}\">Click here to reset your password</a>";

                // Create the email body
                string body = $"Please reset your password by clicking this link: {forgotPassLink}<br><br>";
                body += $"Alternatively, you can use the following link: {resetPasswordHtml}";

                //string body =  $"Please reset your password by clicking this link: {forgotPassLink}";
              
                // Send the MimeMessage
                _emailservice.SendAsync(message, body);

                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = $"Password change request sent to email {user.Email}. Please open your email and click the link." , Email = user.Email,
                    Token = token
                });
            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "Error", Message = $"Could not send the link to email. Please try again later." });
        }

        [HttpGet]
        [ValidateAntiForgeryToken]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(string token, string email)
        {
            var model = new ResetPassword { Token = token, Email = email };
            return Ok(new { model });
        }

        [HttpPost]
        //[ValidateAntiForgeryToken]
        //[AllowAnonymous]
        [Route("reset-password")]
        public async Task<IActionResult> ResetPassword(ResetPassword resetpassword)
        {
            //email trough find user

            //var user = await userManager.FindByEmailAsync(resetpassword.Email);
            var users = await userManager.Users.ToListAsync();
            //  var user = users.Where(x => x.Email.Contains(resetpassword.Email)).FirstOrDefault();
            var user = users.Where(x => resetpassword.Email != null && x.Email.Contains(resetpassword.Email)).FirstOrDefault();
            var token = await userManager.GeneratePasswordResetTokenAsync(user);
            if (user != null)
            {
                var resetpass = await userManager.ResetPasswordAsync(user, token, resetpassword.Password);
                if (!resetpass.Succeeded)
                {
                    foreach (var error in resetpass.Errors)
                    {
                        ModelState.AddModelError(error.Code, error.Description);
                    }
                    return Ok(ModelState);
                }
                return StatusCode(StatusCodes.Status200OK, new Response { Status = "Success", Message = "password has been changed sucessfully.....!" });

            }
            return StatusCode(StatusCodes.Status400BadRequest, new Response { Status = "error", Message = $"coud not sen the link to email,please try again letter." });
        }


    }
}

    

