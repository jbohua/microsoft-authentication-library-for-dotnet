﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Net.NetworkInformation;
using System.Threading;
using System.Threading.Tasks;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Client.Http;
using Microsoft.Identity.Client.UI;

namespace Microsoft.Identity.Client.Platforms.net45
{
    internal abstract class WebUI : IWebUI
    {
        protected Uri RequestUri { get; private set; }
        protected Uri CallbackUri { get; private set; }
        public object OwnerWindow { get; set; }
        protected SynchronizationContext SynchronizationContext { get; set; }

        public RequestContext RequestContext { get; set; }

        public async Task<AuthorizationResult> AcquireAuthorizationAsync(
            Uri authorizationUri,
            Uri redirectUri,
            RequestContext requestContext,
            CancellationToken cancellationToken)
        {
            AuthorizationResult authorizationResult = null;

            var sendAuthorizeRequest = new Action(() =>
            {
                authorizationResult = Authenticate(authorizationUri, redirectUri);
            });

            var sendAuthorizeRequestWithTcs = new Action<object>((tcs) =>
            {
                authorizationResult = Authenticate(authorizationUri, redirectUri);
                ((TaskCompletionSource<object>)tcs).TrySetResult(null);
            });

            // If the thread is MTA, it cannot create or communicate with WebBrowser which is a COM control.
            // In this case, we have to create the browser in an STA thread via StaTaskScheduler object.
            if (Thread.CurrentThread.GetApartmentState() == ApartmentState.MTA)
            {
                if (SynchronizationContext != null)
                {
                    var tcs = new TaskCompletionSource<object>();
                    SynchronizationContext.Post(new SendOrPostCallback(sendAuthorizeRequestWithTcs), tcs);
                    await tcs.Task.ConfigureAwait(false);
                }
                else
                {
                    using (var staTaskScheduler = new StaTaskScheduler(1))
                    {
                        try
                        {
                            Task.Factory.StartNew(
                                sendAuthorizeRequest,
                                cancellationToken,
                                TaskCreationOptions.None,
                                staTaskScheduler).Wait();
                        }
                        catch (AggregateException ae)
                        {
                            requestContext.Logger.ErrorPii(ae.InnerException);
                            // Any exception thrown as a result of running task will cause AggregateException to be thrown with
                            // actual exception as inner.
                            Exception innerException = ae.InnerExceptions[0];

                            // In MTA case, AggregateException is two layer deep, so checking the InnerException for that.
                            if (innerException is AggregateException)
                            {
                                innerException = ((AggregateException)innerException).InnerExceptions[0];
                            }

                            throw innerException;
                        }
                    }
                }
            }
            else
            {
                sendAuthorizeRequest();
            }

            return await Task.Factory.StartNew(() => authorizationResult).ConfigureAwait(false);
        }

        internal AuthorizationResult Authenticate(Uri requestUri, Uri callbackUri)
        {
            RequestUri = requestUri;
            CallbackUri = callbackUri;

            ThrowOnNetworkDown();
            return OnAuthenticate();
        }

        private static void ThrowOnNetworkDown()
        {
            if (!NetworkInterface.GetIsNetworkAvailable())
            {
                throw new MsalClientException(MsalError.NetworkNotAvailableError, MsalErrorMessage.NetworkNotAvailable);
            }
        }

        protected abstract AuthorizationResult OnAuthenticate();

        public void ValidateRedirectUri(Uri redirectUri)
        {
            RedirectUriHelper.Validate(redirectUri, usesSystemBrowser: false);
        }
    }
}
