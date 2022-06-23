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
using AppName.DataCenter.Server.Extensions;
using AppName.DataCenter.Server.Options;
using AppName.DataCenter.Server.Services.Abstractions;
using AppName.DataCenter.Shared.Requests;
using AppName.DataCenter.Shared.ViewModels;
using static AppName.DataCenter.Server.Helpers.DateTimeHelper;

namespace AppName.DataCenter.Server.Controllers
{
    [Authorize]
    public class ModNameController : ApiControllerBase
    {
        private readonly IMapper _mapper;

        public ModNameController(
            ILogger<ModNameController> logger,
            AppNameDbContext dbContext,
            IMapper mapper) : base(logger, dbContext)
        {
            _mapper = mapper;
        }

        [HttpGet(nameof(List))]
        public async Task<PageListViewModel<ModNameListItem>> List([FromQuery] ModNamePageListRequest request)
        {
            var query = EFContext.ModNames
                .WhereIF(request.Id > 0, p => p.Id == request.Id.Value)
                .WhereIF(Convert2LocalDateTime(request.CreateTimeMin, out var createTimeMin), e => e.CreateTime >= createTimeMin)
                .WhereIF(Convert2LocalDateTime(request.CreateTimeMax, out var createTimeMax), e => e.CreateTime < createTimeMax);
            int total = await query.CountAsync();
            if (total < 1)
            {
                return new() { Items = Array.Empty<ModNameListItem>() };
            }
            var items = await query
                .OrderBy(p => p.Id)
                .Skip((request.Index - 1) * request.Size)
                .Take(request.Size)
                .Select(p => new ModNameListItem()
                {
                    Id = p.Id,
                    Name = p.Name,
                    CreateTime = p.CreateTime,
                })
                .ToArrayAsync();
            return new() { Total = total, Items = items };
        }

        [HttpGet("{id:int}")]
        public async Task<ModNameDetailViewModel> Retrieve(int id)
        {
            if (id < 1)
            {
                return new() { Id = -1 };
            }
            var detail = await EFContext.ModNames
                .Where(p => p.Id == id)
                .Select(p => new ModNameDetailViewModel()
                {
                    Id = p.Id,
                    Name = p.Name,
                    CreateTime = p.CreateTime,
                })
                .FirstOrDefaultAsync();
            return detail;
        }

        [HttpPut("{id:int}")]
        public async Task<ModNameDetailViewModel> Update(int id, [FromForm] ModNameUpdateRequest request)
        {
            if (id < 1)
            {
                return new() { Id = -1 };
            }
            var entity = await EFContext.ModNames.FindAsync(id);
            if (entity is null)
            {
                return new() { Id = -1 };
            }
            if (!CopyChanges())
            {
                return new();
            }
            await EFContext.SaveChangesAsync();
            var detail = _mapper.Map<ModName, ModNameDetailViewModel>(entity);
            return detail;

            bool CopyChanges()
            {
                if (!string.IsNullOrEmpty(request.Name) && request.Name != entity.Name)
                {
                    return false;
                }
                return false;
            }
        }

        [HttpDelete("{id:int}")]
        public async Task<int> Delete(int id)
        {
            if (id < 1)
            {
                return -1;
            }
            var entity = await EFContext.ModNames.FindAsync(id);
            if (entity is null)
            {
                return 0;
            }
            EFContext.ModNames.Remove(entity);
            int affectedCount = await EFContext.SaveChangesAsync();
            return affectedCount;
        }
    }
}