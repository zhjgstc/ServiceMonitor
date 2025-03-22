# Service Monitor 服务监控

## 概述
Service Monitor 是一个用于监控各种服务（如数据库、Redis、MongoDB、HTTP 服务等）状态的系统。它通过定期检查服务的连接状态或可用性，并在服务不可用时采取相应的措施（如停止监控任务、发送短信或邮件通知）。

## 功能特性
- **多服务支持**：支持监控 MSSQL、MySQL、Redis、MongoDB、PostgreSQL、Oracle 和 HTTP 服务。
- **配置灵活**：通过 `config.json` 文件配置监控任务，包括服务类型、监控间隔、超时时间等。
- **定时检查**：使用定时器定期检查服务状态。
- **故障处理**：当服务不可用时，停止监控任务并记录状态。
- **Web 服务**：提供简单的 Web 服务，可通过控制器访问。

## 项目结构
```
ServiceMonitor/
├── Controllers/
│   └── TaskController.cs
├── Logics/
│   ├── DbConnection.cs
│   ├── EmailHelper.cs
│   ├── HttpHelper.cs
│   └── SmsHelper.cs
├── Models/
│   └── TaskConfig.cs
├── Properties/
│   └── launchSettings.json
├── appsettings.Development.json
├── config.json
└── Program.cs
```

## 安装与配置
### 1. 克隆项目
```bash
git clone https://github.com/your-repo/ServiceMonitor.git
cd ServiceMonitor
```

### 2. 配置服务信息
编辑 `config.json` 文件，配置要监控的服务信息。例如：
```json
{
  "tasks": [
    {
      "name": "主数据库",
      "type": "MSSQL",
      "interval": 30,
      "unit": "seconds",
      "ip": "127.0.0.1",
      "data": "data source=127.0.0.1;initial catalog=Your-DatabaseName;user id=YourUserName;password=YourPassword;Encrypt=false;"
    },
    {
      "name": "一个Http服务",
      "type": "HTTP-Active",
      "interval": 10,
      "unit": "seconds",
      "ip": "127.0.0.1",
      "data": "http://localhost:5062",
      "Timeout": 20
    }
  ]
}
```

### 3. 配置邮件和短信服务（可选）
如果需要在服务不可用时发送邮件或短信通知，可以在 `EmailHelper.cs` 和 `SmsHelper.cs` 中配置相应的信息。

### 4. 运行项目
```bash
dotnet run
```

## 使用说明
- 项目启动后，会根据 `config.json` 中的配置信息创建监控任务。
- 每个监控任务会按照指定的间隔时间定期检查服务状态。
- 如果服务不可用，任务将被停止，并记录状态为 `offline`。
- 可以通过 Web 服务访问监控任务的状态（需要在 `TaskController.cs` 中实现相应的接口）。

## 注意事项
- 请确保配置文件中的服务信息（如数据库连接字符串、HTTP 地址等）正确。
- 邮件和短信服务的配置需要根据实际情况进行修改。
- 项目中使用了 `Polly` 库进行超时处理，请确保已安装该库。

## 贡献
欢迎对本项目进行贡献！如果您发现任何问题或有改进建议，请提交 issue 或 pull request。

## 许可证
本项目采用 [MIT 许可证](LICENSE)。

# Service Monitor Service Monitoring System

## Overview
Service Monitor is a system used to monitor the status of various services (such as databases, Redis, MongoDB, HTTP services, etc.). It periodically checks the connection status or availability of services and takes corresponding measures (such as stopping monitoring tasks, sending SMS or email notifications) when a service is unavailable.

## Features
- **Multi-service support**: Supports monitoring of MSSQL, MySQL, Redis, MongoDB, PostgreSQL, Oracle, and HTTP services.
- **Flexible configuration**: Configure monitoring tasks through the `config.json` file, including service type, monitoring interval, timeout period, etc.
- **Periodic checks**: Use timers to periodically check service status.
- **Fault handling**: Stop monitoring tasks and record the status when a service is unavailable.
- **Web service**: Provide a simple web service that can be accessed via controllers.

## Project Structure
```
ServiceMonitor/
├── Controllers/
│   └── TaskController.cs
├── Logics/
│   ├── DbConnection.cs
│   ├── EmailHelper.cs
│   ├── HttpHelper.cs
│   └── SmsHelper.cs
├── Models/
│   └── TaskConfig.cs
├── Properties/
│   └── launchSettings.json
├── appsettings.Development.json
├── config.json
└── Program.cs
```

## Installation and Configuration
### 1. Clone the project
```bash
git clone https://github.com/your-repo/ServiceMonitor.git
cd ServiceMonitor
```

### 2. Configure service information
Edit the `config.json` file to configure the information of the services to be monitored. For example:
```json
{
  "tasks": [
    {
      "name": "Main Database",
      "type": "MSSQL",
      "interval": 30,
      "unit": "seconds",
      "ip": "127.0.0.1",
      "data": "data source=127.0.0.1;initial catalog=Your-DatabaseName;user id=YourUserName;password=YourPassword;Encrypt=false;"
    },
    {
      "name": "An HTTP Service",
      "type": "HTTP-Active",
      "interval": 10,
      "unit": "seconds",
      "ip": "127.0.0.1",
      "data": "http://localhost:5062",
      "Timeout": 20
    }
  ]
}
```

