using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.VisualStudio.TestTools.UnitTesting;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using ContactBookAPI.Models;
using ContactBookAPI.Controllers;

namespace UnitTestContactAPI
{
    [TestClass]
    public class UnitTest1
    {

        public static readonly DbContextOptions<ContactBookAPIContext> options= new DbContextOptionsBuilder<ContactBookAPIContext>().UseInMemoryDatabase(databaseName: "testDatabase").Options;
        public static IConfiguration configuration = null;
        public static readonly IList<string> memeTitles = new List<string> { "contactItem1", "contactitem2" };

        [TestInitialize]
        public void SetupDb()
        {
            using (var context = new ContactBookAPIContext(options))
            {
                ContactItem contactitem1 = new ContactItem()
                {
                    Title = memeTitles[0]
                };

                ContactItem contactitem2 = new ContactItem()
                {
                    Title = memeTitles[1]
                };

                context.ContactItem.Add(contactitem1);
                context.ContactItem.Add(contactitem2);
                context.SaveChanges();
            }
        }

        [TestCleanup]
        public void ClearDb()
        {
            using (var context = new ContactBookAPIContext(options))
            {
                context.ContactItem.RemoveRange(context.ContactItem);
                context.SaveChanges();
            };
        }

        [TestMethod]
        public async Task TestPutMemeContactNoContentStatusCode()
        {
            using (var context = new ContactBookAPIContext(options))
            {
                // Given
                string title = "putMeme";
                ContactItem memeItem1 = context.ContactItem.Where(x => x.Title == memeTitles[0]).Single();
                memeItem1.Title = title;

                // When
                ContactItemsController ContactController = new ContactItemsController(context, configuration);
                IActionResult result = await ContactController.PutContactItem(memeItem1.Id, memeItem1) as IActionResult;

                // Then
                Assert.IsNotNull(result);
                Assert.IsInstanceOfType(result, typeof(NoContentResult));
            }
        }

        [TestMethod]
        public async Task TestPutContactItemUpdate()
        {
            using (var context = new ContactBookAPIContext(options))
            {
                // Given
                string title = "putMeme";
                ContactItem memeItem1 = context.ContactItem.Where(x => x.Title == memeTitles[0]).Single();
                memeItem1.Title = title;

                // When
                ContactItemsController ContactController = new ContactItemsController(context, configuration);
                IActionResult result = await ContactController.PutContactItem(memeItem1.Id, memeItem1) as IActionResult;

                // Then
                memeItem1 = context.ContactItem.Where(x => x.Title == title).Single();
            }
        }

        [TestMethod]
        public async Task TestPostContactItem()
        {

            using (var context = new ContactBookAPIContext(options))
            {
                // Given
                ContactItem memeitem1 = new ContactItem();

                // When
                ContactItemsController ContactController = new ContactItemsController(context, configuration);
                IActionResult result = await ContactController.PostContactItem(memeitem1) as IActionResult;

                
            }

        }
    }
}
