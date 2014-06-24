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
using System.ComponentModel;
using System.Runtime.CompilerServices;
using CalendarCreator.Annotations;

#endregion

namespace CalendarCreator
{
    public class CalendarEntry : INotifyPropertyChanged
    {
        private DateTime _startDate;
        private DateTime _endDate;
        private EntryType _eventType;
        private string _description;
        private EntryForm _eventForm;

        public CalendarEntry()
        {
            EndDate = DateTime.Now;
            StartDate = DateTime.Now;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            CreateTime = DateTime.Now;
            EventForm = EntryForm.Practice;
            EventType = EntryType.Training;
        }

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                _startDate = value;
                OnPropertyChanged();
                if (_startDate > EndDate)
                {
                    EndDate = EndDate.AddDays(_startDate.Day - EndDate.Day);
                    EndDate = EndDate.AddMonths(_startDate.Month - EndDate.Month);
                    EndDate = EndDate.AddYears(_startDate.Year - EndDate.Year);
                }
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                if (value.Equals(_endDate)) return;
                _endDate = value;
                OnPropertyChanged();
            }
        }

        public EntryType EventType
        {
            get { return _eventType; }
            set
            {
                if (value == _eventType) return;
                _eventType = value;
                OnPropertyChanged();
            }
        }

        public string Description
        {
            get { return _description; }
            set
            {
                if (value == _description) return;
                _description = value;
                OnPropertyChanged();
            }
        }

        public EntryForm EventForm
        {
            get { return _eventForm; }
            set
            {
                if (value == _eventForm) return;
                _eventForm = value;
                OnPropertyChanged();
            }
        }

        public DateTime CreateTime { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        [NotifyPropertyChangedInvocator]
        protected virtual void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChangedEventHandler handler = PropertyChanged;
            if (handler != null) handler(this, new PropertyChangedEventArgs(propertyName));
        }
    }

    public enum EntryType
    {
        [Description("Übung")]
        Training,
        [Description("Dienstsitzung")]
        Meeting,
        [Description("Arbeitsdienst")]
        WorkService,
        [Description("Sonstige Veranstaltungen")]
        SpecialEvent,
        [Description("Stüberlabend")]
        Community
    }

    public enum EntryForm
    {
        [ShortDescription("P")]
        [Description("Praxis")]
        Practice,
        [ShortDescription("T")]
        [Description("Theorie")]
        Theory,
        [ShortDescription("T/P")]
        [Description("Theorie und Praxis")]
        PracticeTheory,
        [ShortDescription("Sonstiges")]
        [Description("Sonstiges")]
        Other
    }
}