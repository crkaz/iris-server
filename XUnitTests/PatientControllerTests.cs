using Xunit;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using iris_server.Models;
using XUnitTests.Utils;
using System.Threading.Tasks;

namespace XUnitTests
{
    /// <summary>
    /// ENSURE SERVER IS RUNNING BEFORE EXECUTING TESTS.
    /// HOST ADDRESS CAN BE CHANGED IN UTILS.TESTCLIENT.CS
    /// </summary>
    public class PatientControllerTests
    {

        [Fact]
        public async Task GetPatientsOkRequest()
        {
            /// Test response for a request to GET an existing patient.

            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/list/?id=testpatient";
            Patient expectedPatient = new Patient();
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient actualPatient = JsonConvert.DeserializeObject<Patient>(responseContentJson);
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatient, actualPatient);
        }

        [Fact]
        public async Task GetPatientsBadRequest()
        {
            /// Test response for a request to GET a non-existent patient.

            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/list/?id=afduasfoi";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public async Task DeletePatientsOkRequest()
        {
            /// Test response for a request to DELETE an existing patient.

            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/delete/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public async Task DeletePatientsBadRequest()
        {
            /// Test response for a request to DELETE a non-existent patient.

            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/delete/?id=afduasfoi";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }

        [Fact]
        public async Task GetPatientStatusOkRequest()
        {
            /// Test response for a request to GET status of an existing patient.

            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/status/?id=testpatient";
            Patient expectedPatient = new Patient();
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient actualPatient = JsonConvert.DeserializeObject<Patient>(responseContentJson);
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatient, actualPatient);
        }

        [Fact]
        public async Task GetPatientStatusBadRequest()
        {
            /// Test response for a request to GET the status of a non-existent patient.

            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "patient/status/?id=afduasfoi";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }
    }
}
