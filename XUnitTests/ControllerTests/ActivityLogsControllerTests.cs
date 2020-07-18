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
    public class ActivityLogsControllerTests
    {
        [Fact]
        public async Task GetActivityLogsOkRequest_HasLogs()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "activitylogs/get/?id=testpatient&page=1&nitems=5";
            const int expectedNLogs = 2;
            List<string> expectedLogIds = new List<string>() { "testlog1", "testlog2" };
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testcarer");
            List<string> actualLogIds = new List<string>();

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            ActivityLog[] logs = JsonConvert.DeserializeObject<ActivityLog[]>(responseContentJson);
            int actualNLogs = logs.Length;
            HttpStatusCode actualStatusCode = response.StatusCode;
            foreach (ActivityLog a in logs)
            {
                actualLogIds.Add(a.Id);
            }

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedNLogs, actualNLogs);
            Assert.Equal(expectedLogIds, actualLogIds);

            testClient.Destroy();
        }

        [Fact]
        public async Task GetActivityLogsOkRequest_NegativePagination()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "activitylogs/get/?id=testpatient&page=1&nitems=-5";
            const int expectedNLogs = 2;
            List<string> expectedLogIds = new List<string>() { "testlog1", "testlog2" };
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testcarer");
            List<string> actualLogIds = new List<string>();

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            ActivityLog[] logs = JsonConvert.DeserializeObject<ActivityLog[]>(responseContentJson);
            int actualNLogs = logs.Length;
            HttpStatusCode actualStatusCode = response.StatusCode;
            foreach (ActivityLog a in logs)
            {
                actualLogIds.Add(a.Id);
            }

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedNLogs, actualNLogs);
            Assert.Equal(expectedLogIds, actualLogIds);

            testClient.Destroy();
        }


        [Fact]
        public async Task GetActivityLogsOkRequest_HasNoLogs()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "activitylogs/get/?id=testpatient2&page=1&nitems=5";
            const int expectedNLogs = 0;
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            ActivityLog[] logs = JsonConvert.DeserializeObject<ActivityLog[]>(responseContentJson);
            int actualNLogs = logs.Length;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedNLogs, actualNLogs);

            testClient.Destroy();
        }


        [Fact]
        public async Task GetActivityLogsUnauthorised()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "activitylogs/get/?id=testpatient&page=1&nitems=5";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            testClient.AddHeader("ApiKey", "testcarer_nopatients");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);

            testClient.Destroy();
        }


        [Fact]
        public async Task PostActivityLogsOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "activitylogs/post/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            ActivityLog newLog = new ActivityLog();
            string requestBody = JsonConvert.SerializeObject(newLog);
            const string expectedResponseBody = "Activity log added successfully.";
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
            string actualResponseBody = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponseBody, actualResponseBody);

            testClient.Destroy();
        }


        [Fact]
        public async Task PostActivityLogsUnauthorised()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "activitylogs/post/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            ActivityLog newLog = new ActivityLog();
            string requestBody = JsonConvert.SerializeObject(newLog);
            const string expectedResponseBody = "Invalid credentials.";
            testClient.AddHeader("ApiKey", "testpatient2");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
            string actualResponseBody = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponseBody, actualResponseBody);

            testClient.Destroy();
        }


        [Fact]
        public async Task PostActivityLogsBadRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "activitylogs/post/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            Carer carer = new Carer();
            string requestBody = JsonConvert.SerializeObject(carer);
            const string expectedResponseBody = "Failed to log activity.";
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
            string actualResponseBody = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponseBody, actualResponseBody);

            testClient.Destroy();
        }


    }
}
