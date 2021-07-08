using Renci.SshNet;
using System;
using System.IO;
using System.Text;

namespace SshService
{
    public static class SshHelper
    {
        private static SshClient _Client { get; set; }

        public static bool IsOpen
        {
            get
            {
                bool result = false;
                try
                {
                    result = _Client != null && _Client.IsConnected;
                }
                catch
                {
                    // Nothing, the _Client should be disposed of if this errors out.
                    // So result is still 'false'!
                }
                return result;
            }
        }

        public static void CloseConnection()
        {
            try
            {
                if(_Client != null)
                {
                    if(_Client.IsConnected)
                        _Client.Disconnect();
                    _Client.Dispose();
                }
            }
            catch
            {
                // Nothing, the _Client should be disposed of if this errors out.
            }
        }

        public static bool OpenConnection()
        {
            string user = Properties.Settings.Default.user;
            string pass = Properties.Settings.Default.pass;  // This isn't always needed. Might depend on your environment.
            string host = Properties.Settings.Default.host;
            int port = Properties.Settings.Default.port;
            string privateKeyFilePath = Properties.Settings.Default.privateKeyFile;
            ForwardedPortLocal forwardedPortLocal = null;

            var connInfo = new ConnectionInfo(host, port, user,
                new AuthenticationMethod[]
                {
                    new PrivateKeyAuthenticationMethod(user, new PrivateKeyFile[]{ new PrivateKeyFile(privateKeyFilePath, pass), }),
                });
            
            try
            {
                _Client = new SshClient(connInfo);

                // Could be configurable.
                int keepAliveIntervalSeconds = 5;
                _Client.KeepAliveInterval = new TimeSpan(0, 0, keepAliveIntervalSeconds);

                // Could be configurable.
                int connectionTimeoutSeconds = 20;
                _Client.ConnectionInfo.Timeout = new TimeSpan(0, 0, connectionTimeoutSeconds);
                _Client.Connect();

                if(_Client.IsConnected)
                {
                    var _forwardedPort = Properties.Settings.Default.forwardedPort;
                    var _forwardedAddress = Properties.Settings.Default.forwardedAddress;
                    forwardedPortLocal = new ForwardedPortLocal(_forwardedAddress, uint.Parse(_forwardedPort.ToString()), _forwardedAddress, uint.Parse(_forwardedPort.ToString()));
                    _Client.AddForwardedPort(forwardedPortLocal);
                    forwardedPortLocal.Start();
                }

                return true;
            }
            catch(Exception ex)
            {
                if (forwardedPortLocal != null && forwardedPortLocal.IsStarted)
                {
                    forwardedPortLocal.Stop();
                    forwardedPortLocal.Dispose();
                }
                CloseConnection();
                throw ex;
            }
        }
    }
}
