using FdevsQuiz.Command;
using FdevsQuiz.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;

namespace FdevsQuiz.Controllers
{
    [Route("quiz")]
    [ApiController]
    public class QuizesController : ControllerBase
    {
        private ICollection<T> CarregarDados<T>()
        {
            using var openStream = System.IO.File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "quizzes.json"));
            return System.Text.Json.JsonSerializer.DeserializeAsync<ICollection<T>>(openStream, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }).Result;
        }

        [HttpGet]
        public IActionResult Quiz()
        {
            var quizzes = CarregarDados<Quiz>();

            return Ok(quizzes);
        }

        [HttpGet("{id}")]
        public IActionResult Quiz([FromRoute] long id)
        {
            var quizzes = CarregarDados<Quiz>();
            var quiz = quizzes.Where(e => e.CodigoQuiz == id);

            return Ok(quiz);
        }

        [HttpPost]
        public async Task<ActionResult> adicionarQuiz([FromBody] QuizCommand command)
        {
            if (command == null)
                return BadRequest("Pergunta não encontrado");

            if (string.IsNullOrEmpty(command.Titulo))
                throw new Exception("Título do quiz necessário!");

            if(command.Perguntas.Count < 4)
                throw new Exception("É necessário no mínimo 4 perguntas!");

            if (string.IsNullOrEmpty(command.Dificuldade))
                throw new Exception("Dificuldade necessária!");

            var quizzes = CarregarDados<Quiz>();
            var quiz = new Quiz
            {
                Titulo = command.Titulo,
                Perguntas = command.Perguntas,
                Dificuldade = command.Dificuldade,
                Pontos = command.Pontos
            };

            quiz.CodigoQuiz = quizzes.Select(e => e.CodigoQuiz).ToList().Max() + 1;
            quizzes.Add(quiz);

            using var createStream = System.IO.File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "quizzes.json"));
            await System.Text.Json.JsonSerializer.SerializeAsync(createStream, quizzes, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return Created("quizzes/{id}", quiz);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> alterarQuiz([FromBody] QuizCommand command, int id)
        {
            if (command == null)
                return BadRequest("Pergunta não encontrado");

            if (string.IsNullOrEmpty(command.Titulo))
                throw new Exception("Título do quiz necessário!");

            if (command.Perguntas.Count < 4)
                throw new Exception("É necessário no mínimo 4 perguntas!");

            if (string.IsNullOrEmpty(command.Dificuldade))
                throw new Exception("Dificuldade necessária!");

            var quizzes = CarregarDados<Quiz>();
            var quiz = quizzes.Where(e => e.CodigoQuiz == id).FirstOrDefault();

            quiz.Titulo = command.Titulo;
            quiz.Perguntas = command.Perguntas;
            quiz.Dificuldade = command.Dificuldade;
            quiz.Pontos = command.Pontos;

            quizzes.Add(quiz);

            using var createStream = System.IO.File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "quizzes.json"));
            await System.Text.Json.JsonSerializer.SerializeAsync(createStream, quizzes, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult excluirUsuario(int id)
        {
            var quizzes = CarregarDados<Quiz>();

            var quiz = quizzes.Where(e => e.CodigoQuiz == id).FirstOrDefault();

            quizzes.Remove(quiz);

            using var createStream = System.IO.File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "usuarios.json"));
            System.Text.Json.JsonSerializer.SerializeAsync(createStream, quizzes, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return NoContent();
        }
    }
}
