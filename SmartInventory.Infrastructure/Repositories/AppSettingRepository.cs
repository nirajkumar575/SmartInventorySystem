using Microsoft.EntityFrameworkCore;
using SmartInventory.Domain.Entities;
using SmartInventory.Domain.Interfaces;
using SmartInventory.Infrastructure.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Infrastructure.Repositories
{
    public class AppSettingRepository : GenericRepository<AppSetting>, IAppSettingRepository
    {
        public AppSettingRepository(AppDbContext context) : base(context) {}

        public async Task<AppSetting?> GetSettingAsync()
        {
            return await _context.AppSettings.FirstOrDefaultAsync();
        }
    }
}
