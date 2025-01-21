using Delivery.Resutruant.API.Models;

namespace Delivery.Resutruant.API.Configrations
{


    public interface IExamplesProvider<T>
    {
        T GetExamples();
    }
    public class ErrorResponseExample : IExamplesProvider<CustomErrorSchema>
    {
        public CustomErrorSchema GetExamples()
        {
            return new CustomErrorSchema
            {
                Status = "InternalServerError",
                Message = "An unexpected error occurred."
            };
        }
    }
}
