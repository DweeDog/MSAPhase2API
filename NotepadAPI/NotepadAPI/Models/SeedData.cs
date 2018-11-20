using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace NotepadAPI.Models
{
    public class SeedData
    {

        public static void Initialize(IServiceProvider serviceProvider)
        {
            using (var context = new NotepadAPIContext(
                serviceProvider.GetRequiredService<DbContextOptions<NotepadAPIContext>>()))
            {
                // Look for any movies.
                if (context.NotepadItem.Count() > 0)
                {
                    return;   // DB has been seeded
                }

                context.NotepadItem.AddRange(
                    new NotepadItem
                    {
                        Title = "StickyNote -> Test",
                        Url = "https://www.maplesoft.com/products/maple/new_features/images2016/mathequation.png",
                        Tags = "Maths",
                        Uploaded = "07-10-18 4:20T18:25:43.511Z",
                        Width = "768",
                        Height = "432"
                    }


                );
                context.SaveChanges();
            }
        }


    }
}
