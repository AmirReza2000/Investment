using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace Investment.Api.Controllers;

public class BaseController(IMediator mediator):Controller
{
    protected readonly IMediator _mediator = mediator;
}