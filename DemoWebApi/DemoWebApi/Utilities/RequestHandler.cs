using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Security.Principal;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Security;

namespace DemoWebApi.Utilities
{
    public class RequestHandler : DelegatingHandler
    {
        #region Constant Strings

        const string Origin = "Origin";
        const string AccessControlRequestMethod = "Access-Control-Request-Method";
        const string AccessControlRequestHeaders = "Access-Control-Request-Headers";
        const string AccessControlAllowOrigin = "Access-Control-Allow-Origin";
        const string AccessControlAllowMethods = "Access-Control-Allow-Methods";
        const string AccessControlAllowHeaders = "Access-Control-Allow-Headers";

        #endregion

        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            bool isCorsRequest = request.Headers.Contains(Origin);
            bool isPreflightRequest = request.Method == HttpMethod.Options;
            string userName = string.Empty;
            string password = string.Empty;

            if (isPreflightRequest)
            {
                return Task.Factory.StartNew<HttpResponseMessage>(() =>
                {
                    HttpResponseMessage response = new HttpResponseMessage(HttpStatusCode.OK);
                    response.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());

                    string accessControlRequestMethod = request.Headers.GetValues(AccessControlRequestMethod).FirstOrDefault();
                    if (accessControlRequestMethod != null)
                    {
                        response.Headers.Add(AccessControlAllowMethods, accessControlRequestMethod);
                    }

                    string requestedHeaders = string.Join(", ", request.Headers.GetValues(AccessControlRequestHeaders));
                    if (!string.IsNullOrEmpty(requestedHeaders))
                    {
                        response.Headers.Add(AccessControlAllowHeaders, requestedHeaders);
                    }

                    return response;
                }, cancellationToken);
            }
            else
            {
                if (request.Headers.Contains("username"))
                {
                    userName = request.Headers.GetValues("username").FirstOrDefault();
                }
                if (request.Headers.Contains("password"))
                {
                    password = request.Headers.GetValues("password").FirstOrDefault();
                }

                if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(password))
                {
                    return Task<HttpResponseMessage>.Factory.StartNew(() =>
                    {
                        HttpResponseMessage response = new HttpResponseMessage();
                        response.StatusCode = HttpStatusCode.NonAuthoritativeInformation;
                        response.ReasonPhrase = "Missing Credentials(username/password)";
                        response.Headers.Add("Message", "Missing Credentials");

                        return response;
                    });
                }
                if (userName == ConfigurationManager.AppSettings["UserName"] && password == ConfigurationManager.AppSettings["Password"])
                {
                    return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(t =>
                    {
                        HttpResponseMessage resp = t.Result;
                        if (isCorsRequest)
                        {
                            resp.Headers.Add(AccessControlAllowOrigin, request.Headers.GetValues(Origin).First());

                        }
                        return resp;
                    });
                }
                else
                {
                    request.Content.Dispose();

                    return base.SendAsync(request, cancellationToken).ContinueWith<HttpResponseMessage>(t =>
                    {
                        HttpResponseMessage resp = new HttpResponseMessage(HttpStatusCode.Unauthorized);
                        return resp;
                    });
                }
            }
        }
    }
}