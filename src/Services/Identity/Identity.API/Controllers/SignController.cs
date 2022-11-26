using Microsoft.AspNetCore.Mvc;
using sattec.Identity.Application.Users.Commands.Signup;
using sattec.Identity.Application.Users.Commands.Signin;
using sattec.Identity.Application.Users.Commands.ConfirmMobile;
using sattec.Identity.Application.Common.Models;

namespace sattec.Identity.WebUI.Controllers;

public class SignController : ApiControllerBase
{
    [Route("Signup"),HttpPost]
    public async Task<ActionResult<SignupResponse>> Signup(SignupCommand command)
    {
        return await Mediator.Send(command);
    }
    [Route("ConfirmMobile"), HttpPost]
    public async Task<ActionResult<Result>> ConfirmMobile([FromBody] ConfirmMobileCommand command)
    {
        return await Mediator.Send(command);
    }
    [Route("Signin"),HttpPost]
    public async Task<ActionResult<SigninResponse>> Signin(SigninCommand command)
    {
        return await Mediator.Send(command);
    }
}
