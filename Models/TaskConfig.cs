namespace ServiceMonitor.Web.Models
{
    public class Config
    {
        public List<TaskConfig> Tasks { get; set; }
    }

    public class TaskConfig
    {
        public string Ip { get; set; } = "127.0.0.1";
        public string Name { get; set; }
        public string Type { get; set; }
        public int Interval { get; set; } = 5;
        public string Unit { get; set; } = "seconds";
        public string Data { get; set; }

        public DateTime LastReportTime { get; set; }

        public int Timeout { get; set; } = 5;

        public string Status { get; set; }
    }
}
