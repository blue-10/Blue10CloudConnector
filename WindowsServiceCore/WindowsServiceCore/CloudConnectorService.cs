using System.Diagnostics;
using System.Text.Json;
using System.Timers;
using WindowsServiceCore.Models;

namespace WindowsServiceCore
{
    public class CloudConnectorService
    {
        
        private readonly IBlobService mBlobService;
        private readonly string mExecutableDirectory = AppDomain.CurrentDomain.BaseDirectory;

        // Executable constants
        private const string EXECUTABLE_NAME = "WindowsServiceCore.exe";
        private const string TEMP_NAME = "WindowsServiceCoreTemp.exe";
        private const string ARCHIVED_NAME = "WindowsServiceCoreOld.exe";

        // Versioning and debug constants
        private const string VERSION_FILE = "versions.json";
        private const string DEBUG_FILE = "debug.txt";

        public CloudConnectorService(IBlobService pBlobService) => 
            mBlobService = pBlobService;

        public void Start()
        {
            Log("Starting");

            // Set up a timer that triggers every minute.
            var fTimer = new System.Timers.Timer
            {
                Interval = 60000 // 60 seconds
            };
            fTimer.Elapsed += new ElapsedEventHandler(OnTimer);
            fTimer.Start();
        }

        private async void OnTimer(object? pSender, ElapsedEventArgs pArgs)
        {
            // Check for update, return if there isn't one.
            if (!await HasUpdate()) return;

            Log("Update available, installing now");
            await Update();
        }

        private async Task<bool> HasUpdate()
        {
            // Try to download the versions json, otherwise log exception
            try
            {
                await mBlobService.DownloadFile(VERSION_FILE, GetDirectoryPath(VERSION_FILE));
            }
            catch (Exception fException)
            {
                Log(fException.Message);
                return false;
            }

            // Compare versions
            var fRemoteVersionFile = await File.ReadAllTextAsync(GetDirectoryPath(VERSION_FILE));
            var fRemoteVersion = JsonSerializer.Deserialize<VersionData>(fRemoteVersionFile);
            if (fRemoteVersion == null) return false;

            // Get current version of executable into an array
            var fLocalVersionInfo = FileVersionInfo.GetVersionInfo(mExecutableDirectory + EXECUTABLE_NAME);
            var fLocalVersionString = fLocalVersionInfo.FileVersion ?? "0.0.0.0";
            var fLocalVersionArray = fLocalVersionString.Split('.');

            Log("Checking for update..");

            Log($"Local Version: {fLocalVersionString}, Remote Version: {fRemoteVersion.Version}");

            // Loop version from major -> minor -> patch -> revision
            for (var f = 0; f < fLocalVersionArray.Length; f++)
            {
                // If Major, Minor, Patch or Revision Number is higher or equal in current continue to next.
                if (int.Parse(fLocalVersionArray[f]) >= int.Parse(fRemoteVersion.VersionArray[f])) continue;

                // Update available
                return true;
            }

            return false;
        }

        private async Task Update()
        {
            // Get useful variables
            var fExecutablePath = GetDirectoryPath(EXECUTABLE_NAME);
            var fUpdatePath = GetDirectoryPath(TEMP_NAME);

            // Try to download the update, otherwise log exception
            try
            {
                await mBlobService.DownloadFile(EXECUTABLE_NAME, fUpdatePath);
            }
            catch (Exception fException)
            {
                Log(fException.Message);
                return;
            }

            // Update Application by moving the executables
            File.Move(fExecutablePath, GetDirectoryPath(ARCHIVED_NAME));
            File.Move(fUpdatePath, fExecutablePath);

            // Stop the service
            Environment.Exit(1);
        }

        public void Stop() => 
            Log("Stopping");

        private string GetDirectoryPath(string pFileName) => 
            $@"{mExecutableDirectory}{pFileName}";

        private void Log(string pMessage) =>
            File.AppendAllText(GetDirectoryPath(DEBUG_FILE), $"{DateTime.Now} : {pMessage}\n");

    }
}
