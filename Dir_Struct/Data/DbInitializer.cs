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
                new Folder_Entity{ID=1, Name="Creating Digital Images", OwnerID=0},
                new Folder_Entity{ID=2, Name="Resources", OwnerID=1},
                new Folder_Entity{ID=3, Name="Evidence", OwnerID=1},
                new Folder_Entity{ID=4, Name="Graphic Products", OwnerID=1},
                new Folder_Entity{ID=5, Name="Primary Sources", OwnerID=2},
                new Folder_Entity{ID=6, Name="Secondary Sources", OwnerID=2},
                new Folder_Entity{ID=7, Name="Process", OwnerID=4},
                new Folder_Entity{ID=8, Name="Final Product", OwnerID=4}
            };
            foreach (Folder_Entity f in folders)
                context.Folder_Entities.Add(f);
            context.SaveChanges();
        }
    }
}

