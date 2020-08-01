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
    public class PatientControllerTests
    {
        /// <summary>
        /// Test response for a request to GET an existing patient.
        /// </summary>
        [Fact]
        public async Task GetPatientsOkRequest_Individiual()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/list/?id=testpatient";
            const string expectedPatientId = "testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patient = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId = patient[0].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientId, actualPatientId);
        }


        /// <summary>
        /// Test response for a request to GET an existing patient.
        /// </summary>
        [Fact]
        public async Task GetPatientsOkRequest_Multiple()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/list/?id=testpatient&id=testpatient2";
            const string expectedPatientId1 = "testpatient";
            const string expectedPatientId2 = "testpatient2";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patients = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId1 = patients[0].Id;
            string actualPatientId2 = patients[1].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(patients.Length == 2);
            Assert.Equal(expectedPatientId1, actualPatientId1);
            Assert.Equal(expectedPatientId2, actualPatientId2);
        }


        /// <summary>
        /// Test response for a request to GET a non-existent patient.
        /// </summary>
        [Fact]
        public async Task GetPatientsBadRequest_NoPatient()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/list/?id=doesnotexist123";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }


        /// <summary>
        /// Test response for a request to GET a patient not assigned to the requesting carer.
        /// </summary>
        [Fact]
        public async Task GetPatientsUnauthorised()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/list/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            testClient.AddHeader("ApiKey", "testcarer_nopatients");
            const string expectedResponseBody = "You are not assigned to all of the specified patients.";

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponseBody = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponseBody, actualResponseBody);
        }


        /// <summary>
        /// Test response for a request to DELETE an existing patient.
        /// </summary>
        [Fact]
        public async Task DeletePatientsOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/delete/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.DeleteRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }


        /// <summary>
        /// Test response for a request to DELETE a non-existent patient.
        /// </summary>
        [Fact]
        public async Task DeletePatientsNotFound_NoPatient()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/delete/?id=doesnotexist123";
            const string expectedResponse = "Patient not found.";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.DeleteRequest(endpoint);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }

        /// <summary>
        /// Test response for a request to GET status of an existing patient.
        /// </summary>
        [Fact]
        public async Task GetPatientStatusOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/status/?id=testpatient";
            const string expectedPatientStatus = "offline";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string actualPatientStatus = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientStatus, actualPatientStatus);
        }


        /// <summary>
        /// Test response for a request to GET the status of a patient not assigned to the requesting carer.
        /// </summary>
        [Fact]
        public async Task GetPatientStatusUnauthorised()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/status/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            testClient.AddHeader("ApiKey", "testcarer_nopatients");
            const string expectedResponseBody = "You are not assigned to this patient.";

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponseBody = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponseBody, actualResponseBody);
        }


        /// <summary>
        /// Test response for a request to update the status of an existing patient. Because no data is passed, this is a get request.
        /// </summary>
        [Fact]
        public async Task UpdatePatientStatusOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/updatestatus/?status=offline";
            string expectedPatientStatus = Patient.PatientStatus.offline.ToString();
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string actualPatientStatus = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientStatus, actualPatientStatus);
        }


        /// <summary>
        /// Test response for a request to update the status of a patient to an unrecognised status value.
        /// </summary>
        [Fact]
        public async Task UpdatePatientStatusBadRequest_InvalidStatus()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/updatestatus/?status=AnInvalidStatus";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            const string expectedResponseBody = "Invalid status argument.";
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponseBody = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponseBody, actualResponseBody);
        }


        [Fact]
        public async Task GetActivityLogsOkRequest_HasLogs()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/logs/?id=testpatient&page=1&nitems=5";
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
            Assert.True(actualNLogs >= expectedNLogs);
            Assert.Contains(expectedLogIds[0], actualLogIds);
            Assert.Contains(expectedLogIds[1], actualLogIds);
        }


        [Fact]
        public async Task GetActivityLogsOkRequest_NegativePagination()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/logs/?id=testpatient&page=1&nitems=-5";
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
            Assert.True(actualNLogs >= expectedNLogs);
            Assert.Contains(expectedLogIds[0], actualLogIds);
            Assert.Contains(expectedLogIds[1], actualLogIds);
        }


        [Fact]
        public async Task GetActivityLogsOkRequest_HasNoLogs()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/logs/?id=testpatient5&page=1&nitems=5";
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
        }


        [Fact]
        public async Task GetActivityLogsUnauthorised()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/logs/?id=testpatient&page=1&nitems=5";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            testClient.AddHeader("ApiKey", "testcarer_nopatients");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }


        [Fact]
        public async Task PostActivityLogsOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/logs/";
            string requestBody = JsonConvert.SerializeObject(new ActivityLog() { Caption = "test", Location = "test", JsonDescription = "test" });
            const string expectedResponse = "Logged successfully.";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task PostActivityLogsBadRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/logs/";
            string requestBody = JsonConvert.SerializeObject(new Carer()); // Incorrect object.
            const string expectedResponse = "Logging failed.";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }
    }
}
