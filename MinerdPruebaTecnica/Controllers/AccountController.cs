using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json.Linq;
using MinerdPruebaTecnica.Data.Contracts;
using MinerdPruebaTecnica.Models;
using MinerdPruebaTecnica.Utils;
using MinerdPruebaTecnica.ViewModels;
using System;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace MinerdPruebaTecnica.Controllers
{
    public class AccountController : Controller
    {
        IAccountRepository _accountRepository;
        IConfiguration _configuration;

        public AccountController(IAccountRepository accountRepository, IConfiguration configuration)
        {
            _accountRepository = accountRepository;
            _configuration = configuration;
        }

        [AllowAnonymous]
        public IActionResult SignIn()
        {
            if (Request.Cookies.ContainsKey("X-Access-Token"))
            {
                return Redirect("/Home");

            }
            return View();
        }

        [HttpPost]
        [AllowAnonymous]
        public async Task<IActionResult> SignIn(SignInViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            try
            {
                var account = await _accountRepository.GetByEmailAsync(model.Email);

                if (account == null)
                {
                    ViewData["ErrorMessage"] = "Correo o contraseña incorrecta";
                    return View(model);
                }

                if (account.Password == "1234")
                {
                    account.Password = CryptUtil.HashPassword(model.Password);
                    await _accountRepository.EditAsync(account);
                }

                if (!CryptUtil.VerifyPassword(model.Password, account.Password))
                {
                    ViewData["ErrorMessage"] = "Correo o contraseña incorrecta";
                    return View(model);
                }

                Response.Cookies.Append("X-Access-Token", GenerateJSONWebToken(account), new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });
                Response.Cookies.Append("FirstName", account.Nombre, new CookieOptions() { HttpOnly = true, SameSite = SameSiteMode.Strict });

                return Redirect("/Home");
            }
            catch (Exception ex)
            {
                ViewData["ErrorMessage"] = ex.Message;
                return View(model);
            }
        }

        [Authorize]
        public IActionResult LogOut()
        {
            Response.Cookies.Delete("X-Access-Token");

            return RedirectToAction(nameof(SignIn));
        }

        private string GenerateJSONWebToken(Account account)
        {
            var securityKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Jwt:Key"]));
            var credentials = new SigningCredentials(securityKey, SecurityAlgorithms.HmacSha256);

            var claims = new[] { 
                new Claim(JwtRegisteredClaimNames.Email, account.Email),
                new Claim(JwtRegisteredClaimNames.UniqueName, account.Nombre)
            };

            var token = new JwtSecurityToken(_configuration["Jwt:Issuer"],
                                             _configuration["Jwt:Issuer"],
                                             claims,
                                             expires: DateTime.Now.AddMinutes(120),
                                             signingCredentials: credentials);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
    }
}
