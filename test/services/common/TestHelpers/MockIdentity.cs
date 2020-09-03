// <copyright file="MockIdentity.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Collections.Generic;
using System.Security.Claims;
using System.Threading;
using Moq;

namespace Mmm.Iot.Common.TestHelpers
{
    public static class MockIdentity
    {
        public static void MockClaims(string tenant)
        {
            var cp = new Mock<ClaimsPrincipal>();
            cp.Setup(m => m.HasClaim(It.IsAny<string>(), It.IsAny<string>()))
              .Returns(true);

            List<Claim> claims = new List<Claim>();
            claims.Add(new Claim("tenant", tenant));
            cp.Setup(m => m.Claims).Returns(claims);
            Thread.CurrentPrincipal = cp.Object;
        }
    }
}