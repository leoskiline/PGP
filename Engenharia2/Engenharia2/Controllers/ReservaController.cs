using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.Controllers
{
    public class ReservaController : Controller
    {
        [Authorize("Autorizacao")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Gravar([FromBody] System.Text.Json.JsonElement dados)
        {
            string msg = new Models.Reserva().Gravar(dados);

            return Json(new
            {
                msg
            });
        }

        [HttpGet]
        public IActionResult Pesquisar(string cpf, string status)
        {
            return Json(new Models.Reserva().BuscarReservaPorCPFEStatus(cpf, status).AsEnumerable());
        }

        [HttpPut]
        public IActionResult Cancelar(int id)
        {
            string msg = new Models.Reserva().CancelarReserva(id);
            return Json(new
            {
                msg
            });
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Json(new Models.Reserva().obterTodasReserva().AsEnumerable());
        }

        [HttpGet]
        public IActionResult ObterLivros()
        {
            return Json(new Models.Livro().obterTodosLivros().AsEnumerable());
        }
    }
}
