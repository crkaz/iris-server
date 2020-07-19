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
    public class StickiesControllerTests
    {
        //[Fact]
        //public async Task GetPatientConfigUnauthorised()
        //{
        //    // arrange

        //    const string endpoint = "config/get/?id=testpatient";
        //    const HttpStatusCode expectedStatusCode = HttpStatusCode.Unauthorized;
        //    TestClient.Instance.AddHeader("ApiKey", "testcarer_nopatients");
        //    const string expectedResponseBody = "You are not assigned to this patient.";

        //    // act
        //    HttpResponseMessage response = await TestClient.Instance.GetRequest(endpoint);
        //    HttpStatusCode actualStatusCode = response.StatusCode;
        //    string actualResponseBody = await response.Content.ReadAsStringAsync();

        //    // assert
        //    Assert.Equal(expectedStatusCode, actualStatusCode);
        //    Assert.Equal(expectedResponseBody, actualResponseBody);
        //}
    }
}
