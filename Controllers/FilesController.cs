using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;

namespace CityInfo.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FilesController : ControllerBase
    {
        // This added the builder for the content type
        private readonly FileExtensionContentTypeProvider _extensionContentTypeProvider;

        public FilesController(FileExtensionContentTypeProvider extensionContentTypeProvider)
        {
            _extensionContentTypeProvider =
                extensionContentTypeProvider ?? throw new
                    System.ArgumentNullException(nameof(_extensionContentTypeProvider));
        }
        
        [HttpGet("{filesId}")] // returns a file by ID in this case we do not -
                               // have file ids so it will just return the file.
        
        public ActionResult GetFiles(string filesId)
        {
            //FileContentResult
            var pathToFile = "JustJordanT_RepoList.txt";
            if (!System.IO.File.Exists(pathToFile))
            {
                return NotFound();
            }

            if (!_extensionContentTypeProvider.TryGetContentType(pathToFile, out var contentType))
            {
                contentType = "application/octet-stream";
            }

            var bytes = System.IO.File.ReadAllBytes(pathToFile);
            return File(bytes, contentType,
                Path.GetFileName(pathToFile));
        }
    }
}
