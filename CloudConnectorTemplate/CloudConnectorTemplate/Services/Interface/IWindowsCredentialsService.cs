namespace CloudConnectorTemplate.Interface
{
    public interface IWindowsCredentialsService
    {
        string GetApiKey();
        void SetApiKey(string pApiKey);
    }
}
