﻿
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;
using Microsoft.Identity.Client;
using Microsoft.Identity.Client.Cache.Items;
using Microsoft.Identity.Client.Core;
using Microsoft.Identity.Test.Common.Core.Helpers;

namespace DesktopTestApp
{
    public partial class MsalUserRefreshTokenControl : UserControl
    {
        private readonly ITokenCacheInternal _cache;
        private readonly PublicClientApplication _publicClient;
        private readonly MsalRefreshTokenCacheItem _rtItem;
        private readonly MsalAccountCacheItem _accountItem;
        public delegate Task RefreshViewAsync();

        private const string GarbageRtValue = "garbage-refresh-token";

        public RefreshViewAsync RefreshViewAsyncDelegate { get; set; }

        internal MsalUserRefreshTokenControl(PublicClientApplication publicClient, MsalRefreshTokenCacheItem rtItem) : this()
        {
            _publicClient = publicClient;
            _cache = publicClient.UserTokenCacheInternal;
            _rtItem = rtItem;

            _accountItem = _cache.GetAccountAsync(_rtItem).GetAwaiter().GetResult(); // todo: yuck
            upnLabel.Text = _accountItem.PreferredUsername;

            invalidateRefreshTokenBtn.Enabled = !_rtItem.Secret.Equals(GarbageRtValue, StringComparison.OrdinalIgnoreCase);
        }

        public MsalUserRefreshTokenControl()
        {
            InitializeComponent();
        }

        private void InvalidateRefreshTokenBtn_Click(object sender, System.EventArgs e)
        {
            _rtItem.Secret = GarbageRtValue;
            _cache.AddRefreshTokenCacheItem(_rtItem);
            invalidateRefreshTokenBtn.Enabled = false;
        }

        private async void signOutUserOneBtn_Click(object sender, System.EventArgs e)
        {
            IEnumerable<IAccount> accounts = await _publicClient.GetAccountsAsync().ConfigureAwait(false);

            while (accounts.Any())
            {
                await _publicClient.RemoveAsync(accounts.FirstOrDefault()).ConfigureAwait(false);
                accounts = await _publicClient.GetAccountsAsync().ConfigureAwait(false);
            }

            RefreshViewAsyncDelegate?.Invoke();
        }
    }
}
