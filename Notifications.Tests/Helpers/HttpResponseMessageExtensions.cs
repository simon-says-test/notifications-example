using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Notifications.Tests.Helpers
{
    public static class HttpResponseMessageExtensions
    {
        public async static Task<T> GetObjectFromContentString<T>(this HttpResponseMessage httpResponseMessage)
        {
            var getContent = await httpResponseMessage.Content.ReadAsStringAsync().ConfigureAwait(false);
            return JsonSerializer.Deserialize<T>(getContent, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
        }
    }
}
