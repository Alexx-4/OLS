using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace OpenLatino.Admin.Application.Utils
{
    public static class FIleHandler
    {
        public static async Task<byte[]> GetContent(IFormFile uploadIcon)
        {
            var filePath = Path.GetTempFileName();

            if (uploadIcon != null && uploadIcon.Length > 0 && uploadIcon.ContentType.Contains("image"))
            {
                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    await uploadIcon.CopyToAsync(stream);
                    using (var reader = new BinaryReader(stream))
                    {
                        reader.BaseStream.Position = 0;
                        var result = reader.ReadBytes((int)uploadIcon.Length);
                        return result;
                    }
                }
            }
            return null;
        }
    }
}
