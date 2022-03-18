namespace CloudConnectorTemplate.Services
{
    public class ApiCredentialsService
    {
        public string ApiKey { get; }

        public ApiCredentialsService(string pPass)
        {
            ApiKey = pPass;
        }
    }
}
