using System.ComponentModel;

namespace WorkQR.Dictionaries
{
    public enum EventType
    {
        [Description("Rozpoczęcie pracy")]
        StartWork,
        [Description("Rozpoczęcie przerwy")]
        StartBreak,
        [Description("Zakończenie przerwy")]
        EndBreak,
        [Description("Zakończenie pracy")]
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

        public static string GetQTimestampTitle(this EventType latestEventType)
        {
            return latestEventType switch
            {
                EventType.StartWork => "Praca",
                EventType.StartBreak => "Przerwa",
                EventType.EndBreak => "Praca",
                EventType.EndWork => "Przerwa",
                _ => "",
            };
        }

        public static string GetQTimestampDescription(this EventType latestEventType)
        {
            return latestEventType switch
            {
                EventType.StartWork => "Rozpoczęcie dnia roboczego",
                EventType.StartBreak => "Zejście na przerwę",
                EventType.EndBreak => "Powrót do pracy",
                EventType.EndWork => "Zakończenie dnia roboczego",
                _ => "",
            };
        }

        public static string GetQTimestampColor(this EventType latestEventType)
        {
            return latestEventType switch
            {
                EventType.StartWork => "#f0ac00",
                EventType.StartBreak => "#f5f5f5",
                EventType.EndBreak => "#f0ac00",
                EventType.EndWork => "#f5f5f5",
                _ => "#f0ac00",
            };
        }
    }

}
