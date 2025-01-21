using Microsoft.OpenApi.Models;
using Swashbuckle.AspNetCore.SwaggerGen;

public class RemoveExampleFilter : IOperationFilter
{
    public void Apply(OpenApiOperation operation, OperationFilterContext context)
    {
        foreach (var response in operation.Responses)
        {
            if (response.Key == "404")
            {
                response.Value.Content.Clear(); // Remove example content
            }
            if (response.Key == "400")
            {
                response.Value.Content.Clear(); // Remove example content
            }
            if (response.Key == "403")
            {
                response.Value.Content.Clear(); // Remove example content
            }
            if (response.Key == "401")
            {
                response.Value.Content.Clear(); // Remove example content
            }
        }
    }
}
