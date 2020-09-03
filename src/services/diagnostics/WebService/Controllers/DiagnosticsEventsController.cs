// <copyright file="DiagnosticsEventsController.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Mvc;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.Filters;
using Mmm.Iot.Diagnostics.Services;
using Mmm.Iot.Diagnostics.Services.Exceptions;
using Mmm.Iot.Diagnostics.Services.Models;

namespace Mmm.Iot.Diagnostics.WebService.Controllers
{
    [Route("v1/[controller]")]
    [TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class DiagnosticsEventsController : Controller
    {
        private readonly IDiagnosticsClient diagnosticsClient;

        public DiagnosticsEventsController(IDiagnosticsClient diagnosticsClient)
        {
            this.diagnosticsClient = diagnosticsClient;
        }

        [HttpPost("")]
        [Authorize("ReadAll")]
        public StatusCodeResult Post([FromBody] DiagnosticsEventModel diagnosticsEvent)
        {
            if (diagnosticsEvent == null)
            {
                throw new BadRequestException("The body of the request was null. Please include a json diagnostics event in the body of the request.");
            }

            try
            {
                this.diagnosticsClient.LogDiagnosticsEvent(diagnosticsEvent);
            }
            catch (BadDiagnosticsEventException e)
            {
                throw new BadRequestException(e.Message, e);
            }

            // Status Code 201 = Successfully Created
            return new StatusCodeResult(201);
        }
    }
}