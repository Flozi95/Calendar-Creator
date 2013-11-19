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
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EntryType EventType { get; set; }
        public string Description { get; set; }
        public EntryForm EventForm { get; set; }
        public DateTime CreateTime { get; set; }

        public CalendarEntry() {
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
