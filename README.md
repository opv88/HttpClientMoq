HttpClientHandlerMoq
===

Package to mock HttpClient for .NET

[![Version](https://img.shields.io/nuget/vpre/HttpClientMoq.svg)](https://www.nuget.org/packages/HttpClientMoq)
[![Downloads](https://img.shields.io/nuget/dt/HttpClientMoq.svg)](https://www.nuget.org/packages/HttpClientMoq)

Example of mocking http get request:

```csharp
  // Set test url
  var url = "http://url/user/1";

  // Create httpClientHandler for url with text response data.
  var httpClientHandlerMocked = new HttpClientHandlerMoqBuilder(url, "response data").Build();

  // Create http client by handler.
  var httpClient = new HttpClient(httpClientHandlerMocked);

  // Send get request.
  var actualHttpResponseMessage = await httpClient.GetAsync(url);

  // Read response content as string.
  var actualContent = await actualHttpResponseMessage.Content.ReadAsStringAsync();
  
  // Assertion using Shoudly:
  // HttpClientHandlerStatistics contains http request and response statistics
  //    SentContentBody is collection of request bodies
  //    VisitedUrls is collection of visited urls
  _ = httpClientHandlerMocked.HttpClientHandlerStatistics.SentContentBody.Should().BeEmpty();
  _ = httpClientHandlerMocked.HttpClientHandlerStatistics.VisitedUrls.Should().HaveCount(1);
  _ = httpClientHandlerMocked.HttpClientHandlerStatistics.VisitedUrls!.Single().Should().Be("http://url/user/1");
  _ = actualHttpResponseMessage.StatusCode.Should().Be(HttpStatusCode.OK);
  _ = actualContent.Should().Be("response data");
```

It's possible to create and send multiple requests. Example xUnit test of mocking get and post requests: 

```csharp
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
```
