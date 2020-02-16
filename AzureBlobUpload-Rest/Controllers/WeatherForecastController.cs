using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;

namespace AzureBlobUpload_Rest.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class WeatherForecastController : ControllerBase
    {

        private DataUploader du;

        private static readonly string[] Summaries = new[]
        {
            "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
        };

        private readonly ILogger<WeatherForecastController> _logger;

        public WeatherForecastController(ILogger<WeatherForecastController> logger)
        {
            _logger = logger;
            du = new DataUploader();
        }

        [Route("Test")]
        [HttpGet]
        public string Test()
        {
            return "Test Successful";
        }

        [Route("PostFile")]
        [Microsoft.AspNetCore.Mvc.HttpPost]
        public async Task<String> PostFiles(List<IFormFile> files)
        {
            long size = files.Sum(f => f.Length);
            List<String> results = new List<string>();
            foreach (var formFile in files)
            {
                if (formFile.Length > 0)
                {
                    var ms = new MemoryStream();
                    formFile.CopyTo(ms);
                    var fileBytes = ms.ToArray();
                    var result = du.UploadFileToBlobAsync(formFile.FileName, fileBytes, formFile.ContentType).Result;
                    results.Add(result);
                }
            }
            return results.ToString();
        }

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }
    }
}
