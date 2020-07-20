using Xunit;
using System.Net.Http;
using System.Net;
using Newtonsoft.Json;
using iris_server.Models;
using XUnitTests.Utils;
using System.Threading.Tasks;
using System;
using System.Collections.Generic;

namespace XUnitTests
{
    public class CalendarControllerTests
    {
        [Fact]
        public async Task PostCalendarOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/post/?id=testpatient";
            string requestBody = JsonConvert.SerializeObject(new CalendarEntry() { Start = DateTime.Now.AddDays(1), End = DateTime.Now.AddDays(2) });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            const string expectedResponse = "Successfully added calendar entry.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        // All calendar entries must be in the future.
        [Fact]
        public async Task PostCalendarBadRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/post/?id=testpatient";
            string requestBody = JsonConvert.SerializeObject(new CalendarEntry() { Start = DateTime.Now.AddDays(-2), End = DateTime.Now.AddDays(-1) });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            const string expectedResponse = "Invalid start date.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        // Start date must be before end date.
        [Fact]
        public async Task PostCalendarBadRequest_EndBeforeStart()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/post/?id=testpatient";
            string requestBody = JsonConvert.SerializeObject(new CalendarEntry() { Start = DateTime.Now, End = DateTime.Now.AddDays(-1) });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            const string expectedResponse = "Invalid start date.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        // Only a carer who is assigned to that patient can create calendar entries.
        [Fact]
        public async Task PostCalendarUnauthorised()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/post/?id=testpatient";
            string requestBody = JsonConvert.SerializeObject(new CalendarEntry() { Start = DateTime.Now.AddDays(1), End = DateTime.Now.AddDays(2) });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            const string expectedResponse = "You are not assigned to this patient.";
            testClient.AddHeader("ApiKey", "testcarer_nopatients");

            // act
            HttpResponseMessage response = await testClient.PostRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        [Fact]
        public async Task PutCalendarOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/put/?id=testpatient";
            string requestBody = JsonConvert.SerializeObject(new CalendarEntry() { Start = DateTime.Now.AddDays(1), End = DateTime.Now.AddDays(2) });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            const string expectedResponse = "Calendar entry updated successfully.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        // Cannot edit date to be in the past.
        [Fact]
        public async Task PutCalendarBadRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/put/?id=testpatient";
            string requestBody = JsonConvert.SerializeObject(new CalendarEntry() { Start = DateTime.Now.AddDays(-5) });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            const string expectedResponse = "Invalid start date.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        // Cannot edit date to be in the past.
        [Fact]
        public async Task PutCalendarBadRequest_EndBeforeStart()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/put/?id=testpatient";
            string requestBody = JsonConvert.SerializeObject(new CalendarEntry() { Start = DateTime.Now.AddDays(-5) });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
            const string expectedResponse = "Invalid start date.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.PutRequest(endpoint, body: requestBody);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        // Only carers assigned to the patient can modify their calendar.
        [Fact]
        public async Task PutCalendarUnauthorised()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/put/?id=testpatient";
            string requestBody = JsonConvert.SerializeObject(new CalendarEntry() { Start = DateTime.Now.AddDays(-1) });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            const string expectedResponse = "You are not assigned to this patient.";
            testClient.AddHeader("ApiKey", "testcarer_nopatients");

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
        public async Task PutCalendarNotFound()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/put/?id=thisdoesntexist";
            string requestBody = JsonConvert.SerializeObject(new CalendarEntry() { Start = DateTime.Now.AddDays(-1) });
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            const string expectedResponse = "Could not find an entry with that id.";
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
        public async Task DeleteCalendarOkRequest()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/delete/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            const string expectedResponse = "Calendar entry deleted successfully.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.DeleteRequest(endpoint);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        // Cannot delete a calendar entry that doesn't exist.
        [Fact]
        public async Task DeleteCalendarNotFound()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/delete/?id=thisdoesntexist";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.NotFound;
            const string expectedResponse = "Could not find an entry with that id.";
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.DeleteRequest(endpoint);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }



        // Cannot delete a calendar entry of a patient that isn't assigned to the carer.
        [Fact]
        public async Task DeleteCalendarUnauthorised()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/delete/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            const string expectedResponse = "You are not assigned to that patient.";
            testClient.AddHeader("ApiKey", "testcarer_nopatients");

            // act
            HttpResponseMessage response = await testClient.DeleteRequest(endpoint);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }


        // Patient should be able to see all appointments for current day and next.
        [Fact]
        public async Task GetCalendarOkRequest_Patient()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/patientget/";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testpatient");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseBody = await response.Content.ReadAsStringAsync();
            ICollection<CalendarEntry> entries = JsonConvert.DeserializeObject<ICollection<CalendarEntry>>(responseBody);
            bool withinRange = true;
            foreach (CalendarEntry e in entries)
            {
                if (e.Start.Day < DateTime.Now.Day || e.Start.Day > DateTime.Now.Day + 1)
                {
                    withinRange = false;
                }
            }
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(withinRange);
        }


        // Carer should be able to get all appointments from today onwards in a paginated fashion.
        [Fact]
        public async Task GetActivityLogsOkRequest_HasLogs()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/carerget/?id=testpatient&page=1&nitems=5";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseBody = await response.Content.ReadAsStringAsync();
            ICollection<CalendarEntry> entries = JsonConvert.DeserializeObject<ICollection<CalendarEntry>>(responseBody);
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(entries.Count > 0);
        }


        // Negative pagination is ignored; arguments are treating in absolute terms.
        [Fact]
        public async Task GetActivityLogsOkRequest_NegativePagination()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/carerget/?id=testpatient&page=1&nitems=-5";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseBody = await response.Content.ReadAsStringAsync();
            ICollection<CalendarEntry> entries = JsonConvert.DeserializeObject<ICollection<CalendarEntry>>(responseBody);
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.True(entries.Count > 0);
        }


        // The get request should handle the scenario where no calendar entries exist for a patient.
        [Fact]
        public async Task GetActivityLogsOkRequest_HasNoLogs()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/carerget/?id=testpatient2&page=1&nitems=-5";
            const int expectedNumberOfEntries = 0;
            const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
            testClient.AddHeader("ApiKey", "testcarer");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseBody = await response.Content.ReadAsStringAsync();
            ICollection<CalendarEntry> entries = JsonConvert.DeserializeObject<ICollection<CalendarEntry>>(responseBody);
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedNumberOfEntries, entries.Count);
        }


        // A carer can only view appointments if they are assigned to the patient.
        [Fact]
        public async Task GetCalendarUnauthorised()
        {
            // arrange
            TestClient testClient = new TestClient();
            const string endpoint = "calendar/carerget/?id=testpatient";
            const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
            const string expectedResponse = "You are not assigned to this patient.";
            testClient.AddHeader("ApiKey", "testcarer_nopatients");

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string actualResponse = await response.Content.ReadAsStringAsync();
            HttpStatusCode actualStatusCode = response.StatusCode;

            // assert
            Assert.Equal(expectedStatusCode, actualStatusCode);
            Assert.Equal(expectedResponse, actualResponse);
        }
    }
}
