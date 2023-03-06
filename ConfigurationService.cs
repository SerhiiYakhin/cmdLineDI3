using System.Text.Json;
using Microsoft.Extensions.Configuration;

namespace ConsoleApp
{
    public class ConfigurationService : IConfigurationService
    {
        private const string ConfigurationFileName = "config.json";
        private readonly IConfiguration _configuration;

        public ConfigurationService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public async Task SetEnvironmentAsync(EnvironmentType environment)
        {
            _configuration["Environment"] = environment.ToString();
            await SaveConfigurationAsync();
        }

        public async Task<EnvironmentType> GetEnvironmentAsync()
        {
            var environment = _configuration.GetValue<string>("Environment");
            if (string.IsNullOrEmpty(environment))
            {
                environment = EnvironmentType.Test.ToString();
                await SetEnvironmentAsync(EnvironmentType.Test);
            }

            return Enum.Parse<EnvironmentType>(environment);
        }

        public async Task SetRoleAsync(RoleType role)
        {
            _configuration["Role"] = role.ToString();
            await SaveConfigurationAsync();
        }

        public async Task<RoleType> GetRoleAsync()
        {
            var role = _configuration.GetValue<string>("Role");
            if (string.IsNullOrEmpty(role))
            {
                role = RoleType.GlobalAdmin.ToString();
                await SetRoleAsync(RoleType.GlobalAdmin);
            }

            return Enum.Parse<RoleType>(role);
        }

        public async Task SetUserAsync(string email, string password)
        {
            _configuration["Email"] = email;
            _configuration["Password"] = password;
            await SaveConfigurationAsync();
        }

        public async Task<(string email, string password)> GetUserAsync()
        {
            var email = _configuration.GetValue<string>("Email");
            var password = _configuration.GetValue<string>("Password");
            return (
                string.IsNullOrEmpty(email) ? "global-admin-user@gmail.com" : email,
                string.IsNullOrEmpty(password) ? "abc123" : password
            );
        }

        public async Task PrintCurrentConfigurationAsync()
        {
            var environment = await GetEnvironmentAsync();
            var role = await GetRoleAsync();
            var user = await GetUserAsync();

            await Console.Out.WriteLineAsync($"Environment: {environment}");
            await Console.Out.WriteLineAsync($"Role: {role}");
            await Console.Out.WriteLineAsync($"User email: {user.email}");
            await Console.Out.WriteLineAsync($"User password: {user.password}");
        }

        private async Task SaveConfigurationAsync()
        {
            var configurationRoot = (ConfigurationRoot)_configuration;
            var configPath = configurationRoot.GetDebugView().Split('\n')[0]
                .Replace("Path: ", "")
                .Trim();
            var configDirectory = Path.GetDirectoryName(configPath);

            var configJson = JsonSerializer.Serialize(
                _configuration.GetSection("Context"),
                new JsonSerializerOptions { WriteIndented = true }
            );
            await File.WriteAllTextAsync(
                Path.Combine(configDirectory, ConfigurationFileName),
                configJson
            );
        }
    }
}
