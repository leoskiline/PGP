using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Engenharia2.Models
{
    public class Posicao
    {
        private int id;
        private string setor;
        private string prateleira;

        public Posicao()
        {
        }

        public Posicao(int id, string setor, string prateleira)
        {
            this.Id = id;
            this.Setor = setor;
            this.Prateleira = prateleira;
        }

        public int Id { get => id; set => id = value; }
        public string Setor { get => setor; set => setor = value; }
        public string Prateleira { get => prateleira; set => prateleira = value; }
    }
}
