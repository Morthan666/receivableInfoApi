using Microsoft.AspNetCore.Mvc;

namespace ReceivableInfoApi.WebApi.Controllers;

public static class ControllerExtensions
{
    public static ActionResult SeeOther(this Controller controller, string location)
    {
        controller.Response.Headers.Add("Location", location);
        return new StatusCodeResult(303);
    }
}
