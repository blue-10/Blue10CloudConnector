using System;
using System.IO;
using System.Threading.Tasks;
using System.Timers;
using System.Text.Json;
using WindowsServiceFramework.Services.Interfaces;
using System.Diagnostics;
using WindowsServiceFramework.Models;

namespace WindowsServiceFramework
{
    public class CloudConnectorService
    {
        private IBlobService mBlobService;
        private readonly string mExecutableDirectory = AppDomain.CurrentDomain.BaseDirectory;

        // Executable constants
        private const string EXECUTABLE_NAME = "WindowsServiceFramework.exe";
        private const string TEMP_NAME = "WindowsServiceFrameworkTemp.exe";
        private const string ARCHIVED_NAME = "WindowsServiceFrameworkOld.exe";

        // Versioning and Debug constants
        private const string VERSION_FILE = "versions.json";
        private const string DEBUG_FILE = "debug.txt";

        private readonly bool CheckingForUpdates = false;

        public CloudConnectorService(IBlobService pDownloadClient) =>
            mBlobService = pDownloadClient;

        public void Start()
        {
            Log("Starting");
            var fTimer = new System.Timers.Timer(60000);
            fTimer.Elapsed += async (object pSender, ElapsedEventArgs pE) =>
            {
                if(CheckingForUpdates)
                    await CheckForUpdate();
            };
            fTimer.AutoReset = true;
            fTimer.Start();
        }

        private async Task CheckForUpdate()
        {
            //Check for update
            if (!await HasUpdate()) return;

            Log("Update available, installing now");
            // Install update
            await Update();
        }

        private async Task<bool> HasUpdate()
        {
            // Try to download the versions json, otherwise log exception
            try
            {
                await mBlobService.DownloadAsync(VERSION_FILE, VERSION_FILE);
            }
            catch (Exception fException)
            {
                Log(fException.Message);
                return false;
            }

            // Compare versions
            var fRemoteVersionFile = File.ReadAllText($@"{mExecutableDirectory}{VERSION_FILE}");

            var fRemoteVersion = JsonSerializer.Deserialize<VersionModel>(fRemoteVersionFile);
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
            var fExecutablePath = mExecutableDirectory + EXECUTABLE_NAME;

            // Try to download the update, otherwise log exception
            try
            {
                await mBlobService.DownloadAsync(TEMP_NAME, EXECUTABLE_NAME);
            }
            catch (Exception fException)
            {
                Log(fException.Message);
                return;
            }

            // Update Application by moving the executables
            File.Move(fExecutablePath, $"{mExecutableDirectory}{ARCHIVED_NAME}");
            File.Move($"{mExecutableDirectory}{TEMP_NAME}", fExecutablePath);

            // Stop the service
            Environment.Exit(1);
        }

        public void Stop() =>
            Log("Stopping");

        private void Log(string pMessage) =>
            File.AppendAllText($@"{mExecutableDirectory}{DEBUG_FILE}", DateTime.UtcNow.ToString() + " : " + pMessage + Environment.NewLine);
    }
}
