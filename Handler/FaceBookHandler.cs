
//using System;
//using System.Dynamic;
//using System.IO;
//using System.Text;
//using System.Net;
//using Caveret.Models;
//using Newtonsoft.Json;
//using Facebook;
//using System.Linq;

//using System.Web;
////using System.Web.
////using
//using System.Collections.Generic;
//using Microsoft.AspNetCore.Http;
//using System.Net.Http;
//using System.Threading.Tasks;
//using System.Threading;

//namespace Caveret.Handler
//{
//    public class FaceBookHandler
//    {
//        private const string FacebookApiID = "floLxXG3C88KwuKb84zWpzQ89";
//        private const string FacebookApiSecret = "zjSm9Z97YIfGEjKdanD5mCsmCUJyRVTqXa3kuwXXZ6XGIITvch";

//        //private const string PageID = "101180562217935";
//        private const string fb_exchange_token = "AAAAAAAAAAAAAAAAAAAAAL43QwEAAAAATAViQig5KSRKLsTwr3MTQA19YLo%3Dp3TIuAjyE1yet9HqHtFbbg5iewZavlpkFT8df8GGbdnp3S8OyI";

//        private const string AuthenticationUrlFormat =
//            "https://twitter.com/oauth/authorize?oauth_token={0}";


//        //protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
//        //{
//        //    if(!request.RequestUri.AbsolutePath.Contains("/oauth"))
//        //    {
//        //        request.RequestUri = new Uri(request.RequestUri.AbsoluteUri.Replace("?access_token", "&access_token"));
//        //    }
//        //    return await base.SendAsync(request, cancellationToken);
//        //}
//        static string GetAccessToken(string apiID, string apiSecret, string pageID)
//        {
//            string accessToken = string.Empty;
//            string url = string.Format(AuthenticationUrlFormat, FacebookApiID);

//            WebRequest request = WebRequest.Create(url);
//            WebResponse response = request.GetResponse();

//            using (System.IO.Stream responseStream = response.GetResponseStream())
//            {
//                StreamReader reader = new StreamReader(responseStream, Encoding.Default);
//                String responseString = reader.ReadToEnd();

//                dynamic stuff = JsonConvert.DeserializeObject(responseString);

//                accessToken = stuff["access_token"];
//            }

//            if (accessToken.Trim().Length == 0)
//                throw new Exception("There is no Access Token");

//            return accessToken;
//        }

//        public static void PostMessage(Products product)
//        {
//            try
//            {
//                string accessToken = fb_exchange_token;
//                FacebookClient facebookClient = new FacebookClient(accessToken);

//                dynamic messagePost = new ExpandoObject();
//                messagePost.access_token = accessToken;
//                messagePost.message = "NEW!🎉 NEW!🎉 NEW!🎉\n\n" +
//                                      "Check Out our new product " + product.productName + " !🥳\n";

//                string url = string.Format("/{0}/feed", PageID);
//                var result = facebookClient.Post(url, messagePost);
//            }
//            catch (IOException e)
//            {
//            }
//        }

//        //public static void Post(Bar bar) { }
//    }
//}

using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

public class Twitter
{
    // Bearer - AAAAAAAAAAAAAAAAAAAAAL43QwEAAAAANmEun5rr3JMaPslAXLr1sEJd9F0%3DYhBW1euMJ8AJz1CJB2rRCeG64pXzqDt8KfpF6TN7mBAykloe7P
    // Access Token Secret - 18w1jtfoFBl6TP2t5ANHjAM9JMqew2ECwVkOnXDsApCAE 
    // Access Token - 1405897163943645187-z2h91De8yyjv2EnK5ylLBeYst5V319
    // API Key - UpMHTiAYtK3FreAeR4cxyk6qt
    // API Secret Key - hFG7Q9LihjU9k7d9mxYFGN3123bX3xGmMBHxkPaNRJXGGg8BHT
    readonly HMACSHA1 hasher = new HMACSHA1(
            new ASCIIEncoding().GetBytes(
                "hFG7Q9LihjU9k7d9mxYFGN3123bX3xGmMBHxkPaNRJXGGg8BHT&18w1jtfoFBl6TP2t5ANHjAM9JMqew2ECwVkOnXDsApCAE"
            )
    );
    readonly DateTime _epochUtc = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
    readonly HttpClient http = new HttpClient();

    readonly string _TwitterTextAPI = "https://api.twitter.com/1.1/statuses/update.json";

    public async Task<HttpResponseMessage> SendText(string text)
    {
        http.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("OAuth", PrepareOAuth(text));

        return await http.PostAsync(_TwitterTextAPI, new FormUrlEncodedContent(new Dictionary<string, string> { { "status", text } }));
    }


    string PrepareOAuth(string data)
    {
        int timestamp = (int)((DateTime.UtcNow - _epochUtc).TotalSeconds);

        // Add all the OAuth headers we'll need to use when constructing the hash
        Dictionary<string, string> oAuthData = new Dictionary<string, string>
        {
            { "oauth_consumer_key", "UpMHTiAYtK3FreAeR4cxyk6qt" },
            { "oauth_signature_method", "HMAC-SHA1" },
            { "oauth_timestamp", timestamp.ToString() },
            { "oauth_nonce", Guid.NewGuid().ToString() },
            { "oauth_token", "1405897163943645187-z2h91De8yyjv2EnK5ylLBeYst5V319" },
            { "oauth_version", "1.0" },
            { "status", data }
        };

        // Generate the OAuth signature and add it to our payload
        oAuthData.Add("oauth_signature", GenerateSignature(oAuthData));

        // Build the OAuth HTTP Header from the data
        return GenerateOAuthHeader(oAuthData);
    }

    string GenerateSignature(Dictionary<string, string> data)
    {
        var sigString = string.Join(
            "&",
            data.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}").OrderBy(s => s)
        );

        return Convert.ToBase64String(
            hasher.ComputeHash(
                new ASCIIEncoding().GetBytes($"POST&{Uri.EscapeDataString(_TwitterTextAPI)}&{Uri.EscapeDataString(sigString)}")
            )
        );
    }

    string GenerateOAuthHeader(Dictionary<string, string> data)
    {
        return string.Join
        (
            ", ",
            data
                .Where(kvp => kvp.Key.StartsWith("oauth_"))
                .Select(kvp => $"{kvp.Key}=\"{Uri.EscapeDataString(kvp.Value)}\"")
                .OrderBy(s => s)
       );
    }
}
