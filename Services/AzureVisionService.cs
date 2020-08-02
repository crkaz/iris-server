using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace iris_server.Services
{
    public class AzureVisionService
    {
        // Add your Computer Vision subscription key and endpoint to your environment variables.
        private static readonly string SUBSCRIPTION_KEY = "55b899e4acf94176a1bda1e0874ec440";
        private static readonly string ENDPOINT = "https://iris-cv.cognitiveservices.azure.com/vision/v3.0/analyze?visualFeatures=Description";
        #region Param types.
        // Request parameters. A third optional parameter is "details".
        // The Analyze Image method returns information about the following
        // visual features:
        // Categories:  categorizes image content according to a
        //              taxonomy defined in documentation.
        // Description: describes the image content with a complete
        //              sentence in supported languages.
        // Color:       determines the accent color, dominant color, 
        //              and whether an image is black & white.
        #endregion

        // Use Azure Vision REST API.
        public static async Task<string> Analyse(byte[] imageBytes)
        {
            try
            {
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SUBSCRIPTION_KEY);

                    HttpResponseMessage response;
                    using (ByteArrayContent content = new ByteArrayContent(imageBytes))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response = await client.PostAsync(ENDPOINT, content);
                    }

                    string contentString = await response.Content.ReadAsStringAsync();
                    return contentString;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine("\n" + e.Message);
            }
            return "Failed.";
        }
    }
}
