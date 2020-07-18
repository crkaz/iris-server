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
    public class CalendarControllerTests
    {
        [Fact]
        public async Task PostCalendarOkRequest()
        {
            // arrange
            
            const string endpoint = "calendar/post/?id=testpatient";
            const string expectedPatientId = "testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patient = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId = patient[0].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientId, actualPatientId);

            
        }


        // All calendar entries must be in the future.
        [Fact]
        public async Task PostCalendarBadRequest()
        {
            // arrange
            
            const string endpoint = "calendar/post/?id=testpatient";
            const string expectedPatientId = "testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patient = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId = patient[0].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientId, actualPatientId);

            
        }


        // Only a carer who is assigned to that patient can create calendar entries.
        [Fact]
        public async Task PostCalendarUnauthorised()
        {
            // arrange
            
            const string endpoint = "calendar/post/?id=testpatient";
            const string expectedPatientId = "testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patient = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId = patient[0].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientId, actualPatientId);

            
        }


        [Fact]
        public async Task PutCalendarOkRequest()
        {
            // arrange
            
            const string endpoint = "calendar/put/?id=testpatient";
            const string expectedPatientId = "testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patient = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId = patient[0].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientId, actualPatientId);

            
        }


        // Cannot edit date to be in the past.
        [Fact]
        public async Task PutCalendarBadRequest()
        {
            // arrange
            
            const string endpoint = "calendar/put/?id=testpatient";
            const string expectedPatientId = "testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patient = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId = patient[0].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientId, actualPatientId);

            
        }


        // Only carers assigned to the patient can modify their calendar.
        [Fact]
        public async Task PutCalendarUnauthorised()
        {
            // arrange
            
            const string endpoint = "calendar/put/?id=testpatient";
            const string expectedPatientId = "testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patient = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId = patient[0].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientId, actualPatientId);

            
        }


        // Cannot update a calendar entry that does not exist.
        [Fact]
        public async Task PutCalendarNotFound()
        {
            // arrange
            
            const string endpoint = "calendar/put/?id=testpatient";
            const string expectedPatientId = "testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patient = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId = patient[0].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientId, actualPatientId);

            
        }


        [Fact]
        public async Task DeleteCalendarOkRequest()
        {
            // arrange
            
            const string endpoint = "calendar/delete/?id=testpatient";
            const string expectedPatientId = "testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patient = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId = patient[0].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientId, actualPatientId);

            
        }


        // Only carers assigned to the patient can delete a calendar entry.
        [Fact]
        public async Task DeleteCalendarUnauthorised()
        {
            // arrange
            
            const string endpoint = "calendar/delete/?id=testpatient";
            const string expectedPatientId = "testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patient = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId = patient[0].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientId, actualPatientId);

            
        }


        // Cannot delte a calendar entry that doesn't exist.
        [Fact]
        public async Task DeleteCalendarNotFound()
        {
            // arrange
            
            const string endpoint = "calendar/delete/?id=testpatient";
            const string expectedPatientId = "testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patient = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId = patient[0].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientId, actualPatientId);

            
        }


        // Patient should be able to see all appointments for current day and next.
        [Fact]
        public async Task GetCalendarOkRequest_Patient()
        {
            // arrange
            
            const string endpoint = "calendar/delete/?id=testpatient";
            const string expectedPatientId = "testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patient = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId = patient[0].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientId, actualPatientId);

            
        }

        // Carer should be able to see all appointments for current day and into the future.
        [Fact]
        public async Task GetCalendarOkRequest_Carer()
        {
            // arrange
            
            const string endpoint = "calendar/delete/?id=testpatient";
            const string expectedPatientId = "testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patient = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId = patient[0].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientId, actualPatientId);

            
        }

        // Patient cannot get another patient's calendar.
        [Fact]
        public async Task GetCalendarUnauthorised_Patient()
        {
            // arrange
            
            const string endpoint = "calendar/delete/?id=testpatient";
            const string expectedPatientId = "testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patient = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId = patient[0].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientId, actualPatientId);

            
        }

        // A carer can only view appointments if they are assigned to the patient.
        [Fact]
        public async Task GetCalendarUnauthorised_Carer()
        {
            // arrange
            
            const string endpoint = "calendar/delete/?id=testpatient";
            const string expectedPatientId = "testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            TestClient.Instance.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
            string responseContentJson = await response.Content.ReadAsStringAsync();
            Patient[] patient = JsonConvert.DeserializeObject<Patient[]>(responseContentJson);
            string actualPatientId = patient[0].Id;
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedPatientId, actualPatientId);

            
        }
    }
}
