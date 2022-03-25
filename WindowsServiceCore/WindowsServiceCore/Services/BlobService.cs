namespace WindowsServiceCore
{
    public class BlobService : IBlobService
    {
        private readonly HttpClient mHttpClient;

        public BlobService(HttpClient pHttpClient)
        {
            mHttpClient = pHttpClient;
        }

        public async Task DownloadFile(string pFileName, string pFilePath)
        {
            // Downloading file using http
            var fResponse = await mHttpClient.GetAsync(pFileName);
            fResponse.EnsureSuccessStatusCode();

            // Writing response stream to file stream
            await using var fStream = await fResponse.Content.ReadAsStreamAsync();
            await using var fFileStream = File.Create(pFilePath);
            fStream.Seek(0, SeekOrigin.Begin);
            fStream.CopyTo(fFileStream);
        }
    }
}
