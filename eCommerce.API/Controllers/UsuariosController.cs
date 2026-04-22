using eCommerce.API.Models;
using eCommerce.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace eCommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private IUsuarioRepository _repository;

        public UsuariosController(IUsuarioRepository repository)
        {
            _repository = repository;
        }

        [HttpGet]
        public Task<ActionResult> Get()
        {
            return Task.FromResult<ActionResult>(Ok(_repository.Get()));
        }

        [HttpGet("{id}")]
        public ActionResult Get(int id)
        {
            var usuario = _repository.Get(id);

            if (usuario is null)
                return NotFound();

            return Ok(usuario);
        }

        [HttpPost]
        public ActionResult Insert([FromBody] Usuario usuario)
        {
            _repository.Insert(usuario);

            return Created();
        }

        [HttpPut]
        public ActionResult Update([FromBody] Usuario usuario)
        {
            _repository.Update(usuario);

            return Ok(usuario);
        }

        [HttpDelete("{id}")]
        public ActionResult Delete(int id)
        {
            return Ok();
        }
    }
}
