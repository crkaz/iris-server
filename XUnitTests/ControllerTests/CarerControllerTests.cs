using Xunit;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using iris_server.Models;
using XUnitTests.Utils;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;
using System.Linq;

namespace XUnitTests
{
    public class CarerControllerTests
    {
        [Fact]
        public async Task PostCarerOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/post";
            const string expectedResponse = "New carer added successfully.";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            string requestBody = JsonConvert.SerializeObject("testcarer@" + Guid.NewGuid().ToString());
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task PostCarerBadRequest_AlreadyExists()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/post";
            const string expectedResponse = "Email address already in use.";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            string requestBody = JsonConvert.SerializeObject("testcarer@exists");
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task PostCarerBadRequest_InvalidEmail()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/post";
            const string expectedResponse = "Invalid email format.";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            string requestBody = JsonConvert.SerializeObject("testcarer");
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task GetCarerOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/get";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseBody = await response.Content.ReadAsStringAsync();
            ICollection<Carer> carers = JsonConvert.DeserializeObject<ICollection<Carer>>(responseBody);
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(carers.Count > 1);
        }


        #region Reset carer password (using firebase library in frontend instead).
        // USE FIRBASE IN FRONTEND INSTEAD
        //[Fact]
        //public async Task ResetCarerEmailOkRequest()
        //{
        //    // arrange
        //    TestClient testClient = new TestClient();
        //    const string endpoint = "carer/reset/?id=testcarer";
        //    const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        //    const string expectedResponse = "Password reset sent successfully.";
        //    testClient.AddHeader("ApiKey", "testcarer");

        //    // act
        //    HttpResponseMessage response = await testClient.GetRequest(endpoint);
        //    string actualResponse = await response.Content.ReadAsStringAsync();
        //    HttpStatusCode actualStatusCode = response.StatusCode;

        //    // assert
        //    Assert.Equal(expectedStatusCode, actualStatusCode);
        //    Assert.Equal(expectedResponse, actualResponse);
        //}


        //[Fact]
        //public async Task ResetCarerEmailNotFound()
        //{
        //    // arrange
        //    TestClient testClient = new TestClient();
        //    const string endpoint = "carer/reset/?id=testpatient";
        //    const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
        //    const string expectedResponse = "Could not find a carer with that email.";
        //    testClient.AddHeader("ApiKey", "testcarer");

        //    // act
        //    HttpResponseMessage response = await testClient.GetRequest(endpoint);
        //    string actualResponse = await response.Content.ReadAsStringAsync();
        //    HttpStatusCode actualStatusCode = response.StatusCode;

        //    // assert
        //    Assert.Equal(expectedStatusCode, actualStatusCode);
        //    Assert.Equal(expectedResponse, actualResponse);
        //}


        //[Fact]
        //public async Task DeleteCarerOkRequest()
        //{
        //    // arrange
        //    TestClient testClient = new TestClient();
        //    const string endpoint = "carer/delete/?id=testcarer";
        //    const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        //    const string expectedResponse = "Carer deleted successfully.";
        //    testClient.AddHeader("ApiKey", "testcarer_nopatients");

        //    // act
        //    HttpResponseMessage response = await testClient.GetRequest(endpoint);
        //    string actualResponse = await response.Content.ReadAsStringAsync();
        //    HttpStatusCode actualStatusCode = response.StatusCode;

        //    // assert
        //    Assert.Equal(expectedStatusCode, actualStatusCode);
        //    Assert.Equal(expectedResponse, actualResponse);
        //}
        #endregion


        [Fact]
        public async Task DeleteCarerNotFound()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/delete/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            const string expectedResponse = "Could not find a carer with that email.";
            testClient.AddHeader("ApiKey", "testcarer_nopatients");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        // Carer cannot delete self.
        [Fact]
        public async Task DeleteCarerUnauthorised()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/delete/?id=testcarer";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            const string expectedResponse = "Accounts must be deleted by an admin other than yourself.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task AllocateOkRequest_Assign()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/allocate/?id=testpatient";
            string requestBody = JsonConvert.SerializeObject(new Dictionary<string, object>() { { "patient", "testpatient" }, { "carer", "testcarer" }, { "assign", true } });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            const string expectedResponse = "Assigned successfully.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task AllocateOkRequest_Unassign()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/allocate/?id=testpatient";
            string requestBody = JsonConvert.SerializeObject(new Dictionary<string, object>() { { "patient", "testpatient" }, { "carer", "testcarer" }, { "assign", false } });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            const string expectedResponse = "Unassigned successfully.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        // Cannot update a calendar entry that does not exist.
        [Fact]
        public async Task AllocateNotFound_Patient()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/allocate/?id=testpatient";
            string requestBody = JsonConvert.SerializeObject(new Dictionary<string, object>() { { "patient", "thisdoesntexist" }, { "carer", "testcarer" }, { "assign", false } });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            const string expectedResponse = "Either the patient or carer does not exist.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        // Cannot update a calendar entry that does not exist.
        [Fact]
        public async Task AllocateNotFound_Carer()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/allocate/?id=testpatient";
            string requestBody = JsonConvert.SerializeObject(new Dictionary<string, object>() { { "patient", "testpatient" }, { "carer", "thisdoesntexist" }, { "assign", false } });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            const string expectedResponse = "Either the patient or carer does not exist.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task RoleOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/role";
            string requestBody = JsonConvert.SerializeObject(new Dictionary<string, object>() { { "carer", "testcarer" }, { "role", "admin" } });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            const string expectedResponse = "Role changed successfully.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        // Cannot update a calendar entry that does not exist.
        [Fact]
        public async Task RoleNotFound_Role()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/role";
            string requestBody = JsonConvert.SerializeObject(new Dictionary<string, object>() { { "carer", "testcarer" }, { "role", "doesnotexist" } });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            const string expectedResponse = "Invalid role specified.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        // Cannot update a calendar entry that does not exist.
        [Fact]
        public async Task RoleNotFound_Carer()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/role";
            string requestBody = JsonConvert.SerializeObject(new Dictionary<string, object>() { { "carer", "doesnotexist" }, { "role", "admin" } });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            const string expectedResponse = "Carer does not exist.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task GetPatientsOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/patients";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
        }


        [Fact]
        public async Task GetPatientsNotFound()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/patients";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            const string expectedResponse = "This carer does not have any patients assigned.";
            testClient.AddHeader("ApiKey", "testcarer_nopatients");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }

        // Informal carers may only have 1 assigned patient.
        [Fact]
        public async Task AllocateBadRequest_Informal()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "carer/allocate/?id=testpatient";
            string requestBody = JsonConvert.SerializeObject(new Dictionary<string, object>() { { "patient", "testpatient" }, { "carer", "testcarer_informal2" }, { "assign", true } });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            const string expectedResponse = "Informal carers may only have a single assigned patient.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }
    }
}
