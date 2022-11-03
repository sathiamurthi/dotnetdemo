using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Demo.Core.Domain.Models;
using System.IO.Compression;
using System.IO;
using Microsoft.AspNetCore.StaticFiles;
using System.Net;
using System.Net.Http.Headers;
using ICSharpCode.SharpZipLib.Zip;
using Demo.Core.Api.Extensions;
using static System.Net.WebRequestMethods;

namespace Demo.Core.Api.Controllers
{
    [ApiController]

    public class FileProcessingController : ControllerBase
    {
        [HttpGet, Route("/download/zip", Name = "GetZipFile")]

        public async Task<IActionResult> DownloadZip()
        {
            //const string contentType = "application/zip";
            //HttpContext.Response.ContentType = contentType;
            var filename = "demozip";

            //var result = new FileContentResult(System.IO.File.ReadAllBytes(@"Files/cqrs-mediatr-aspnet-core-master.zip"), contentType)
            //{
            //    FileDownloadName = $"{filename}.zip"
            //};

            //return result;
            HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
           
            try
            {
                string folderPath = @"Files";
                DirectoryInfo d = new DirectoryInfo(folderPath);
                IList<FileInfo> Files = d.GetFiles().ToList();
                IList<FileDetail> files = new List<FileDetail>();
                Dictionary<string, Stream> streams = new Dictionary<string, Stream>();

                Parallel.ForEach(Files , file=>
                {
                    streams.Add(file.Name, new FileStream(Path.Combine(folderPath, file.Name), FileMode.Open, FileAccess.Read));
                });
               
                //foreach (FileInfo file in Files)
                //{
                //    streams.Add(file.Name, new FileStream(Path.Combine(folderPath, file.Name), FileMode.Open, FileAccess.Read));

                //}
                HttpContent? Content = new StreamContent(PackageManyZip(streams));
                const string contentType = "application/zip";
                HttpContext.Response.ContentType = contentType;
                byte[] byteArray  = await Content.ReadAsByteArrayAsync();
                var result = new FileContentResult(byteArray, contentType)
                {
                    FileDownloadName = $"{filename}.zip"
                };
                
                return result;
                // response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment") { FileName = string.Format("download_compressed_{0}.zip", DateTime.Now.ToString("yyyyMMddHHmmss")) };
                // response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
            }
            catch (Exception ex)
            {
                response.StatusCode = HttpStatusCode.InternalServerError;
                response.Content = new StringContent(ex.ToString());
            }
            return new EmptyResult();
        }

        private Stream PackageManyZip(Dictionary<string, Stream> streams)
        {
            byte[] buffer = new byte[6500];
            MemoryStream returnStream = new MemoryStream();
            var zipMs = new MemoryStream();

            using (ZipOutputStream zipStream = new ZipOutputStream(zipMs))
            {
                zipStream.SetLevel(9);
                foreach (var kv in streams)
                {
                    string fileName = kv.Key;
                    using (var streamInput = kv.Value)
                    {
                        zipStream.PutNextEntry(new ZipEntry(fileName));
                        while (true)
                        {
                            var readCount = streamInput.Read(buffer, 0, buffer.Length);
                            if (readCount > 0)
                            {
                                zipStream.Write(buffer, 0, readCount);
                            }
                            else
                            {
                                break;
                            }
                        }
                        zipStream.Flush();
                    }
                }

                zipStream.Finish();
                zipMs.Position = 0;
                zipMs.CopyTo(returnStream, 5600);
            }

            returnStream.Position = 0;
            return returnStream;
        }


        [HttpGet]
        [Route("/download/{id}")]
        public async Task<IActionResult> DownloadFile(int id)
        {
            DirectoryInfo d = new DirectoryInfo(@"Files");
            FileInfo[] Files = d.GetFiles();
            var filePath = @"Files/" + Files[id].Name;
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(filePath, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = await System.IO.File.ReadAllBytesAsync(filePath);
            return File(bytes, contentType, Path.GetFileName(filePath));

        }
        [HttpGet]
        [Route("/files")]
        public async Task<IEnumerable<FileDetail>> Index()
        {

            DirectoryInfo d = new DirectoryInfo(@"Files");
            FileInfo[] Files = d.GetFiles();
            IList<FileDetail> files = new List<FileDetail>();
            int i = 0;
            foreach (FileInfo file in Files)
            {
                var fileDetail = new FileDetail
                {

                    Name = file.Name,
                    Type = file.Extension,
                    CreationTime = file.CreationTime,
                    Description = "Demo file download",
                    Filelocation = Helper.GetBaseUrl(Request) + "/download/" + i.ToString(),
                    Id = i++

                };
                files.Add(fileDetail);

            }

            return files;

        }
       
    }
}
