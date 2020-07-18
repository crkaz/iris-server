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
    public class ActivityLogsControllerTests
    {
        //[Fact]
        //public async Task PostActivityLogsOkRequest()
        //{
        //    // arrange
        //    
        //    const string endpoint = "activitylogs/post/?id=testpatient";
        //    const HttpStatusCode expectedStatusCode = HttpStatusCode.OK;
        //    ActivityLog newLog = new ActivityLog();
        //    string requestBody = JsonConvert.SerializeObject(newLog);
        //    const string expectedResponseBody = "Activity log added successfully.";
        //    TestClient.Instance.AddHeader("ApiKey", "testpatient");

        //    // act
        //    HttpResponseMessage response = await TestClient.Instance.PostRequest(endpoint, body: requestBody);
        //    string actualResponseBody = await response.Content.ReadAsStringAsync();
        //    HttpStatusCode actualStatusCode = response.StatusCode;

        //    // assert
        //    Assert.Equal(expectedStatusCode, actualStatusCode);
        //    Assert.Equal(expectedResponseBody, actualResponseBody);

        //    
        //}


        //[Fact]
        //public async Task PostActivityLogsUnauthorised()
        //{
        //    // arrange
        //    
        //    const string endpoint = "activitylogs/post/?id=testpatient";
        //    const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
        //    ActivityLog newLog = new ActivityLog();
        //    string requestBody = JsonConvert.SerializeObject(newLog);
        //    const string expectedResponseBody = "Invalid credentials.";
        //    TestClient.Instance.AddHeader("ApiKey", "testpatient2");

        //    // act
        //    HttpResponseMessage response = await TestClient.Instance.PostRequest(endpoint, body: requestBody);
        //    string actualResponseBody = await response.Content.ReadAsStringAsync();
        //    HttpStatusCode actualStatusCode = response.StatusCode;

        //    // assert
        //    Assert.Equal(expectedStatusCode, actualStatusCode);
        //    Assert.Equal(expectedResponseBody, actualResponseBody);

        //    
        //}


        //[Fact]
        //public async Task PostActivityLogsBadRequest()
        //{
        //    // arrange
        //    
        //    const string endpoint = "activitylogs/post/?id=testpatient";
        //    const HttpStatusCode expectedStatusCode = HttpStatusCode.BadRequest;
        //    Carer carer = new Carer();
        //    string requestBody = JsonConvert.SerializeObject(carer);
        //    const string expectedResponseBody = "Failed to log activity.";
        //    TestClient.Instance.AddHeader("ApiKey", "testpatient");

        //    // act
        //    HttpResponseMessage response = await TestClient.Instance.PostRequest(endpoint, body: requestBody);
        //    string actualResponseBody = await response.Content.ReadAsStringAsync();
        //    HttpStatusCode actualStatusCode = response.StatusCode;

        //    // assert
        //    Assert.Equal(expectedStatusCode, actualStatusCode);
        //    Assert.Equal(expectedResponseBody, actualResponseBody);

        //    
        //}


    }
}
