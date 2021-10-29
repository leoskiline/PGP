using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.Controllers
{
    public class CadEmprestimoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Cadastrar()
        {
            return View(); //@TODO cadastrar
        }

        public IActionResult Cancelar()
        {
            return View(); //@TODO cancelar
        }
    }
}
