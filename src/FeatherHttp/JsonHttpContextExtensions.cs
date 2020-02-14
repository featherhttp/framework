using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace Microsoft.AspNetCore.Http
{
    /// <summary>
    /// Various extension methods to read and write JSON to and from the <see cref="HttpRequest"/> and <see cref="HttpResponse"/>.
    /// </summary>
    public static class JsonHttpContextExtensions
    {
        /// <summary>
        /// Asynchronously reads the UTF-8 encoded text representing a single JSON value
        /// into an instance of a type specified by a generic type parameter. The <see cref="HttpRequest"/> body
        /// will be read to completion.
        /// </summary>
        /// <typeparam name="TValue">The target type of the JSON value.</typeparam>
        /// <param name="request">The <see cref="HttpRequest"/> to read the JSON from.</param>
        /// <param name="options">Options to control the behavior during reading.</param>
        /// <param name="cancellationToken">A token that may be used to cancel the read operation.</param>
        /// <returns>A TValue representation of the JSON value.</returns>
        public static ValueTask<TValue> ReadJsonAsync<TValue>(this HttpRequest request, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            // TODO: Handle non-utf8 encoding
            return JsonSerializer.DeserializeAsync<TValue>(request.Body, options, cancellationToken);
        }

        /// <summary>
        /// Asynchronously converts a value of a type specified by a generic type parameter
        /// to UTF-8 encoded JSON text and writes it to the <see cref="HttpResponse"/> body with an application/json content-type.
        /// </summary>
        /// <typeparam name="TValue">The type of the value to serialize.</typeparam>
        /// <param name="response">The <see cref="HttpResponse"/> to write the JSON to.</param>
        /// <param name="value">The value to convert</param>
        /// <param name="options">Options to control behavior during writing.</param>
        /// <param name="cancellationToken">A token that may be used to cancel the write operation.</param>
        /// <returns>A task that represents the asynchronous write operation.</returns>
        public static Task WriteJsonAsync<TValue>(this HttpResponse response, TValue value, JsonSerializerOptions options = null, CancellationToken cancellationToken = default)
        {
            // Set the content type to application/json if it's not already set
            response.ContentType ??= "application/json";

            return JsonSerializer.SerializeAsync(response.Body, value, options, cancellationToken);
        }
    }
}
