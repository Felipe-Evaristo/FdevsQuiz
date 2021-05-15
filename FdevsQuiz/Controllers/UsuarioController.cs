using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Text.Json;
using System.IO;
using FdevsQuiz.Command;

namespace FdevsQuiz.Controllers
{
    [Route("usuario")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {

        private ICollection<T> CarregarDados<T>()
        {
            using var openStream = System.IO.File.OpenRead(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "usuarios.json"));
            return System.Text.Json.JsonSerializer.DeserializeAsync<ICollection<T>>(openStream, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            }).Result;
        }

        [HttpGet]
        public IActionResult Usuario()
        {
            var usuario = CarregarDados<Usuario>();

            return Ok(usuario);
        }

        [HttpGet("{id}")]
        public IActionResult Usuario([FromRoute] long id)
        {
            var usuarios = CarregarDados<Usuario>();
            var usuario = usuarios.Where(usuario => usuario.CodigoUsuario == id);

            return Ok(usuario);
        }

        [HttpPost]
        public async Task<ActionResult> adicionarUsuario([FromBody] UsuarioCommand command)
        {
            if (command == null)
                return BadRequest("Arquivo não encontrado");

            if (string.IsNullOrEmpty(command.NomeUsuario))
                throw new Exception("Nome de Usuário necessário");

            var usuarios = CarregarDados<Usuario>();
            var usuario = new Usuario
            {
                NomeUsuario = command.NomeUsuario,
                Email = command.Email,
                Imagem = command.Imagem,
                Pontos = command.Pontos
            };

            usuario.CodigoUsuario = usuarios.Select(e => e.CodigoUsuario).ToList().Max() + 1;
            usuarios.Add(usuario);

            using var createStream = System.IO.File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "usuarios.json"));
            await System.Text.Json.JsonSerializer.SerializeAsync(createStream, usuarios, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return Created("usuarios/{id}", usuario);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> alterarUsuario([FromBody] UsuarioCommand command, int id) 
        {
            if (command == null)
                return BadRequest("Arquivo não encontrado");

            var usuarios = CarregarDados<Usuario>();
            var usuario = usuarios.Where(e => e.CodigoUsuario == id).FirstOrDefault();

            usuario.NomeUsuario = command.NomeUsuario;
            usuario.Imagem = command.Imagem;
            usuario.Email = command.Email;
            usuario.Pontos = command.Pontos;

            usuarios.Add(usuario);

            using var createStream = System.IO.File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "usuarios.json"));
            await System.Text.Json.JsonSerializer.SerializeAsync(createStream, usuarios, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult excluirUsuario(int id) 
        {
            var usuarios = CarregarDados<Usuario>();

            var usuario = usuarios.Where(e => e.CodigoUsuario == id).FirstOrDefault();

            usuarios.Remove(usuario);

            using var createStream = System.IO.File.Create(Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Data", "usuarios.json"));
            System.Text.Json.JsonSerializer.SerializeAsync(createStream, usuarios, new JsonSerializerOptions()
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            return NoContent();
        }

    }
}
