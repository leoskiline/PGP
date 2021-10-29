using Engenharia2.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.Models
{
    public class Emprestimo
    {
        private int _id;
        private DateTime _DataEmp;
        private Pagamento _Pagamento;
        private Leitor _Leitor;
        private DateTime _DataPrevistaDevol;
        private Atendente _Atendente;
        private string _Situacao;
        private DateTime _DataDevolucao;
        private List<Exemplar> _Exemplar;

        public Emprestimo()
        {

        }

        public Emprestimo(int id, DateTime dataEmp, Pagamento pagamento, Leitor leitor, DateTime dataPrevistaDevol, Atendente atendente, string situacao, DateTime dataDevolucao, List<Exemplar> exemplar)
        {
            Id = id;
            DataEmp = dataEmp;
            Pagamento = pagamento;
            Leitor = leitor;
            DataPrevistaDevol = dataPrevistaDevol;
            Atendente = atendente;
            Situacao = situacao;
            DataDevolucao = dataDevolucao;
            Exemplar = exemplar;
        }

        public int Id { get => _id; set => _id = value; }
        public DateTime DataEmp { get => _DataEmp; set => _DataEmp = value; }
        public Pagamento Pagamento { get => _Pagamento; set => _Pagamento = value; }
        public Leitor Leitor { get => _Leitor; set => _Leitor = value; }
        public DateTime DataPrevistaDevol { get => _DataPrevistaDevol; set => _DataPrevistaDevol = value; }
        public Atendente Atendente { get => _Atendente; set => _Atendente = value; }
        public string Situacao { get => _Situacao; set => _Situacao = value; }
        public DateTime DataDevolucao { get => _DataDevolucao; set => _DataDevolucao = value; }
        public List<Exemplar> Exemplar { get => _Exemplar; set => _Exemplar = value; }


        public string Gravar(string cpf, List<Livro> livro)
        {
            string msg = "Falha ao Gravar Emprestimo!";
            EmprestimoDAL emprestimodal = new EmprestimoDAL();
            msg = emprestimodal.gravar(cpf, livro);
            return msg;
        }

        public List<Emprestimo> BuscarEmprestimoPorCPF(string cpf)
        {
            return new DAL.EmprestimoDAL().selecionarTodos(cpf);
        }

        public string DevolverEmprestimo(int id)
        {
            return new EmprestimoDAL().devolver(id);
        }

        public List<Emprestimo> obterTodosEmprestimo()
        {
            return new DAL.EmprestimoDAL().selecionarTodos("");
        }

    }
}
