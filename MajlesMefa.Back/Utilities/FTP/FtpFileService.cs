using FluentFTP;
using System.Net;

namespace MajlesMefa.Back.Utilities.FTP
{
    public sealed class FtpFileService : IFtpFileService
    {
        private readonly FtpOptions _options;

        public FtpFileService(FtpOptions options)
        {
            _options = options;
        }

        private AsyncFtpClient CreateFtpClient()
        {
            var ftpClient = new AsyncFtpClient();
            ftpClient.Host = _options.Host;
            ftpClient.Credentials = new NetworkCredential(_options.Username, _options.Password);
            return ftpClient;
        }

        public async void CreateDirectoryIfNotExistAsync(string path, CancellationToken cancellationToken)
        {
            using (var ftpClient = CreateFtpClient())
            {
                await ftpClient.AutoConnect();

                await ftpClient.CreateDirectory(path);
            }
            
        }

        public async Task<Stream> DownloadAsync(Stream outStream, string remoteFilePath, CancellationToken cancellationToken)
        {
            using (var ftpClient = CreateFtpClient())
            {
                await ftpClient.AutoConnect();

                outStream.Position = 0;

                var isDownloaded = await ftpClient.DownloadStream(outStream, remoteFilePath, token: cancellationToken);

                return (isDownloaded) ? outStream : throw new Exception("The file couldn't be downloded from ftp server");
            }
        }

        public async Task UploadAsync(Stream stream, string remoteFilePath, CancellationToken cancellationToken)
        {
            using var ftpClient = CreateFtpClient();
            
                await ftpClient.AutoConnect();

                stream.Position = 0;

                FtpStatus ftpStatus = await ftpClient.UploadStream(stream, remoteFilePath, FtpRemoteExists.Overwrite, token: cancellationToken);

                if (ftpStatus == FtpStatus.Failed)
                    throw new Exception("The file couldn't upload to ftp server");
            
        }

        public async Task<bool> CheckIfExistsAsync(string remoteFilePath, CancellationToken cancellationToken)
        {
            using (var ftpClient = CreateFtpClient())
            {
                await ftpClient.AutoConnect();

                return await ftpClient.FileExists(remoteFilePath, cancellationToken);
            }
        }

        public async Task DeleteFileAsync(string filePath, CancellationToken cancellationToken)
        {
            using (var ftpClient = CreateFtpClient())
            {
                await ftpClient.AutoConnect();

                await ftpClient.DeleteFile(filePath, token: cancellationToken);
            }
        }

        public async Task RenameAsync(string oldFilePath, string newFilePath)
        {
            using (var ftpClient = CreateFtpClient())
            {
                await ftpClient.AutoConnect();

                await ftpClient.Rename(oldFilePath, newFilePath);
            }
        }

        public async Task<MemoryStream> FetchFileToMemoryAsync(string path)
        {

            using var ftpClient = CreateFtpClient();

            await ftpClient.AutoConnect();

            var memoryStream = new MemoryStream();

            await using var fileStream = new FileStream(path, FileMode.Open);

            await fileStream.CopyToAsync(memoryStream);

            memoryStream.Position = 0;

            return memoryStream;
        }
    }
}