﻿using Diploma.Events;
using Diploma.Models;
using Prism.Events;
using Xamarin.Essentials;

namespace Diploma.Services.Settings
{
    public class AuthorizationSettings
    {
        #region -- Public properties --

        public int UserId
        {
            get => Preferences.Get(nameof(UserId), 0);
            set => Preferences.Set(nameof(UserId), value);
        }

        public string JWTToken
        {
            get => Preferences.Get(nameof(JWTToken), default(string));
            set => Preferences.Set(nameof(JWTToken), value);
        }

        #endregion

        #region -- Protected properties --

        private IEventAggregator _eventAggregator;
        protected IEventAggregator EventAggregator => _eventAggregator ??= App.Resolve<IEventAggregator>();

        #endregion

        #region -- Public helpers --

        public void AuthorizeUser(string jwtToken, UserModel userModel)
        {
            if (userModel is not null && !string.IsNullOrWhiteSpace(jwtToken))
            {
                JWTToken = jwtToken;
                UserId = userModel.Id;
                EventAggregator?.GetEvent<AuthChangedEvent>()?.Publish(true);
            }
        }

        public void ResetSettings()
        {
            JWTToken = default;
            UserId = 0;
            EventAggregator?.GetEvent<AuthChangedEvent>()?.Publish(false);
        }

        #endregion
    }
}
