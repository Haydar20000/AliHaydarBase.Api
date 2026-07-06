using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AliHaydarBase.Api.HelperFunctions
{
    public static class ImageHelper
    {
        public static string? ConvertImageUrlToBase64(string? url)
        {
            if (string.IsNullOrWhiteSpace(url))
                return null;

            try
            {
                using var http = new HttpClient();
                var bytes = http.GetByteArrayAsync(url).Result;
                return Convert.ToBase64String(bytes);
            }
            catch
            {
                return null;
            }
        }
    }
}