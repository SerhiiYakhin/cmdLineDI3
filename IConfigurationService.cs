using System.Threading.Tasks;

namespace ConsoleApp
{
    public interface IConfigurationService
    {
        Task<EnvironmentType> GetEnvironmentAsync();
        Task<RoleType> GetRoleAsync();
        Task<(string email, string password)> GetUserAsync();
        Task PrintCurrentConfigurationAsync();
        Task SetEnvironmentAsync(EnvironmentType environment);
        Task SetRoleAsync(RoleType role);
        Task SetUserAsync(string email, string password);
    }
}
