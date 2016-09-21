using System;
namespace WebFramework.Models.GDrive
{
    public class FileContent
    {
        public int Id { get; set; }
        public string SecurityCode { get; set; }
        public string FileName { get; set; }
        public byte[] Content { get; set; }
        public DateTime? UploadedOn { get; set; }
    }
}
