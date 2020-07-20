using Xunit;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using iris_server.Models;
using XUnitTests.Utils;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Linq;

namespace XUnitTests
{
    public class StickiesControllerTests
    {
        [Fact]
        public async Task StickiesGetOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "stickies/get";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            StickyNote expectedSticky = new StickyNote() { Content = "no content", Scale = 0.5f, PatientId = "testpatient" };
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponseBody = await response.Content.ReadAsStringAsync();
            ICollection<StickyNote> actualStickies = JsonConvert.DeserializeObject<ICollection<StickyNote>>(actualResponseBody);

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedSticky.Content, actualStickies.ToList()[0].Content);
            Assert.Equal(expectedSticky.Scale, actualStickies.ToList()[0].Scale);
            Assert.Equal(expectedSticky.PatientId, actualStickies.ToList()[0].PatientId);
        }


        [Fact]
        public async Task StickiesGetNotFound()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "stickies/get";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            const string expectedResponse = "Patient has no sticky notes.";
            testClient.AddHeader("ApiKey", "testpatient2");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponse = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task StickiesPostOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "stickies/post";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            StickyNote sticky = new StickyNote() { Content = "no content", Scale = 0.5f, PatientId = "testpatient" };
            const string expectedResponse = "Successfully added sticky note.";
            string requestBody = JsonConvert.SerializeObject(new Dictionary<string, object>() { { "content", "no content" }, { "scale", 0.5f } });
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponse = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task StickiesPostBadRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "stickies/post";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            Carer sticky = new Carer(); // Incorrect serialisation
            const string expectedResponse = "Failed to add sticky note.";
            string requestBody = JsonConvert.SerializeObject(sticky);
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponse = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task StickiesPutOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "stickies/put/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            const string expectedResponse = "Sticky note updated successfully.";
            string requestBody = JsonConvert.SerializeObject(new Dictionary<string, object>() { { "scale", 1.0f } });
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint, body: requestBody);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponse = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task StickiesPutBadRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "stickies/put/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            Carer sticky = new Carer(); // Incorrect serialisation.
            const string expectedResponse = "Failed to update sticky note.";
            string requestBody = JsonConvert.SerializeObject(sticky);
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint, body: requestBody);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponse = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task StickiesPutUnauthorised()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "stickies/put/?id=dasdaoi";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            StickyNote sticky = new StickyNote();
            const string expectedResponse = "Patient does not own that sticky note.";
            string requestBody = JsonConvert.SerializeObject(sticky);
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint, body: requestBody);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponse = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task StickiesDeleteOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "stickies/delete/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            const string expectedResponse = "Sticky note deleted successfully.";
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.DeleteRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponse = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task StickiesDeleteUnauthorised()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "stickies/delete/?id=dasdaoi";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            const string expectedResponse = "Patient does not own that sticky note.";
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.DeleteRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;
            string actualResponse = await response.Content.ReadAsStringAsync();

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }
    }
}
