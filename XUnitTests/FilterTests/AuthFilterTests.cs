using Xunit;
using System.Net.Http;
using System.Net;
using XUnitTests.Utils;
using System.Threading.Tasks;

namespace XUnitTests
{

    // Also validates authmiddleware.
    public class AuthFilterTests
    {
        [Fact]
        public async Task PatientRequests()
        {
            // arrange
            TestClient testClient = new TestClient();
            testClient.AddHeader("ApiKey", "testpatient");
            string endpoint;

            // act
            endpoint = "test/authfilterpatient";
            HttpResponseMessage patientResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilterinformalcarer";
            HttpResponseMessage formalCarerResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilterformalcarer";
            HttpResponseMessage informalCarerResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilteradmin";
            HttpResponseMessage adminResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilterunknown";
            HttpResponseMessage unknownResponse = await testClient.GetRequest(endpoint);

            // assert
            Assert.Equal(HttpStatusCode.OK, patientResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, formalCarerResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, informalCarerResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, adminResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, unknownResponse.StatusCode);
        }


        [Fact]
        public async Task InformalCarerRequests()
        {
            // arrange
            TestClient testClient = new TestClient();
            testClient.AddHeader("ApiKey", "testcarer_informal");
            string endpoint;

            // act
            endpoint = "test/authfilterpatient";
            HttpResponseMessage patientResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilterinformalcarer";
            HttpResponseMessage formalCarerResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilterformalcarer";
            HttpResponseMessage informalCarerResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilteradmin";
            HttpResponseMessage adminResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilterunknown";
            HttpResponseMessage unknownResponse = await testClient.GetRequest(endpoint);

            // assert
            Assert.Equal(HttpStatusCode.Unauthorized, patientResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, formalCarerResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, informalCarerResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, adminResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, unknownResponse.StatusCode);
        }


        [Fact]
        public async Task FormalCarerRequests()
        {
            // arrange
            TestClient testClient = new TestClient();
            testClient.AddHeader("ApiKey", "testcarer_formal");
            string endpoint;

            // act
            endpoint = "test/authfilterpatient";
            HttpResponseMessage patientResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilterinformalcarer";
            HttpResponseMessage formalCarerResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilterformalcarer";
            HttpResponseMessage informalCarerResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilteradmin";
            HttpResponseMessage adminResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilterunknown";
            HttpResponseMessage unknownResponse = await testClient.GetRequest(endpoint);

            // assert
            Assert.Equal(HttpStatusCode.Unauthorized, patientResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, formalCarerResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, informalCarerResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, adminResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, unknownResponse.StatusCode);
        }


        [Fact]
        public async Task AdminRequests()
        {
            // arrange
            TestClient testClient = new TestClient();
            testClient.AddHeader("ApiKey", "testcarer_admin");
            string endpoint;

            // act
            endpoint = "test/authfilterpatient";
            HttpResponseMessage patientResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilterinformalcarer";
            HttpResponseMessage formalCarerResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilterformalcarer";
            HttpResponseMessage informalCarerResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilteradmin";
            HttpResponseMessage adminResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilterunknown";
            HttpResponseMessage unknownResponse = await testClient.GetRequest(endpoint);

            // assert
            Assert.Equal(HttpStatusCode.Unauthorized, patientResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, formalCarerResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, informalCarerResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, adminResponse.StatusCode);
            Assert.Equal(HttpStatusCode.OK, unknownResponse.StatusCode);
        }


        [Fact]
        public async Task UnknownRequests()
        {
            // arrange
            TestClient testClient = new TestClient();
            string endpoint;

            // act
            endpoint = "test/authfilterpatient";
            HttpResponseMessage patientResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilterinformalcarer";
            HttpResponseMessage formalCarerResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilterformalcarer";
            HttpResponseMessage informalCarerResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilteradmin";
            HttpResponseMessage adminResponse = await testClient.GetRequest(endpoint);
            endpoint = "test/authfilterunknown";
            HttpResponseMessage unknownResponse = await testClient.GetRequest(endpoint);

            // assert
            Assert.Equal(HttpStatusCode.Unauthorized, patientResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, formalCarerResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, informalCarerResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, adminResponse.StatusCode);
            Assert.Equal(HttpStatusCode.Unauthorized, unknownResponse.StatusCode);
        }
    }
}
