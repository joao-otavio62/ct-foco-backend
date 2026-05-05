using ct_foco_backend.Data;
using ct_foco_backend.DTOs;
using ct_foco_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

[ApiController]
[Route("api/[controller]")]
public class MembersController : ControllerBase
{
    private readonly CtFocoDbContext _db;
    public MembersController(CtFocoDbContext db) => _db = db;

    // GET api/members
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var members = await _db.Members
            .OrderBy(m => m.Nome)
            .Select(m => new MembersDto
            {
                Id = m.Id,
                Nome = m.Nome,
                Email = m.Email,
                Pagamento = m.Pagamento,
                Vencimento = m.Vencimento,  // DateTime? → DateTime? direto
                Telefone = m.Telefone,
                DataNascimento = DateOnly.FromDateTime(m.DataNascimento),
                Altura = m.Altura,
                Modalidade = m.Modalidade,
                Horario = m.Horario,
                DataEntrada = DateOnly.FromDateTime(m.DataEntrada)
            })
            .ToListAsync();

        return Ok(members);
    }

    // POST api/members
    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MembersDto dto)
    {
        var member = new Members
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Pagamento = dto.Pagamento,
            Vencimento = dto.Vencimento,
            DataNascimento = DateTime.SpecifyKind(dto.DataNascimento.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc),
            Altura = dto.Altura,
            Modalidade = dto.Modalidade,
            Horario = dto.Horario,
            DataEntrada = DateTime.UtcNow,  
        };

        _db.Members.Add(member);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(GetAll), new { id = member.Id }, ToDto(member));
    }

    // PUT api/members/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MembersDto dto)
    {
        var member = await _db.Members.FindAsync(id);
        if (member is null)
            return NotFound($"Membro com id {id} não encontrado.");

        member.Nome = dto.Nome;
        member.Email = dto.Email;
        member.Pagamento = dto.Pagamento;
        member.Vencimento = dto.Vencimento;  
        member.Telefone = dto.Telefone;
        member.DataNascimento = DateTime.SpecifyKind(dto.DataNascimento.ToDateTime(TimeOnly.MinValue), DateTimeKind.Utc);
        member.Altura = dto.Altura;
        member.Modalidade = dto.Modalidade;
        member.Horario = dto.Horario;
        

        await _db.SaveChangesAsync();

        return Ok(ToDto(member));
    }

    // DELETE api/members/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var member = await _db.Members.FindAsync(id);
        if (member is null)
            return NotFound($"Membro com id {id} não encontrado.");

        _db.Members.Remove(member);
        await _db.SaveChangesAsync();

        return NoContent();
    }

    // Método auxiliar para evitar repetição
    private static MembersDto ToDto(Members m) => new()
    {
        Id = m.Id,
        Nome = m.Nome,
        Email = m.Email,
        Pagamento = m.Pagamento,
        Vencimento = m.Vencimento,
        Telefone = m.Telefone,
        DataNascimento = DateOnly.FromDateTime(m.DataNascimento),
        Altura = m.Altura,
        Modalidade = m.Modalidade,
        Horario = m.Horario,
        DataEntrada = DateOnly.FromDateTime(m.DataEntrada)
    };
}