using System;
using System.Diagnostics;
using System.ServiceProcess;
using System.Timers;

namespace SshService
{
    public static class Program
    {
        public static Timer _Timer { get; private set; }
        public const string ServiceName = "SSH Service";
        public static int IntervalChecks = 0;
        public static int ReopenLimit = Properties.Settings.Default.tryToReopenLimit;
        public static int ReopenTries = 0;
        public static bool RunAsService = true;
        public static Service _service { get; set; }

        // Methods
        public static void CheckStatus()
        {
            bool isOpen = SshHelper.IsOpen;
            if (!isOpen)
            {
                if(IntervalChecks > 0)
                {
                    if (ReopenTries >= ReopenLimit)
                    {
                        if (_service != null)
                        {
                            _service.Stop();
                        }
                        else
                        {
                            Stop();
                        }
                    }
                    else
                    {
                        ReopenTries++;
                        NotifyHelper.SshClosedReopening(ReopenTries, ReopenLimit);
                    }
                }
                try
                {
                    isOpen = SshHelper.OpenConnection();
                    ReopenTries = 0;
                    NotifyHelper.SshOpened();
                }
                catch
                {
                }
            }

            if (isOpen)
            {
                if (!RunAsService)
                    Console.WriteLine("SSH connection open!");
            }
            else
            {
                if (!RunAsService)
                    Console.WriteLine("SSH failed to open.");
            }
        }

        public static void Main(string[] args)
        {
            RunAsService = !Environment.UserInteractive;
            if (RunAsService)
            {
                // running as service
                _service = new Service();
                ServiceBase.Run(_service);
            }
            else
            {
                try
                {
                    // running as console app
                    Console.CancelKeyPress += new ConsoleCancelEventHandler(cancelEventHandler);

                    Start(args);
                }
                catch (Exception ex)
                {
                    Console.WriteLine(ex);
                }
                finally
                {
                    Console.ReadLine();
                }
            }
        }

        public static void Continue()
        {
            if (_Timer == null)
            {
                _Timer = new Timer(Properties.Settings.Default.pollIntervalSeconds * 1000);
                _Timer.AutoReset = true;
                _Timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            }

            if (!_Timer.Enabled)
                _Timer.Enabled = false;
            _Timer.Start();

            if(SshHelper.OpenConnection())
                NotifyHelper.SshOpened();
        }

        public static void Pause()
        {
            if (_Timer != null)
            {
                if (_Timer.Enabled)
                    _Timer.Enabled = false;
                _Timer.Stop();
            }

            SshHelper.CloseConnection();
            NotifyHelper.SshClosed();
        }

        public static void Start(string[] args)
        {
            NotifyHelper.Started();

            // Check status at configured time interval.
            _Timer = new Timer(Properties.Settings.Default.pollIntervalSeconds*1000);
            _Timer.AutoReset = true;
            _Timer.Elapsed += new ElapsedEventHandler(timer_Elapsed);
            _Timer.Enabled = true;
            _Timer.Start();
        }

        public static void Stop()
        {
            try
            {
                if(_Timer != null)
                {
                    if (_Timer.Enabled)
                        _Timer.Enabled = false;
                    _Timer.Stop();
                    _Timer.Dispose();
                }

                if(!RunAsService)
                    Console.WriteLine("Closing SSH connection...");

                SshHelper.CloseConnection();

                if(!RunAsService)
                    Console.WriteLine("SSH connection closed!");

                NotifyHelper.Stopped();
            }
            catch (Exception ex)
            {
                if (!RunAsService)
                    Console.WriteLine(ex);
            }
            finally
            {
                if (!RunAsService)
                    Console.ReadLine();
            }
        }

        // Event Handlers
        public static void cancelEventHandler(object sender, ConsoleCancelEventArgs args)
        {
            Stop();
        }
        private static void timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            CheckStatus();
            IntervalChecks++;
        }
    }

    public class Service : ServiceBase
    {
        public Service()
        {
            ServiceName = Program.ServiceName;
        }

        protected override void OnContinue()
        {
            Program.Continue();
            base.OnContinue();
        }

        protected override void OnPause()
        {
            Program.Pause();
            base.OnPause();
        }

        protected override void OnStart(string[] args)
        {
            try
            {
                Program.Start(args);
                base.OnStart(args);
            }
            catch (Exception ex)
            {
                // If you choose, you can write errors to the Windows Event Log.
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                }
            }
            finally
            {

            }
        }

        protected override void OnStop()
        {
            try
            {
                Program.Stop();
                base.OnStop();
            }
            catch (Exception ex)
            {
                // If you choose, you can write errors to the Windows Event Log.
                using (EventLog eventLog = new EventLog("Application"))
                {
                    eventLog.Source = "Application";
                    eventLog.WriteEntry(ex.Message, EventLogEntryType.Error);
                }
            }
            finally
            {

            }
        }
    }
}
