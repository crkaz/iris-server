using Xunit;
using System.Net.Http;
using XUnitTests.Utils;
using System.Threading.Tasks;
using System;

namespace XUnitTests
{
    public class LoggingMiddlewareTests
    {
        [Fact]
        public async Task LogsAddedSuccessfully()
        {
            // arrange
            TestClient testClient = new TestClient();
            int expectedNLogs = new Random().Next(2, 6); // 2, 3, 4, 5
            const string endpoint = "test/nlogs";
            testClient.AddHeader("ApiKey", "testcarer_admin");
            int initialLogCount = 0;
            int actualLogCount = 0;

            // act
            HttpResponseMessage response = await testClient.GetRequest(endpoint);
            string responseBody = await response.Content.ReadAsStringAsync();
            initialLogCount = int.Parse(responseBody);

            for (int i = 0; i < expectedNLogs; ++i)
            {
                response = await testClient.GetRequest(endpoint);
                responseBody = await response.Content.ReadAsStringAsync();
            }
            actualLogCount = int.Parse(responseBody);

            // assert
            if (initialLogCount + expectedNLogs != actualLogCount)
            {
                throw new Exception("This test may need to be run individually to pass if tests are running in parallel.");
            }
            Assert.Equal(initialLogCount + expectedNLogs, actualLogCount);
        }
    }
}
