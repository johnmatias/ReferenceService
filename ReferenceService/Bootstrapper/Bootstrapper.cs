using Nancy;
using Nancy.Bootstrapper;
using Nancy.TinyIoc;
using ReferenceService.Data;

namespace ReferenceService.Bootstrapper
{
	public class Bootstrapper : DefaultNancyBootstrapper
	{
		protected override void ApplicationStartup(TinyIoCContainer container,
												   Nancy.Bootstrapper.IPipelines pipelines)
		{
			// For production, doesn't matter if singleton or perRequestSingleton, as state is kept
			// in the persistance store, but, more efficient to use a singleton.  
			// No need for per request instances
			container.Register<IManager, ManagersResource>().AsSingleton();
		}
	}
}

