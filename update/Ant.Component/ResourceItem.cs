using System;
using System.Collections.Generic;
using System.Text;

namespace Ant.Component
{
    public class ResourceItem
    {
        public ResourceItem()
        {

        }
        public string Name
        {
            get
            {
                return System.IO.Path.GetFileName(FullName);
            }
        }
        public string FullName
        {
            get;
            set;
        }
        public ResourceType Type
        {
            get;
            set;
        }
        public DateTime ModifyDate
        {
            get;
            set;
        }
        public List<ResourceItem> Childs
        {
            get;
            set;
        }
    }
    public enum ResourceType
    {
        Folder,
        File
    }
   
}
