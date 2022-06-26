using System;
using System.Collections.Generic;
using System.Linq;

namespace HttpClientMoq.Package.Models
{
    public sealed class HttpClientHandlerStatistics
    {
        private readonly Dictionary<HttpClientHandlerConditionMoq, HttpClientVisitedData> _visitedData;

        public HttpClientHandlerStatistics()
        {
            _visitedData = new Dictionary<HttpClientHandlerConditionMoq, HttpClientVisitedData>();
        }

        public IList<HttpClientHandlerConditionMoq> Conditions => _visitedData
            ?.Where(vd => vd.Key != null)
            ?.Select(vd => vd.Key)
            ?.ToList() ?? Enumerable.Empty<HttpClientHandlerConditionMoq>().ToList();

        public IList<string> VisitedUrls => _visitedData
            ?.Where(vd => !string.IsNullOrWhiteSpace(vd.Value.Url))
            ?.Select(vd => vd.Value.Url)
            ?.ToList() ?? Enumerable.Empty<string>().ToList();

        public IList<string> SentContentBody => _visitedData
            ?.Where(vd => !string.IsNullOrWhiteSpace(vd.Value.ContentBody))
            ?.Select(vd => vd.Value.ContentBody)
            ?.ToList() ?? Enumerable.Empty<string>().ToList();

        public void AddVisitedUrl(HttpClientHandlerConditionMoq httpClientHandlerConditionMoq, string url)
        {
            if (string.IsNullOrWhiteSpace(url))
            {
                throw new ArgumentNullException(nameof(url));
            }

            var conditionHash = httpClientHandlerConditionMoq.GetHashCode();
            if (!_visitedData.Any(vd => vd.Key.GetHashCode() == conditionHash))
            {
                _visitedData.Add(httpClientHandlerConditionMoq, new HttpClientVisitedData(httpClientHandlerConditionMoq));
            }

            var key = _visitedData.Single(vd => vd.Key.GetHashCode() == conditionHash).Key;
            _visitedData[key].SetUrl(url);
        }

        public void AddSentContentBody(HttpClientHandlerConditionMoq httpClientHandlerConditionMoq, string contentBody)
        {
            if (string.IsNullOrWhiteSpace(contentBody))
            {
                throw new ArgumentNullException(nameof(contentBody));
            }

            var conditionHash = httpClientHandlerConditionMoq.GetHashCode();
            if (!_visitedData.Any(vd => vd.Key.GetHashCode() == conditionHash))
            {
                _visitedData.Add(httpClientHandlerConditionMoq, new HttpClientVisitedData(httpClientHandlerConditionMoq));
            }

            var key = _visitedData.Single(vd => vd.Key.GetHashCode() == conditionHash).Key;
            _visitedData[key].SetContentBody(contentBody);
        }
    }
}
