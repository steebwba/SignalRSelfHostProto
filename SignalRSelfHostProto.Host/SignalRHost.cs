using Microsoft.Owin.Hosting;
using System;
using System.ServiceProcess;

namespace SignalRSelfHostProto.Host
{
    public class SignalRHost : ServiceBase
    {
        IDisposable _signalRHost { get; set; }

        static void Main(string[] args)
        {
            if (!Environment.UserInteractive)
            {
                Run(new SignalRHost());
            }
            else
            {
                SignalRHost myServ = new SignalRHost();
                myServ.Start();

                Console.WriteLine("Started SignalR Host");
                Console.ReadLine();
            }
        }

        public void Start()
        {
            _signalRHost = WebApp.Start<Startup>("http://*:8080/");
        }

        public new void Stop()
        {
            _signalRHost.Dispose();
        }
        /// <summary>
        /// Set things in motion so your service can do its work.
        /// </summary>
        protected override void OnStart(string[] args)
        {
            Start();
        }

        /// <summary>
        /// Stop this service.
        /// </summary>
        protected override void OnStop()
        {
            Stop();
        }

        protected override void Dispose(bool disposing)
        {
            base.Dispose(disposing);

            if (_signalRHost != null)
            {
                _signalRHost.Dispose();
                _signalRHost = null;
            }
        }
    }
}
