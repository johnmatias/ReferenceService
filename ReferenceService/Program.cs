
 using System;
    using Topshelf;

namespace ReferenceService
{
    public class Program
    {
        public static void Main()
        {
            HostFactory.Run(x =>
            {
                x.UseLinuxIfAvailable();
                x.Service<NancySelfHost>(s =>
                {
                    s.ConstructUsing(name => new NancySelfHost());
                    s.WhenStarted(tc => tc.Start());
                    s.WhenStopped(tc => tc.Stop());
                });

                x.RunAsLocalSystem();
                x.SetDescription("Nancy-SelfHost example");
                x.SetDisplayName("Nancy-SelfHost Service");
                x.SetServiceName("Nancy-SelfHost");
            });
        }
    }
}