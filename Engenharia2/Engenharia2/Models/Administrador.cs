using Engenharia2.DAL;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.Models
{
    public class Administrador
    {
        private int id;
        private string nome;
        private string cpf;
        private string endereco;
        private string telefone;
        private string email;
        private string senha;
        private string status;

        public int Id { get => id; set => id = value; }
        public string Nome { get => nome; set => nome = value; }
        public string Cpf { get => cpf; set => cpf = value; }
        public string Endereco { get => endereco; set => endereco = value; }
        public string Telefone { get => telefone; set => telefone = value; }
        public string Email { get => email; set => email = value; }
        public string Senha { get => senha; set => senha = value; }
        public string Status { get => status; set => status = value; }

        public Administrador()
        {

        }

        public Administrador(int id, string nome, string cpf, string endereco, string telefone, string email, string senha, string status)
        {
            this.Id = id;
            this.Nome = nome;
            this.Cpf = cpf;
            this.Endereco = endereco;
            this.Telefone = telefone;
            this.Email = email;
            this.Senha = senha;
            this.Status = status;
        }

        public Administrador obter(string nome)
        {
            return new AdministradorDAL().obter(nome);
        }

        public Administrador Autenticar(string email,string senha)
        {
            return new AdministradorDAL().Autenticar(email, senha);
        }
    }
}
