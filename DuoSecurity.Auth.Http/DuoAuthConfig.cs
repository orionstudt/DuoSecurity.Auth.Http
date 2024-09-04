namespace DuoSecurity.Auth.Http;

public class DuoAuthConfig
{
    public string Host { get; set; }

    public string IntegrationKey { get; set; }

    public string SecretKey { get; set; }
        
    public DuoAuthConfig(string host, string integrationKey, string secretKey)
    {
        Host = host;
        IntegrationKey = integrationKey;
        SecretKey = secretKey;
    }
}