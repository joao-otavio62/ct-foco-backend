using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ct_foco_backend.Controllers
{
	[ApiController]
	[Route("api/[controller]")]
	public class AuthController : ControllerBase
	{
		private readonly IConfiguration _config;

		public AuthController(IConfiguration config)
		{
			_config = config;
		}

		[HttpPost("login")]
		public IActionResult Login([FromBody] LoginDto dto)
		{
			var adminUser = _config["Admin:User"];
			var adminPass = _config["Admin:Pass"];

			if (dto.Username != adminUser || dto.Password != adminPass)
				return Unauthorized(new { message = "Usuário ou senha incorretos." });

			var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Secret"]!));
			var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

			var token = new JwtSecurityToken(
				claims: [new Claim(ClaimTypes.Role, "admin")],
				expires: DateTime.UtcNow.AddHours(8),
				signingCredentials: creds
			);

			return Ok(new { token = new JwtSecurityTokenHandler().WriteToken(token) });
		}
	}

	public class LoginDto
	{
		public string Username { get; set; } = "";
		public string Password { get; set; } = "";
	}
}