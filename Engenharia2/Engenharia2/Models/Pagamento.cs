using Engenharia2.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.Models
{
    public class Pagamento
    {
        private int _id;
        private DateTime _Data_Pag;
        private double _Valor_Multa;
        private double _Valor_Pago;

        public Pagamento()
        {

        }
        public Pagamento(int id, DateTime data_Pag, double valor_Multa, double valor_Pago)
        {
            Id = id;
            Data_Pag = data_Pag;
            Valor_Multa = valor_Multa;
            Valor_Pago = valor_Pago;
        }

        public Pagamento(DateTime data_Pag, double valor_Multa, double valor_Pago)
        {
            Data_Pag = data_Pag;
            Valor_Multa = valor_Multa;
            Valor_Pago = valor_Pago;
        }

        public int Id { get => _id; set => _id = value; }
        public DateTime Data_Pag { get => _Data_Pag; set => _Data_Pag = value; }
        public double Valor_Multa { get => _Valor_Multa; set => _Valor_Multa = value; }
        public double Valor_Pago { get => _Valor_Pago; set => _Valor_Pago = value; }


        public (string,int) gravar(Pagamento pagamento)
        {
            return new PagamentoDAL().gravar(pagamento);
        }

    }
}
