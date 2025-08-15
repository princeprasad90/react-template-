using Microsoft.Extensions.Configuration;
using Novell.Directory.Ldap;

namespace backend.Services
{
    public class LdapService
    {
        private readonly IConfiguration _config;
        public LdapService(IConfiguration config)
        {
            _config = config;
        }

        public async Task<bool> ValidateCredentials(string username, string password)
        {
            var host = _config["Ldap:Host"] ?? "";
            var port = int.TryParse(_config["Ldap:Port"], out var p) ? p : 389;
            var dnTemplate = _config["Ldap:Dn"] ?? "";
            var userDn = string.Format(dnTemplate, username);

            try
            {
                using var connection = new LdapConnection();
                await connection.ConnectAsync(host, port);
                await connection.BindAsync(userDn, password);
                return connection.Bound;
            }
            catch
            {
                return false;
            }
        }
    }
}
