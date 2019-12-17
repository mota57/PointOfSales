using System.Threading.Tasks;
using PointOfSales.Core.Infraestructure;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using System.IO;
using System;
using System.Collections.Generic;

namespace PointOfSales.WebUI.Controllers
{
    public class FileContentManager
    {
        private readonly IHostingEnvironment webhost;
        private string folderPath;
        private string rootFolder = "images";

        public FileContentManager(IHostingEnvironment webhost, string folderPath)
        {
            this.webhost = webhost;
            this.folderPath = folderPath;
        }

        public List<OperationResponse> RemoveFileFromDisk(string[] fileNames)
        {
            var operations = new List<OperationResponse>();

            foreach (var filename in fileNames)
            {

                try
                {
                    if (System.IO.File.Exists(GetFilePath(filename)))
                    {
                        System.IO.File.Delete(GetFilePath(filename));
                    }

                    operations.Add(new OperationResponse());
                }
                catch (IOException ex)
                {
                    operations.Add(new OperationResponse(ex));
                }
            }
            return operations;
        }

        private string GetFilePath(string filename)
            => Path.Combine(webhost.WebRootPath, rootFolder, folderPath, filename);


        public async Task<List<string>> SaveFileToDisk(IFormFile[] files)
        {
            var result = new List<string>();

            foreach (var file in files)
            {
                if (file != null && file.Length > 0)
                {
                    var extension = Path.GetExtension(file.Name);
                    var fileName = $"{Guid.NewGuid()}.{extension}";
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
    }
}
