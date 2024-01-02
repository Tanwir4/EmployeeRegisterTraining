using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.DTO
{
    public class DocumentDTO
    {
        public int documentId {  get; set; }
        public byte[] fileData { get; set; }
        
    }
}
