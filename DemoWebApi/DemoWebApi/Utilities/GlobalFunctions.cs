using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;

namespace DemoWebApi.Utilities
{
    public class GlobalFunctions
    {
        /// <summary>
        /// This Method Returns  the Response Object with, a Status Code and Message and can also include Data in case of Get Requests
        /// </summary>
        /// <param name="responseStatusCode">Response code to be returned</param>
        /// <param name="responseMessage">Response message to be returned</param>
        /// <param name="returningData">Data to be returned by the request</param>
        /// <returns></returns>
        public static HttpResponseMessage ReturnResponse(HttpStatusCode responseStatusCode, string responseMessage, string returningData = null, HttpRequestMessage request = null)
        {
            HttpResponseMessage response;
            try
            {
                response = new HttpResponseMessage();
                if (!string.IsNullOrEmpty(returningData))
                {
                    if (request != null)
                    {
                        response = request.CreateResponse(responseStatusCode, returningData);
                    }
                    response.Headers.Add("data", returningData);
                }

                response.StatusCode = responseStatusCode;
                response.ReasonPhrase = responseMessage;
                response.Headers.Add("Message", responseMessage);
                return response;
            }
            catch (Exception ex)
            {
                throw ex;
            }
            finally
            {
                response = null;
            }

        }

    }
}