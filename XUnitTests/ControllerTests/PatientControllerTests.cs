using Xunit;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using iris_server.Models;
using XUnitTests.Utils;
using System.Threading.Tasks;

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

            testClient.Destroy();
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

            testClient.Destroy();
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

            testClient.Destroy();
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

            testClient.Destroy();
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

            testClient.Destroy();
        }


        /// <summary>
        /// Test response for a request to DELETE a non-existent patient.
        /// </summary>
        [Fact]
        public async Task DeletePatientsUnauthorised_NoPatient()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/delete/?id=doesnotexist123";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.DeleteRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);

            testClient.Destroy();
        }


        /// <summary>
        /// Test response for a request to DELETE a patient not assigned to the requesting carer.
        /// </summary>
        [Fact]
        public async Task DeletePatientsUnauthorised_NotAssigned()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/delete/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            testClient.AddHeader("ApiKey", "testcarer_nopatients");
            const string expectedResponseBody = "You are not assigned to this patient.";

            // act
            HttpResponseMessage response = await testClient.DeleteRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponseBody = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponseBody, actualResponseBody);

            testClient.Destroy();
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

            testClient.Destroy();
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

            testClient.Destroy();
        }


        /// <summary>
        /// Test response for a request to PUT the status of an existing patient.
        /// </summary>
        [Fact]
        public async Task PutPatientStatusOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/status/?id=testpatient&status=offline";
            string expectedPatientStatus = Patient.PatientStatus.offline.ToString();
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint);
            string actualPatientStatus = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientStatus, actualPatientStatus);

            testClient.Destroy();
        }


        /// <summary>
        /// Test response for a request to PUT the status of a patient to an unrecognised status value.
        /// </summary>
        [Fact]
        public async Task PutPatientStatusBadRequest_InvalidStatus()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/status/?id=testpatient&status=AnInvalidStatus";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            const string expectedResponseBody = "Invalid status argument.";
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponseBody = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponseBody, actualResponseBody);

            testClient.Destroy();
        }

        /// <summary>
        /// Test response for a request to PUT the status of a patient not from a different patient.
        /// </summary>
        [Fact]
        public async Task PutPatientStatusUnauthorised()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/status/?id=testpatient&status=offline";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            testClient.AddHeader("ApiKey", "testpatient2");
            const string expectedResponseBody = "Credentials do not match.";

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponseBody = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponseBody, actualResponseBody);

            testClient.Destroy();
        }
    }
}
