# SSH Service
Tired of PuTTy dropping connections, and not restarting automatically after a server reboot?

This is a console app that doubles as a Windows Service in C#.NET to persist an SSH connection.  It attempts reconnection automatically if a connection drops, and by nature of Windows Services, it starts automatically at Windows startup.

Most properties are configurable within the config settings.

My experience has been that using tools like PuTTY can be hard to persist. Server reboots often require manual opening and starting SSH connections.
This service can be very helpful in cases where you would typically use a tool like PuTTy, but have to manually restart the SSH connection if it gets dropped.

Please feel free to use this tool, and expand on it where necessary.

NOTE: Email functionality purposely left out as devs can add their own email logic as they see fit.

# Configuration
host: The server you're connecting to.<br/>
user: The username you're using to connect.<br/>
pass: The password you're using to connect.<br/>
port: The port number you're using to connect to the host.<br/>
privateKeyFile: The path to the key file if you're using one.<br/>
forwardedPort: The forwarded port number.<br/>
forwardedAddress: The forwarded IP address.<br/>
pollIntervalSeconds: The interval (in seconds) at which the service checks the SSH connection.<br/>
toEmails: The list of email addesses that will receive notifications when the services starts or ends, or when the SSH connection opens or closes.<br/>
tryToRepoenLimit: Number of times the service will attempt to reopen the SSH connection if it drops.
