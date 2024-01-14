using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace DataAccessLayer.Models
{
    public class Attachments
    {
        public int AttachmentID {  get; set; }
        public int ApplicationID {  get; set; }
        [Required]
        public byte[] Attachment { get; set; }
    }
}
