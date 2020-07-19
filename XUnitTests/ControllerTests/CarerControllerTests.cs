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
            TestClient testClient = new TestClient();
            const string endpoint = "carer/post";
            const string expectedResponse = "New carer added successfully.";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            string requestBody = JsonConvert.SerializeObject("testcarer@" + Guid.NewGuid().ToString());
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
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
            TestClient testClient = new TestClient();
            const string endpoint = "carer/post";
            const string expectedResponse = "Email address already in use.";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            string requestBody = JsonConvert.SerializeObject("testcarer@exists");
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
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
            TestClient testClient = new TestClient();
            const string endpoint = "carer/post";
            const string expectedResponse = "Invalid email format.";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            string requestBody = JsonConvert.SerializeObject("testcarer");
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
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
            TestClient testClient = new TestClient();
            const string endpoint = "carer/get";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
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
            TestClient testClient = new TestClient();
            const string endpoint = "carer/reset/?id=testcarer";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            const string expectedResponse = "Password reset sent successfully.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
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
            TestClient testClient = new TestClient();
            const string endpoint = "carer/reset/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            const string expectedResponse = "Could not find a carer with that email.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
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
            TestClient testClient = new TestClient();
            const string endpoint = "carer/delete/?id=testcarer";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            const string expectedResponse = "Carer deleted successfully.";
            testClient.AddHeader("ApiKey", "testcarer_nopatients");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
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
            TestClient testClient = new TestClient();
            const string endpoint = "carer/delete/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            const string expectedResponse = "Could not find a carer with that email.";
            testClient.AddHeader("ApiKey", "testcarer_nopatients");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
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
            TestClient testClient = new TestClient();
            const string endpoint = "carer/delete/?id=testcarer";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            const string expectedResponse = "Accounts must be deleted by an admin other than yourself.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }
    }
}
