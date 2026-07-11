using ECommerce.Application.Common;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ECommerce.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ApiBaseController : ControllerBase
    {
        
        public static ActionResult<T> ToActionResult<T>(Result<T>result)
        {
            if (result.IsSuccess)
                return new OkObjectResult(result.Data);
             return ToProblem (result.Errors);
        }
        public static ActionResult<T> ToActionResult<T>(Result result)
        {
            if (result.IsSuccess)
                return new OkResult();
            return ToProblem(result.Errors);
        }
        public static ObjectResult ToProblem (IReadOnlyList<Error> Errors)
        {
            var First = Errors[0];
            var Status = First.Type switch
            {
                ErrorType.NotFound => StatusCodes.Status404NotFound,
                ErrorType.validation => StatusCodes.Status400BadRequest,
                ErrorType.Conflict => StatusCodes.Status409Conflict,
                ErrorType.UnAuthorized => StatusCodes.Status401Unauthorized,
                ErrorType.Fotbidden => StatusCodes.Status403Forbidden,
                _ => StatusCodes.Status500InternalServerError,
            };
            var Problem = new ProblemDetails
            {
                Status=Status,
                Title=First.code,
                Detail=First.description,
                Extensions = { ["Errors"]=Errors}

            };
            return new ObjectResult (Problem) { StatusCode=Status};

        }
    }
}
