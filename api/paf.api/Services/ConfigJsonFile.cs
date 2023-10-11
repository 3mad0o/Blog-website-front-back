using paf.api.Interfaces;

namespace paf.api.Services
{
    public class ConfigJsonFile:IConfigJsonFile
    {
        private readonly IConfiguration configuration;

        public ConfigJsonFile(IConfiguration configuration)
        {
            this.configuration = configuration;
        }
      

        public  string GetTokenIssuer()
        {
            return configuration["Token:Issuer"];
        }

        public string GetTokenKey()
        {
            return configuration["Token:Key"];
        }
    }
}
