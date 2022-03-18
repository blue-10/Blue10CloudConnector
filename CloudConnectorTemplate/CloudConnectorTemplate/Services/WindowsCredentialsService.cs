using CloudConnectorTemplate.Interface;
using CredentialManagement;

namespace CloudConnectorTemplate.Services
{
    public class WindowsCredentialService : IWindowsCredentialsService
    {
        internal const string API_KEY_TARGET = "CloudConnectorTemplateApiKey";

        private ApiCredentialsService GetCredential(string pTarget)
        {
            var fCm = new Credential { Target = pTarget };
            fCm.Load();
            return new ApiCredentialsService(fCm.Password);
        }

        private bool SetCredentials(string pTarget, string pUsername, string pPassword, PersistanceType pPersistenceType)
        {
            return new Credential
            {
                Target = pTarget,
                Username = pUsername,
                Password = pPassword,
                PersistanceType = pPersistenceType
            }.Save();
        }

        private static bool RemoveCredentials(string pTarget) => new Credential { Target = pTarget }.Delete();

        public string GetApiKey() => GetCredential(API_KEY_TARGET).ApiKey ?? "";

        public void SetApiKey(string pApiKey) => SetCredentials(API_KEY_TARGET, string.Empty, pApiKey, PersistanceType.LocalComputer);
    }
}
