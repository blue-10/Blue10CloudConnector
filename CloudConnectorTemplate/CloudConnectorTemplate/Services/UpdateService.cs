using System;
using System.Threading.Tasks;
using System.Windows;
using CloudConnectorTemplate.Interfaces;
using PureManApplicationDeployment;

namespace CloudConnectorTemplate.Services
{
    internal class UpdateService : IUpdateService
    {
        private readonly PureManClickOnce mClickOnce;

        public UpdateService()
        {
            mClickOnce = new PureManClickOnce("storage-endpoint");
        }

        public async Task UpdateIfAvailable()
        {
            if (mClickOnce == null) return;

            try
            {
                var fUpdateAvailable = await mClickOnce.UpdateAvailable();
                if(fUpdateAvailable) 
                    await Update();
            } 
            catch(Exception fException)
            {
                MessageBox.Show(fException.ToString());
            }
        }

        private async Task Update()
        {
            try
            {
                var fUpdateResult = await mClickOnce.Update();
                if (fUpdateResult)
                    Environment.Exit(0);
            }
            catch (Exception fException)
            {
                MessageBox.Show(fException.ToString());
            }
        }
    }
}
