using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using TTSteelAndroidAPI.Interface;
using TTSteelAndroidAPI.Model.Login;

[Route("api/")]
[ApiController]
public class LoginController : ControllerBase
{
    private readonly ISapService _sapService;
    private readonly IConfiguration _config;
    private readonly ILogger<LoginController> _logger;

    public LoginController(ISapService sapService, IConfiguration config, ILogger<LoginController> logger)
    {
        _sapService = sapService;
        _config = config;
        _logger = logger;
    }

    [HttpPost("Login")]

    public async Task<IActionResult> LoginAsync([FromBody] loginModel loginModel)
    {
        try
        {
            var result = await _sapService.LoginUserAsync(loginModel);
            var tokenJwt = GenerateJwtToken(loginModel.UserName, loginModel.Password, loginModel.CompanyDB);
            return Ok(new
            {
                success = true,
                message = "Login Successful",
                data = result, token = tokenJwt, });
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, "Error occurred during SAP B1 login.");

            return StatusCode(StatusCodes.Status500InternalServerError, new
            {
                Message = "An error occurred while logging into SAP B1. Please try again later.",
                Error = ex.Message
            });
        }
    }

    private string GenerateJwtToken(string username, string password, string dbname)
    {
        var claims = new[]
        {
        new Claim(ClaimTypes.Name, username),
        new Claim("PW", password),
        new Claim("Database", dbname),
        new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
    };

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var token = new JwtSecurityToken(
            issuer: _config["Jwt:Issuer"],
            audience: _config["Jwt:Audience"],
            claims: claims,
            expires: DateTime.UtcNow.AddMinutes(Convert.ToDouble(_config["Jwt:DurationInMinutes"])),
            signingCredentials: creds
        );

        return new JwtSecurityTokenHandler().WriteToken(token);
    }
}