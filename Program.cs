using Newtonsoft.Json;
using ServiceMonitor.Web.Logics;
using ServiceMonitor.Web.Models;
using System.Reflection;
using System.Threading.Tasks;

namespace ServiceMonitor.Web
{
    public class Program
    {
        private static Dictionary<string, Timer> _timers = new Dictionary<string, Timer>();
        private static string _configFilePath = "";
        private static Timer? _configCheckTimer;
        public static Config? CurrentConfig;
        public static DateTime StartProgramDateTime;

        public static void Main(string[] args)
        {
            StartProgramDateTime = DateTime.Now;
            _configFilePath = GetConfigFilePath(args);
            CurrentConfig = LoadConfig(_configFilePath);

            foreach (var task in CurrentConfig.Tasks)
            {
                AddJob(task);
            }

            _configCheckTimer = new Timer(_ =>
            {
                var list = CurrentConfig.Tasks.Where(w => string.IsNullOrEmpty(w.Status));
                foreach (var task in list)
                {
                    if (!_timers.ContainsKey(task.Ip + "_" + task.Name))
                    {
                        try
                        {
                            AddJob(task);
                        }
                        catch
                        {

                        }
                    }
                }
            }, null, 0, 2000);
            // 启动控制台输入监听线程
            Task.Run(() => ListenForCommands());
            StartWebService(args);
        }

        private static void StartWebService(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllers();
            var app = builder.Build();
            app.UseRouting();
            app.MapControllers();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
            app.Run();
        }

        private static string GetConfigFilePath(string[] args)
        {
            string defaultConfigFilePath = "config.json";

            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-config" && i + 1 < args.Length)
                {
                    return args[i + 1];
                }
            }

            return defaultConfigFilePath;
        }

        private static Config LoadConfig(string filePath)
        {
            var json = File.ReadAllText(filePath);
            return JsonConvert.DeserializeObject<Config>(json);
        }

        public static void AddJob(TaskConfig task)
        {
            var timerName = task.Ip + "_" + task.Name;
            if (_timers.ContainsKey(timerName))
            {
                _timers[timerName].Dispose();
                _timers.Remove(timerName);
            }
            var interval = task.Interval * 1000;
            var timer = new Timer(TimerCallback, task, interval, interval);
            _timers.Add(timerName, timer);

            Console.WriteLine($"New Job {task.Name}  {task.Type} {task.Interval}");
        }

        private static void TimerCallback(object state)
        {
            var taskConfig = (TaskConfig)state;
            var timerName = taskConfig.Ip + "_" + taskConfig.Name;
            if (taskConfig.Status == "offline")
            {
                DeleteJob(timerName);
                Console.WriteLine($"{timerName}offline，Stop Job.");
                return;
            }
            var isOffline = false;
            switch (taskConfig.Type)
            {
                case "MSSQL":
                    if (!new DbConnection().CheckMsSqlConnection(taskConfig))
                    {
                        isOffline = true;
                    }
                    else
                    {
                        taskConfig.LastReportTime = DateTime.Now;
                    }
                    break;
                case "MYSQL":
                    if (!new DbConnection().CheckMySqlConnection(taskConfig))
                    {
                        isOffline = true;
                    }
                    else
                    {
                        taskConfig.LastReportTime = DateTime.Now;
                    }
                    break;
                case "REDIS":
                    if (!new DbConnection().CheckRedisConnection(taskConfig))
                    {
                        isOffline = true;
                    }
                    else
                    {
                        taskConfig.LastReportTime = DateTime.Now;
                    }
                    break;
                case "MONGODB":
                    if (!new DbConnection().CheckMongoDbConnection(taskConfig))
                    {
                        isOffline = true;
                    }
                    else
                    {
                        taskConfig.LastReportTime = DateTime.Now;
                    }
                    break;
                case "POSTGRESQL":
                    if (!new DbConnection().CheckPostgreSqlConnection(taskConfig))
                    {
                        isOffline = true;
                    }
                    else
                    {
                        taskConfig.LastReportTime = DateTime.Now;
                    }
                    break;
                case "ORACLE":
                    if (!new DbConnection().CheckOracleConnection(taskConfig))
                    {
                        isOffline = true;
                    }
                    else
                    {
                        taskConfig.LastReportTime = DateTime.Now;
                    }
                    break;
                case "HTTP":
                    if ((DateTime.Now - taskConfig.LastReportTime).TotalSeconds > taskConfig.Timeout)
                    {
                        isOffline = true;
                    }
                    break;
                case "HTTP-Active":
                    {
                        var result = HttpHelper.HttpGetAsync(taskConfig.Data, null, null, TimeSpan.FromSeconds(5)).Result;
                        if (result != null)
                        {
                            taskConfig.LastReportTime = DateTime.Now;
                        }

                        if ((DateTime.Now - taskConfig.LastReportTime).TotalSeconds > taskConfig.Timeout)
                        {
                            isOffline = true;
                        }
                        break;
                    }
            }

            if (isOffline)
            {
                Console.WriteLine($"{timerName} offline，Stop Job.");
                //do some SmsHelper
                //do some EmailHelper
                DeleteJob(timerName);
                taskConfig.Status = "offline";
                return;
            }
        }

        private static void DeleteJob(string name)
        {
            if (_timers.ContainsKey(name))
            {
                _timers[name].Dispose();
                _timers.Remove(name);
                Console.WriteLine($"Deleted job {name}.");
            }
            else
            {
                Console.WriteLine($"Job {name} not found.");
            }
        }

        private static void ListenForCommands()
        {
            while (true)
            {
                var command = Console.ReadLine();
                if (command != null && command.Trim().ToLower() == "reload")
                {
                    Console.WriteLine("read config...");
                    try
                    {
                        CurrentConfig = LoadConfig(_configFilePath);
                        var keys = CurrentConfig.Tasks.Select(s => s.Ip + "_" + s.Name).ToList();
                        foreach (var key in keys)
                        {
                            DeleteJob(key);
                        }

                        foreach (var task in CurrentConfig.Tasks)
                        {
                            AddJob(task);
                        }

                        Console.WriteLine("reload config success.");
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"read conf: {ex.Message}");
                    }
                }
            }
        }
    }
}