### 3. Configure email and SMS services (optional)
If you need to send email or SMS notifications when a service is unavailable, you can configure the corresponding information in `EmailHelper.cs` and `SmsHelper.cs`.

### 4. Run the project
```bash
dotnet run
```

## Usage Instructions
- After the project starts, monitoring tasks will be created based on the configuration information in `config.json`.
- Each monitoring task will periodically check the service status at the specified interval.
- If a service is unavailable, the task will be stopped and the status will be recorded as `offline`.
- You can access the status of monitoring tasks through the web service (you need to implement the corresponding interfaces in `TaskController.cs`).

## Notes
- Please ensure that the service information in the configuration file (such as database connection strings, HTTP addresses, etc.) is correct.
- The configuration of email and SMS services needs to be modified according to the actual situation.
- The project uses the `Polly` library for timeout handling. Please ensure that the library is installed.

## Contribution
Welcome to contribute to this project! If you find any issues or have suggestions for improvement, please submit an issue or a pull request.

## License
This project is licensed under the [MIT License](LICENSE).

### 主动注册和上报说明
ServiceMonitor支持用户主动注册新的监控任务以及上报任务状态。这一功能允许你灵活添加新的需要监控的服务，或者及时更新现有任务的状态信息。
1. **注册监控任务**：通过向`TaskController`的`Register`接口发起请求来注册新的监控任务。请求需包含一个符合`TaskConfig`模型的JSON数据体，其中`name`字段不能为空，它代表监控任务的名称。例如，使用`curl`命令行工具进行注册：
```bash
curl -X POST -H "Content-Type: application/json" -d '{
    "name": "新的HTTP服务监控",
    "type": "HTTP-Active",
    "interval": 15,
    "unit": "seconds",
    "ip": "192.168.1.100",
    "data": "http://192.168.1.100:8080",
    "Timeout": 30
}' http://localhost:5062/Task/Register
```
上述命令向系统注册了一个新的HTTP服务监控任务。如果注册成功，系统会返回`200 OK`状态码；若`name`字段为空，系统将返回`400 Bad Request`错误码。
2. **上报任务状态**：使用`TaskController`的`Report`接口来上报监控任务的状态。同样需要在请求中携带符合`TaskConfig`模型的JSON数据体，`name`字段不能为空。上报时，系统会根据任务的`ip`和`name`查找对应的任务，并更新其`LastReportTime`（上次报告时间）和`Status`（状态）。若找不到匹配的任务，系统会将其作为新任务进行注册。示例`curl`命令如下：
```bash
curl -X POST -H "Content-Type: application/json" -d '{
    "name": "新的HTTP服务监控",
    "type": "HTTP-Active",
    "interval": 15,
    "unit": "seconds",
    "ip": "192.168.1.100",
    "data": "http://192.168.1.100:8080",
    "Timeout": 30
}' http://localhost:5062/Task/Report
```
通过主动注册和上报功能，你可以更好地管理监控任务，确保系统及时获取服务的最新状态信息。

### Instructions for Active Registration and Reporting
ServiceMonitor supports users to actively register new monitoring tasks and report task statuses. This feature allows you to flexibly add new services to be monitored or update the status information of existing tasks in a timely manner.
1. **Registering Monitoring Tasks**: Register a new monitoring task by sending a request to the `Register` interface of the `TaskController`. The request should include a JSON data body that conforms to the `TaskConfig` model. The `name` field cannot be empty, representing the name of the monitoring task. For example, use the `curl` command - line tool to register:
```bash
curl -X POST -H "Content-Type: application/json" -d '{
    "name": "New HTTP Service Monitoring",
    "type": "HTTP-Active",
    "interval": 15,
    "unit": "seconds",
    "ip": "192.168.1.100",
    "data": "http://192.168.1.100:8080",
    "Timeout": 30
}' http://localhost:5062/Task/Register
```
The above command registers a new HTTP service monitoring task in the system. If the registration is successful, the system will return a `200 OK` status code. If the `name` field is empty, the system will return a `400 Bad Request` error code.
2. **Reporting Task Statuses**: Use the `Report` interface of the `TaskController` to report the status of monitoring tasks. Similarly, a JSON data body conforming to the `TaskConfig` model is required in the request, and the `name` field cannot be empty. When reporting, the system will find the corresponding task based on the task's `ip` and `name` and update its `LastReportTime` (last report time) and `Status` (status). If no matching task is found, the system will register it as a new task. An example `curl` command is as follows:
```bash
curl -X POST -H "Content-Type: application/json" -d '{
    "name": "New HTTP Service Monitoring",
    "type": "HTTP-Active",
    "interval": 15,
    "unit": "seconds",
    "ip": "192.168.1.100",
    "data": "http://192.168.1.100:8080",
    "Timeout": 30
}' http://localhost:5062/Task/Report
```
Through the active registration and reporting functions, you can better manage monitoring tasks and ensure that the system obtains the latest status information of services in a timely manner. 