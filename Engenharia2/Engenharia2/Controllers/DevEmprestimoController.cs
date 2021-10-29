using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.Controllers
{
    public class DevEmprestimoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Devolver()
        {
            return View(); //@TODO cadastro de devolucao
        }

        public IActionResult Cancelar()
        {
            return View(); //@TODO
        }
    }
}
