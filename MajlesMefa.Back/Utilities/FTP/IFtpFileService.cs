namespace MajlesMefa.Back.Utilities.FTP
{
    public interface IFtpFileService
    {
        Task<bool> CheckIfExistsAsync(string remoteFilePath, CancellationToken cancellationToken);
        Task DeleteFileAsync(string filePath, CancellationToken cancellationToken);
        Task<Stream> DownloadAsync(Stream outStream, string remoteFilePath, CancellationToken cancellationToken);
        Task RenameAsync(string oldFilePath, string newFilePath);
        Task UploadAsync(Stream stream, string remoteFilePath, CancellationToken cancellationToken);
        void CreateDirectoryIfNotExistAsync(string path, CancellationToken cancellationToken);
        Task<MemoryStream> FetchFileToMemoryAsync(string path);

    }
}