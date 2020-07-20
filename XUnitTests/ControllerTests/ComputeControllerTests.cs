using Xunit;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using iris_server.Models;
using XUnitTests.Utils;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace XUnitTests
{
    public class ComputeControllerTests
    {
        [Fact]
        public async Task GetFallDetectionOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "compute/detectfall/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }


        [Fact]
        public async Task GetFallDetectionBadRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "compute/detectfall/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }


        [Fact]
        public async Task GetRoomDetectionOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "compute/detectroom/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }


        [Fact]
        public async Task GetRoomDetectionBadRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "compute/detectroom/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }


        [Fact]
        public async Task GetConfusionDetectionOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "compute/detectconfusion/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }


        [Fact]
        public async Task GetConfusionDetectionBadRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "compute/detectconfusion/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }
    }
}
