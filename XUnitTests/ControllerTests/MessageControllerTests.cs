using Xunit;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using iris_server.Models;
using XUnitTests.Utils;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;
using Microsoft.VisualStudio.TestPlatform.CommunicationUtilities;
using System.Diagnostics.CodeAnalysis;

namespace XUnitTests
{
    public class MessageControllerTests
    {
        /// <summary>
        /// Test response for a request to PUT the notes of a patient.
        /// </summary>
        [Fact]
        public async Task PostPatientMessageOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "message/post/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            PatientMessage patientMessage = new PatientMessage();
            patientMessage.Title = "test title";
            patientMessage.Message = "test message";
            string requestBody = JsonConvert.SerializeObject(patientMessage);
            testClient.AddHeader("ApiKey", "testcarer");
            const string expectedResponseBody = "Message sent successfully.";

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
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
        public async Task PostPatientMessageUnauthorised()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "message/post/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            testClient.AddHeader("ApiKey", "testcarer_nopatients");
            PatientMessage patientMessage = new PatientMessage();
            string requestBody = JsonConvert.SerializeObject(patientMessage);
            const string expectedResponseBody = "You are not assigned to this patient.";

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
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
        public async Task PostPatientMessageBadRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "message/post/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            testClient.AddHeader("ApiKey", "testcarer");
            Carer carer = new Carer();
            string requestBody = JsonConvert.SerializeObject(carer);
            const string expectedResponseBody = "Failed to send message.";

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
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
        public async Task GetPatientMessageOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "message/get/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string responseBody = await response.Content.ReadAsStringAsync();
            IList<PatientMessage> actualPatientMessages = JsonConvert.DeserializeObject<IList<PatientMessage>>(responseBody);
            bool allMessagesAreUnread = true;
            foreach (var msg in actualPatientMessages)
            {
                if (msg.Read != null)
                {
                    allMessagesAreUnread = false;
                    break;
                }
            }

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(allMessagesAreUnread);

            
        }
    }
}
