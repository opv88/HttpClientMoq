using FluentAssertions;
using HttpClientMoq.Package.Exceptions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using Xunit;


namespace HttpClientMoq.Package.UnitTests
{
    public class HttpClientHandlerMoqTests
    {
        [Fact]
        public async Task GetAsync_SourceDataContainsUriWithoutBody_ShouldBeAsExpected()
        {
            // Arrange
            var url = "http://url/user/1";
            var httpClientHandlerMocked = new HttpClientHandlerMoqBuilder(url, "response data")
                .Build();

            var httpClient = new HttpClient(httpClientHandlerMocked);
            var expectedHttpResponseMessageStatusCode = HttpStatusCode.OK;
            var expectedContent = "response data";
            var expectedVisitedUrl = url;

            // Act
            var actualHttpResponseMessage = await httpClient.GetAsync(url);
            var actualContent = await actualHttpResponseMessage.Content.ReadAsStringAsync();

            // Assert
            _ = httpClientHandlerMocked.HttpClientHandlerStatistics.SentContentBody.Should().BeEmpty();
            _ = httpClientHandlerMocked.HttpClientHandlerStatistics.VisitedUrls.Should().HaveCount(1);
            _ = httpClientHandlerMocked.HttpClientHandlerStatistics.VisitedUrls!.Single().Should().Be(expectedVisitedUrl);
            _ = actualHttpResponseMessage.StatusCode.Should().Be(expectedHttpResponseMessageStatusCode);
            _ = actualContent.Should().Be(expectedContent);
        }

        [Fact]
        public void GetAsync_HandlerNotFound_ShouldThrowException()
        {
            // Arrange
            var url = "http://url/user/1";
            var httpClientHandlerMocked = new HttpClientHandlerMoqBuilder(url, "response data")
                .Build();

            var httpClient = new HttpClient(httpClientHandlerMocked);

            // Act
            Func<Task> getActualHttpResponseMessage = () => httpClient.GetAsync("http://invalid_url");

            // Assert
            _ = getActualHttpResponseMessage.Should().ThrowAsync<HttpClientHandlerMoqException>("Handler for request not found.");
        }

        [Fact]
        public async Task GetAsync_SourceDataContainsUriWithoutBodyWithCustomStatusCode_ShouldBeAsExpected()
        {
            // Arrange
            var url = "http://url/user/1";
            var httpClientHandlerMocked = new HttpClientHandlerMoqBuilder(
                    url: url,
                    httpStatusCode: HttpStatusCode.BadRequest,
                    responseStringData: "bad request response data")
                .Build();

            var httpClient = new HttpClient(httpClientHandlerMocked);
            var expectedHttpResponseMessageStatusCode = HttpStatusCode.BadRequest;
            var expectedContent = "bad request response data";
            var expectedVisitedUrl = url;

            // Act
            var actualHttpResponseMessage = await httpClient.GetAsync(url);
            var actualContent = await actualHttpResponseMessage.Content.ReadAsStringAsync();

            // Assert
            _ = httpClientHandlerMocked.HttpClientHandlerStatistics.SentContentBody.Should().BeEmpty();
            _ = httpClientHandlerMocked.HttpClientHandlerStatistics.VisitedUrls.Should().HaveCount(1);
            _ = httpClientHandlerMocked.HttpClientHandlerStatistics.VisitedUrls!.Single().Should().Be(expectedVisitedUrl);
            _ = actualHttpResponseMessage.StatusCode.Should().Be(expectedHttpResponseMessageStatusCode);
            _ = actualContent.Should().Be(expectedContent);
        }

