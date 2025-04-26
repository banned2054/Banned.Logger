# Banned.Logger

一个简单、高效的 .NET 日志记录库，支持文件日志和日志轮转功能。

## 功能特性

- 📝 支持多种日志级别（Debug、Info、Warning、Error）
- 📂 按月份和日期自动组织日志文件
- 🔄 自动日志轮转，防止单个文件过大
- 🧹 自动清理过期日志文件
- 🎨 支持自定义日志格式
- 💻 支持控制台输出
- 🔒 线程安全设计
- ⚡ 异步写入，不影响主程序性能

## 安装

```bash
dotnet add package Banned.Logger
```

## 快速开始

```csharp
using Banned.Logger;

// 创建日志记录器并配置
var logger = new Logger(options =>
{
    options.Name = "MyApp";
    options.BaseDirectory = "logs";
    options.WriteOnConsole = true;
    options.MinimumLevel = LogLevel.Info;
    options.MaxFileSize = 5 * 1024 * 1024;  // 5MB
    options.MaxRetainedFiles = 7;           // 保留7天的日志
    options.LogFormat = "[{timestamp}] [{level}] {name}: {message}";
});

// 异步记录日志
await logger.InfoAsync("这是一条信息日志");
await logger.WarningAsync("这是一条警告日志");
await logger.ErrorAsync("这是一条错误日志");
```

## 配置选项

| 选项 | 类型 | 默认值 | 说明 |
|------|------|--------|------|
| Name | string | "" | 日志记录器名称 |
| BaseDirectory | string | "" | 日志文件基础目录 |
| WriteOnConsole | bool | false | 是否同时输出到控制台 |
| MinimumLevel | LogLevel | LogLevel.Debug | 最低日志级别 |
| MaxFileSize | long | 10MB | 单个日志文件最大大小 |
| MaxRetainedFiles | int | 30 | 日志文件保留天数 |
| LogFormat | string | "{timestamp:yyyy-MM-dd HH:mm:ss} - {level} - {name} - {message}" | 日志格式模板 |

## 日志格式

日志格式支持以下占位符：
- `{timestamp}`: 时间戳
- `{level}`: 日志级别
- `{name}`: 记录器名称
- `{message}`: 日志消息

## 日志文件组织

日志文件按以下结构组织：
```
{BaseDirectory}/{yyyy-MM}/{yyyy-MM-dd}.log
```

例如：
```
logs/
  ├── 2024-03/
  │   ├── 2024-03-15.log
  │   └── 2024-03-15_153045.log  (轮转文件)
  └── 2024-04/
      └── 2024-04-01.log
```

## 日志轮转

当日志文件达到 `MaxFileSize` 大小时，会自动进行轮转：
1. 当前文件被重命名为 `{日期}_{时间戳}.log`
2. 创建新的日志文件继续写入

## 注意事项

1. 确保应用程序对日志目录有写入权限
2. 建议根据实际需求调整 `MaxFileSize` 和 `MaxRetainedFiles` 参数
3. 日志轮转和清理操作是异步的，不会阻塞主程序
4. 所有日志操作都是异步的，调用日志方法时需要使用 `await`

## 许可证

MIT License 