using Engenharia2.Models;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Text.Json;
using System.Threading.Tasks;

namespace Engenharia2.Controllers
{
    public class AutenticarController : Controller
    {
        UsuarioAutenticado _ua;

        public AutenticarController(UsuarioAutenticado ua)
        {
            _ua = ua;
        }

        public IActionResult Index()
        {
            if(_ua.Autenticado)
            {
                return Redirect("/Home");
            }
            return View();
        }

        public IActionResult Sair()
        {
            Microsoft.AspNetCore.Authentication.AuthenticationHttpContextExtensions.SignOutAsync(HttpContext);
            return View("Views/Autenticar/Index.cshtml");
        }

        public IActionResult Entrar([FromBody] JsonElement dados)
        {
            string email = dados.GetProperty("email").ToString();
            string senha = dados.GetProperty("senha").ToString();
            Administrador adm = new Administrador().Autenticar(email, senha);
            bool sucesso = false;
            string msg = "Erro";
            if (adm != null)
            {
                msg = "Administrador";
                #region Gerar a Cookie do Administrador
                var userClaims = new List<Claim>()
                {
                    new Claim("Id",adm.Id.ToString()),
                    new Claim("Nome",adm.Nome.ToString()),
                    new Claim("Email",adm.Email.ToString()),
                    new Claim("Nivel",msg)
                };
                sucesso = true;
                var identity = new ClaimsIdentity(userClaims, "Identificação do Usuario");
                ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                AuthenticationHttpContextExtensions.SignInAsync(HttpContext, principal, new AuthenticationProperties
                {
                    //IsPersistent = !string.IsNullOrEmpty(cliente.Lembrar) && cliente.Lembrar == "on",
                    ExpiresUtc = DateTime.UtcNow.AddDays(7)
                });
                #endregion
            }
            else
            {
                Atendente at = new Atendente().Autenticar(email, senha);
                if(at != null)
                {
                    msg = "Atendente";
                    #region Gerar a Cookie do Administrador
                    var userClaims = new List<Claim>()
                {
                    new Claim("Id",at.Id.ToString()),
                    new Claim("Nome",at.Nome.ToString()),
                    new Claim("Email",at.Email.ToString()),
                    new Claim("Nivel",msg)
                };
                    sucesso = true;
                    var identity = new ClaimsIdentity(userClaims, "Identificação do Usuario");
                    ClaimsPrincipal principal = new ClaimsPrincipal(identity);

                    AuthenticationHttpContextExtensions.SignInAsync(HttpContext, principal, new AuthenticationProperties
                    {
                        //IsPersistent = !string.IsNullOrEmpty(cliente.Lembrar) && cliente.Lembrar == "on",
                        ExpiresUtc = DateTime.UtcNow.AddDays(7)
                    });
                    #endregion
                }
            }
            return Json(new
            {
                sucesso,
                msg
            });
        }
    }
}
