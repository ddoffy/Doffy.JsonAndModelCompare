namespace Doffy.JsonAndModelCompare;

public class CustomeHttpMessageHandler : DelegatingHandler
{
    public CustomeHttpMessageHandler(HttpMessageHandler innerHandler) : base(innerHandler)
    {

    }

    protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
    {
        if (request.Content != null)
        {
            var content = await request.Content.ReadAsStringAsync();
            Console.WriteLine("Request content:");
            Console.WriteLine(content);
        }
        var response = await base.SendAsync(request, cancellationToken);
        return response;
    }
}