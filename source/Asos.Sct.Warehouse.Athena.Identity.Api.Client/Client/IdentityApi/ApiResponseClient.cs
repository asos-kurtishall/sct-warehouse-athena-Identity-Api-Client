using System.Net;
using System.Text;
using Newtonsoft.Json;

namespace Asos.Sct.Warehouse.Athena.Identity.Api.Client.Client.IdentityApi;

public class ApiResponseClient : IApiResponseClient
{
    private readonly HttpClient _httpClient;

    public ApiResponseClient(HttpClient client)
    {
        _httpClient = client;
    }

    public async Task<ApiResponse<T>> GetAsync<T>(string controller, string action, CancellationToken cancellationToken)
    {
        var request = CreateHttpRequestMessage(controller, action, HttpMethod.Get);

        return await Send<T>(request, cancellationToken);
    }

    public async Task<ApiResponse<T>> PostAsync<T>(string controller, string action, object content, CancellationToken cancellationToken)
    {
        var request = CreateHttpRequestMessage(controller, action, HttpMethod.Post);
        request.Content = SerialiseAsJson(content);

        return await Send<T>(request, cancellationToken);
    }

    public async Task<ApiResponse> PostAsync(string controller, string action, object content, CancellationToken cancellationToken)
    {
        var request = CreateHttpRequestMessage(controller, action, HttpMethod.Post);
        request.Content = SerialiseAsJson(content);

        return await Send(request, cancellationToken);
    }

    public async Task<ApiResponse> PutAsync(string controller, string action, object content, CancellationToken cancellationToken)
    {
        var request = CreateHttpRequestMessage(controller, action, HttpMethod.Put);
        request.Content = SerialiseAsJson(content);

        return await Send(request, cancellationToken);
    }

    public async Task<ApiResponse<T>> PutAsync<T>(string controller, string action, object content, CancellationToken cancellationToken)
    {
        var request = CreateHttpRequestMessage(controller, action, HttpMethod.Put);
        request.Content = SerialiseAsJson(content);

        return await Send<T>(request, cancellationToken);
    }

    private HttpRequestMessage CreateHttpRequestMessage(string controller, string action, HttpMethod httpMethod)
    {
        var actionMethod = string.IsNullOrWhiteSpace(action) ? string.Empty : $"/{action}";

        var request = new HttpRequestMessage
        {
            Method = httpMethod,
            RequestUri = new Uri(_httpClient.BaseAddress + $"{controller}{actionMethod}"),
        };
        return request;
    }

    private async Task<ApiResponse<T>> Send<T>(HttpRequestMessage httpRequestMessage,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);

            //TODO: catch non json 401 response from Azure AD auth, need to look at making it json
            if (response.StatusCode is HttpStatusCode.Unauthorized) { return CreateApiErrorResponse<ApiResponse<T>>(response.StatusCode); }

            return await DeserializeResponse<T>(response);

        }
        catch
        {
            return CreateApiErrorResponse<ApiResponse<T>>(HttpStatusCode.InternalServerError);
        }
    }

    private async Task<ApiResponse> Send(HttpRequestMessage httpRequestMessage,
        CancellationToken cancellationToken)
    {
        try
        {
            var response = await _httpClient.SendAsync(httpRequestMessage, cancellationToken);

            //TODO: catch non json 401 response from Azure AD auth, need to look at making it json
            if (response.StatusCode is HttpStatusCode.Unauthorized) { return CreateApiErrorResponse<ApiResponse>(response.StatusCode); }

            return await DeserializeResponse(response);
        }
        catch
        {
            return CreateApiErrorResponse<ApiResponse>(HttpStatusCode.InternalServerError);
        }
    }

    public T CreateApiErrorResponse<T>(HttpStatusCode statusCode) where T : ApiResponse, new()
    {
        var statusCodeMessage = ((int)statusCode).ToString();
        return new T
        {
            IsFailure = true,
            Error = new Error
            {
                Code = statusCodeMessage,
                Message = $"An error occurred when sending the request. Status Code: {statusCodeMessage}."
            }
        };
    }

    private static async Task<ApiResponse<T>> DeserializeResponse<T>(HttpResponseMessage response)
    {
        var jsonString = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<ApiResponse<T>>(jsonString);

        return result ?? new ApiResponse<T>();
    }

    private static async Task<ApiResponse> DeserializeResponse(HttpResponseMessage response)
    {
        var jsonString = await response.Content.ReadAsStringAsync();

        var result = JsonConvert.DeserializeObject<ApiResponse>(jsonString);

        return result ?? new ApiResponse();
    }

    private static StringContent SerialiseAsJson(object content)
    {
        var json = JsonConvert.SerializeObject(content);

        return new StringContent(json, Encoding.UTF8, "application/json");
    }
}
