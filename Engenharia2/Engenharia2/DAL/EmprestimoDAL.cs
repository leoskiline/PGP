using Engenharia2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.DAL
{
    public class EmprestimoDAL
    {
        MySQLPersistence _bd = new MySQLPersistence();

        public string gravar(string cpf, List<Livro> livros)
        {
            string msg = "Falha ao Gravar Emprestimo";
            Emprestimo emprestimo = null;
            ReservaDAL reservadal = new ReservaDAL();
            int idLeitor = new LeitorDAL().BuscaLeitorPorCPF(cpf).Id;
            string sql = "INSERT INTO emprestimo (DataEmp, idLeitor, DataPrevistaDevol, idAtendente, Situacao, DataDevolucao) VALUES (@dataEmp, @leitor_idLeitor, @dataPrevistaDevol, @atendente_idAtendente, @situacao, @dataDevolucao);Select @@IDENTITY;";

            _bd.LimparParametros();
            _bd.AdicionarParametro("@dataEmp", DateTime.UtcNow.ToString("yyyy-MM-dd"));
            _bd.AdicionarParametro("@leitor_idLeitor", Convert.ToString(idLeitor));
            _bd.AdicionarParametro("@dataPrevistaDevol", DateTime.UtcNow.AddDays(15).ToString("yyyy-MM-dd"));
            _bd.AdicionarParametro("@atendente_idAtendente", Convert.ToString(new AtendenteDAL().obterAtendentePorNome("Maria Luiza").Id));
            _bd.AdicionarParametro("@situacao", "Em Aberto");
            _bd.AdicionarParametro("@dataDevolucao", DateTime.UtcNow.ToString("yyyy-MM-dd"));
            bool estaOk = true;
            foreach(Livro l in livros){
                if (l.Qtd == 0 && reservadal.qtdReservada(l.Id) <= l.Qtd)
                {
                    estaOk = false;
                    break;
                }
            }

            if (estaOk){
                foreach(Livro l in livros)
                {
                    if (reservadal.isReservado(idLeitor, l.Id))
                    {
                        reservadal.finaliza(idLeitor, l.Id);
                    }
                }
                _bd.AbrirConexao();
                int id = _bd.ExecutarNonQueryAndGetID(sql);
                string sql2 = "INSERT INTO emprestimo_has_exemplar (idEmprestimo,idExemplar) VALUES (@idEmprestimo,@idExemplar)";
                int rows2 = 0;
                List<Exemplar> exemplar = new ExemplarDAL().obterExemplaresPorListLivrosID(livros);
                for (int i = 0; i < exemplar.Count; i++)
                {
                    _bd.LimparParametros();
                    _bd.AdicionarParametro("@idEmprestimo", Convert.ToString(id));
                    _bd.AdicionarParametro("@idExemplar", exemplar[i].Id.ToString());
                    rows2 += _bd.ExecutarNonQuery(sql2);
                }
                _bd.FecharConexao();

                if (id != 0 && rows2 > 0)
                {
                    msg = "Emprestimo Gravado com Sucesso!";
                    //DAR MENOS 1 NOS LIVROS EMPRESTADOS
                    foreach (Livro l in livros)
                    {
                        new LivroDAL().menosUm(l);
                    }
                }
            }
            else
            {
                msg = "Algum dos livros selecionados não está disponível!";
            }
            return msg;
        }

        public Emprestimo BuscaEmprestimoPorId(int id)
        {
            string sql = "SELECT emprestimo.idEmprestimo, emprestimo.DataEmp, emprestimo.idPagamento, leitor.idLeitor, emprestimo.DataPrevistaDevol, emprestimo.Situacao, emprestimo.DataDevolucao, exemplar.idExemplar " +
                         "FROM emprestimo_has_exemplar " +
                         "INNER JOIN emprestimo " +
                         "ON emprestimo_has_exemplar.Emprestimo_idEmprestimo = emprestimo.idEmprestimo " +
                         "INNER JOIN leitor " +
                         "ON emprestimo.idLeitor = leitor.idLeitor " +
                         "INNER JOIN exemplar " +
                         "ON exemplar.idExemplar = emprestimo_has_exemplar.idExemplar " +
                         "WHERE emprestimo.idEmprestimo = " + id;
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();


            List<Emprestimo> emprestimos = new List<Emprestimo>();
            Emprestimo emprestimo = null;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (emprestimos.Exists(l => l.Id == Convert.ToInt32(row[0])))
                    {
                        foreach (Emprestimo l in emprestimos)
                        {
                            if (l.Id == Convert.ToInt32(row[0]))
                            {
                                l.Exemplar.Add(new ExemplarDAL().BuscaExemplar(Convert.ToInt32(row[7])));
                            }
                        }
                    }
                    else
                    {
                        List<int> exemplaresId = new List<int>();
                        emprestimo = new Emprestimo();
                        emprestimo.Id = Convert.ToInt32(row[0]);
                        emprestimo.DataEmp = Convert.ToDateTime(row[1]);
                        emprestimo.Leitor = new LeitorDAL().BuscaLeitorPorId(Convert.ToInt32(row[3]));
                        emprestimo.DataPrevistaDevol = Convert.ToDateTime(row[4]);
                        emprestimo.Atendente = new AtendenteDAL().obterAtendentePorNome("Maria Luiza");
                        emprestimo.Situacao = row[5].ToString();
                        emprestimo.DataDevolucao = Convert.ToDateTime(row[6]);

                        exemplaresId.Add(Convert.ToInt32(row[7]));
                        emprestimo.Exemplar = new ExemplarDAL().obterExemplaresPorListID(exemplaresId);
                        emprestimos.Add(emprestimo);
                    }
                }
            }
            return emprestimo;
        }

        public string devolver(int id)
        {
            string msg = "Falha ao Devolver a Emprestimo";
            Emprestimo emprestimo = new EmprestimoDAL().BuscaEmprestimoPorId(id);
            string sql = "UPDATE emprestimo SET Situacao=@devolver, DataDevolucao=@data WHERE idEmprestimo =" + id;
            string sql2 = "";
            string devolver = "Finalizado";
            _bd.LimparParametros();
            _bd.AdicionarParametro("@devolver", devolver);
            _bd.AdicionarParametro("@data", DateTime.UtcNow.ToString("yyyy-MM-dd"));

            _bd.AbrirConexao();
            int rows = _bd.ExecutarNonQuery(sql);
            int rows2 = 0;
            if (rows > 0)
            {
                msg = "Emprestimo devolvido com Sucesso!";
                if (msg == "Emprestimo devolvido com Sucesso!")
                {
                    foreach (Exemplar e in emprestimo.Exemplar)
                    {
                        int qtd = e.Livro.Qtd + 1;
                        sql2 = "UPDATE livro SET qtd=@qtd WHERE idLivro=@idLivro";
                        _bd.LimparParametros();
                        _bd.AdicionarParametro("@qtd", Convert.ToString(qtd));
                        _bd.AdicionarParametro("@idLivro", Convert.ToString(e.Livro.Id));
                        rows2 += _bd.ExecutarNonQuery(sql2);
                    }
                }
            }

            if (rows2 > 0)
            {
                msg = "Emprestimo Devolvido com Sucesso!";
            }

            if (emprestimo.DataPrevistaDevol < DateTime.UtcNow)
            {//Entregou antes do prazo está OK
                Pagamento pagamento = new Pagamento(DateTime.UtcNow, 20.0, 20.0);
                int idPagamento;
                (msg, idPagamento) = pagamento.gravar(pagamento);
                if (msg == "Pagamento Gravado com Sucesso!")
                {
                    string sql3 = "UPDATE emprestimo SET idPagamento="+idPagamento+" WHERE idEmprestimo =" + id;
                    int rows3 = _bd.ExecutarNonQuery(sql3);
                    if(rows3 > 0)
                    {
                        msg = "Valor a ser pago: " + pagamento.Valor_Pago;
                    }
                    else
                    {
                        msg = "Falha ao realizar Devolução do Emprestimo!";
                    }
                }
            }
            _bd.FecharConexao();
            return msg;
        }
        
        public bool isReservado(int leitorid, int livroid)
        {
            string sql = "SELECT * FROM reserva WHERE idLeitor = @leitorid AND idLivro = @livroid";
            _bd.LimparParametros();
            _bd.AdicionarParametro("@leitorid", Convert.ToString(leitorid));
            _bd.AdicionarParametro("@livroid", Convert.ToString(livroid));

            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            if (dt.Rows.Count > 0)
                return true;
            else
                return false;
        }

        

        public List<Emprestimo> selecionarTodos(string cpf)
        {
            List<Reserva> reservas = new List<Reserva>();
            string sql = "";
            if (cpf == null || cpf == "")
            {
                sql = "SELECT emprestimo.idEmprestimo, emprestimo.DataEmp, leitor.idLeitor, emprestimo.DataPrevistaDevol, emprestimo.Situacao, emprestimo.DataDevolucao, exemplar.idExemplar " +
                         "FROM emprestimo_has_exemplar " +
                         "INNER JOIN emprestimo " +
                         "ON emprestimo_has_exemplar.idEmprestimo = emprestimo.idEmprestimo " +
                         "INNER JOIN leitor " +
                         "ON emprestimo.idLeitor = leitor.idLeitor " +
                         "INNER JOIN exemplar " +
                         "ON exemplar.idExemplar = emprestimo_has_exemplar.idExemplar ";
            }
            else
            {
                sql = "SELECT emprestimo.idEmprestimo, emprestimo.DataEmp, leitor.idLeitor, emprestimo.DataPrevistaDevol, emprestimo.Situacao, emprestimo.DataDevolucao, exemplar.idExemplar " +
                         "FROM emprestimo_has_exemplar " +
                         "INNER JOIN emprestimo " +
                         "ON emprestimo_has_exemplar.idEmprestimo = emprestimo.idEmprestimo " +
                         "INNER JOIN leitor " +
                         "ON emprestimo.idLeitor = leitor.idLeitor " +
                         "INNER JOIN exemplar " +
                         "ON exemplar.idExemplar = emprestimo_has_exemplar.idExemplar " +
                         "WHERE leitor.CPF = @cpf";
                _bd.LimparParametros();
                _bd.AdicionarParametro("@cpf", Convert.ToString(cpf));
            }

            


            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            List<Emprestimo> emprestimos = new List<Emprestimo>();
            Emprestimo emprestimo = null;
            if (dt.Rows.Count > 0)
            {
                foreach (DataRow row in dt.Rows)
                {
                    if (emprestimos.Exists(l => l.Id == Convert.ToInt32(row[0])))
                    {
                        foreach (Emprestimo l in emprestimos)
                        {
                            if (l.Id == Convert.ToInt32(row[0]))
                            {
                                l.Exemplar.Add(new ExemplarDAL().BuscaExemplar(Convert.ToInt32(row[6])));
                            }
                        }
                    }
                    else
                    {
                        List<int> exemplaresId = new List<int>();
                        emprestimo = new Emprestimo();
                        emprestimo.Id = Convert.ToInt32(row[0]);
                        emprestimo.DataEmp = Convert.ToDateTime(row[1]);
                        emprestimo.Pagamento = null;
                        emprestimo.Leitor = new LeitorDAL().BuscaLeitorPorId(Convert.ToInt32(row[2]));
                        emprestimo.DataPrevistaDevol = Convert.ToDateTime(row[3]);
                        emprestimo.Atendente = new AtendenteDAL().obterAtendentePorNome("Maria Luiza") ;
                        emprestimo.Situacao = row[4].ToString();
                        emprestimo.DataDevolucao = Convert.ToDateTime(row[5]);
                        
                        exemplaresId.Add(Convert.ToInt32(row[6]));
                        emprestimo.Exemplar = new ExemplarDAL().obterExemplaresPorListID(exemplaresId);
                        emprestimos.Add(emprestimo);
                    }
                }
            }
            return emprestimos;
        }
    }
}
