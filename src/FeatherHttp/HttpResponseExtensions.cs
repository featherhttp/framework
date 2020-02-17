using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Net.Http.Headers;

namespace Microsoft.AspNetCore.Http
{
    public static class HttpResponseExtensions
    {
        public static Task StatusCode(this HttpResponse httpResponse, int statusCode)
        {
            httpResponse.StatusCode = statusCode;
            return httpResponse.StartAsync();
        }

        public static Task StatusCode<T>(this HttpResponse httpResponse, int statusCode, T value)
        {
            httpResponse.StatusCode = statusCode;
            return value is null ? httpResponse.StartAsync() : httpResponse.WriteJsonAsync(value);
        }

        public static Task Accepted<T>(this HttpResponse httpResponse, string uri, T value) => Accepted<T>(httpResponse, new Uri(uri), value);

        public static Task Accepted<T>(this HttpResponse httpResponse, Uri uri, T value)
        {
            string location;
            if (uri.IsAbsoluteUri)
            {
                location = uri.AbsoluteUri;
            }
            else
            {
                location = uri.GetComponents(UriComponents.SerializationInfoString, UriFormat.UriEscaped);
            }

            httpResponse.Headers[HeaderNames.Location] = location;

            return httpResponse.StatusCode(StatusCodes.Status202Accepted, value);
        }

        public static Task Accepted(this HttpResponse httpResponse, string uri) => Accepted(httpResponse, new Uri(uri));

        public static Task Accepted(this HttpResponse httpResponse, Uri uri)
        {
            string location;
            if (uri.IsAbsoluteUri)
            {
                location = uri.AbsoluteUri;
            }
            else
            {
                location = uri.GetComponents(UriComponents.SerializationInfoString, UriFormat.UriEscaped);
            }

            httpResponse.Headers[HeaderNames.Location] = location;

            return httpResponse.StatusCode(StatusCodes.Status202Accepted);
        }

        public static Task NotFound(this HttpResponse httpResponse) => httpResponse.StatusCode(StatusCodes.Status404NotFound);
        public static Task NotFound<T>(this HttpResponse httpResponse, T value) => httpResponse.StatusCode(StatusCodes.Status404NotFound, value);

        public static Task BadRequest(this HttpResponse httpResponse) => httpResponse.StatusCode(StatusCodes.Status400BadRequest);
        public static Task BadRequest<T>(this HttpResponse httpResponse, T value) => httpResponse.StatusCode(StatusCodes.Status400BadRequest, value);

        public static Task Ok(this HttpResponse httpResponse) => httpResponse.StatusCode(StatusCodes.Status200OK);
        public static Task Ok<T>(this HttpResponse httpResponse, T value) => httpResponse.StatusCode(StatusCodes.Status200OK, value);

        public static Task Conflict(this HttpResponse httpResponse) => httpResponse.StatusCode(StatusCodes.Status409Conflict);
        public static Task Conflict<T>(this HttpResponse httpResponse, T value) => httpResponse.StatusCode(StatusCodes.Status409Conflict, value);

        public static Task Unauthroized(this HttpResponse httpResponse) => httpResponse.StatusCode(StatusCodes.Status401Unauthorized);
        public static Task Unauthroized<T>(this HttpResponse httpResponse, T value) => httpResponse.StatusCode(StatusCodes.Status401Unauthorized, value);

        public static Task UnprocessableEntity(this HttpResponse httpResponse) => httpResponse.StatusCode(StatusCodes.Status422UnprocessableEntity);
        public static Task UnprocessableEntity<T>(this HttpResponse httpResponse, T value) => httpResponse.StatusCode(StatusCodes.Status422UnprocessableEntity, value);

        public static Task NoContent(this HttpResponse httpResponse) => httpResponse.StatusCode(StatusCodes.Status204NoContent);
    }
}
