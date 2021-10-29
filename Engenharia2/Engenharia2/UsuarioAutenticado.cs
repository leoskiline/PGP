using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2
{
    public class UsuarioAutenticado
    {
        public int Id { get; set; }
        public string Nome { get; set; }
        public string Email { get; set; }
        public bool Autenticado { get; set; }

        public UsuarioAutenticado(IHttpContextAccessor accessor)
        {
            Autenticado = false;
            if (accessor.HttpContext.User.Claims.Count() > 0)
            {
                try
                {
                    Id = Convert.ToInt32(accessor.HttpContext.User.Claims.ElementAt(0).Value);
                    //Nome = accessor.HttpContext.User.Claims.ElementAt(1).Value;
                    Nome = accessor.HttpContext.User.Claims.Where(w => w.Type == "Nome").First().Value;
                    Email = accessor.HttpContext.User.Claims.ElementAt(2).Value;
                    Autenticado = true;
                }
                catch
                {

                }
            }
        }
    }
}
