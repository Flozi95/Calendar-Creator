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

#endregion

namespace CalendarCreator
{
    public class CalendarEntry
    {
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

        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public EntryType EventType { get; set; }
        public string Description { get; set; }
        public EntryForm EventForm { get; set; }
        public DateTime CreateTime { get; set; }
    }

    public enum EntryType
    {
        [Description("Übung")] Training,
        [Description("Dienstsitzung")] Meeting,
        [Description("Arbeitsdienst")] WorkService,
        [Description("Sonstige Veranstaltungen")] SpecialEvent,
        [Description("Stüberlabend")] Community
    }

    public enum EntryForm
    {
        [ShortDescription("P")] [Description("Praxis")] Practice,
        [ShortDescription("T")] [Description("Theorie")] Theory,
        [ShortDescription("T/P")] [Description("Theorie und Praxis")] PracticeTheory,
        [ShortDescription("Sonstiges")] [Description("Sonstiges")] Other
    }
}