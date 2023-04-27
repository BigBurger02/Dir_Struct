using System;

namespace Dir_Struct.Models
{
    public class Folder_Entity
    {
        public int ID { get; set; }
        public string Name { get; set; }
        public int OwnerID { get; set; }

        public List<Folder_Entity>? NestedFolders { get; set; }
    }
}

