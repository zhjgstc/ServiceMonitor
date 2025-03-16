using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace ServiceMonitor.Web.Controllers
{
    public class HomeController : Controller
    {
        public IActionResult Index()
        {
            var runTime = System.DateTime.Now - Program.StartProgramDateTime;
            var runTimeStr = "";
            if (runTime.TotalSeconds < 60)
            {
                runTimeStr = $"{runTime.Seconds}s";
            }
            else if (runTime.TotalMinutes < 60)
            {
                runTimeStr = $"{runTime.Minutes}m";
            }
            else if (runTime.TotalHours < 24)
            {
                runTimeStr = $"{runTime.TotalHours}h{runTime.Minutes}m{runTime.Seconds}s";
            }
            else
            {
                runTimeStr = $"{runTime.Days}day{runTime.Hours}h{runTime.Minutes}m{runTime.Seconds}s";
            }
            var html = $"<h1>Job List Run {runTimeStr}</h1>";
            int i = 1;
            foreach (var task in Program.CurrentConfig.Tasks)
            {
                var status = string.IsNullOrEmpty(task.Status) ? "online" : task.Status;
                html += $"<p>{i}、Job Name：{task.Name} - {task.Ip} - {task.Type} - {task.Interval} - {task.Unit} - {task.Data} - {task.LastReportTime} - {task.Timeout} - Status：{status}；</p>";
                i++;
            }
            return Content(html, "text/html", System.Text.Encoding.UTF8);
        }
    }
}
