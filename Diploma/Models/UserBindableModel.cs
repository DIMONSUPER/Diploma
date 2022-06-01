using System;
using Prism.Mvvm;

namespace Diploma.Models
{
    public class UserBindableModel : BindableBase
    {
        #region -- Public properties --

        private int _id;
        public int Id
        {
            get => _id;
            set => SetProperty(ref _id, value);
        }

        private string _username;
        public string Username
        {
            get => _username;
            set => SetProperty(ref _username, value);
        }

        private string _email;
        public string Email
        {
            get => _email;
            set => SetProperty(ref _email, value);
        }

        private string _provider;
        public string Provider
        {
            get => _provider;
            set => SetProperty(ref _provider, value);
        }

        private bool _isConfirmed;
        public bool IsConfirmed
        {
            get => _isConfirmed;
            set => SetProperty(ref _isConfirmed, value);
        }

        private bool _isBlocked;
        public bool IsBlocked
        {
            get => _isBlocked;
            set => SetProperty(ref _isBlocked, value);
        }

        private DateTime _createdAt;
        public DateTime CreatedAt
        {
            get => _createdAt;
            set => SetProperty(ref _createdAt, value);
        }

        private DateTime _updatedAt;
        public DateTime UpdatedAt
        {
            get => _updatedAt;
            set => SetProperty(ref _updatedAt, value);
        }

        private string _name;
        public string Name
        {
            get => _name;
            set => SetProperty(ref _name, value);
        }

        private string _surname;
        public string Surname
        {
            get => _surname;
            set => SetProperty(ref _surname, value);
        }

        private string _description;
        public string Description
        {
            get => _description;
            set => SetProperty(ref _description, value);
        }

        private int _roleId;
        public int RoleId
        {
            get => _roleId;
            set => SetProperty(ref _roleId, value);
        }

        #endregion
    }
}
