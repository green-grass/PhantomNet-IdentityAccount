﻿using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using PhantomNet.Entities;
using System.Collections.Generic;

namespace PhantomNet.AspNetCore.IdentityAccount
{
    public class IdentityAccountManager<TAccount> : IdentityAccountManager<TAccount, IdentityAccountManager<TAccount>>
        where TAccount : IdentityUser, new()
    {
        public IdentityAccountManager(
            UserManager<TAccount> userManager,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor contextAccessor)
            : base(userManager, roleManager, contextAccessor)
        { }
    }

    public class IdentityAccountManager<TAccount, TAccountManager>
        : IEntityManager<IdentityAccountViewModel>
        where TAccount : IdentityUser, new()
        where TAccountManager : IdentityAccountManager<TAccount, TAccountManager>
    {
        private readonly HttpContext _context;

        #region Constructors

        public IdentityAccountManager(
            UserManager<TAccount> userManager,
            RoleManager<IdentityRole> roleManager,
            IHttpContextAccessor contextAccessor)
        {
            _context = contextAccessor?.HttpContext;

            UserManager = userManager;
            RoleManager = roleManager;
        }

        #endregion

        #region Properties

        protected virtual CancellationToken CancellationToken => _context?.RequestAborted ?? CancellationToken.None;

        protected UserManager<TAccount> UserManager { get; }

        protected RoleManager<IdentityRole> RoleManager { get; }

        #endregion

        #region Public Operations

        public virtual async Task<GenericResult> CreateAsync(IdentityAccountViewModel viewModel)
        {
            var user = new TAccount { UserName = viewModel.Email, Email = viewModel.Email };
            var identityResult = await UserManager.CreateAsync(user, viewModel.Password);
            if (identityResult.Succeeded)
            {
                identityResult = await UserManager.AddToRolesAsync(user, viewModel.Roles);

                if (identityResult.Succeeded)
                {
                    return GenericResult.Success;
                }
                else
                {
                    return GenericResult.Failed(GenericErrorsFromIdentityResult(identityResult));
                }
            }
            else
            {
                return GenericResult.Failed(GenericErrorsFromIdentityResult(identityResult));
            }
        }

        public virtual async Task<GenericResult> UpdateAsync(IdentityAccountViewModel viewModel)
        {
            GenericResult result = null;
            IdentityResult identityResult;
            var user = await UserManager.FindByIdAsync(viewModel.Id);

            identityResult = await UserManager.SetEmailAsync(user, viewModel.Email);
            if (identityResult.Succeeded)
            {
                result = GenericResult.Success;
            }
            else
            {
                result = GenericResult.Failed(GenericErrorsFromIdentityResult(identityResult));
            }

            identityResult = await UserManager.SetUserNameAsync(user, viewModel.Email);
            if (!identityResult.Succeeded)
            {
                var errors = GenericErrorsFromIdentityResult(identityResult);
                result = GenericResult.Failed(result.Succeeded ? errors : result.Errors.Concat(errors).ToArray());
            }

            if (!string.IsNullOrWhiteSpace(viewModel.Password))
            {
                var token = await UserManager.GeneratePasswordResetTokenAsync(user);
                identityResult = await UserManager.ResetPasswordAsync(user, token, viewModel.Password);
                if (!identityResult.Succeeded)
                {
                    var errors = GenericErrorsFromIdentityResult(identityResult);
                    result = GenericResult.Failed(result.Succeeded ? errors : result.Errors.Concat(errors).ToArray());
                }
            }

            var oldRoles = await UserManager.GetRolesAsync(user);
            var rolesToRemove = oldRoles.Where(x => !viewModel.Roles.Contains(x));
            identityResult = await UserManager.RemoveFromRolesAsync(user, rolesToRemove);
            if (!identityResult.Succeeded)
            {
                var errors = GenericErrorsFromIdentityResult(identityResult);
                result = GenericResult.Failed(result.Succeeded ? errors : result.Errors.Concat(errors).ToArray());
            }

            var rolesToAdd = viewModel.Roles.Where(x => !oldRoles.Contains(x));
            identityResult = await UserManager.AddToRolesAsync(user, rolesToAdd);
            if (!identityResult.Succeeded)
            {
                var errors = GenericErrorsFromIdentityResult(identityResult);
                result = GenericResult.Failed(result.Succeeded ? errors : result.Errors.Concat(errors).ToArray());
            }

            return result;
        }

        public virtual async Task<GenericResult> DeleteAsync(IdentityAccountViewModel viewModel)
        {
            var user = await UserManager.FindByIdAsync(viewModel.Id);
            var identityResult = await UserManager.DeleteAsync(user);
            if (identityResult.Succeeded)
            {
                return GenericResult.Success;
            }
            else
            {
                return GenericResult.Failed(GenericErrorsFromIdentityResult(identityResult));
            }
        }

        public virtual async Task<IdentityAccountViewModel> FindByIdAsync(string id)
        {
            return Mapper.Map<IdentityAccountViewModel>(await UserManager.FindByIdAsync(id));
        }

        public virtual async Task<EntityQueryResult<IdentityAccountViewModel>> SearchAsync(IEntitySearchDescriptor<IdentityAccountViewModel> searchDescriptor)
        {
            // TODO:: Implement search

            var users = await UserManager.Users.ToListAsync(CancellationToken);
            var viewModels = new List<IdentityAccountViewModel>(users.Count);
            foreach(var user in users)
            {
                var viewModel = Mapper.Map<IdentityAccountViewModel>(user);
                viewModel.Roles = await UserManager.GetRolesAsync(user);
                viewModels.Add(viewModel);
            }

            var result = new EntityQueryResult<IdentityAccountViewModel> {
                TotalCount = UserManager.Users.Count(),
                FilteredCount = UserManager.Users.Count(),
                Results = viewModels.AsQueryable()
            };

            return result;
        }

        public virtual async Task<IEnumerable<string>> SearchRolesAsync(string search)
        {
            return await RoleManager.Roles.Where(x => x.Name.Contains(search))
                                          .Select(x => x.Name)
                                          .ToListAsync(CancellationToken);
        }

        #endregion

        #region Helpers

        protected virtual GenericError[] GenericErrorsFromIdentityResult(IdentityResult identityResult)
        {
            return identityResult.Errors.Select(x => new GenericError(x.Code, x.Description)).ToArray();
        }

        #endregion

        #region IDisposable Support

        private bool _disposed = false;

        protected void ThrowIfDisposed()
        {
            if (_disposed)
            {
                throw new ObjectDisposedException(GetType().Name);
            }
        }

        protected virtual void Dispose(bool disposing)
        {
            if (!_disposed)
            {
                if (disposing)
                {
                    // TODO: dispose managed state (managed objects).
                    UserManager.Dispose();
                }

                // TODO: free unmanaged resources (unmanaged objects) and override a finalizer below.
                // TODO: set large fields to null.

                _disposed = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }

        #endregion
    }
}
