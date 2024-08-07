# BannedLogger

Create log file by times, every log file is like [TargetDirectory]/yyyy-MM/yyyy-MM-dd.log.

And all output looks like python log, it should set logger's name like this(controller):

```bash
2024-08-08 03:50:00 - controller - Debug - test log
```

Default is not write on console, but you can set:

```c#
var logger = new Logger("test", "log", true);
```