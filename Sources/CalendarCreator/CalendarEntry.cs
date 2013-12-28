using System;
using System.ComponentModel;

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
