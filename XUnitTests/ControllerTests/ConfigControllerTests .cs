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
    public class ConfigControllerTests
    {
        /// <summary>
        /// Test response for a request to GET the notes of a patient from a non-assigned carer.
        /// </summary>
        [Fact]
        public async Task GetPatientConfigUnauthorised()
        {
            // arrange
            
            const string endpoint = "config/get/?id=testpatient";
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


        /// <summary>
        /// Test response for a request to PUT the notes of a patient.
        /// </summary>
        [Fact]
        public async Task PutPatientConfigOkRequest()
        {
            // arrange
            
            const string endpoint = "config/put/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            PatientConfig patientConfig = new PatientConfig();
            patientConfig.Id = "testpatient";
            string requestBody = JsonConvert.SerializeObject(patientConfig);
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
        public async Task PutPatientConfigUnauthorised()
        {
            // arrange
            
            const string endpoint = "config/put/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            TestClient.Instance.AddHeader("ApiKey", "testcarer_nopatients");
            PatientConfig patientConfig = new PatientConfig();
            string requestBody = JsonConvert.SerializeObject(patientConfig);
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
        public async Task PutPatientConfigBadRequest()
        {
            // arrange
            
            const string endpoint = "config/put/?id=testpatient";
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
        public async Task GetPatientConfigOkRequest()
        {
            // arrange
            
            const string endpoint = "config/get/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            PatientConfig patientConfig = new PatientConfig();
            string test = JsonConvert.SerializeObject(patientConfig);
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string responseBody = await response.Content.ReadAsStringAsync();
            PatientConfig actualPatientConfig = JsonConvert.DeserializeObject<PatientConfig>(responseBody);

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal("testpatient", actualPatientConfig.Id);
            Assert.Equal(patientConfig.EnabledFeatures, actualPatientConfig.EnabledFeatures);

            
        }

    }
}
