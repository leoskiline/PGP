using Engenharia2.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.Models
{
    public class Reserva
    {
        private int id;
        private DateTime dataInicio;
        private DateTime dataFim;
        private Leitor leitor;
        private Livro livro;
        private string status;
        public Reserva()
        {

        }

        public Reserva(int id, DateTime dataInicio, DateTime dataFim, Leitor leitor, Livro livro, string status)
        {
            this.Id = id;
            this.DataInicio = dataInicio;
            this.DataFim = dataFim;
            this.Leitor = leitor;
            this.livro = livro;
            this.status = status;
        }

        public int Id { get => id; set => id = value; }
        public DateTime DataInicio { get => dataInicio; set => dataInicio = value; }
        public DateTime DataFim { get => dataFim; set => dataFim = value; }
        public Leitor Leitor { get => leitor; set => leitor = value; }
        public string Status { get => status; set => status = value; }
        public Livro Livro { get => livro; set => livro = value; }

        public string Gravar(System.Text.Json.JsonElement dados)
        {
            string msg = "Falha ao Gravar Reserva!";
            string msg2 = "";
            ReservaDAL reservadal = new ReservaDAL();
            Livro livro = new LivroDAL().seleciona(Convert.ToInt32(dados.GetProperty("livro").ToString()));
            Leitor leitor = new LeitorDAL().BuscaLeitorPorCPF(dados.GetProperty("cpf").ToString());
            if (dados.GetProperty("cpf").ToString() == "")
                return "Campo de CPF Vazio...";
            bool reservado = reservadal.isReservado(leitor.Id, livro.Id);

            if (livro.Qtd > 0 && !reservado)
                msg = "Livro disponível!, vá para tela de empréstimo";
            else
            {
                if (livro.Qtd > 0)
                    msg = "Reserva realizada!, Livro está em estoque, verificar fila de reserva!";
                else
                    msg = "Reserva realizada!, Assim que o livro estiver disponível entraremos em contato.";
                Reserva reserva = new Reserva();
                reserva.DataInicio = DateTime.Today;
                reserva.DataFim = DateTime.Today.AddMonths(1);
                reserva.Leitor = leitor;
                reserva.Livro = livro;
                reserva.status = "Em Aberto";
                msg2 = reservadal.gravar(reserva);
            }

            if (msg2 == "Reserva Gravada com Sucesso!")
                return msg2;
            else
                return msg;
        }

        public List<Reserva> BuscarReservaPorCPFEStatus(string cpf, string status)
        {
            return new DAL.ReservaDAL().BuscarReservaPorCPFEStatus(cpf, status);
        }

        public string CancelarReserva(int id)
        {
            return new ReservaDAL().cancelar(id);
        }

        public List<Reserva> obterTodasReserva()
        {
            return new DAL.ReservaDAL().selecionarTodos();
        }
    }
}
