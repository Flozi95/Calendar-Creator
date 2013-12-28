using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Reflection;
using System.Text;

namespace CalendarCreator
{
    class Helper
    {
        public static string GenerateICalendar(IEnumerable<CalendarEntry> events)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("BEGIN:VCALENDAR");
            builder.AppendLine("VERSION:2.0");
            builder.AppendLine("PRODID:Calendar-Creator");
            builder.AppendLine("METHOD:PUBLISH");
            foreach (var calendarEntry in events)
            {
                builder.AppendLine("BEGIN:VEVENT");
                builder.AppendLine(string.Format("UID:{0}", Guid.NewGuid()));
                builder.AppendLine(string.Format("DTSTART:{0}", calendarEntry.StartDate.ToUniversalTime().ToString("yyyyMMddTHHmmssZ")));
                builder.AppendLine(string.Format("DTEND:{0}", calendarEntry.EndDate.ToUniversalTime().ToString("yyyyMMddTHHmmssZ")));
                builder.AppendLine(string.Format("DTSTAMP:{0}", calendarEntry.CreateTime.ToUniversalTime().ToString("yyyyMMddTHHmmssZ")));
                switch (calendarEntry.EventType)
                {
                    case EntryType.Training:

                        builder.AppendLine(string.Format("SUMMARY:{0}: {1} ({2})",
                            GetDescription(calendarEntry.EventType),
                            calendarEntry.Description,
                            GetShortDescription(calendarEntry.EventForm)));
                        break;
                    default:
                        if (string.IsNullOrWhiteSpace(calendarEntry.Description))
                        {
                            builder.AppendLine(string.Format("SUMMARY:{0}", GetDescription(calendarEntry.EventType)));
                        }
                        else
                        {
                            builder.AppendLine(string.Format("SUMMARY:{0}: {1}", GetDescription(calendarEntry.EventType), calendarEntry.Description));
                        }
                        break;
                }
                builder.AppendLine("SEQUENCE:0");
                builder.AppendLine("STATUS:CONFIRMED");
                builder.AppendLine("CLASS:PUBLIC");
                builder.AppendLine("TRANSP:OPAQUE");
                builder.AppendLine("LOCATION:");
                builder.AppendLine("END:VEVENT");
            }
            builder.AppendLine("END:VCALENDAR");
            return builder.ToString();
        }

        private static string GetDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(DescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((DescriptionAttribute)attrs[0]).Description;
                }
            }
            return en.ToString();
        }

        private static string GetShortDescription(Enum en)
        {
            Type type = en.GetType();
            MemberInfo[] memInfo = type.GetMember(en.ToString());
            if (memInfo != null && memInfo.Length > 0)
            {
                object[] attrs = memInfo[0].GetCustomAttributes(typeof(ShortDescriptionAttribute), false);
                if (attrs != null && attrs.Length > 0)
                {
                    return ((ShortDescriptionAttribute)attrs[0]).ShortDescription;
                }
            }
            return en.ToString();
        }

    }

    public class ShortDescriptionAttribute : System.Attribute
    {
        public ShortDescriptionAttribute(string shortDescription) 
        {
            this.ShortDescription = shortDescription;
        }

        public string ShortDescription { get; set; }
    }
}
