using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using TrilhaApiDesafio.Context;
using TrilhaApiDesafio.Models;

namespace TrilhaApiDesafio.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TarefaController : ControllerBase
    {
        private readonly OrganizadorContext _context;

        public TarefaController(OrganizadorContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> ObterPorId(int id)
        {
            var entidade = await _context.Tarefas.FindAsync(id);
            
            if (entidade == null)
                return NotFound();
            
            return Ok(entidade);
        }

        [HttpGet("ObterTodos")]
        public async Task<IActionResult> ObterTodos()
        {
            var entidades = await _context.Tarefas.ToListAsync();

            if (entidades == null)
                return NotFound();
            
            return Ok(entidades);
        }

        [HttpGet("ObterPorTitulo")]
        public async Task<IActionResult> ObterPorTitulo(string titulo)
        {
            var entidades = await _context.Tarefas
                .Where(x => x.Titulo == titulo)
                .ToListAsync();

            if (entidades == null)
                return NotFound();
                
            return Ok(entidades);
        }

        [HttpGet("ObterPorData")]
        public async Task<IActionResult> ObterPorData(DateTime data)
        {
            var entities = await _context.Tarefas
                .Where(x => x.Data.Date == data.Date)
                .ToListAsync();
            
            if (entities == null)
                return NotFound();

            return Ok(entities);
        }

        [HttpGet("ObterPorStatus")]
        public async Task<IActionResult> ObterPorStatus(EnumStatusTarefa status)
        {
            var entidades = await _context.Tarefas
                .Where(x => x.Status == status)
                .ToListAsync();

            if (entidades == null)
                return NotFound();
                
            return Ok(entidades);
        }

        [HttpPost]
        public async Task<IActionResult> Criar(Tarefa tarefa)
        {
            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });
                
            if ((await _context.Tarefas.AddAsync(tarefa)) == null)
                return BadRequest();
                
            _context.SaveChanges();

            return CreatedAtAction(nameof(ObterPorId), new { id = tarefa.Id }, tarefa);
        }

        [HttpPut("{id}")]
        public IActionResult Atualizar(int id, Tarefa tarefa)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            if (tarefa.Data == DateTime.MinValue)
                return BadRequest(new { Erro = "A data da tarefa não pode ser vazia" });

            tarefaBanco.Titulo = tarefa.Titulo;
            tarefaBanco.Descricao = tarefa.Descricao;
            tarefaBanco.Status = tarefa.Status;
            tarefaBanco.Data = DateTime.Now;

            _context.Entry(tarefaBanco).State = EntityState.Modified;

            _context.SaveChanges();

            return Ok(tarefaBanco);
        }


        [HttpDelete("{id}")]
        public IActionResult Deletar(int id)
        {
            var tarefaBanco = _context.Tarefas.Find(id);

            if (tarefaBanco == null)
                return NotFound();

            _context.Tarefas.Remove(tarefaBanco);
            _context.SaveChanges();

            return NoContent();
        }
    }
}
