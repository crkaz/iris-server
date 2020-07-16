using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;


namespace XUnitTests.Utils
{
    /// <summary>
    /// Simple rest client for communicating with the server endpoints during testing.
    /// </summary>
    class TestClient
    {
        private const string HOST = "http://localhost:54268/api/";
        private readonly HttpClient Client = new HttpClient();


        // Constructor
        public TestClient(string host = HOST)
        {
            /// Initialise a new HTTP connection to a host URI.
            Client.BaseAddress = new Uri(host);
        }


        // GET
        public async Task<HttpResponseMessage> GetRequest(string endpoint, Dictionary<string, string> headers = null)
        {
            AddHeaders(headers);
            HttpResponseMessage response = await Client.GetAsync(endpoint);
            return response;
        }


        // POST
        public async Task<HttpResponseMessage> PostRequest(string endpoint, Dictionary<string, string> headers = null, HttpContent body = null)
        {
            AddHeaders(headers);
            HttpResponseMessage response = await Client.PostAsync(endpoint, body);
            return response;
        }


        // PUT
        public async Task<HttpResponseMessage> PutRequest(string endpoint, Dictionary<string, string> headers = null, HttpContent body = null)
        {
            AddHeaders(headers);
            HttpResponseMessage response = await Client.PutAsync(endpoint, body);
            return response;
        }


        // DELETE
        public async Task<HttpResponseMessage> DeleteRequest(string endpoint, Dictionary<string, string> headers = null)
        {
            AddHeaders(headers);
            HttpResponseMessage response = await Client.DeleteAsync(endpoint);
            return response;
        }


        // Utility functions.
        private void AddHeaders(Dictionary<string, string> headers)
        {
            if (headers != null)
            {
                foreach (KeyValuePair<string, string> header in headers)
                {
                    string key = header.Key;
                    string value = header.Value;
                    bool keyInHeader = Client.DefaultRequestHeaders.Contains(key);

                    if (keyInHeader)
                    {
                        Client.DefaultRequestHeaders.Remove(key);
                    }

                    Client.DefaultRequestHeaders.Add(key, value);
                }
            }
        }
    }
}