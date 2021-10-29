using Engenharia2.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.Models
{
    public class Atendente
    {
        private int id;
        private string nome;
        private string endereco;
        private string telefone;
        private string status;
        private string cpf;
        private string senha;
        private string email;

        public Atendente(int id, string nome, string endereco, string telefone, string status, string cpf, string senha, string email)
        {
            this.id = id;
            this.nome = nome;
            this.endereco = endereco;
            this.telefone = telefone;
            this.status = status;
            this.cpf = cpf;
            this.senha = senha;
            this.email = email;
        }

        public Atendente()
        {
            this.id = 0;
            this.nome = "";
            this.endereco = "";
            this.telefone = "";
            this.status = "";
            this.cpf = "";
            this.senha = "";
            this.email = "";
        }

        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string Telefone { get => telefone; set => telefone = value; }
        public string Status { get => status; set => status = value; }
        public string Cpf { get => cpf; set => cpf = value; }
        public string Senha { get => senha; set => senha = value; }
        public string Email { get => email; set => email = value; }

        public Atendente Autenticar(string email, string senha)
        {
            return new AtendenteDAL().Autenticar(email, senha);
        }
    }
    
   
}
