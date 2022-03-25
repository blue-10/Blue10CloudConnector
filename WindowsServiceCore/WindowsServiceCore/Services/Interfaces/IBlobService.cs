namespace WindowsServiceCore
{
    public interface IBlobService
    {
        /// <summary>
        /// Function that downloads files from the blob storage
        /// </summary>
        /// <param name="pFileName">The name of the file that will be downloaded.</param>
        /// <param name="pFilePath">The path (incl. name) where the file will be saved.</param>
        /// <returns></returns>
        Task DownloadFile(string pFileName, string pFilePath);
    }
}
