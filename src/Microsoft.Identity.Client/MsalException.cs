﻿//----------------------------------------------------------------------
//
// Copyright (c) Microsoft Corporation.
// All rights reserved.
//
// This code is licensed under the MIT License.
//
// Permission is hereby granted, free of charge, to any person obtaining a copy
// of this software and associated documentation files(the "Software"), to deal
// in the Software without restriction, including without limitation the rights
// to use, copy, modify, merge, publish, distribute, sublicense, and / or sell
// copies of the Software, and to permit persons to whom the Software is
// furnished to do so, subject to the following conditions :
//
// The above copyright notice and this permission notice shall be included in
// all copies or substantial portions of the Software.
//
// THE SOFTWARE IS PROVIDED "AS IS", WITHOUT WARRANTY OF ANY KIND, EXPRESS OR
// IMPLIED, INCLUDING BUT NOT LIMITED TO THE WARRANTIES OF MERCHANTABILITY,
// FITNESS FOR A PARTICULAR PURPOSE AND NONINFRINGEMENT.IN NO EVENT SHALL THE
// AUTHORS OR COPYRIGHT HOLDERS BE LIABLE FOR ANY CLAIM, DAMAGES OR OTHER
// LIABILITY, WHETHER IN AN ACTION OF CONTRACT, TORT OR OTHERWISE, ARISING FROM,
// OUT OF OR IN CONNECTION WITH THE SOFTWARE OR THE USE OR OTHER DEALINGS IN
// THE SOFTWARE.
//
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Globalization;
using Microsoft.Identity.Client.Internal;

namespace Microsoft.Identity.Client
{
    /// <summary>
    /// The exception type thrown when an error occurs during token acquisition.
    /// </summary>
    public class MsalException : Exception
    {
        private static readonly Dictionary<string, string> ErrorMessages = new Dictionary<string, string>
        {
            [MsalError.InvalidCredentialType] = MsalErrorMessage.InvalidCredentialType,
            [MsalError.IdentityProtocolLoginUrlNull] = MsalErrorMessage.IdentityProtocolLoginUrlNull,
            [MsalError.IdentityProtocolMismatch] = MsalErrorMessage.IdentityProtocolMismatch,
            [MsalError.EmailAddressSuffixMismatch] = MsalErrorMessage.EmailAddressSuffixMismatch,
            [MsalError.IdentityProviderRequestFailed] = MsalErrorMessage.IdentityProviderRequestFailed,
            [MsalError.StsTokenRequestFailed] = MsalErrorMessage.StsTokenRequestFailed,
            [MsalError.EncodedTokenTooLong] = MsalErrorMessage.EncodedTokenTooLong,
            [MsalError.StsMetadataRequestFailed] = MsalErrorMessage.StsMetadataRequestFailed,
            [MsalError.AuthorityNotInValidList] = MsalErrorMessage.AuthorityNotInValidList,
            [MsalError.UnsupportedUserType] = MsalErrorMessage.UnsupportedUserType,
            [MsalError.UnknownUser] = MsalErrorMessage.UnknownUser,
            [MsalError.UserRealmDiscoveryFailed] = MsalErrorMessage.UserRealmDiscoveryFailed,
            [MsalError.AccessingWsMetadataExchangeFailed] = MsalErrorMessage.AccessingMetadataDocumentFailed,
            [MsalError.ParsingWsMetadataExchangeFailed] = MsalErrorMessage.ParsingMetadataDocumentFailed,
            [MsalError.WsTrustEndpointNotFoundInMetadataDocument] = MsalErrorMessage.WsTrustEndpointNotFoundInMetadataDocument,
            [MsalError.ParsingWsTrustResponseFailed] = MsalErrorMessage.ParsingWsTrustResponseFailed,
            [MsalError.AuthenticationCanceled] = MsalErrorMessage.AuthenticationCanceled,
            [MsalError.NetworkNotAvailable] = MsalErrorMessage.NetworkIsNotAvailable,
            [MsalError.AuthenticationUiFailed] = MsalErrorMessage.AuthenticationUiFailed,
            [MsalError.UserInteractionRequired] = MsalErrorMessage.UserInteractionRequired,
            [MsalError.MissingFederationMetadataUrl] = MsalErrorMessage.MissingFederationMetadataUrl,
            [MsalError.IntegratedAuthFailed] = MsalErrorMessage.IntegratedAuthFailed,
            [MsalError.UnauthorizedResponseExpected] = MsalErrorMessage.UnauthorizedResponseExpected,
            [MsalError.MultipleTokensMatched] = MsalErrorMessage.MultipleTokensMatched,
            [MsalError.PasswordRequiredForManagedUserError] = MsalErrorMessage.PasswordRequiredForManagedUserError,
            [MsalError.GetUserNameFailed] = MsalErrorMessage.GetUserNameFailed,
            // MsalErrorMessage.Unknown will be set as the default error message in GetErrorMessage(string errorCode).
        };

        /// <summary>
        /// Initializes a new instance of the exception class.
        /// </summary>
        public MsalException()
            : base(MsalErrorMessage.Unknown)
        {
            ErrorCode = MsalError.Unknown;
        }

        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code.
        /// </summary>
        /// <param name="errorCode">
        /// The error code returned by the service or generated by client. This is the code you can rely on
        /// for exception handling.
        /// </param>
        public MsalException(string errorCode)
            : base(GetErrorMessage(errorCode))
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code and error message.
        /// </summary>
        /// <param name="errorCode">
        /// The error code returned by the service or generated by client. This is the code you can rely on
        /// for exception handling.
        /// </param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public MsalException(string errorCode, string message)
            : base(message)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The error code returned by the service or generated by client. This is the code you can rely on
        /// for exception handling.
        /// </param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified. It may especially contain the actual error message returned by the service.
        /// </param>
        public MsalException(string errorCode, Exception innerException)
            : base(GetErrorMessage(errorCode), innerException)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Initializes a new instance of the exception class with a specified
        /// error code, error message and a reference to the inner exception that is the cause of
        /// this exception.
        /// </summary>
        /// <param name="errorCode">
        /// The error code returned by the service or generated by client. This is the code you can rely on
        /// for exception handling.
        /// </param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">
        /// The exception that is the cause of the current exception, or a null reference if no inner
        /// exception is specified. It may especially contain the actual error message returned by the service.
        /// </param>
        public MsalException(string errorCode, string message, Exception innerException)
            : base(message, innerException)
        {
            ErrorCode = errorCode;
        }

        /// <summary>
        /// Gets the protocol error code returned by the service or generated by client. This is the code you can rely on for
        /// exception handling.
        /// </summary>
        public string ErrorCode { get; }

        /// <summary>
        /// Creates and returns a string representation of the current exception.
        /// </summary>
        /// <returns>A string representation of the current exception.</returns>
        public override string ToString()
        {
            return base.ToString() + string.Format(CultureInfo.InvariantCulture, "\n\tErrorCode: {0}", ErrorCode);
        }

        /// <summary>
        /// Gets the Error Message
        /// </summary>
        protected static string GetErrorMessage(string errorCode)
        {
            string message = ErrorMessages.ContainsKey(errorCode) ? ErrorMessages[errorCode] : MsalErrorMessage.Unknown;
            return String.Format(CultureInfo.InvariantCulture, "{0}: {1}", errorCode, message);
        }

        internal enum ErrorFormat
        {
            Json,
            Other
        }
    }
}