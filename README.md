# Service Monitor 服务监控系统

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