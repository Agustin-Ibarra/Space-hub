using System.Diagnostics;

namespace SpaceHub.Application.Logs;

public class RequestLoggin
{
  private readonly RequestDelegate? _next;
  private readonly ILogger<RequestLoggin> _logger;
  public RequestLoggin(RequestDelegate next, ILogger<RequestLoggin> logger)
  {
    _logger = logger;
    _next = next;
  }

  public async Task Invoke(HttpContext context)
  {
    var stopwatch = Stopwatch.StartNew();
    if (_next != null && _logger != null)
    {
      await _next(context);
      stopwatch.Stop();
      var request = context.Request;
      var response = context.Response;
      if (response.StatusCode < 400) // logs para respuestas con statdo 100, 200,300
      {
        _logger.LogInformation(
          "Method: {Method} Path: {Path} code:{StatusCode} in {ElapsedMilliseconds}ms ip addres{IP}",
          request.Method,
          request.Path,
          response.StatusCode,
          stopwatch.ElapsedMilliseconds,
          context.Connection.RemoteIpAddress
        );
      }
      else if (response.StatusCode >= 400 && response.StatusCode < 500)
      {
        _logger.LogWarning(
          "Method: {Method} Path: {Path} code:{StatusCode} in {ElapsedMilliseconds}ms ip addres{IP}",
          request.Method,
          request.Path,
          response.StatusCode,
          stopwatch.ElapsedMilliseconds,
          context.Connection.RemoteIpAddress
        );
      }
      else
      {
        _logger.LogError( // logs para respuestas con estado 500 en adelante
          "Method: {Method} Path: {Path} code:{StatusCode} in {ElapsedMilliseconds}ms ip addres{IP}",
          request.Method,
          request.Path,
          response.StatusCode,
          stopwatch.ElapsedMilliseconds,
          context.Connection.RemoteIpAddress
        );
      }
    }
  }
}