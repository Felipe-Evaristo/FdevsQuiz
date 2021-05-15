using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FdevsQuiz.Models
{
    public class Quiz
    {
        public int CodigoQuiz { get; set; }
        public string Titulo { get; set; }
        public List<object> Perguntas { get; set; }
        public string Dificuldade { get; set; }
        public int Pontos { get; set; }
    }
}
