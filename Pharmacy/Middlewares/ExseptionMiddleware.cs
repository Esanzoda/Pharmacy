using Pharmasy.Exeption;

namespace Pharmasy.Middlewares;

public class ExseptionMiddleware
{
    private readonly RequestDelegate _next;
    private readonly ILogger<ExseptionMiddleware> _logger;

    public ExseptionMiddleware(RequestDelegate next, ILogger<ExseptionMiddleware> logger)
    {
        _next = next;
        _logger = logger;
    }

    public async Task Invoke(HttpContext context)
    {
        try
        {
            await _next(context);
        }
        catch (ResourseIsAlredyExsistExeption ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (ResourseNotFoundExeption ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch (BusinessExseption ex)
        {
            _logger.LogError(ex, ex.Message);
        }
        catch(Exception ex)
        {
            _logger.LogError(ex.Message);
            
        }
    }
   
    
}