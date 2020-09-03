// <copyright file="ConfigurationController.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Formatters;
using Mmm.Iot.Common.Services.Config;
using Mmm.Iot.Common.Services.Filters;
using Mmm.Iot.IdentityGateway.Services.Helpers;
using Mmm.Iot.IdentityGateway.Services.Models;
using Newtonsoft.Json;

namespace Mmm.Iot.IdentityGateway.WebService.Controllers
{
    [Route("")]
    [TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class ConfigurationController : Controller
    {
        public const string ContentType = "application/json";
        private readonly IOpenIdProviderConfiguration openIdProviderConfiguration;
        private readonly IRsaHelpers rsaHelpers;
        private AppConfig config;

        public ConfigurationController(AppConfig config, IOpenIdProviderConfiguration openIdProviderConfiguration, IRsaHelpers rsaHelpers)
        {
            this.config = config;
            this.openIdProviderConfiguration = openIdProviderConfiguration;
            this.rsaHelpers = rsaHelpers;
        }

        [HttpGet(".well-known/openid-configuration")]
        public IActionResult GetOpenIdProviderConfiguration()
        {
            return new OkObjectResult(this.openIdProviderConfiguration) { ContentTypes = new MediaTypeCollection { ContentType } };
        }

        // GET api/values
        [HttpGet(".well-known/openid-configuration/jwks")]
        public ContentResult GetJsonWebKeySet()
        {
            var serializerSettings = new JsonSerializerSettings();
            serializerSettings.ContractResolver = new LowercaseContractResolver();
            return new ContentResult() { Content = JsonConvert.SerializeObject(this.rsaHelpers.GetJsonWebKey(this.config.IdentityGatewayService.PublicKey), serializerSettings), ContentType = ContentType, StatusCode = StatusCodes.Status200OK };
        }
    }
}