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
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;
using CalendarCreator.Annotations;
using Microsoft.Win32;

#endregion

namespace CalendarCreator
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Fields

        private ObservableCollection<CalendarEntry> _events;
        private string _fileName;
        private CalendarEntry _selection;

        #endregion Fields

        #region Properties

        public ObservableCollection<CalendarEntry> Events
        {
            get { return _events; }
            set
            {
                if (Equals(value, _events)) return;
                _events = value;
                OnPropertyChanged();
            }
        }

        public CalendarEntry Selection
        {
            get { return _selection; }
            set
            {
                if (Equals(value, _selection)) return;
                _selection = value;
                OnPropertyChanged();
            }
        }

        #endregion Properties

        #region SaveCommand

        public ICommand SaveCommand { get; set; }

        private void SaveCommand_Execute(object parameter)
        {
            if (string.IsNullOrWhiteSpace(_fileName))
            {
                SaveAsCommand.Execute(null);
            }
            else
            {
                XmlSerializer serializer = new XmlSerializer(_events.GetType());
                using (Stream stream = new FileStream(_fileName, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(stream, new ObservableCollection<CalendarEntry>(_events.OrderBy(x => x.StartDate)));
                }
                MessageBox.Show("Erfolgreich gespeichert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);

            }
        }

        #endregion SaveCommand

        #region SaveAsCommand

        public ICommand SaveAsCommand { get; set; }

        private void SaveAsCommand_Execute(object parameter)
        {
            // Configure save file dialog box
            SaveFileDialog dlg = new SaveFileDialog();
            if (string.IsNullOrWhiteSpace(_fileName))
            {
                dlg.FileName = "Calendar"; // Default file name
            }
            else
            {
                dlg.FileName = Path.GetFileNameWithoutExtension(_fileName);
            }
            dlg.DefaultExt = ".cal"; // Default file extension
            dlg.Filter = "Calendar-file (.cal)|*.cal"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                _fileName = dlg.FileName;
                // Save document
                string filename = dlg.FileName;
                XmlSerializer serializer = new XmlSerializer(_events.GetType());
                using (Stream stream = new FileStream(filename, FileMode.Create))
                {
                    serializer.Serialize(stream, new ObservableCollection<CalendarEntry>(_events.OrderBy(x => x.StartDate)));
                }
                MessageBox.Show("Erfolgreich gespeichert!", "Erfolg", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        #endregion SaveAsCommand

        #region OpenCommand

        public ICommand OpenCommand { get; set; }

        private void OpenCommand_Execute(object parameter)
        {
            OpenFileDialog dlg = new OpenFileDialog();
            if (string.IsNullOrWhiteSpace(_fileName))
            {
                dlg.FileName = "Calendar"; // Default file name
            }
            else
            {
                dlg.FileName = Path.GetFileNameWithoutExtension(_fileName);
            }
            dlg.DefaultExt = ".cal"; // Default file extension
            dlg.Filter = "Calendar-file (.cal)|*.cal"; // Filter files by extension
            // Show open file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                _fileName = dlg.FileName;

                Events.Clear();
                // open document
                string filename = dlg.FileName;
                XmlSerializer serializer = new XmlSerializer(_events.GetType());
                using (Stream stream = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    Events = (ObservableCollection<CalendarEntry>)serializer.Deserialize(stream);
                }
            }
        }

        #endregion OpenCommand

        #region NewCommand

        public ICommand NewCommand { get; set; }

        private void NewCommand_Execute(object parameter)
        {
            Events = new ObservableCollection<CalendarEntry>();
            _fileName = String.Empty;
        }

        #endregion NewCommand

        #region ExportCommand

        public ICommand ExportCommand { get; set; }

        private void ExportCommand_Execute(object parameter)
        {
            // Configure save file dialog box
            SaveFileDialog dlg = new SaveFileDialog();
            if (string.IsNullOrWhiteSpace(_fileName))
            {
                dlg.FileName = "Calendar"; // Default file name
            }
            else
            {
                dlg.FileName = _fileName;
            }
            dlg.DefaultExt = ".ics"; // Default file extension
            dlg.Filter = "iCalendar (.ics)|*.ics"; // Filter files by extension

            // Show save file dialog box
            bool? result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                _fileName = Path.GetFileNameWithoutExtension(dlg.FileName);

                // Save document
                string filename = dlg.FileName;

                string content = Helper.GenerateICalendar(Events);
                File.WriteAllText(filename, content);
            }
        }

        #endregion ExportCommand

        #region AddRowCommand

        public ICommand AddRowCommand { get; set; }

        private void AddRowCommand_Execute(object parameter)
        {
            if (Selection != null)
            {
                Events.Add(new CalendarEntry
                {
                    Description = Selection.Description,
                    EndDate = Selection.EndDate,
                    EventForm = Selection.EventForm,
                    EventType = Selection.EventType,
                    StartDate = Selection.StartDate
                });
            }
            else
            {
                Events.Add(new CalendarEntry());
            }
            Selection = Events.LastOrDefault();
            UpdateProperties();
        }

        #endregion AddRowCommand

        #region DeleteRowCommand

        public ICommand DeleteRowCommand { get; set; }

        private bool DeleteRowCommand_CanExecute(object parameter)
        {
            return Selection != null;
        }

        private void DeleteRowCommand_Execute(object parameter)
        {
            Events.Remove(Selection);
            Selection = Events.LastOrDefault();
            UpdateProperties();
        }

        #endregion DeleteRowCommand

        #region Constructor

        public MainWindow()
        {
            InitializeComponent();
            AddRowCommand = new RelayCommand(AddRowCommand_Execute);
            DeleteRowCommand = new RelayCommand(DeleteRowCommand_Execute, DeleteRowCommand_CanExecute);
            ExportCommand = new RelayCommand(ExportCommand_Execute);
            OpenCommand = new RelayCommand(OpenCommand_Execute);
            SaveCommand = new RelayCommand(SaveCommand_Execute);
            SaveAsCommand = new RelayCommand(SaveAsCommand_Execute);
            NewCommand = new RelayCommand(NewCommand_Execute);
            Events = new ObservableCollection<CalendarEntry>();
            DataContext = this;
        }

        #endregion Constructor

        #region Methods

        private void UpdateProperties()
        {
            OnPropertyChanged("Selection");
            OnPropertyChanged("Events");
        }

        #endregion Methods

        #region PropertyChanged-Member

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }

        #endregion PropertyChanged-Member
    }
}