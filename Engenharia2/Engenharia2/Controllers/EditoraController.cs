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
    public class EditoraController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

        
        public IActionResult Gravar([FromBody] System.Text.Json.JsonElement dados)
        {
            string msg = new Editora().Gravar(dados);
            return Json(new
            {
                msg
            });
        }

        [HttpGet]
        public IActionResult Pesquisar(string nome)
        {
            return Json(new Models.Editora().BuscarEditoraPorNome(nome).AsEnumerable());
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Json(new Models.Editora().obterTodasEditoras().AsEnumerable());
        }

        [HttpPut]
        public IActionResult Alterar(int id)
        {
            Editora editora = new Editora().obterEditoraPorID(id);
            return Json(new
            {
                editora.Id,
                editora.Nome,
                editora.Descricao,
                editora.Telefone
            });
        }

        [HttpDelete]
        public IActionResult Deletar(int id)
        {
            string msg = "";
            msg = new Editora().Deletar(id);
            return Json(new
            {
                msg
            });
        }
    }
}
