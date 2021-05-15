using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace FdevsQuiz.Command
{
    public class QuizCommand
    {
        public string Titulo { get; set; }
        public List<object> Perguntas { get; set; }
        public string Dificuldade { get; set; }
        public int Pontos { get; set; }
    }
}
