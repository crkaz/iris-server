using Xunit;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using iris_server.Models;
using XUnitTests.Utils;
using System.Threading.Tasks;

namespace XUnitTests
{
    public class PatientInfoControllerTests
    {
        /// Test response for a request to PUT the notes of a patient.
        /// </summary>
        [Fact]
        public async Task PutPatientInfoOkRequest()
        {
            // arrange
            const string endpoint = "patientinfo/put/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            PatientNotes patientNotes = new PatientNotes(PatientNotes.AgeRange._56_65, PatientNotes.Severity.mild, "no notes");
            string requestBody = JsonConvert.SerializeObject(patientNotes);
            TestClient.Instance.AddHeader("ApiKey", "testcarer");
            const string expectedResponseBody = "Updated patient successfully.";

            // act
            HttpResponseMessage response = await TestClient.Instance.PutRequest(endpoint, body: requestBody);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponseBody = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponseBody, actualResponseBody);
        }


        /// <summary>
        /// Test response for a request to PUT the notes of a patient from a non-assigned carer.
        /// </summary>
        [Fact]
        public async Task PutPatientInfoUnauthorised()
        {
            // arrange

            const string endpoint = "patientinfo/put/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            TestClient.Instance.AddHeader("ApiKey", "testcarer_nopatients");
            PatientNotes patientNotes = new PatientNotes(PatientNotes.AgeRange._56_65, PatientNotes.Severity.mild, "no notes");
            string requestBody = JsonConvert.SerializeObject(patientNotes);
            const string expectedResponseBody = "You are not assigned to this patient.";

            // act
            HttpResponseMessage response = await TestClient.Instance.PutRequest(endpoint, body: requestBody);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponseBody = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponseBody, actualResponseBody);
        }


        /// <summary>
        /// Test response for a request to PUT the notes of a patient when the 'notes' are provided in an invalid format.
        /// </summary>
        [Fact]
        public async Task PutPatientInfoBadRequest()
        {
            // arrange

            const string endpoint = "patientinfo/put/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");
            Carer carer = new Carer();
            string requestBody = JsonConvert.SerializeObject(carer);
            const string expectedResponseBody = "Failed to update the patient.";

            // act
            HttpResponseMessage response = await TestClient.Instance.PutRequest(endpoint, body: requestBody);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponseBody = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponseBody, actualResponseBody);
        }


        /// <summary>
        /// Test response for a request to GET the notes of a patient.
        /// </summary>
        [Fact]
        public async Task GetPatientInfoOkRequest()
        {
            // arrange

            const string endpoint = "patientinfo/get/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            PatientNotes expectedPatientNotes = new PatientNotes(PatientNotes.AgeRange._56_65, PatientNotes.Severity.mild, "no notes");
            string test = JsonConvert.SerializeObject(expectedPatientNotes);
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string responseBody = await response.Content.ReadAsStringAsync();
            PatientNotes actualPatientNotes = JsonConvert.DeserializeObject<PatientNotes>(responseBody);

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal("testpatient", actualPatientNotes.Id);
            Assert.Equal(expectedPatientNotes.Age, actualPatientNotes.Age);
            Assert.Equal(expectedPatientNotes.Diagnosis, actualPatientNotes.Diagnosis);
            Assert.Equal(expectedPatientNotes.Notes, actualPatientNotes.Notes);
        }


        /// <summary>
        /// Test response for a request to GET the notes of a patient from a non-assigned carer.
        /// </summary>
        [Fact]
        public async Task GetPatientInfoUnauthorised()
        {
            // arrange

            const string endpoint = "patientinfo/get/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            TestClient.Instance.AddHeader("ApiKey", "testcarer_nopatients");
            const string expectedResponseBody = "You are not assigned to this patient.";

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponseBody = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponseBody, actualResponseBody);
        }
    }
}
