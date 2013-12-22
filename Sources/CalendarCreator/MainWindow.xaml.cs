using System.Windows.Controls;
using CalendarCreator.Annotations;
using System;
using System.Diagnostics;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows;
using System.Windows.Input;
using System.Xml.Serialization;

namespace CalendarCreator
{
    /// <summary>
    ///     Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window, INotifyPropertyChanged
    {
        #region Fields

        private ObservableCollection<CalendarEntry> _events;
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
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Calendar"; // Default file name
            dlg.DefaultExt = ".cal"; // Default file extension
            dlg.Filter = "Calendar-file (.cal)|*.cal"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
                // Save document
                string filename = dlg.FileName;
                XmlSerializer serializer = new XmlSerializer(_events.GetType());
                using (Stream stream = new FileStream(filename, FileMode.OpenOrCreate))
                {
                    serializer.Serialize(stream, _events);
                }
            }
        }

        #endregion SaveCommand

        #region OpenCommand

        public ICommand OpenCommand { get; set; }

        private void OpenCommand_Execute(object parameter)
        {
            Microsoft.Win32.OpenFileDialog dlg = new Microsoft.Win32.OpenFileDialog();
            dlg.FileName = "Calendar"; // Default file name
            dlg.DefaultExt = ".cal"; // Default file extension
            dlg.Filter = "Calendar-file (.cal)|*.cal"; // Filter files by extension
            // Show open file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
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

        #region ExportCommand

        public ICommand ExportCommand { get; set; }

        private void ExportCommand_Execute(object parameter)
        {
            // Configure save file dialog box
            Microsoft.Win32.SaveFileDialog dlg = new Microsoft.Win32.SaveFileDialog();
            dlg.FileName = "Calendar"; // Default file name
            dlg.DefaultExt = ".ics"; // Default file extension
            dlg.Filter = "iCalendar (.ics)|*.ics"; // Filter files by extension

            // Show save file dialog box
            Nullable<bool> result = dlg.ShowDialog();

            // Process save file dialog box results
            if (result == true)
            {
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
