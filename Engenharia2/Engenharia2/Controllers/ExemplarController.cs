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
    public class ExemplarController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Gravar([FromBody] System.Text.Json.JsonElement dados)
        {
            string msg = "Falhou";
            msg = new Exemplar().Gravar(dados);
            return Json(new
            {
                msg
            });
        }
        [HttpGet]
        public IActionResult ObterLivros()
        {
            return Json(new Models.Livro().obterTodosLivros().AsEnumerable());
        }
        [HttpGet]
        public IActionResult Listar()
        {
            return Json(new Exemplar().ObterTodosExemplares());
        }
        [HttpGet]
        public IActionResult Pesquisar(string nome)
        {
            return Json(new Exemplar().ObterExemplares(nome));
        }
        [HttpDelete]
        public IActionResult Deletar(int id)
        {
            string msg = "";
            msg = new Exemplar().Deletar(id);
            return Json(new { msg });
        }

        public IActionResult Alterar(int id)
        {
            Exemplar exemplar = new Exemplar().BuscarExemplarID(id);
            return Json(new {
                exemplar
            });
        }
    }
}
