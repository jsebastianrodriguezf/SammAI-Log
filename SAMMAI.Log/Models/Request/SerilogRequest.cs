namespace SAMMAI.Log.Models.Request
{
    public class SerilogRequest
    {
        public DateTime? Timestamp { get; set; }
        public string? Level { get; set; }
        public string? MessageTemplate { get; set; }
        public string? RenderedMessage { get; set; }
        public Properties? Properties { get; set; }
        public Renderings? Renderings { get; set; }
    }

    public class EventId
    {
        public int? Id { get; set; }
        public string? Name { get; set; }
    }

    public class Properties
    {
        public string? Protocol { get; set; }
        public string? Method { get; set; }
        public string? ContentType { get; set; }
        public int? ContentLength { get; set; }
        public string? Scheme { get; set; }
        public string? Host { get; set; }
        public string? PathBase { get; set; }
        public string? Path { get; set; }
        public string? Querystring { get; set; }
        public EventId? EventId { get; set; }
        public string? SourceContext { get; set; }
        public string? RequestId { get; set; }
        public string? RequestPath { get; set; }
        public string? ConnectionId { get; set; }
        public string? Application { get; set; }
        public string? Company { get; set; }
        public string? Product { get; set; }
        public string? Url { get; set; }
        public string? State { get; set; }
        public string? HttpMethod { get; set; }
        public string? Uri { get; set; }
        public List<string?>? Scope { get; set; }
        public double? ElapsedMilliseconds { get; set; }
        public int? StatusCode { get; set; }
        public string? Body { get; set; }
    }

    public class Renderings
    {
        public List<State?>? State { get; set; }
    }

    public class State
    {
        public string? Format { get; set; }
        public string? Rendering { get; set; }
    }
}