        [Fact]
        public async Task GetAsync_SourceDataContainsUriWithoutBodyWithCustomStatusCodeAndFormUrlEncodedContent_ShouldBeAsExpected()
        {
            // Arrange
            var url = "http://url/user/1";
            var formUrlEncodedContentDictionary = new Dictionary<string, string>()
            {
                { "param 1", "value 1" },
                { "param 2", "value 2" },
            };

            var httpClientHandlerMocked = new HttpClientHandlerMoqBuilder(
                    url: url,
                    httpStatusCode: HttpStatusCode.OK,
                    httpContent: new FormUrlEncodedContent(formUrlEncodedContentDictionary))
                .Build();

            var httpClient = new HttpClient(httpClientHandlerMocked);
            var expectedHttpResponseMessageStatusCode = HttpStatusCode.OK;
            var expectedContent = "param+1=value+1&param+2=value+2";
            var expectedVisitedUrl = url;

            // Act
            var actualHttpResponseMessage = await httpClient.GetAsync(url);
            var actualContent = await actualHttpResponseMessage.Content.ReadAsStringAsync();

            // Assert
            _ = httpClientHandlerMocked.HttpClientHandlerStatistics.SentContentBody.Should().BeEmpty();
            _ = httpClientHandlerMocked.HttpClientHandlerStatistics.VisitedUrls.Should().HaveCount(1);
            _ = httpClientHandlerMocked.HttpClientHandlerStatistics.VisitedUrls!.Single().Should().Be(expectedVisitedUrl);
            _ = actualHttpResponseMessage.StatusCode.Should().Be(expectedHttpResponseMessageStatusCode);
            _ = actualContent.Should().Be(expectedContent);
        }

        [Fact]
        public async Task DeleteAsync_SourceDataContainsUriWithoutBody_ShouldBeAsExpected()
        {
            // Arrange
            var url = "http://url/user/delete/1";
            var httpClientHandlerMocked = new HttpClientHandlerMoqBuilder(
                    httpMethod: HttpMethod.Delete,
                    url: url,
                    httpStatusCode: HttpStatusCode.OK,
                    responseStringData: "test content")
                .Build();

            var httpClient = new HttpClient(httpClientHandlerMocked);
            var expectedHttpResponseMessageStatusCode = HttpStatusCode.OK;
            var expectedContent = "test content";
            var expectedVisitedUrl = url;

            // Act
            var actualHttpResponseMessage = await httpClient.DeleteAsync(url);
            var actualContent = await actualHttpResponseMessage.Content.ReadAsStringAsync();

            // Assert
            _ = httpClientHandlerMocked.HttpClientHandlerStatistics.SentContentBody.Should().BeEmpty();
            _ = httpClientHandlerMocked.HttpClientHandlerStatistics.VisitedUrls.Should().HaveCount(1);
            _ = httpClientHandlerMocked.HttpClientHandlerStatistics.VisitedUrls!.Single().Should().Be(expectedVisitedUrl);
            _ = actualHttpResponseMessage.StatusCode.Should().Be(expectedHttpResponseMessageStatusCode);
            _ = actualContent.Should().Be(expectedContent);
        }

        [Fact]
        public async Task PostAsync_SourceDataContainsUriWithBody_ShouldBeAsExpected()
        {
            // Arrange
            var url = "http://url/user/save";
            var jsonInputContent = @"
                {
                    ""Name"" : ""John"",
                    ""Surname"": ""Doe""
                }";

            var httpClientHandlerMocked = new HttpClientHandlerMoqBuilder(
                    httpMethod: HttpMethod.Post,
                    url: url,
                    body: jsonInputContent,
                    httpStatusCode: HttpStatusCode.Created,
                    responseStringData: "{{ \"Id\" : 1 }}")
                .Build();

            var httpClient = new HttpClient(httpClientHandlerMocked);
            var expectedHttpResponseMessageStatusCode = HttpStatusCode.Created;
            var expectedContent = "{{ \"Id\" : 1 }}";
            var expectedVisitedUrl = url;

            // Act
            var actualHttpResponseMessage = await httpClient.PostAsync(url, new StringContent(jsonInputContent));
            var actualContent = await actualHttpResponseMessage.Content.ReadAsStringAsync();

            // Assert
            _ = httpClientHandlerMocked.HttpClientHandlerStatistics.SentContentBody.Single().Should().Be(jsonInputContent);
            _ = httpClientHandlerMocked.HttpClientHandlerStatistics.VisitedUrls.Should().HaveCount(1);
            _ = httpClientHandlerMocked.HttpClientHandlerStatistics.VisitedUrls!.Single().Should().Be(expectedVisitedUrl);
            _ = actualHttpResponseMessage.StatusCode.Should().Be(expectedHttpResponseMessageStatusCode);
            _ = actualContent.Should().Be(expectedContent);
        }

