using Engenharia2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.DAL
{
    public class ReservaDAL
    {
        MySQLPersistence _bd = new MySQLPersistence();

        public string gravar(Reserva reserva)
        {
            string msg = "Falha ao Gravar Reserva";
            string sql = "INSERT INTO reserva (DataInicio, DataFim, idLeitor, idLivro, Status) VALUES(@datainicio, @datafim, @idleitor, @idlivro, @status) ";
            _bd.LimparParametros();
            _bd.AdicionarParametro("@datainicio", Convert.ToString(reserva.DataInicio));
            _bd.AdicionarParametro("@datafim", Convert.ToString(reserva.DataFim));
            _bd.AdicionarParametro("@idleitor", Convert.ToString(reserva.Leitor.Id));
            _bd.AdicionarParametro("@idlivro", Convert.ToString(reserva.Livro.Id));
            _bd.AdicionarParametro("@status", Convert.ToString(reserva.Status));

            string sqlBusca = "SELECT * FROM reserva WHERE idLeitor =" + reserva.Leitor.Id + " AND idLivro = " + reserva.Livro.Id;
            //_bd.AdicionarParametro("@idleitorBusca", Convert.ToString(reserva.Leitor.Id));
           // _bd.AdicionarParametro("@idlivroBusca", Convert.ToString(reserva.Livro.Id));

            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sqlBusca);

            if (dt.Rows.Count == 0)
            {
                int rows = _bd.ExecutarNonQuery(sql);
                if (rows > 0)
                {
                    msg = "Reserva Gravada com Sucesso!";
                }
            }
            else
                msg = "Este livro já foi reservado pelo " + reserva.Leitor.Nome + "!";
            _bd.FecharConexao();

            return msg;
        }


        public string cancelar(int id)
        {
            string msg = "Falha ao Cancelar a Reserva";
            string sql = "UPDATE reserva SET Status=@cancelar WHERE idReserva =" + id;
            _bd.LimparParametros();
            _bd.AdicionarParametro("@cancelar", "Cancelado");

            _bd.AbrirConexao();
            int rows = _bd.ExecutarNonQuery(sql);
            _bd.FecharConexao();
            if (rows > 0)
            {
                msg = "Reserva cancelada com Sucesso!";
            }
            return msg;

        }

        public string finaliza(int idLeitor, int idLivro)
        {
            string msg = "Falha ao Finalizar a Reserva";
            string sql = "UPDATE reserva SET Status=@finalizar WHERE Status='Em Aberto' AND idLeitor =" + idLeitor + " AND idLivro=" + idLivro;
            _bd.LimparParametros();
            _bd.AdicionarParametro("@finalizar", "Finalizado");

            _bd.AbrirConexao();
            int rows = _bd.ExecutarNonQuery(sql);
            _bd.FecharConexao();
            if (rows > 0)
            {
                msg = "Reserva finalizada com Sucesso!";
            }
            return msg;

        }

        public int qtdReservada(int livroId)
        {
            string sql = "SELECT * FROM reserva WHERE Status='Em Aberto' AND idLivro = @livroid";
            _bd.LimparParametros();
            _bd.AdicionarParametro("@livroid", Convert.ToString(livroId));

            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            return dt.Rows.Count;
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

        public List<Reserva> BuscarReservaPorCPFEStatus(string cpf, string status)
        {
            List<Reserva> reservas = new List<Reserva>();
            string sqlLeitor = "SELECT * FROM leitor WHERE CPF = @cpf";
            _bd.LimparParametros();
            _bd.AdicionarParametro("@cpf", cpf);
            string sql = "";

            _bd.AbrirConexao();
            DataTable dtLeitor = _bd.ExecutarSelect(sqlLeitor);
            if (dtLeitor.Rows.Count > 0)
            {
                var idLeitorInt = 0;
                idLeitorInt = Convert.ToInt32(dtLeitor.Rows[0]["IdLeitor"]);
                sql = "SELECT * FROM reserva WHERE idLeitor = "+ idLeitorInt + " AND Status = '" + status + "'";
            }
            else
            {
                sql = "SELECT * FROM reserva WHERE Status = '" + status + "'";
            }
            
            DataTable dt = _bd.ExecutarSelect(sql);
            foreach (DataRow row in dt.Rows)
            {
                var reserva = new Reserva()
                {
                    Id = Convert.ToInt32(row["idReserva"]),
                    DataInicio = Convert.ToDateTime(row["DataInicio"]),
                    DataFim = Convert.ToDateTime(row["DataFim"]),
                    Leitor = new LeitorDAL().BuscaLeitorPorId(Convert.ToInt32(row["idLeitor"])),
                    Livro = new LivroDAL().seleciona(Convert.ToInt32(row["idLivro"])),
                    Status = row["Status"].ToString()
                };
                reservas.Add(reserva);
            }

            _bd.FecharConexao();
            return reservas;

        }

        public List<Reserva> selecionarTodos()
        {
            List<Reserva> reservas = new List<Reserva>();

            string sql = "SELECT * FROM reserva";
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            foreach (DataRow row in dt.Rows)
            {
                var reserva = new Reserva()
                {
                    Id = Convert.ToInt32(row["idReserva"]),
                    DataInicio = Convert.ToDateTime(row["DataInicio"]),
                    DataFim = Convert.ToDateTime(row["DataFim"]),
                    Leitor = new LeitorDAL().BuscaLeitorPorId(Convert.ToInt32(row["idLeitor"])),
                    Livro = new LivroDAL().seleciona(Convert.ToInt32(row["idLivro"])),
                    Status = row["Status"].ToString()
                };
                reservas.Add(reserva);
            }
            return reservas;
        }
    }
}
