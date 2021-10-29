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
    public class EmprestimoController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

       public IActionResult Gravar([FromBody] System.Text.Json.JsonElement dados)
        {
            //cpf
            //livro

            string msg = "Falha ao Gravar Emprestimo!";
            List<int> livrosId = new List<int>();
            string cpf = dados.GetProperty("cpf").ToString();
            int contlivros = dados.GetProperty("livro").GetArrayLength();
            for (int i = 0; i < contlivros; i++)
            {
                livrosId.Add(Convert.ToInt32(dados.GetProperty("livro")[i].ToString()));
            }
            List<Livro> livros = new LivroDAL().obterLivrosPorListID(livrosId);

            if(cpf.Length > 0 && contlivros > 0)
            {
                msg = new Emprestimo().Gravar(cpf, livros);
            }
            return Json(new
            {
                msg
            });
        }

        [HttpGet]
        public IActionResult Pesquisar(string cpf)
        {
            return Json(new Models.Emprestimo().BuscarEmprestimoPorCPF(cpf).AsEnumerable());
        }

        [HttpPut]
        public IActionResult Devolver(int id)
        {
            string msg = new Models.Emprestimo().DevolverEmprestimo(id);
            return Json(new
            {
                msg
            });
        }

        [HttpGet]
        public IActionResult Listar()
        {
            return Json(new Models.Emprestimo().obterTodosEmprestimo().AsEnumerable());
        }

        [HttpGet]
        public IActionResult ObterLivros()
        {
            return Json(new Models.Livro().obterTodosLivros().AsEnumerable());
        }
    }
}
