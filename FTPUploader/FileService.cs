using Microsoft.Extensions.Configuration;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Threading;
using System.Threading.Tasks;

namespace FTPUploader
{
    public class FileService
    {
        private readonly string processedRoot, ftpUrl, ftpUser, ftpPass;
        private readonly int batchSize, maxParallelUploads;
        private readonly ConcurrentDictionary<string, bool> createdDirs = new();

        public FileService(IConfiguration config)
        {
            var settings = config.GetSection("FtpSettings");
            processedRoot = settings["ProcessedRoot"];
            ftpUrl = settings["FtpUrl"];
            ftpUser = settings["FtpUser"];
            ftpPass = settings["FtpPass"];
            batchSize = int.Parse(settings["BatchSize"] ?? "5");
            maxParallelUploads = int.Parse(settings["MaxParallelUploads"] ?? "3");
        }

        // Task ab bool return karega
        public async Task<bool> ProcessFiles(string pendingRoot, IProgress<int> progress)
        {
            if (!Directory.Exists(pendingRoot)) return false;

            string[] allFiles = Directory.GetFiles(pendingRoot, "*.*", SearchOption.AllDirectories);

            // AGAR FILES NAHI HAI: Toh false return karo
            if (allFiles == null || allFiles.Length == 0)
            {
                return false;
            }

            int totalFiles = allFiles.Length;
            int uploadedCount = 0;

            var folderGroups = allFiles.GroupBy(f => Path.GetDirectoryName(f))
                                       .ToDictionary(g => g.Key, g => new Queue<string>(g));

            SemaphoreSlim semaphore = new SemaphoreSlim(maxParallelUploads);

            foreach (var folder in folderGroups.Keys.ToList())
            {
                var queue = folderGroups[folder];
                string relativeDir = folder.Substring(pendingRoot.Length).TrimStart(Path.DirectorySeparatorChar).Replace("\\", "/");

                if (!string.IsNullOrEmpty(relativeDir) && !createdDirs.ContainsKey(relativeDir))
                {
                    await CreateFtpDirectory(relativeDir);
                    createdDirs.TryAdd(relativeDir, true);
                }

                while (queue.Count > 0)
                {
                    var batch = new List<string>();
                    for (int i = 0; i < batchSize && queue.Count > 0; i++) batch.Add(queue.Dequeue());

                    var tasks = batch.Select(async file =>
                    {
                        await semaphore.WaitAsync();
                        try
                        {
                            string relPath = file.Substring(pendingRoot.Length).TrimStart(Path.DirectorySeparatorChar).Replace("\\", "/");
                            await UploadFileWithRetry(file, ftpUrl.TrimEnd('/') + "/" + relPath);
                            MoveToProcessed(file, relPath);

                            Interlocked.Increment(ref uploadedCount);
                            int percent = (int)((double)uploadedCount / totalFiles * 100);
                            progress?.Report(percent);
                        }
                        finally { semaphore.Release(); }
                    });
                    await Task.WhenAll(tasks);
                }
            }
            return true; // Sab theek raha toh true
        }

        private async Task CreateFtpDirectory(string dirPath)
        {
            string currentPath = ftpUrl.TrimEnd('/');
            foreach (string folder in dirPath.Split('/'))
            {
                if (string.IsNullOrEmpty(folder)) continue;
                currentPath += "/" + folder;
                try
                {
                    var request = (FtpWebRequest)WebRequest.Create(currentPath);
                    request.Method = WebRequestMethods.Ftp.MakeDirectory;
                    request.Credentials = new NetworkCredential(ftpUser, ftpPass);
                    request.Proxy = null;
                    using (var resp = await request.GetResponseAsync()) { }
                }
                catch (WebException ex)
                {
                    if (!(ex.Response is FtpWebResponse resp && resp.StatusCode == FtpStatusCode.ActionNotTakenFileUnavailable)) throw;
                }
            }
        }

        private async Task UploadFileWithRetry(string localFile, string ftpPath)
        {
            for (int i = 0; i < 3; i++)
            {
                try
                {
                    var request = (FtpWebRequest)WebRequest.Create(ftpPath);
                    request.Method = WebRequestMethods.Ftp.UploadFile;
                    request.Credentials = new NetworkCredential(ftpUser, ftpPass);
                    request.Proxy = null;
                    using (var fs = new FileStream(localFile, FileMode.Open, FileAccess.Read))
                    using (var rs = await request.GetRequestStreamAsync()) await fs.CopyToAsync(rs);
                    return;
                }
                catch { if (i == 2) throw; await Task.Delay(1000); }
            }
        }

        private void MoveToProcessed(string src, string rel)
        {
            string dest = Path.Combine(processedRoot, rel);
            Directory.CreateDirectory(Path.GetDirectoryName(dest));
            if (File.Exists(dest)) File.Delete(dest);
            File.Move(src, dest);
        }
    }
}