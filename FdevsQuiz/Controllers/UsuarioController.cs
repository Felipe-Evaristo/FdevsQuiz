using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace FdevsQuiz.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuarioController : ControllerBase
    {
        public Usuario RecuperarUsuario() 
        {
            var jsonUsuario = System.IO.File.ReadAllText("Usuario.json");

            var retornoUsuario = JsonConvert.DeserializeObject<Usuario>(jsonUsuario);

            return retornoUsuario;
        }
    }
}
