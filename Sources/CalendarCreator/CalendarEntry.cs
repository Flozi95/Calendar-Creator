using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace CalendarCreator
{
    public class CalendarEntry
    {
        private DateTime _startDate;
        private DateTime _endDate;

        public DateTime StartDate
        {
            get { return _startDate; }
            set
            {
                DateTime @old = _startDate;
                DateTime @new = value;
                if (@old.Day != @new.Day || @old.Month != @new.Month || @old.Year != @new.Year)
                {
                    value = new DateTime(@new.Year, @new.Month, @new.Day, @old.Hour, @old.Minute, @old.Second);
                }
                _startDate = value;
            }
        }

        public DateTime EndDate
        {
            get { return _endDate; }
            set
            {
                DateTime @old = _endDate;
                DateTime @new = value;
                if (@old.Day != @new.Day || @old.Month != @new.Month || @old.Year != @new.Year)
                {
                    value = new DateTime(@new.Year, @new.Month, @new.Day, @old.Hour, @old.Minute, @old.Second);
                }
                _endDate = value;
            }
        }

        public EntryType EventType { get; set; }
        public string Description { get; set; }
        public EntryForm EventForm { get; set; }
        public DateTime CreateTime { get; set; }

        public CalendarEntry()
        {
            _endDate = DateTime.Now;
            _startDate = DateTime.Now;
            StartDate = DateTime.Now;
            EndDate = DateTime.Now;
            CreateTime = DateTime.Now;
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
        SpecialEvent
    }
    public enum EntryForm
    {
        [Description("Praxis")]
        Practice,
        [Description("Theorie")]
        Theory,
        [Description("Theorie und Praxis")]
        PracticeTheory,
        [Description("Sonstiges")]
        Other
    }
}
