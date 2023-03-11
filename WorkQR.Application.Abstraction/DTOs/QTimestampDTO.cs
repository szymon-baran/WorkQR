using WorkQR.Dictionaries;

namespace WorkQR.Application
{
    public class QTimestampDTO
    {
        /// </summary>
        /// YYYY-MM-DD
        /// </summary>
        public string Date { get; set; } = "";
        /// <summary>
        /// Start time (HH:MM) - optional
        public string Time { get; set; } = "";
        public string Title { get; set; } = "";
        public EventType EventType { get; set; }
        public string Header { get; set; } = "";
        public string Details { get; set; } = "";
        public double Duration { get; set; } = 0;
        public string Bgcolor { get; set; } = "green";
    }
}
