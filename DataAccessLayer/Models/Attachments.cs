using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Attachments
    {
        public int AttachmentID {  get; set; }
        public string AttachmentName {  get; set; }
        public int ApplicationID {  get; set; }
        public byte[] Attachment { get; set; }
    }
}
