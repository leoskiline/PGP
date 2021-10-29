using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.DAL
{
    public class MySQLPersistence
    {
        string _strCon;

        MySqlConnection _conexao;
        MySqlCommand _comando;
        public MySQLPersistence()
        {
            _strCon = "Server=leonardocds15.sytes.net;Database=leofipp;Uid=cassia;Pwd=Fipp@123456789";
            _conexao = new MySqlConnection(_strCon);
            _comando = _conexao.CreateCommand();
        }

        public void AbrirConexao()
        {
            if (_conexao.State != System.Data.ConnectionState.Open)
                _conexao.Open();
        }

        public void FecharConexao()
        {
            _conexao.Close();
        }

        public void LimparParametros()
        {
            _comando.Parameters.Clear();
        }

        /// <summary>
        /// Executa Insert,Delete ou Update
        /// </summary>
        public int ExecutarNonQuery(string sql, Dictionary<string, object> parametros = null)
        {
            _comando.CommandText = sql;
            if (parametros != null)
            {
                foreach (var item in parametros)
                {
                    _comando.Parameters.AddWithValue(item.Key, item.Value);
                }
            }

            int rows = _comando.ExecuteNonQuery();
            return rows;
        }

        public int ExecutarNonQueryAndGetID(string sql, Dictionary<string, object> parametros = null)
        {
            _comando.CommandText = sql;
            if (parametros != null)
            {
                foreach (var item in parametros)
                {
                    _comando.Parameters.AddWithValue(item.Key, item.Value);
                }
            }

            int id = Convert.ToInt32(_comando.ExecuteScalar());
            return id;
        }

        public void AdicionarParametro(string param, string valor)
        {
            _comando.Parameters.AddWithValue(param, valor);
        }

        public DataTable ExecutarSelect(string sql, Dictionary<string, object> parametros = null)
        {
            DataTable tabMemoria = new DataTable();
            _comando.CommandText = sql;
            if (parametros != null)
            {
                foreach (var item in parametros)
                {
                    _comando.Parameters.AddWithValue(item.Key, item.Value);
                }
            }
            tabMemoria.Load(_comando.ExecuteReader());
            return tabMemoria;
        }

        public object ExecutarConsultaSimples(string sql)
        {
            object valor = null;
            _comando.CommandText = sql;
            valor = _comando.ExecuteScalar();
            return valor;
        }
    }
}
