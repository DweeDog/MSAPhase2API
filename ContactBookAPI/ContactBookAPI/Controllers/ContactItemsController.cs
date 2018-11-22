using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ContactBookAPI.Models;
using Microsoft.WindowsAzure.Storage.Blob;
using Microsoft.WindowsAzure.Storage;
using Microsoft.WindowsAzure.Storage.Auth;
using ContactBookAPI.Helpers;
using Microsoft.Extensions.Configuration;

namespace ContactBookAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ContactItemsController : ControllerBase
    {
        private readonly ContactBookAPIContext _context;

        private IConfiguration _configuration;

        public ContactItemsController(ContactBookAPIContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        // GET: api/ContactItems
        [HttpGet]
        public IEnumerable<ContactItem> GetContactItem()
        {
            return _context.ContactItem;
        }

        // GET: api/ContactItems/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetContactItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactItem = await _context.ContactItem.FindAsync(id);

            if (contactItem == null)
            {
                return NotFound();
            }

            return Ok(contactItem);
        }

        // PUT: api/ContactItems/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutContactItem([FromRoute] int id, [FromBody] ContactItem contactItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (id != contactItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(contactItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!ContactItemExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/ContactItems
        [HttpPost]
        public async Task<IActionResult> PostContactItem([FromBody] ContactItem contactItem)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            _context.ContactItem.Add(contactItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetContactItem", new { id = contactItem.Id }, contactItem);
        }

        // DELETE: api/ContactItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteContactItem([FromRoute] int id)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var contactItem = await _context.ContactItem.FindAsync(id);
            if (contactItem == null)
            {
                return NotFound();
            }

            _context.ContactItem.Remove(contactItem);
            await _context.SaveChangesAsync();

            return Ok(contactItem);
        }

        private bool ContactItemExists(int id)
        {
            return _context.ContactItem.Any(e => e.Id == id);
        }

        // GET: api/Meme/Tags
        [Route("tags")]
        [HttpGet]
        public async Task<List<string>> GetTags()
        {
            var contacts = (from m in _context.ContactItem
                         select m.Tags).Distinct();

            var returned = await contacts.ToListAsync();

            return returned;
        }

        [HttpPost, Route("upload")]
        public async Task<IActionResult> UploadFile([FromForm]ContactBookItem meme)
        {
            if (!MultipartRequestHelper.IsMultipartContentType(Request.ContentType))
            {
                return BadRequest($"Expected a multipart request, but got {Request.ContentType}");
            }
            try
            {
                using (var stream = meme.Image.OpenReadStream())
                {
                    var cloudBlock = await UploadToBlob(meme.Image.FileName, null, stream);
                    //// Retrieve the filename of the file you have uploaded
                    //var filename = provider.FileData.FirstOrDefault()?.LocalFileName;
                    if (string.IsNullOrEmpty(cloudBlock.StorageUri.ToString()))
                    {
                        return BadRequest("An error has occured while uploading your file. Please try again.");
                    }

                    ContactItem memeItem = new ContactItem();
                    memeItem.Title = meme.Title;
                    memeItem.Tags = meme.Tags;

                    System.Drawing.Image image = System.Drawing.Image.FromStream(stream);
                    memeItem.Height = image.Height.ToString();
                    memeItem.Width = image.Width.ToString();
                    memeItem.Url = cloudBlock.SnapshotQualifiedUri.AbsoluteUri;
                    memeItem.Uploaded = DateTime.Now.ToString();

                    _context.ContactItem.Add(memeItem);
                    await _context.SaveChangesAsync();

                    return Ok($"File: {meme.Title} has successfully uploaded");
                }
            }
            catch (Exception ex)
            {
                return BadRequest($"An error has occured. Details: {ex.Message}");
            }


        }

        private async Task<CloudBlockBlob> UploadToBlob(string filename, byte[] imageBuffer = null, System.IO.Stream stream = null)
        {

            var accountName = _configuration["AzureBlob:name"];
            var accountKey = _configuration["AzureBlob:key"]; ;
            var storageAccount = new CloudStorageAccount(new StorageCredentials(accountName, accountKey), true);
            CloudBlobClient blobClient = storageAccount.CreateCloudBlobClient();

            CloudBlobContainer imagesContainer = blobClient.GetContainerReference("images");

            string storageConnectionString = _configuration["AzureBlob:connectionString"];

            // Check whether the connection string can be parsed.
            if (CloudStorageAccount.TryParse(storageConnectionString, out storageAccount))
            {
                try
                {
                    // Generate a new filename for every new blob
                    var fileName = Guid.NewGuid().ToString();
                    fileName += GetFileExtention(filename);

                    // Get a reference to the blob address, then upload the file to the blob.
                    CloudBlockBlob cloudBlockBlob = imagesContainer.GetBlockBlobReference(fileName);

                    if (stream != null)
                    {
                        await cloudBlockBlob.UploadFromStreamAsync(stream);
                    }
                    else
                    {
                        return new CloudBlockBlob(new Uri(""));
                    }

                    return cloudBlockBlob;
                }
                catch (StorageException ex)
                {
                    return new CloudBlockBlob(new Uri(""));
                }
            }
            else
            {
                return new CloudBlockBlob(new Uri(""));
            }

        }

        private string GetFileExtention(string fileName)
        {
            if (!fileName.Contains("."))
                return ""; //no extension
            else
            {
                var extentionList = fileName.Split('.');
                return "." + extentionList.Last(); //assumes last item is the extension 
            }
        }

        // GET: api/Meme/Tags

        [HttpGet]
        [Route("tag")]
        public async Task<List<ContactItem>> GetTagsItem([FromQuery] string tags)
        {
            var memes = from m in _context.ContactItem
                        select m; //get all the memes


            if (!String.IsNullOrEmpty(tags)) //make sure user gave a tag to search
            {
                memes = memes.Where(s => s.Tags.ToLower().Equals(tags.ToLower())); // find the entries with the search tag and reassign
            }

            var returned = await memes.ToListAsync(); //return the memes

            return returned;
        }
    }
}