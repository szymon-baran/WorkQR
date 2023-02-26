using System;
using System.ComponentModel.DataAnnotations;

namespace WorkQR.Dictionaries
{
    public enum EventType
    {
        [Display(Name = "Rozpoczęcie pracy")]
        StartWork,
        [Display(Name = "Rozpoczęcie przerwy")]
        StartBreak,
        [Display(Name = "Zakończenie przerwy")]
        EndBreak,
        [Display(Name = "Zakończenie pracy")]
        EndWork,
    }

    public static class EventTypeExtensions
    {
        public static EventType GetDefaultNewEventType(this EventType latestEventType)
        {
            return latestEventType switch
            {
                EventType.StartWork => EventType.StartBreak,
                EventType.StartBreak => EventType.EndBreak,
                EventType.EndBreak => EventType.EndWork,
                EventType.EndWork => EventType.StartWork,
                _ => EventType.StartWork,
            };
        }
    }

}
