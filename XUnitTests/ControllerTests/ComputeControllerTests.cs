using Xunit;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using XUnitTests.Utils;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace XUnitTests
{
    public class ComputeControllerTests
    {
        [Fact]
        public async Task PostFallDetection_Fall1()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "compute/detectfall";
            List<double> transforms = new List<double>() { 1.75, 1.72, 1.73, 1.72, 1.74, 1.6, 1.67, 1.75, 1.5, 1.5, 1.45, 1.3, 1.0, 1.0 };
            string requestBody = JsonConvert.SerializeObject(transforms);
            bool expectedResult = false;
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            bool actualResult = !(responseContentJson == "true");

            // assert
            Assert.Equal(expectedResult, actualResult);
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
            //string responseContentJson = await response.Content.ReadAsStringAsync();
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
            //string responseContentJson = await response.Content.ReadAsStringAsync();
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
            //string responseContentJson = await response.Content.ReadAsStringAsync();
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
            //string responseContentJson = await response.Content.ReadAsStringAsync();
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
            //string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }
    }
}
