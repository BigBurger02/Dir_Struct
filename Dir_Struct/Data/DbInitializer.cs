using System;
using System.Linq;
using Dir_Struct.Models;

namespace Dir_Struct.Data
{
    public class DbInitializer
    {
        public static void Initialize(FolderContext context)
        {
            context.Database.EnsureCreated();

            if (context.Folder_Entities.Any())
                return;

            var folders = new Folder_Entity[]
            {
                new Folder_Entity{ID=1, Name="/", OwnerID=0},
                new Folder_Entity{ID=2, Name="Creating Digital Images", OwnerID=1},
                new Folder_Entity{ID=3, Name="Resources", OwnerID=2},
                new Folder_Entity{ID=4, Name="Evidence", OwnerID=2},
                new Folder_Entity{ID=5, Name="Graphic Products", OwnerID=2},
                new Folder_Entity{ID=6, Name="Primary Sources", OwnerID=3},
                new Folder_Entity{ID=7, Name="Secondary Sources", OwnerID=3},
                new Folder_Entity{ID=8, Name="Process", OwnerID=5},
                new Folder_Entity{ID=9, Name="Final Product", OwnerID=5}
            };
            foreach (Folder_Entity f in folders)
                context.Folder_Entities.Add(f);
            context.SaveChanges();
        }
    }
}

