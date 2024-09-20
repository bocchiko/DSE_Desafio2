using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsuarioAPI.Models;
using StackExchange.Redis;
using System.Text.Json;

namespace UsuarioAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly UsuarioContext _context;
        private readonly IConnectionMultiplexer _redis;

        public UsuariosController(UsuarioContext context, IConnectionMultiplexer redis)
        {
            _context = context;
            _redis = redis;
        }

        // GET: api/Usuarios
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuario()
        {
            var db = _redis.GetDatabase();
            string cacheKey = "usuarioList";
            var usuariosCache = await db.StringGetAsync(cacheKey);
            if (!usuariosCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<List<Usuario>>(usuariosCache);
            }
            var usuarios = await _context.Usuario.ToListAsync();
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(usuarios), TimeSpan.FromMinutes(10));
            return usuarios;
        }

        // GET: api/Usuarios/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var db = _redis.GetDatabase();
            string cacheKey = "usuario_" + id.ToString();
            var usuarioCache = await db.StringGetAsync(cacheKey);
            if (!usuarioCache.IsNullOrEmpty)
            {
                return JsonSerializer.Deserialize<Usuario>(usuarioCache);
            }
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }
            await db.StringSetAsync(cacheKey, JsonSerializer.Serialize(usuario), TimeSpan.FromMinutes(10));
            return usuario;
        }

        // PUT: api/Usuarios/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUsuario(int id, Usuario usuario)
        {
            if (id != usuario.Id)
            {
                return BadRequest();
            }

            _context.Entry(usuario).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
                var db = _redis.GetDatabase();
                string cacheKeyUsuario = "usuario_" + id.ToString();
                string cacheKeyList = "usuarioList";
                await db.StringGetAsync(cacheKeyUsuario);
                await db.StringGetAsync(cacheKeyList);
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UsuarioExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Usuarios
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            _context.Usuario.Add(usuario);
            await _context.SaveChangesAsync();
            var db = _redis.GetDatabase();
            string cacheKeyList = "usuarioList";
            await db.StringGetAsync(cacheKeyList);

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/Usuarios/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuario.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuario.Remove(usuario);
            await _context.SaveChangesAsync();
            var db = _redis.GetDatabase();
            string cacheKeyUsuario = "usuario_" + id.ToString();
            string cacheKeyList = "usuarioList";
            await db.StringGetAsync(cacheKeyUsuario);
            await db.StringGetAsync(cacheKeyList);

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuario.Any(e => e.Id == id);
        }
    }
}