        [Fact]
        public void PostAsync_SourceDataContainsUriWithIncorrectBody_ShouldThrowException()
        {
            // Arrange
            var url = "http://url/user/save";
            var jsonInputContent = @"
                {
                    ""Name"" : ""John"",
                    ""Surname"": ""Doe""
                }";

            var httpClientHandlerMocked = new HttpClientHandlerMoqBuilder(
                    httpMethod: HttpMethod.Post,
                    url: url,
                    body: jsonInputContent,
                    httpStatusCode: HttpStatusCode.Created,
                    responseStringData: "{{ \"Id\" : 1 }}")
                .Build();

            var httpClient = new HttpClient(httpClientHandlerMocked);

            // Act
            Func<Task> actualHttpResponseMessage = () => httpClient.PostAsync(url, new StringContent("invalid body"));

            // Assert
            _ = actualHttpResponseMessage.Should().ThrowExactlyAsync<HttpClientHandlerMoqException>("Handler for request not found.");
        }

        [Fact]
        public async Task GetAsync_SourceDataContainsMultipleRequests_ShouldBeAsExpected()
        {
            // Arrange
            var url1 = "http://url/user/1";

            var jsonInputContent = @"
                {
                    ""Name"" : ""John"",
                    ""Surname"": ""Doe""
                }";
            var url2 = "http://url/user/save";

            var httpClientHandlerMoq = new HttpClientHandlerMoqBuilder(url1, "response data 1")
                .SetupRequest(
                    httpMethod: HttpMethod.Post,
                    url: url2,
                    body: jsonInputContent,
                    httpStatusCode: HttpStatusCode.Created,
                    responseStringData: "{{ \"Id\" : 1 }}")
                .Build();

            var httpClient = new HttpClient(httpClientHandlerMoq);
            var expectedVisitedUrls = new List<string>
            {
                url1,
                url2,
            };

            var expectedBody = new List<string>()
            {
                jsonInputContent
            };

            var expectedHttpResponseMessageStatusCode1 = HttpStatusCode.OK;
            var expectedContent1 = "response data 1";

            var expectedHttpResponseMessageStatusCode2 = HttpStatusCode.Created;
            var expectedContent2 = "{{ \"Id\" : 1 }}";

            // Act
            var actualHttpResponseMessage1 = await httpClient.GetAsync(url1);
            var actualContent1 = await actualHttpResponseMessage1.Content.ReadAsStringAsync();
            var actualHttpResponseMessage2 = await httpClient.PostAsync(url2, new StringContent(jsonInputContent));
            var actualContent2 = await actualHttpResponseMessage2.Content.ReadAsStringAsync();

            // Assert
            _ = httpClientHandlerMoq.HttpClientHandlerStatistics.SentContentBody.Should().BeEquivalentTo(expectedBody);
            _ = httpClientHandlerMoq.HttpClientHandlerStatistics.VisitedUrls.Should().BeEquivalentTo(expectedVisitedUrls);
            _ = actualHttpResponseMessage1.StatusCode.Should().Be(expectedHttpResponseMessageStatusCode1);
            _ = actualContent1.Should().Be(expectedContent1);
            _ = actualHttpResponseMessage2.StatusCode.Should().Be(expectedHttpResponseMessageStatusCode2);
            _ = actualContent2.Should().Be(expectedContent2);
        }
    }
}