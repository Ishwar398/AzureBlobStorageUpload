using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using System.IO;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace AzureBlobUpload_Rest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BlobController : ControllerBase
    {
        private DataUploader du;

        BlobController()
        {
            du = new DataUploader();
        }

        [Route("Test")]
        [HttpGet]
        public string Test()
        {
            return "Test Successful";
        }

        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<String> PostFiles(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);

            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var ms = new MemoryStream();
                    formFile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    var result = du.UploadFileToBlobAsync(formFile.FileName, fileBytes, formFile.ContentType).Result;
                    return result;
                }
            }

            return null;
        }
    }
}