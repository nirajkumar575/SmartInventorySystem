using SmartInventory.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Domain.Interfaces
{
    public interface IAppSettingRepository : IGenericRepository<AppSetting>
    {
        Task<AppSetting?> GetSettingAsync();
    }
}
