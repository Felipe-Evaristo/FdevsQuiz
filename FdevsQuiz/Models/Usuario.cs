using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FdevsQuiz
{
    public class Usuario
    {
        public int CodigoUsuario { get; set; }
        public string NomeUsuario { get; set; }
        public string Email { get; set; }
        public string Imagem { get; set; }
        public int Pontos { get; set; }
    }
}
