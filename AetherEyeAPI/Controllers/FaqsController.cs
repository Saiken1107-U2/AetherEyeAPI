using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using AetherEyeAPI.Data;
using AetherEyeAPI.Models;

namespace AetherEyeAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FaqsController : ControllerBase
    {
        private readonly AetherEyeDbContext _context;

        public FaqsController(AetherEyeDbContext context)
        {
            _context = context;
        }

        // GET: api/Faqs/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Faq>> GetFaq(int id)
        {
            var faq = await _context.Faqs.FindAsync(id);

            if (faq == null)
            {
                return NotFound();
            }
            return faq;
        }

        [HttpPost("crear")]
        public async Task<IActionResult> CrearFaq([FromBody] Faq faq)
        {
            if (string.IsNullOrWhiteSpace(faq.Pregunta) || string.IsNullOrWhiteSpace(faq.Respuesta))
                return BadRequest("La pregunta y respuesta son obligatorias.");

            _context.Faqs.Add(faq);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                faq.Id,
                faq.Pregunta,
                faq.Respuesta
            });
        }

        [HttpGet]
        public async Task<IActionResult> ObtenerTodas()
        {
            var faqs = await _context.Faqs
                .Select(f => new
                {
                    f.Id,
                    f.Pregunta,
                    f.Respuesta
                })
                .ToListAsync();
            return Ok(faqs);
        }

        [HttpPut("{id}")]
        public async Task<IActionResult> EditarFaq(int id, [FromBody] Faq faq)
        {
            if (id != faq.Id)
                return BadRequest("El ID no coincide.");

            var existente = await _context.Faqs.FindAsync(id);
            if (existente == null)
                return NotFound("FAQ no encontrada.");

            existente.Pregunta = faq.Pregunta;
            existente.Respuesta = faq.Respuesta;

            await _context.SaveChangesAsync();
            return Ok("FAQ actualizada.");
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> EliminarFaq(int id)
        {
            var faq = await _context.Faqs.FindAsync(id);
            if (faq == null)
                return NotFound("FAQ no encontrada.");

            _context.Faqs.Remove(faq);
            await _context.SaveChangesAsync();

            return Ok("FAQ eliminada.");
        }
    }
}