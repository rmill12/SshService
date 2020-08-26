# SshService
SSH Windows Service in C#.NET to persist an SSH connection.

Completely configurable within the app.config settings.

This is a Windows service that starts an SSH connection and attempts to persist that connection.

Using tools like PuTTY can be hard to persist.
This service can be very helpful in cases where you would typically use a tool like PuTTy, but have to manually restart the SSH connection if it gets dropped.
