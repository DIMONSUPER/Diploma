﻿using System;
using System.Threading.Tasks;
using System.Windows.Input;
using Xamarin.CommunityToolkit.Helpers;

namespace Diploma.Helpers
{
    public class SingleExecutionCommand : ICommand
    {
        public static bool IsNavigating;

        private readonly DelegateWeakEventManager _canExecuteChangedEventManager = new();

        private Func<object, Task> _func;
        private bool _isExecuting;
        private int _delayMillisec;
        private bool _canExecute;

        public SingleExecutionCommand()
        {
        }

        #region -- Public static methods --

        public static SingleExecutionCommand FromFunc(Func<Task> func, int delayMillisec = 400, Func<bool> canExecute = null)
        {
            var ret = new SingleExecutionCommand
            {
                _func = (obj) => func(),
                _delayMillisec = delayMillisec,
                _canExecute = canExecute is null || canExecute(),
            };

            return ret;
        }

        public static SingleExecutionCommand FromFunc(Func<object, Task> func, int delayMillisec = 400, Func<bool> canExecute = null)
        {
            var ret = new SingleExecutionCommand
            {
                _func = func,
                _delayMillisec = delayMillisec,
                _canExecute = canExecute is null || canExecute(),
            };

            return ret;
        }

        public static SingleExecutionCommand FromFunc<T>(Func<T, Task> func, int delayMillisec = 400, Func<bool> canExecute = null)
        {
            var ret = new SingleExecutionCommand
            {
                _func = (object obj) =>
                {
                    var objT = default(T);
                    objT = (T)obj;
                    return func(objT);
                },
                _delayMillisec = delayMillisec,
                _canExecute = canExecute is null || canExecute(),
            };

            return ret;
        }

        #endregion

        #region -- ICommand implementation --

        public event EventHandler CanExecuteChanged
        {
            add => _canExecuteChangedEventManager.AddEventHandler(value);
            remove => _canExecuteChangedEventManager.RemoveEventHandler(value);
        }

        public bool CanExecute(object parameter)
        {
            return _canExecute;
        }

        public async void Execute(object parameter)
        {
            if (_isExecuting || IsNavigating) return;

            _isExecuting = true;

            await _func(parameter);
            if (_delayMillisec > 0)
            {
                await Task.Delay(_delayMillisec);
            }

            _isExecuting = false;
        }

        #endregion
    }
}