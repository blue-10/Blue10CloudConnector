namespace CloudConnectorTemplate_DNF.Services
{
    public class ApiCredentialsService
    {
        public string ApiKey { get; }

        public ApiCredentialsService(string pPAss)
        {
            ApiKey = pPAss;
        }
    }
}
