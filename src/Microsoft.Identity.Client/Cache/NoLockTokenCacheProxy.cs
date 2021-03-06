﻿// Copyright (c) Microsoft Corporation. All rights reserved.
// Licensed under the MIT License.

using System;
using System.Threading.Tasks;

namespace Microsoft.Identity.Client.Cache
{
    internal class NoLockTokenCacheProxy : ITokenCache
    {
        private readonly TokenCache _tokenCache;

        internal NoLockTokenCacheProxy(TokenCache tokenCache)
        {
            _tokenCache = tokenCache;

        }

        public void DeserializeAdalV3(byte[] adalV3State)
        {
#if !ANDROID && !iOS
            _tokenCache.DeserializeAdalV3NoLocks(adalV3State);
#endif
        }

        public void DeserializeMsalV3(byte[] msalV3State, bool shouldClearExistingCache = false)
        {
#if !ANDROID && !iOS
            _tokenCache.DeserializeMsalV3NoLocks(msalV3State, shouldClearExistingCache);
#endif
        }

        public byte[] SerializeAdalV3()
        {
#if !ANDROID && !iOS
            return _tokenCache.SerializeAdalV3NoLocks();
#else
            return null;
#endif
        }

        public byte[] SerializeMsalV3()
        {
#if !ANDROID && !iOS
            return _tokenCache.SerializeMsalV3NoLocks();
#else
            return null;
#endif
        }

        public byte[] SerializeMsalV2()
        {
#if !ANDROID && !iOS
            return _tokenCache.SerializeMsalV2NoLocks();
#else
            return null;
#endif
        }

        public void DeserializeMsalV2(byte[] msalV2State)
        {
#if !ANDROID && !iOS
            _tokenCache.DeserializeMsalV2NoLocks(msalV2State);
#endif
        }

        [Obsolete("This is expected to be removed in MSAL.NET v3 and ADAL.NET v5. We recommend using SerializeMsalV3/DeserializeMsalV3. Read more: https://aka.ms/msal-net-3x-cache-breaking-change", false)]
        public void DeserializeUnifiedAndAdalCache(CacheData cacheData) => throw new NotImplementedException();

        public byte[] Serialize() => throw new NotImplementedException(MsalErrorMessage.AkaMsmsalnet3BreakingChanges);
        public void Deserialize(byte[] msalV2State) => throw new NotImplementedException(MsalErrorMessage.AkaMsmsalnet3BreakingChanges);

        [Obsolete("This is expected to be removed in MSAL.NET v3 and ADAL.NET v5. We recommend using SerializeMsalV3/DeserializeMsalV3. Read more: https://aka.ms/msal-net-3x-cache-breaking-change", false)]
        public CacheData SerializeUnifiedAndAdalCache() => throw new NotImplementedException(MsalErrorMessage.AkaMsmsalnet3BreakingChanges);

        public void SetAfterAccess(TokenCacheCallback afterAccess) => throw new NotImplementedException(MsalErrorMessage.TokenCacheSetCallbackFunctionalityNotAvailableFromWithinCallback);
        public void SetAfterAccessAsync(Func<TokenCacheNotificationArgs, Task> afterAccess) => throw new NotImplementedException(MsalErrorMessage.TokenCacheSetCallbackFunctionalityNotAvailableFromWithinCallback);
        public void SetBeforeAccessAsync(Func<TokenCacheNotificationArgs, Task> beforeAccess) => throw new NotImplementedException(MsalErrorMessage.TokenCacheSetCallbackFunctionalityNotAvailableFromWithinCallback);
        public void SetBeforeWriteAsync(Func<TokenCacheNotificationArgs, Task> beforeWrite) => throw new NotImplementedException(MsalErrorMessage.TokenCacheSetCallbackFunctionalityNotAvailableFromWithinCallback);
        public void SetBeforeAccess(TokenCacheCallback beforeAccess) => throw new NotImplementedException(MsalErrorMessage.TokenCacheSetCallbackFunctionalityNotAvailableFromWithinCallback);
        public void SetBeforeWrite(TokenCacheCallback beforeWrite) => throw new NotImplementedException(MsalErrorMessage.TokenCacheSetCallbackFunctionalityNotAvailableFromWithinCallback);
    }
}
