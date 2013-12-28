// This file is part of CalendarCreator.
// 
// CalendarCreator is free software: you can redistribute it and/or modify
// it under the terms of the GNU General Public License as published by
// the Free Software Foundation, either version 3 of the License, or
// (at your option) any later version.
// 
// CalendarCreator is distributed in the hope that it will be useful,
// but WITHOUT ANY WARRANTY; without even the implied warranty of
// MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
// GNU General Public License for more details.
// 
// You should have received a copy of the GNU General Public License
// along with AlarmWorkflow.  If not, see <http://www.gnu.org/licenses/>.

#region

using System;
using System.Diagnostics;
using System.Windows.Input;

#endregion

namespace CalendarCreator
{
    /// <summary>
    /// From http://msdn.microsoft.com/en-us/magazine/dd419663.aspx
    /// </summary>
    internal class RelayCommand : ICommand
    {
        private readonly Predicate<object> _canExecute;
        private readonly Action<object> _execute;

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand" /> class.
        /// </summary>
        /// <param name="execute">
        /// The method to be called when the command is
        /// invoked.
        /// </param>
        public RelayCommand(Action<object> execute)
            : this(execute, null)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RelayCommand" /> class.
        /// </summary>
        /// <param name="execute">
        /// The method to be called when the command is
        /// invoked.
        /// </param>
        /// <param name="canExecute">
        /// the method that determines whether the command
        /// can execute in its current state.
        /// </param>
        public RelayCommand(Action<object> execute, Predicate<object> canExecute)
        {
            if (execute == null)
                throw new ArgumentNullException("execute");

            _execute = execute;
            _canExecute = canExecute;
        }

        /// <summary>
        /// Defines the method that determines whether the command can execute in
        /// its current state.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.  If the command does
        /// not require data to be passed, this object can be set to null.
        /// </param>
        /// <returns>
        /// true if this command can be executed; otherwise, false.
        /// </returns>
        [DebuggerStepThrough]
        public bool CanExecute(object parameter)
        {
            return _canExecute == null ? true : _canExecute(parameter);
        }

        /// <summary>
        /// Occurs when changes occur that affect whether or not the command should
        /// execute.
        /// </summary>
        public event EventHandler CanExecuteChanged
        {
            add { CommandManager.RequerySuggested += value; }
            remove { CommandManager.RequerySuggested -= value; }
        }

        /// <summary>
        /// Defines the method to be called when the command is invoked.
        /// </summary>
        /// <param name="parameter">
        /// Data used by the command.  If the command does
        /// not require data to be passed, this object can be set to null.
        /// </param>
        public void Execute(object parameter)
        {
            _execute(parameter);
        }
    }
}