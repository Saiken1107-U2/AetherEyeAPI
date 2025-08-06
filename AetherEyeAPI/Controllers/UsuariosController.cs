using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AetherEyeAPI.Data;
using AetherEyeAPI.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Configuration;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using Microsoft.IdentityModel.Tokens;

namespace AetherEyeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsuariosController : ControllerBase
    {
        private readonly AetherEyeDbContext _context;
        private readonly IConfiguration _configuration;

        public UsuariosController(AetherEyeDbContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/Usuarios
        [Authorize(Roles = "Administrador,Admin")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Usuario>>> GetUsuarios()
        {
            return await _context.Usuarios.ToListAsync();
        }

        // GET: api/Usuarios/5
        [Authorize(Roles = "Administrador,Admin")]
        [HttpGet("{id}")]
        public async Task<ActionResult<Usuario>> GetUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);

            if (usuario == null)
            {
                return NotFound();
            }

            return usuario;
        }

        // PUT: api/Usuarios/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [Authorize(Roles = "Administrador,Admin")]
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
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Usuario>> PostUsuario(Usuario usuario)
        {
            Console.WriteLine($"🔧 PostUsuario - Iniciando creación de usuario: {usuario.NombreCompleto}");
            
            // Si no hay usuarios administradores, permitir crear el primero sin autorización
            var adminExistente = await _context.Usuarios
                .Include(u => u.Rol)
                .AnyAsync(u => u.Rol!.Nombre == "Administrador" || u.Rol!.Nombre == "Admin");
                
            Console.WriteLine($"🔧 PostUsuario - ¿Admin existente?: {adminExistente}");

            if (adminExistente)
            {
                // Si ya hay admin, requerir autorización
                var currentUser = HttpContext.User;
                var hasAdminRole = currentUser.IsInRole("Administrador");
                var hasAdminRoleAlt = currentUser.IsInRole("Admin");
                
                Console.WriteLine($"🔧 PostUsuario - Usuario actual roles: Administrador={hasAdminRole}, Admin={hasAdminRoleAlt}");
                Console.WriteLine($"🔧 PostUsuario - Claims del usuario: {string.Join(", ", currentUser.Claims.Select(c => $"{c.Type}={c.Value}"))}");
                
                if (!hasAdminRole && !hasAdminRoleAlt)
                {
                    Console.WriteLine("🔧 PostUsuario - ACCESO DENEGADO: Usuario no tiene permisos de administrador");
                    return Forbid("Solo los administradores pueden crear usuarios");
                }
                
                Console.WriteLine("🔧 PostUsuario - ACCESO PERMITIDO: Usuario tiene permisos de administrador");
            }

            Console.WriteLine("🔧 PostUsuario - Guardando usuario en base de datos...");
            _context.Usuarios.Add(usuario);
            await _context.SaveChangesAsync();
            Console.WriteLine("🔧 PostUsuario - Usuario creado exitosamente");

            return CreatedAtAction("GetUsuario", new { id = usuario.Id }, usuario);
        }

        // DELETE: api/Usuarios/5
        [Authorize(Roles = "Administrador,Admin")]
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUsuario(int id)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
            {
                return NotFound();
            }

            _context.Usuarios.Remove(usuario);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UsuarioExists(int id)
        {
            return _context.Usuarios.Any(e => e.Id == id);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequest request)
        {
            var user = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Correo == request.Correo && u.Contrasena == request.Contrasena);

            if (user == null || user.Rol == null)
                return Unauthorized("Credenciales inválidas");

            var jwtSettings = _configuration.GetSection("Jwt");
            var jwtKey = jwtSettings["Key"];
            
            if (string.IsNullOrEmpty(jwtKey))
                return StatusCode(500, "Configuración JWT inválida");
                
            var key = Encoding.ASCII.GetBytes(jwtKey);

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
            new Claim(ClaimTypes.NameIdentifier, user.Id.ToString()),
            new Claim(ClaimTypes.Email, user.Correo),
            new Claim(ClaimTypes.Role, user.Rol.Nombre)
        }),
                Expires = DateTime.UtcNow.AddMinutes(Convert.ToDouble(jwtSettings["DurationInMinutes"])),
                Issuer = jwtSettings["Issuer"],
                Audience = jwtSettings["Audience"],
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };

            var token = tokenHandler.CreateToken(tokenDescriptor);
            var tokenString = tokenHandler.WriteToken(token);

            return Ok(new
            {
                token = tokenString,
                usuario = new
                {
                    user.Id,
                    user.NombreCompleto,
                    user.Correo,
                    Rol = user.Rol.Nombre
                }
            });
        }

        [HttpGet("perfil/{id}")]
        public async Task<IActionResult> ObtenerPerfil(int id)
        {
            var usuario = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Id == id);

            if (usuario == null)
                return NotFound("Usuario no encontrado.");

            return Ok(new
            {
                usuario.Id,
                usuario.NombreCompleto,
                usuario.Correo,
                Rol = usuario.Rol?.Nombre ?? "Sin Rol"
            });
        }

        [HttpPut("actualizar/{id}")]
        public async Task<IActionResult> ActualizarPerfil(int id, [FromBody] ActualizarPerfilRequest request)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado.");

            usuario.NombreCompleto = request.NombreCompleto;
            usuario.Correo = request.Correo;

            await _context.SaveChangesAsync();
            return Ok("Perfil actualizado.");
        }

        [HttpPut("cambiar-password/{id}")]
        public async Task<IActionResult> CambiarPassword(int id, [FromBody] CambiarPasswordRequest request)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado.");

            if (usuario.Contrasena != request.ContrasenaActual)
                return BadRequest("La contraseña actual no es correcta.");

            usuario.Contrasena = request.NuevaContrasena;
            await _context.SaveChangesAsync();

            return Ok("Contraseña actualizada.");
        }

        [Authorize(Roles = "Administrador,Admin")]
        [HttpPatch("{id}/estado")]
        public async Task<IActionResult> CambiarEstadoUsuario(int id, [FromBody] CambiarEstadoUsuarioRequest request)
        {
            var usuario = await _context.Usuarios.FindAsync(id);
            if (usuario == null)
                return NotFound("Usuario no encontrado.");

            usuario.EstaActivo = request.EstaActivo;
            await _context.SaveChangesAsync();

            return Ok(new { message = "Estado del usuario actualizado correctamente." });
        }

        [Authorize(Roles = "Administrador,Admin")]
        [HttpGet("con-roles")]
        public async Task<ActionResult<IEnumerable<object>>> GetUsuariosConRoles()
        {
            Console.WriteLine("🔧 GetUsuariosConRoles - Iniciando consulta de usuarios");
            var currentUser = HttpContext.User;
            Console.WriteLine($"🔧 GetUsuariosConRoles - Claims del usuario: {string.Join(", ", currentUser.Claims.Select(c => $"{c.Type}={c.Value}"))}");
            
            var usuarios = await _context.Usuarios
                .Include(u => u.Rol)
                .Select(u => new
                {
                    u.Id,
                    u.NombreCompleto,
                    u.Correo,
                    u.EstaActivo,
                    u.FechaRegistro,
                    Rol = new
                    {
                        u.Rol!.Id,
                        u.Rol.Nombre
                    }
                })
                .ToListAsync();

            return Ok(usuarios);
        }

        [HttpPost("crear-admin-bootstrap")]
        public async Task<IActionResult> CrearAdministradorBootstrap()
        {
            // Verificar si ya existe un administrador
            var adminRole = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Administrador");
            if (adminRole == null)
            {
                // Crear rol de administrador si no existe
                adminRole = new Rol { Nombre = "Administrador" };
                _context.Roles.Add(adminRole);
                await _context.SaveChangesAsync();
            }

            // También asegurarnos de que existe el rol Cliente
            var clienteRole = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Cliente");
            if (clienteRole == null)
            {
                clienteRole = new Rol { Nombre = "Cliente" };
                _context.Roles.Add(clienteRole);
                await _context.SaveChangesAsync();
            }

            // Verificar si ya existe un usuario administrador
            var adminExistente = await _context.Usuarios
                .Include(u => u.Rol)
                .FirstOrDefaultAsync(u => u.Rol!.Nombre == "Administrador");

            if (adminExistente != null)
            {
                return Ok(new
                {
                    message = "Ya existe un usuario administrador en el sistema",
                    usuario = new
                    {
                        adminExistente.Id,
                        adminExistente.NombreCompleto,
                        adminExistente.Correo,
                        Rol = adminExistente.Rol?.Nombre ?? "Sin Rol"
                    }
                });
            }

            // Crear usuario administrador por defecto
            var admin = new Usuario
            {
                NombreCompleto = "Administrador del Sistema",
                Correo = "admin@aethereye.com",
                Contrasena = "admin123", // Contraseña temporal
                RolId = adminRole.Id,
                EstaActivo = true,
                FechaRegistro = DateTime.Now
            };

            _context.Usuarios.Add(admin);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                message = "Usuario administrador creado exitosamente",
                usuario = new
                {
                    admin.Id,
                    admin.NombreCompleto,
                    admin.Correo,
                    Rol = adminRole.Nombre,
                    PasswordTemporal = "admin123"
                }
            });
        }

        [HttpPost("corregir-rol-admin")]
        public async Task<IActionResult> CorregirRolAdmin()
        {
            // Buscar el rol "Admin" y cambiarlo a "Administrador"
            var rolAdmin = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Admin");
            if (rolAdmin != null)
            {
                rolAdmin.Nombre = "Administrador";
                await _context.SaveChangesAsync();
                
                return Ok(new { message = "Rol 'Admin' corregido a 'Administrador'" });
            }

            // Verificar si ya existe el rol correcto
            var rolAdministrador = await _context.Roles.FirstOrDefaultAsync(r => r.Nombre == "Administrador");
            if (rolAdministrador != null)
            {
                return Ok(new { message = "El rol 'Administrador' ya existe correctamente" });
            }

            // Si no existe ninguno, crear el rol correcto
            var nuevoRol = new Rol { Nombre = "Administrador" };
            _context.Roles.Add(nuevoRol);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Rol 'Administrador' creado correctamente" });
        }

    }
}