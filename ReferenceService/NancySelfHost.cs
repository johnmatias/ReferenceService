
  using System;
    using System.Diagnostics;
    using Nancy.Hosting.Self;

 namespace ReferenceService
{
    public class NancySelfHost
    {
        private NancyHost m_nancyHost;

        public void Start()
        {

            var hostConfiguration = new HostConfiguration
            {
                UrlReservations = new UrlReservations() { CreateAutomatically = true }
            };

            m_nancyHost = new NancyHost(hostConfiguration, new Uri("http://localhost:5000"));
            m_nancyHost.Start();

        }

        public void Stop()
        {
            m_nancyHost.Stop();
            Console.WriteLine("Stopped. Good bye!");
        }
    }
}