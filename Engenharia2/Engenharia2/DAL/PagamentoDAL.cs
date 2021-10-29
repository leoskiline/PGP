using Engenharia2.Models;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.DAL
{
    public class PagamentoDAL
    {
        MySQLPersistence _bd = new MySQLPersistence();

        public (string, int) gravar(Pagamento pagamento)
        {
            string msg = "Falha ao Gravar Pagamento";
            string sql = "INSERT INTO pagamento (Data_Pag,Valor_Multa, Valor_Pago) VALUES (@data_Pag,@valor_Multa, @valor_Pago);Select @@IDENTITY;";
            _bd.AdicionarParametro("@data_Pag", Convert.ToString(pagamento.Data_Pag));
            _bd.AdicionarParametro("@valor_Multa", Convert.ToString(pagamento.Valor_Multa));
            _bd.AdicionarParametro("@valor_Pago", Convert.ToString(pagamento.Valor_Pago));

            _bd.AbrirConexao();
            int rows = _bd.ExecutarNonQueryAndGetID(sql);
            _bd.FecharConexao();
            if (rows > 0)
            {
                msg = "Pagamento Gravado com Sucesso!";
            }
            return (msg, rows);
        }

        public Pagamento BuscaPagamentoPorId(int id)
        {
            string sql = "SELECT * FROM pagamento WHERE idPagmento=" + id;
            _bd.LimparParametros();
            _bd.AdicionarParametro("@idPagmento", id.ToString());
            _bd.AbrirConexao();
            DataTable dt = _bd.ExecutarSelect(sql);
            _bd.FecharConexao();
            Pagamento pagamento = null;
            if (dt.Rows.Count > 0)
            {
                pagamento = new Pagamento();
                pagamento.Id = Convert.ToInt32(dt.Rows[0]["idPagamento"]);
                pagamento.Data_Pag = Convert.ToDateTime(dt.Rows[0]["Data_Pag"]);
                pagamento.Valor_Multa = Convert.ToDouble(dt.Rows[0]["Valor_Multa"]);
                pagamento.Valor_Pago = Convert.ToDouble(dt.Rows[0]["Valor_Pago"]);
            }

            return pagamento;
        }
    }
}
