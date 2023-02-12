using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Xunit;

namespace Game.WebAPI.Tests.Integration
{
	public class Request
	{
		private readonly HttpClient _client;

		public Request(HttpClient client)
		{
			_client = client;
		}
		
		private StringContent StringifyRequest(object request) =>
			new StringContent(JsonConvert.SerializeObject(request), Encoding.UTF8, "application/json");

		public async Task<HttpResponseMessage> Get(string route) => await _client.GetAsync($"/api/{route}");

		public async Task<T> Get<T>(string route)
		{
			using (var getResponse = await _client.GetAsync($"/api/{route}"))
			{
				Assert.True(getResponse.IsSuccessStatusCode);
				var getResponseContent = await getResponse.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T>(getResponseContent);
			}
		}
		
		public async Task<T> Post<T>(string route, object request, IHeaderDictionary headerDictionary = null)
		{
			var requestContent = StringifyRequest(request);
			if (headerDictionary != null)
			{
				foreach (var header in headerDictionary)
				{
					requestContent.Headers.Add(header.Key, (string)header.Value);
				}
			}

			using (var postResponse = await _client.PostAsync($"/api/{route}", requestContent))
			{
				if (!postResponse.IsSuccessStatusCode) return default(T);
				var postResponseContent = await postResponse.Content.ReadAsStringAsync();
				return JsonConvert.DeserializeObject<T>(postResponseContent);
			}
		}

		public async Task<HttpResponseMessage> Post(string route, object request)
		{
			var requestContent = StringifyRequest(request);

			return await _client.PostAsync($"/api/{route}", requestContent);
		}

		public async Task<HttpResponseMessage> Post(string route, object request, IHeaderDictionary headerDictionary)
		{
			var requestContent = StringifyRequest(request);
			foreach (var header in headerDictionary)
			{
				requestContent.Headers.Add(header.Key, (string)header.Value);
			}
			return await _client.PostAsync($"/api/{route}", requestContent);
		}
	}
}
