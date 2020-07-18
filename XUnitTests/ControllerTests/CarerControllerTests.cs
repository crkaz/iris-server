using Xunit;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using iris_server.Models;
using XUnitTests.Utils;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace XUnitTests
{
    public class CarerControllerTests
    {
        [Fact]
        public async Task PostCarerOkRequest()
        {
            // arrange
            
            const string endpoint = "carer/post";
            const string expectedResponse = "New carer added successfully.";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            string requestBody = JsonConvert.SerializeObject("testcarer@" + Guid.NewGuid().ToString());
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.PostRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);

            
        }


        [Fact]
        public async Task PostCarerBadRequest_AlreadyExists()
        {
            // arrange
            
            const string endpoint = "carer/post";
            const string expectedResponse = "Email address already in use.";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            string requestBody = JsonConvert.SerializeObject("testcarer@exists");
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.PostRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);

            
        }


        [Fact]
        public async Task PostCarerBadRequest_InvalidEmail()
        {
            // arrange
            
            const string endpoint = "carer/post";
            const string expectedResponse = "Invalid email format.";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            string requestBody = JsonConvert.SerializeObject("testcarer");
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.PostRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);

            
        }


        [Fact]
        public async Task GetCarerOkRequest()
        {
            // arrange
            
            const string endpoint = "carer/get";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseBody = await response.Content.ReadAsStringAsync();
            ICollection<Carer> carers = JsonConvert.DeserializeObject<ICollection<Carer>>(responseBody);
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(carers.Count > 1);

            
        }


        [Fact]
        public async Task ResetCarerEmailOkRequest()
        {
            // arrange
            
            const string endpoint = "carer/reset/?id=testcarer";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            const string expectedResponse = "Password reset sent successfully.";
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);

            
        }


        [Fact]
        public async Task ResetCarerEmailNotFound()
        {
            // arrange
            
            const string endpoint = "carer/reset/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            const string expectedResponse = "Could not find a carer with that email.";
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);

            
        }


        [Fact]
        public async Task DeleteCarerOkRequest()
        {
            // arrange
            
            const string endpoint = "carer/delete/?id=testcarer";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            const string expectedResponse = "Carer deleted successfully.";
            TestClient.Instance.AddHeader("ApiKey", "testcarer_nopatients");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);

            
        }


        [Fact]
        public async Task DeleteCarerNotFound()
        {
            // arrange
            
            const string endpoint = "carer/delete/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            const string expectedResponse = "Could not find a carer with that email.";
            TestClient.Instance.AddHeader("ApiKey", "testcarer_nopatients");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);

            
        }


        // Carer cannot delete self.
        [Fact]
        public async Task DeleteCarerUnauthorised()
        {
            // arrange
            
            const string endpoint = "carer/delete/?id=testcarer";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            const string expectedResponse = "Accounts must be deleted by an admin other than yourself.";
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);

            
        }
    }
}
