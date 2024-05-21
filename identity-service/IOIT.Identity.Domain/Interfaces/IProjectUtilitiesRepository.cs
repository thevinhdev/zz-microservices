using IOIT.Identity.Domain.Entities;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace IOIT.Identity.Domain.Interfaces
{
    public interface IProjectUtilitiesRepository : IAsyncGenericRepository<ProjectUtilities, int>
    {
        Task<List<ProjectUtilities>> getByUtilitiesId(int utilitiesId);
        Task<List<int>> getProjectIdByUtilitiesId(int utilitiesId);
        Task<ProjectUtilities> getByProjectIdAndUtilitiesId(int projectId, int utilitiesId);
    }
}
