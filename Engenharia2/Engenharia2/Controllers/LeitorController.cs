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
    public class LeitorController : Controller
    {
        [Authorize("Autorizacao")]
        public IActionResult Index()
        {
            return View();
        }

        public IActionResult Gravar([FromBody] System.Text.Json.JsonElement dados)
        {
            string msg = new Leitor().Gravar(dados);

            return Json(new
            {
                msg
            });
        }
        [HttpGet]
        public IActionResult Listar()
        {
            return Json(new Models.Leitor().selecionarTodos());
        }

        [HttpGet]
        public IActionResult Pesquisar(string cpf)
        {
            return Json(new Models.Leitor().BuscarLeitorPorCPF(cpf));
        }

        [HttpPut]
        public IActionResult Alterar(int id)
        {
            Leitor leitor = new Leitor().BuscarLeitorPorId(id);
            return Json(new
            {
                leitor.Id,
                leitor.Nome,
                leitor.Cpf,
                leitor.Endereco,
                leitor.DataNasc
            });
        }

        [HttpDelete]
        public IActionResult Deletar(int id)
        {
            string msg = "";
            msg = new Leitor().Deletar(id);
            return Json(new
            {
                msg
            });
        }
    }
}
