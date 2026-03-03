using Azure.Core;
using ct_foco_backend.Data;
using ct_foco_backend.DTOs;
using ct_foco_backend.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore; 

[ApiController]
[Route("api/[controller]")]
public class StaffMembersController : ControllerBase
{
    private readonly CtFocoDbContext _db;
    private readonly IWebHostEnvironment _env;

    public StaffMembersController(CtFocoDbContext db, IWebHostEnvironment env)
    {
        _db = db;
        _env = env;
    }

    // GET api/staffmembers
    [HttpGet]
    public async Task<IActionResult> GetAll()
    {
        var staff = await _db.TeamMembers.ToListAsync();
        var result = staff.Select(s => new
        {
            s.Id,
            s.Nome,
            s.Cargo,
            s.Email,
            s.Telefone,
            s.Especialidade,
            s.Status,
            FotoUrl = s.Foto != null
                ? $"{Request.Scheme}://{Request.Host}/uploads/{s.Foto}"
                : null
        });
        return Ok(result);
    }

    // POST api/staffmembers
    [HttpPost]
    public async Task<IActionResult> Create([FromForm] StaffMembersDto dto)
    {
        var staff = new StaffMembers
        {
            Nome = dto.Nome,
            Cargo = dto.Cargo,
            Email = dto.Email,
            Telefone = dto.Telefone,
            Especialidade = dto.Especialidade,
            Status = dto.Status
        };

        if (dto.Foto != null)
            staff.Foto = await SalvarFoto(dto.Foto);

        _db.TeamMembers.Add(staff);
        await _db.SaveChangesAsync();
        return Ok(staff);
    }

    // PUT api/staffmembers/5
    [HttpPut("{id}")]
    public async Task<IActionResult> Update(int id, [FromForm] StaffMembersDto dto)
    {
        var staff = await _db.TeamMembers.FindAsync(id);
        if (staff == null) return NotFound();

        staff.Nome = dto.Nome;
        staff.Cargo = dto.Cargo;
        staff.Email = dto.Email;
        staff.Telefone = dto.Telefone;
        staff.Especialidade = dto.Especialidade;
        staff.Status = dto.Status;

        if (dto.Foto != null)
        {
            // apaga foto antiga
            DeletarFoto(staff.Foto);
            staff.Foto = await SalvarFoto(dto.Foto);
        }

        await _db.SaveChangesAsync();
        return Ok(staff);
    }

    // DELETE api/staffmembers/5
    [HttpDelete("{id}")]
    public async Task<IActionResult> Delete(int id)
    {
        var staff = await _db.TeamMembers.FindAsync(id);
        if (staff == null) return NotFound();

        DeletarFoto(staff.Foto);
        _db.TeamMembers.Remove(staff);
        await _db.SaveChangesAsync();
        return NoContent();
    }

    // ── Helpers ──────────────────────────────────────────────────
    private async Task<string> SalvarFoto(IFormFile foto)
    {
        var webRootPath = _env.WebRootPath ?? Path.Combine(Directory.GetCurrentDirectory(), "wwwroot");
        var uploadsDir = Path.Combine(webRootPath, "uploads");
        Directory.CreateDirectory(uploadsDir);

        var fileName = $"{Guid.NewGuid()}{Path.GetExtension(foto.FileName)}";
        var filePath = Path.Combine(uploadsDir, fileName);

        using var stream = new FileStream(filePath, FileMode.Create);
        await foto.CopyToAsync(stream);

        return fileName;

        // salva só o nome, a URL é montada no GET
    }

    private void DeletarFoto(string? fileName)
    {
        if (string.IsNullOrEmpty(fileName)) return;
        var path = Path.Combine(_env.WebRootPath, "uploads", fileName);
        if (System.IO.File.Exists(path)) System.IO.File.Delete(path);
    }
}