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
            Init(host);
        }

        private void Init(string host)
        {
            /// Initialise a new HTTP connection to a host URI.
            Client.BaseAddress = new Uri(host);

            // Attempt to connect to the host via the status controller.
            try
            {
                string endpoint = Client.BaseAddress + "status";
                var worker = Client.GetAsync(endpoint);
                var response = worker.GetAwaiter().GetResult();
                worker.Wait();
            }
            catch (HttpRequestException e)
            {
                // Host unavailable.
                throw new Exception("Connection refused. Please check that the host is online before running the tests.");
            }
        }

        // GET
        public async Task<HttpResponseMessage> GetRequest(string endpoint, Dictionary<string, string> headers = null)
        {
            AddHeaders(headers);
            endpoint = Client.BaseAddress + endpoint;
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

        //
        // Utility functions.
        //

        /// <summary>
        /// Add a collection of headers to the request.
        /// </summary>
        /// <param name="headers"></param>
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

        /// <summary>
        /// Add an individual header to the request.
        /// </summary>
        /// <param name="key"></param>
        /// <param name="value"></param>
        public void AddHeader(string key, string value)
        {
            bool keyInHeader = Client.DefaultRequestHeaders.Contains(key);

            if (keyInHeader)
            {
                Client.DefaultRequestHeaders.Remove(key);
            }

            Client.DefaultRequestHeaders.Add(key, value);
        }
    }
}