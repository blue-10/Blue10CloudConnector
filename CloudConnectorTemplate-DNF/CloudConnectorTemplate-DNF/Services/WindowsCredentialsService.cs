using CloudConnectorTemplate_DNF.Interface;
using CredentialManagement;

namespace CloudConnectorTemplate_DNF.Services
{
    public class WindowsCredentialService : IAuthenticationService
    {
        internal const string API_KEY_TARGET = "ApiKey";

        private ApiCredentialsService GetCredential(string pTarget)
        {
            var fCm = new Credential { Target = pTarget };
            fCm.Load();
            return new ApiCredentialsService(fCm.Password);
        }

        private bool SetCredentials(string pTarget, string pUsername, string pPAssword, PersistanceType pPErsistenceType)
        {
            return new Credential
            {
                Target = pTarget,
                Username = pUsername,
                Password = pPAssword,
                PersistanceType = pPErsistenceType
            }.Save();
        }

        private static bool RemoveCredentials(string pTarget) => new Credential { Target = pTarget }.Delete();

        public string GetApiKey() => GetCredential(API_KEY_TARGET).ApiKey ?? "";

        public void SetApiKey(string pApiKey) => SetCredentials(API_KEY_TARGET, string.Empty, pApiKey, PersistanceType.LocalComputer);
    }
}
