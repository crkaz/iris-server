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
            
            const string endpoint = "compute/detectfall/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            TestClient.Instance.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);

            
        }


        [Fact]
        public async Task GetFallDetectionUnauthorised()
        {
            // arrange
            
            const string endpoint = "compute/detectfall/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);

            
        }


        [Fact]
        public async Task GetFallDetectionBadRequest()
        {
            // arrange
            
            const string endpoint = "compute/detectfall/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);

            
        }


        [Fact]
        public async Task GetRoomDetectionOkRequest()
        {
            // arrange
            
            const string endpoint = "compute/detectroom/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            TestClient.Instance.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);

            
        }


        [Fact]
        public async Task GetRoomDetectionUnauthorised()
        {
            // arrange
            
            const string endpoint = "compute/detectroom/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);

            
        }


        [Fact]
        public async Task GetRoomDetectionBadRequest()
        {
            // arrange
            
            const string endpoint = "compute/detectroom/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);

            
        }


        [Fact]
        public async Task GetConfusionDetectionOkRequest()
        {
            // arrange
            
            const string endpoint = "compute/detectconfusion/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            TestClient.Instance.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);

            
        }


        [Fact]
        public async Task GetConfusionDetectionUnauthorised()
        {
            // arrange
            
            const string endpoint = "compute/detectconfusion/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);

            
        }


        [Fact]
        public async Task GetConfusionDetectionBadRequest()
        {
            // arrange
            
            const string endpoint = "compute/detectconfusion/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);

            
        }
    }
}
