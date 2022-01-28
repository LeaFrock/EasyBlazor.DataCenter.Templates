using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using AppName.DataCenter.Server.Data;
using AppName.DataCenter.Server.Data.Models;
using AppName.DataCenter.Server.Options;
using AppName.DataCenter.Server.Services.Abstractions;
using AppName.DataCenter.Shared.Requests;
using AppName.DataCenter.Shared.ViewModels;

namespace AppName.DataCenter.Server.Controllers
{
    [Authorize]
    public class AccountController : ApiControllerBase
    {
        private readonly IMapper _mapper;
        private readonly IPasswordCryptor _passwordCryptor;

        private readonly JwtOptions _jwtOpt;

        public AccountController(
            ILogger<AccountController> logger,
            AppNameDbContext dbContext,
            IMapper mapper,
            IPasswordCryptor passwordCryptor,
            IOptions<JwtOptions> jwtOpt) : base(logger, dbContext)
        {
            _jwtOpt = jwtOpt.Value;
            _mapper = mapper;
            _passwordCryptor = passwordCryptor;
        }

        [AllowAnonymous]
        [HttpPost(nameof(Login))]
        public async Task<IActionResult> Login([FromForm] AccountLoginRequest request)
        {
            string pwd = _passwordCryptor.Encrypt(request.Password, default);
            var admin = await EFContext.Admins
                .AsNoTracking()
                .Where(a => a.Account == request.Account && a.Password == pwd)
                .Select(a => new { a.Id, a.Account, a.Name, a.Role })
                .FirstOrDefaultAsync();
            if (admin is null)
            {
                return Unauthorized();
            }
            var claims = new List<Claim>
            {
                new(ClaimTypes.NameIdentifier, admin.Id.ToString()),
                new(ClaimTypes.Name, admin.Account),
                new(ClaimTypes.GivenName, admin.Name),
                new(ClaimTypes.Role, admin.Role)
            };
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_jwtOpt.Key));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiry = DateTime.Now.AddHours(_jwtOpt.ExpiryInHours);
            var token = new JwtSecurityToken(_jwtOpt.Issuer, _jwtOpt.Audience, claims, expires: expiry, signingCredentials: creds);
            var tokenText = new JwtSecurityTokenHandler().WriteToken(token);
            return Ok(tokenText);
        }

        [HttpGet]
        public async Task<AccountInfoViewModel> Retrieve()
        {
            string idStr = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idStr, out int id))
            {
                return default;
            }
            var admin = await EFContext.Admins
                .AsNoTracking()
                .Where(a => a.Id == id)
                .Select(a => new AccountInfoViewModel()
                {
                    Id = a.Id,
                    Account = a.Account,
                    Name = a.Name,
                    CreateTime = a.CreateTime,
                })
                .FirstOrDefaultAsync();
            return admin;
        }

        [HttpPut]
        public async Task<AccountInfoViewModel> Update([FromForm] AccountInfoUpdateRequest request)
        {
            string idStr = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idStr, out int id))
            {
                return default;
            }
            var admin = await EFContext.Admins.FindAsync(id);
            if (admin is null)
            {
                return default;
            }
            if (!CopyChanges())
            {
                return new();
            }
            int affectedCount = await EFContext.SaveChangesAsync();
            if (affectedCount < 1)
            {
                return new();
            }
            return _mapper.Map<Admin, AccountInfoViewModel>(admin);

            bool CopyChanges()
            {
                bool hasChanges = false;
                if (admin.Name != request.Name)
                {
                    admin.Name = request.Name;
                    hasChanges = true;
                }
                return hasChanges;
            }
        }

        [HttpPut(nameof(Password))]
        public async Task<int> Password([FromForm] AccountPasswordRequest request)
        {
            if (request.Old == request.New)
            {
                return 0;
            }
            string idStr = User?.FindFirstValue(ClaimTypes.NameIdentifier);
            if (!int.TryParse(idStr, out int id))
            {
                return -1;
            }
            var admin = await EFContext.Admins.FindAsync(id);
            if (admin is null)
            {
                return -1;
            }
            string oldPwd = _passwordCryptor.Encrypt(request.Old, default);
            if (admin.Password != oldPwd)
            {
                return -2;
            }
            string newPwd = _passwordCryptor.Encrypt(request.New, default);
            admin.Password = newPwd;
            int affectedCount = await EFContext.SaveChangesAsync();
            return affectedCount;
        }

        [HttpPost(nameof(Logout))]
        public async Task<IActionResult> Logout()
        {
            /// TODO
            await Task.CompletedTask;
            return NoContent();
        }
    }
}