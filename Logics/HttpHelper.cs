using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;

namespace ServiceMonitor.Web.Logics
{
    public class HttpHelper
    {
        public static async Task<Stream?> HttpGetAsync(string url, Encoding encode = null, Dictionary<string, string> dicHeader = null, TimeSpan? timeout = null)
        {
            encode = encode ?? Encoding.UTF8;

            try
            {
                using (var httpClient = new HttpClient())
                {
                    if (timeout.HasValue)
                    {
                        httpClient.Timeout = timeout.Value;
                    }

                    if (dicHeader != null)
                    {
                        foreach (var kv in dicHeader)
                        {
                            switch (kv.Key)
                            {
                                case "ContentType": httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Content-Type", kv.Value); break;
                                case "Accept": httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Accept", kv.Value); break;
                                case "UserAgent": httpClient.DefaultRequestHeaders.TryAddWithoutValidation("User-Agent", kv.Value); break;
                                case "Referer": httpClient.DefaultRequestHeaders.TryAddWithoutValidation("Referer", UrlEncode(kv.Value, encode)); break;
                                default: httpClient.DefaultRequestHeaders.TryAddWithoutValidation(kv.Key, kv.Value); break;
                            }
                        }
                    }

                    Uri uri;
                    if (!Uri.TryCreate(url, UriKind.Absolute, out uri))
                    {
                        throw new InvalidOperationException("The provided URL is not a valid absolute URI.");
                    }
                    var response = await httpClient.GetAsync(uri);
                    response.EnsureSuccessStatusCode();
                    var responseStream = await response.Content.ReadAsStreamAsync();
                    return responseStream;
                }
            }
            catch (HttpRequestException ex)
            {

            }
            catch (Exception ex)
            {

            }

            return null;
        }

        private static string UrlEncode(string url, Encoding encode)
        {
            return WebUtility.UrlEncode(url);
        }

        public static async Task<string> StreamToStringAsync(Stream stream, Encoding encoding, bool isGzip)
        {
            if (isGzip)
            {
                using (var gzipStream = new System.IO.Compression.GZipStream(stream, System.IO.Compression.CompressionMode.Decompress))
                using (var reader = new StreamReader(gzipStream, encoding))
                {
                    return await reader.ReadToEndAsync();
                }
            }
            else
            {
                using (var reader = new StreamReader(stream, encoding))
                {
                    return await reader.ReadToEndAsync();
                }
            }
        }
    }
}
