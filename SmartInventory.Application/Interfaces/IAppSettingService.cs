using SmartInventory.Application.DTOs.Settings;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SmartInventory.Application.Interfaces
{
    public interface IAppSettingService
    {
        Task<AppSettingDto?> GetAsync();
        Task<bool> SaveAsync(UpdateAppSettingDto dto);
    }
}
