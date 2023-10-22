namespace HqPocket.Extensions.Logging
{
    public class ObservableCollectionLoggerConfiguration
    {
        public int EventId { get; set; }
        public bool AddClassName { get; set; }
        public bool AddEventId { get; set; }
        public bool AddDateTime { get; set; } = true;
    }
}
