using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace RandEXom.Utility
{
    public class SeedGenerator
    {
        private static readonly HttpClient randomOrgClient = CreateRandomOrgClient();
        private static readonly SemaphoreSlim randomOrgRequest = new SemaphoreSlim(1, 1);
        private const string randomOrgUrl = "https://www.random.org/integers/?num=2&min=0&max=999999999&col=1&base=10&format=plain&rnd=new";

        private static HttpClient CreateRandomOrgClient()
        {
            HttpClient client = new HttpClient { Timeout = TimeSpan.FromMinutes(3) };
            client.DefaultRequestHeaders.UserAgent.ParseAdd("RandEXom/1.5 (+https://github.com/miputra/RandEXom)");
            return client;
        }

        /// <summary>
        /// Get a new seed from Random.org without an API key.
        /// </summary>
        public static Task<long> GetRandomOrgSeedAsync(CancellationToken cancellationToken = default)
        {
            return GetRandomOrgSeedAsync(randomOrgClient, cancellationToken);
        }

        /// <summary>
        /// Use a supplied HTTP client to get a new seed from Random.org.
        /// </summary>
        public static async Task<long> GetRandomOrgSeedAsync(HttpClient client, CancellationToken cancellationToken = default)
        {
            if (client == null) throw new ArgumentNullException(nameof(client));
            await randomOrgRequest.WaitAsync(cancellationToken).ConfigureAwait(false);
            try
            {
                using (HttpResponseMessage response = await client.GetAsync(randomOrgUrl, cancellationToken).ConfigureAwait(false))
                {
                    response.EnsureSuccessStatusCode();
                    string body = await response.Content.ReadAsStringAsync().ConfigureAwait(false);
                    string[] values = body.Split((char[])null, StringSplitOptions.RemoveEmptyEntries);
                    if (values.Length > 0 && values[0].StartsWith("Error:", StringComparison.Ordinal))
                        throw new InvalidOperationException("Random.org returned an error: " + body.Trim());
                    if (values.Length != 2 ||
                        !long.TryParse(values[0], NumberStyles.None, CultureInfo.InvariantCulture, out long first) ||
                        !long.TryParse(values[1], NumberStyles.None, CultureInfo.InvariantCulture, out long second) ||
                        first > 999999999 || second > 999999999)
                        throw new InvalidDataException("Random.org returned an invalid seed response.");
                    return first * 1000000000 + second;
                }
            }
            finally
            {
                randomOrgRequest.Release();
            }
        }

        public static long GetJoinedCurrentDate()
        {
            return long.Parse(
                    System.DateTime.Now.Millisecond.ToString() +
                    System.DateTime.Now.Second.ToString() +
                    System.DateTime.Now.Minute.ToString() +
                    System.DateTime.Now.Hour.ToString() +
                    System.DateTime.Now.Day.ToString() +
                    System.DateTime.Now.Month.ToString() +
                    System.DateTime.Now.Year.ToString().Substring(2, 2)

                    );
        }
    }
}
