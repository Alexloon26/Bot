using System;
using System.Collections.Generic;

namespace BrawlHelper.Models
{
    public class ScheduledEvents
    {
        public List<EventItem> Items { get; set; }
    }

    public class EventItem
    {
        public int SlotId { get; set; }
        public EventDetails Event { get; set; }
        public string StartTime { get; set; }
        public string EndTime { get; set; }
    }

    public class EventDetails
    {
        public int Id { get; set; }
        public string Mode { get; set; }
        public string Map { get; set; }
    }
}