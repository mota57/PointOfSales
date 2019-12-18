using System.Threading.Tasks;
using PointOfSales.Core.Infraestructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Linq;
using System.Collections.Generic;
using PointOfSales.Core.Extensions;

namespace PointOfSales.WebUI.Controllers
{
    public class FileContentManager
    {
        private readonly IHostingEnvironment webhost;
        private string fullFolderPath;
        private const string rootFolder = "images";

        public FileContentManager(IHostingEnvironment webhost, string folderName)
        {
            if (folderName.IsBlank()) throw new ArgumentNullException("folderName");

            this.webhost = webhost;
            fullFolderPath = Path.Combine(webhost.WebRootPath, rootFolder, folderName);

            if (!Directory.Exists(fullFolderPath))
                throw new DirectoryNotFoundException($"the path doesnt exists for {folderName} in {rootFolder}");
        }

        private string GetFilePath(string filename)
            => Path.Combine(fullFolderPath, filename);

        public List<OperationResponse> RemoveFileFromDisk(string[] fileNames)
        {
            var operations = new List<OperationResponse>();

            foreach (var filename in fileNames)
            {
                var pathToSave = GetFilePath(filename);

                try
                {

                    if (System.IO.File.Exists(pathToSave))
                        System.IO.File.Delete(pathToSave);

                    operations.Add(new OperationResponse());
                }
                catch (IOException ex)
                {
                    operations.Add(new OperationResponse(ex));
                }
            }
            return operations;
        }



        public async Task<List<string>> SaveFileToDisk(IFormFile[] files, Func<IFormFile, bool> ShouldSaveFile = null)
        {
            var result = new List<string>();

            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    if (ShouldSaveFile != null && !ShouldSaveFile(file))
                    {
                        result.Add(null);
                        continue;
                    }

                    var extension = Path.GetExtension(file.FileName);
                    var fileName = $"{Guid.NewGuid()}{extension}";
                    var filePath = GetFilePath(fileName.ToString());
                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await file.CopyToAsync(fileStream);
                    }
                    result.Add(fileName);
                }
            }
            return result;
        }

        public async Task<List<string>> SaveImages(IFormFile[] files, bool allOrNothing = true)
        {
            if(allOrNothing == true && files.Any(file => !file.ContentType.StartsWith("image/")))
            {
                throw new FormatException("ALL FILES MUST BE AN IMAGE");
            }
            var result = await SaveFileToDisk(files, (IFormFile file) => file.ContentType.StartsWith("image/"));
            return result;
        }
    }
}
