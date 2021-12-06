using System;
using System.Threading.Tasks;
using API.Extensions;
using API.Interface;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.DependencyInjection;

namespace API.Helpers
{
    /*   IAsyncActionFilter : Used to filter action result/API result
     befor or after excution of request whereby context is used to filter before 
     and next used for apply filter on result after execution    */
    public class LogUserActivity : IAsyncActionFilter
    {
        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var resultContext = await next();

            if(!resultContext.HttpContext.User.Identity.IsAuthenticated) return;

            var userId = resultContext.HttpContext.User.GetUserId();
            var repo = resultContext.HttpContext.RequestServices.GetService<IUserRepository>();            
            var user = await repo.GetUserByIdAsync(userId);           
            user.LastActive = DateTime.Now;
            await repo.SaveAllAsync();
        }
    }
}