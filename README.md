# SshService
SSH Windows Service to persist an SSH connection

This is a Windows service that starts an SSH connection and attempts to persist that connection.

This can be very helpful in cases where you typically use a tool (ie: PuTTy) to do this for you.
The issue with tools like PuTTy is that it's hard to automatically start the connection if a server restarts, or the connection gets dropped.

Using a Windows service has helped us.

We send emails from the service to inform us that the SSH connection has dropped, or the service is having issues.
