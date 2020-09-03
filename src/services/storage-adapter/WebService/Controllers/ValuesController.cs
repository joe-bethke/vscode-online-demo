// <copyright file="ValuesController.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Mmm.Iot.Common.Services.Exceptions;
using Mmm.Iot.Common.Services.Filters;
using Mmm.Iot.Common.Services.Wrappers;
using Mmm.Iot.StorageAdapter.Services;
using Mmm.Iot.StorageAdapter.Services.Helpers;
using Mmm.Iot.StorageAdapter.Services.Models;
using Mmm.Iot.StorageAdapter.WebService.Models;

namespace Mmm.Iot.StorageAdapter.WebService.Controllers
{
    [Route("v1")]
    [TypeFilter(typeof(ExceptionsFilterAttribute))]
    public class ValuesController : Controller
    {
        private readonly IKeyValueContainer container;
        private readonly IKeyGenerator keyGenerator;
        private readonly ILogger logger;

        public ValuesController(
            IKeyValueContainer container,
            IKeyGenerator keyGenerator,
            ILogger<ValuesController> logger)
        {
            this.container = container;
            this.keyGenerator = keyGenerator;
            this.logger = logger;
        }

        [HttpGet("collections/{collectionId}/values/{key}")]
        public async Task<ValueApiModel> Get(string collectionId, string key)
        {
            this.EnsureValidId(collectionId, key);

            var result = await this.container.GetAsync(collectionId, key);

            return new ValueApiModel(result);
        }

        [HttpGet("collections/{collectionId}/values")]
        public async Task<ValueListApiModel> Get(string collectionId)
        {
            this.EnsureValidId(collectionId);

            var result = await this.container.GetAllAsync(collectionId);

            return new ValueListApiModel(result, collectionId);
        }

        [HttpPost("collections/{collectionId}/values")]
        public async Task<ValueApiModel> Post(string collectionId, [FromBody] ValueServiceModel model)
        {
            if (model == null)
            {
                throw new InvalidInputException("The request is empty");
            }

            string key = this.keyGenerator.Generate();
            this.EnsureValidId(collectionId, key);

            var result = await this.container.CreateAsync(collectionId, key, model);

            return new ValueApiModel(result);
        }

        [HttpPut("collections/{collectionId}/values/{key}")]
        public async Task<ValueApiModel> Put(string collectionId, string key, [FromBody] ValueServiceModel model)
        {
            if (model == null)
            {
                throw new InvalidInputException("The request is empty");
            }

            this.EnsureValidId(collectionId, key);

            var result = model.ETag == null ? await this.container.CreateAsync(collectionId, key, model) : await this.container.UpsertAsync(collectionId, key, model);

            return new ValueApiModel(result);
        }

        [HttpDelete("collections/{collectionId}/values/{key}")]
        public async Task Delete(string collectionId, string key)
        {
            this.EnsureValidId(collectionId, key);

            await this.container.DeleteAsync(collectionId, key);
        }

        private void EnsureValidId(string collectionId, string key = "")
        {
            // Currently, there is no official document describing valid character set of document ID
            // We just verified and enabled characters below
            var validCharacters = "_-";

            if (!collectionId.All(c => char.IsLetterOrDigit(c) || validCharacters.Contains(c)))
            {
                this.logger.LogInformation("Invalid collectionId {collectionId}", collectionId);
                throw new BadRequestException($"Invalid collectionId: '{collectionId}'");
            }

            if (key.Any() && !key.All(c => char.IsLetterOrDigit(c) || validCharacters.Contains(c)))
            {
                this.logger.LogInformation("Invalid key {key}", key);
                throw new BadRequestException($"Invalid key: '{key}'");
            }

            // "The id is a user defined string, of up to 256 characters that is unique within the context of a specific parent resource."
            //   - from https://docs.microsoft.com/en-us/azure/cosmos-db/documentdb-resources
            // But, currently portal.azure.com reject any document id contains 255 or more characters.
            // We just follow the experience result here: No more than 255 characters
            string id = DocumentIdHelper.GenerateId(collectionId, key);
            if (id.Length > 255)
            {
                this.logger.LogInformation("The collectionId/Key are too long: '{collectionId}', '{key}', '{id}'", collectionId, key, id);
                throw new BadRequestException($"The collectionId/Key are too long: '{collectionId}', '{key}'");
            }
        }
    }
}