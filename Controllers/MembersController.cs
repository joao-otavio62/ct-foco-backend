using System;
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

    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var members = await _db.Members
            .Select(m => new MembersDto
            {
                Id = m.Id,
                Nome = m.Nome,
                Email = m.Email,
                Pagamento = m.Pagamento,
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

    [HttpPost]
    public async Task<IActionResult> Create([FromBody] MembersDto dto)
    {
        var member = new Members
        {
            Nome = dto.Nome,
            Email = dto.Email,
            Telefone = dto.Telefone,
            DataNascimento = dto.DataNascimento.ToDateTime(TimeOnly.MinValue),
            Altura = dto.Altura,
            Modalidade = dto.Modalidade,
            Horario = dto.Horario,
            DataEntrada = dto.DataEntrada.ToDateTime(TimeOnly.MinValue)
        };
        _db.Members.Add(member);
        await _db.SaveChangesAsync();
        return CreatedAtAction(nameof(GetAll), new { id = member.Id }, new MembersDto
        {
            Id = member.Id,
            Nome = member.Nome,
            Email = member.Email,
            Pagamento = member.Pagamento,
            Telefone = member.Telefone,
            DataNascimento = DateOnly.FromDateTime(member.DataNascimento),
            Altura = member.Altura,
            Modalidade = member.Modalidade,
            Horario = member.Horario,
            DataEntrada = DateOnly.FromDateTime(member.DataEntrada)
        });
    }

    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromBody] MembersDto dto)
    {
        var member = await _db.Members.FindAsync(id);
        if (member == null)
        {
            return NotFound($"Membro com id {id} não encontrado.");
        }

        member.Nome = dto.Nome;
        member.Email = dto.Email;
        member.Pagamento = dto.Pagamento;
        member.Telefone = dto.Telefone;
        member.DataNascimento = dto.DataNascimento.ToDateTime(TimeOnly.MinValue);
        member.Altura = dto.Altura;
        member.Modalidade = dto.Modalidade;
        member.Horario = dto.Horario;
        member.DataEntrada = dto.DataEntrada.ToDateTime(TimeOnly.MinValue);

        _db.Members.Update(member);
        await _db.SaveChangesAsync();

        return Ok(new MembersDto
        {
            Id = member.Id,
            Nome = member.Nome,
            Email = member.Email,
            Pagamento = member.Pagamento,
            Telefone = member.Telefone,
            DataNascimento = DateOnly.FromDateTime(member.DataNascimento),
            Altura = member.Altura,
            Modalidade = member.Modalidade,
            Horario = member.Horario,
            DataEntrada = DateOnly.FromDateTime(member.DataEntrada)
        });
    }
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var member = await _db.Members.FindAsync(id);
        if (member == null)
        {
            return NotFound($"Membro com id {id} não encontrado.");
        }

        _db.Members.Remove(member);
        await _db.SaveChangesAsync();

        return NoContent();
    }
}