namespace CloudConnectorTemplate_DNF.Interface
{
    public interface IAuthenticationService
    {
        string GetApiKey();
        void SetApiKey(string pApiKey);
    }
}
