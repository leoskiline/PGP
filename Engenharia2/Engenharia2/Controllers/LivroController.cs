using Engenharia2.DAL;
using Engenharia2.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace Engenharia2.Controllers
{
    public class LivroController : Controller
    {
        [Authorize("Autorizacao")]
        public IActionResult Index()
        {
            return View();
        }
        public IActionResult Gravar([FromBody] System.Text.Json.JsonElement dados)
        {
            string msg = "Falha ao Gravar Livro!";
            Livro livro = new LivroDAL().seleciona(Convert.ToInt32(dados.GetProperty("idLivro").ToString()));
            if (livro == null)
            {
                string nome = dados.GetProperty("nome").ToString();
                int editoraId;
                Int32.TryParse(dados.GetProperty("editora").ToString(), out editoraId);
                int contautores = dados.GetProperty("autor").GetArrayLength();
                List<int> autoresId = new List<int>();
                int qtd;
                Int32.TryParse(dados.GetProperty("qtd").ToString(), out qtd);
                for (int i = 0; i < contautores; i++)
                {
                    autoresId.Add(Convert.ToInt32(dados.GetProperty("autor")[i].ToString()));
                }
                Editora editora = new Editora().obterEditoraPorID(editoraId);
                List<Autor> autores = new Autor().obterAutoresPorListID(autoresId);
                Administrador adm = new Administrador().obter("Leonardo Custodio dos Santos");
                msg = "Preencha Todos os Campos!!!";
                if (nome.Trim().Length > 0 && editoraId != 0 && contautores > 0 && autoresId != null && qtd > 0 && editora != null && autores != null && adm != null)
                {
                    msg = new Livro().Gravar(nome, autores, editora, adm, qtd);
                }
            }
            else
            {
                string nome = dados.GetProperty("nome").ToString();
                int editoraId;
                Int32.TryParse(dados.GetProperty("editora").ToString(), out editoraId);
                int contautores = dados.GetProperty("autor").GetArrayLength();
                List<int> autoresId = new List<int>();
                int qtd;
                Int32.TryParse(dados.GetProperty("qtd").ToString(), out qtd);
                for (int i = 0; i < contautores; i++)
                {
                    autoresId.Add(Convert.ToInt32(dados.GetProperty("autor")[i].ToString()));
                }
                Editora editora = new Editora().obterEditoraPorID(editoraId);
                List<Autor> autores = new Autor().obterAutoresPorListID(autoresId);
                Administrador adm = new Administrador().obter("Leonardo Custodio dos Santos");
                msg = "Preencha Todos os Campos!!!";
                if (nome.Trim().Length > 0 && editoraId != 0 && contautores > 0 && autoresId != null && qtd > 0 && editora != null && autores != null && adm != null)
                {
                    msg = new Livro().Alterar(nome, autores, editora, adm, qtd);
                }
            }
            
            return Json(new
            {
                msg
            });
        }
        [HttpGet]
        public IActionResult ObterEditoras()
        {
            return Json(new Models.Editora().obterTodasEditoras().AsEnumerable());
        }

        [HttpGet]
        public IActionResult ObterAutores()
        {
            return Json(new Models.Autor().obterTodosAutores().AsEnumerable());
        }

        public IActionResult Pesquisar([FromBody] System.Text.Json.JsonElement dados)
        {
            string termoPesquisado = dados.GetProperty("termo").ToString();
            string tipoPesquisado = dados.GetProperty("tipo").ToString();
            List<Livro> livros = new Livro().PesquisarLivros(termoPesquisado, tipoPesquisado);
            return Json(new
            {
                livros
            });
        }

        public IActionResult Deletar(int id)
        {
            string msg = "Falha";
            msg = new Livro().DeletarLivro(id);
            return Json(new
            {
                msg
            });
        }

        public IActionResult Alterar(int id)
        {
            Livro livro = new Livro().buscarLivroPorID(id);
            return Json(new
            {
                livro
            });
        }
    }
}
