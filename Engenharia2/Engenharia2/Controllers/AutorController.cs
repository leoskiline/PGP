using Engenharia2.DAL;
using Engenharia2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.Controllers
{
    [Authorize("Autorizacao")]
    public class AutorController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Gravar([FromBody] System.Text.Json.JsonElement dados)
        {
            string msg = new Autor().Gravar(dados);

            return Json(new
            {
                msg
            });
        }

        [HttpGet]
        public IActionResult Pesquisar(string nome)
        {
            return Json(new Models.Autor().BuscarAutorPorNome(nome).AsEnumerable());
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Json(new Models.Autor().obterTodosAutores().AsEnumerable());
        }

        [HttpPut]
        public IActionResult Alterar(int id)
        {
            Autor autor = new Autor().obterAutorPorID(id);
            return Json(new
            {
                autor.Id,
                autor.Nome,
            });
        }

        [HttpDelete]
        public IActionResult Deletar(int id)
        {
            string msg = "";
            msg = new Autor().Deletar(id);
            return Json(new
            {
                msg
            });
        }
    }
}
