# SSH Service
Tired of PuTTy dropping connections, and not restarting automatically after a server reboot?

This is a console app that doubles as a Windows Service in C#.NET to persist an SSH connection.  It attempts reconnection automatically if a connection drops, and by nature of Windows Services, it starts automatically at Windows startup.

Most properties are configurable within the config settings.

My experience has been that using tools like PuTTY can be hard to persist. Server reboots often require manual opening and starting SSH connections.
This service can be very helpful in cases where you would typically use a tool like PuTTy, but have to manually restart the SSH connection if it gets dropped.

Please feel free to use this tool, and expand on it where necessary.

NOTE: Email functionality purposely left out as devs can add their own email logic as they see fit.
