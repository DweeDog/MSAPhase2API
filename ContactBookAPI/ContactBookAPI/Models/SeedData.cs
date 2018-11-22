using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace ContactBookAPI.Models
{
    public class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new ContactBookAPIContext(
                serviceProvider.GetRequiredService<DbContextOptions<ContactBookAPIContext>>()))
            {
                // Look for any movies.
                if (context.ContactItem.Count() > 0)
                {
                    return;   // DB has been seeded
                }

                context.ContactItem.AddRange(
                    new ContactItem
                    {
                        Title = "John Dingy",
                        Url = "https://i.kym-cdn.com/photos/images/original/001/371/723/be6.jpg",
                        Tags = "Plumber",
                        Description = "Great plumber cheap cost",
                        Uploaded = "07-10-18 4:20T18:25:43.511Z",
                        Width = "768",
                        Height = "432",
                        MobilePhone = 123456789
                    }


                );
                context.SaveChanges();
            }
        }
    }
}
