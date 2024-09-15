using System.Xml.Serialization;

namespace XboxDisplaySwitcher
{
    [XmlRoot("monitors_list")]
    public class MonitorsList
    {
        [XmlElement("item")]
        public List<Item> Items { get; set; }
    }

    public class Item
    {
        [XmlElement("short_monitor_id")]
        public string ShortMonitorId { get; set; }
        [XmlElement("monitor_name")]
        public string MonitorName { get; set; }
        [XmlElement("primary")]
        public string Primary { get; set; }

        public bool IsPrimary => Primary == "Yes";
    }
}

