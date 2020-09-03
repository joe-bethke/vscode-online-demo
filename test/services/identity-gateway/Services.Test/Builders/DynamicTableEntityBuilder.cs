// <copyright file="DynamicTableEntityBuilder.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using Microsoft.Azure.Cosmos.Table;
using TestStack.Dossier;
using TestStack.Dossier.EquivalenceClasses;

namespace Mmm.Iot.IdentityGateway.Services.Test.Helpers.Builders
{
    public class DynamicTableEntityBuilder : TestDataBuilder<DynamicTableEntity, DynamicTableEntityBuilder>
    {
        public virtual DynamicTableEntityBuilder WithRandomValueProperty()
        {
            return this.Set(dte => dte.Properties, new Dictionary<string, EntityProperty> { { "Value", new EntityProperty(this.Any.String()) } });
        }

        public virtual DynamicTableEntityBuilder WithRandomRolesProperty()
        {
            return this.Set(dte => dte.Properties, new Dictionary<string, EntityProperty> { { "Roles", new EntityProperty(this.Any.String()) } });
        }

        protected override DynamicTableEntity BuildObject()
        {
            return new DynamicTableEntity(this.Get(dte => dte.PartitionKey), this.Get(dte => dte.RowKey), this.Get(dte => dte.ETag), this.Get(dte => dte.Properties));
        }
    }
}