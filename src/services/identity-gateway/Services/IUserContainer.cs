// <copyright file="IUserContainer.cs" company="3M">
// Copyright (c) 3M. All rights reserved.
// </copyright>

using System.Threading.Tasks;

namespace Mmm.Iot.IdentityGateway.Services
{
    public interface IUserContainer<TModel, TUiInput>
    {
        Task<TModel> GetAsync(TUiInput input);

        Task<TModel> CreateAsync(TUiInput input);

        Task<TModel> UpdateAsync(TUiInput input);

        Task<TModel> DeleteAsync(TUiInput input);
    }
}