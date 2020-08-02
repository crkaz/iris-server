using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace iris_server.Services
{
    /// <summary>
    /// Analyses images (as byte arrays) using the Azure Cognitive Services API.
    /// </summary>
    public class AzureVisionService
    {
        // Add your Computer Vision subscription key and endpoint to your environment variables.
        private static readonly string SUBSCRIPTION_KEY = "55b899e4acf94176a1bda1e0874ec440";
        private static readonly string ENDPOINT = "https://iris-cv.cognitiveservices.azure.com/vision/v3.0/analyze?visualFeatures=";
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

        // Get 'description' of the scene.
        public static async Task<string> Analyse(byte[] imageBytes)
        {
            try
            {
                const string VISUAL_FEATURES = "Description"; // Tags to return.
                string endpoint = ENDPOINT + VISUAL_FEATURES; // Parameterised endpoint.

                // Connect to Azure REST API.
                using (HttpClient client = new HttpClient())
                {
                    client.DefaultRequestHeaders.Add("Ocp-Apim-Subscription-Key", SUBSCRIPTION_KEY);

                    HttpResponseMessage response;
                    using (ByteArrayContent content = new ByteArrayContent(imageBytes))
                    {
                        content.Headers.ContentType = new MediaTypeHeaderValue("application/octet-stream");
                        response = await client.PostAsync(endpoint, content);
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
