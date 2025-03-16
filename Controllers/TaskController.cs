using Microsoft.AspNetCore.Mvc;
using ServiceMonitor.Web.Models;
using System.Threading.Tasks;

namespace ServiceMonitor.Web.Controllers
{
    public class TaskController : Controller
    {
        public IActionResult Register(TaskConfig task)
        {
            if (string.IsNullOrEmpty(task.Name))
            {
                return BadRequest();
            }
            AddConfig(task);
            return Ok();
        }

        public IActionResult Report(TaskConfig task)
        {
            if (string.IsNullOrEmpty(task.Name))
            {
                return BadRequest();
            }

            task.Ip = GetIpAddress(HttpContext);
            if (Program.CurrentConfig != null && Program.CurrentConfig.Tasks.Any(t => t.Ip == task.Ip && t.Name == task.Name))
            {
                Program.CurrentConfig.Tasks.First(t => t.Ip == task.Ip && t.Name == task.Name).LastReportTime = DateTime.Now;
                Program.CurrentConfig.Tasks.First(t => t.Ip == task.Ip && t.Name == task.Name).Status = "";
            }
            else
            {
                AddConfig(task);
            }
            return Ok();
        }

        private string GetIpAddress(HttpContext httpContext)
        {
            var ip = HttpContext.Connection?.RemoteIpAddress?.ToString();
            return ip ?? "no.ip.address";
        }

        private void AddConfig(TaskConfig task)
        {
            task.Ip = GetIpAddress(HttpContext);
            if (string.IsNullOrEmpty(task.Type))
            {
                task.Type = "HTTP";
            }
            task.LastReportTime = System.DateTime.Now;
            if (Program.CurrentConfig != null)
            {
                if (Program.CurrentConfig.Tasks.Any(t => t.Ip == task.Ip && t.Name == task.Name))
                {
                    Program.CurrentConfig.Tasks.Remove(Program.CurrentConfig.Tasks.First(t => t.Name == task.Name));
                }
                Program.CurrentConfig.Tasks.Add(task);
            }
        }
    }
}
