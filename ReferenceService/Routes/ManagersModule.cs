using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using ReferenceService.Data;
using Nancy;

namespace ReferenceService.Resources
{
	
	/// <summary>
	/// On each HTTP action, an instance is instantiated
	/// Nancy sets up routes for all classes deriving from NancyModule
	/// </summary>
    public class ManagersModule : NancyModule
    {
		// TODO - make routes async

		public ManagersModule(IManager managers)
		{
			//Post["/v1/managers/{name}/{longName}/{alias}"] = p => {

			//	HttpStatusCode status = HttpStatusCode.Created;
			//	if (!managers.Contains(p.name))
			//	{
			//		var properties = new Dictionary<string, string>();
			//		properties["longName"] = p.longName;
			//		properties["alias"] = p.alias;

			//		managers.Add(p.name, properties);
			//		//managers.AddAsync(p.name, properties);   TODO - make this POST async
			//	}
			//	else
			//	{
			//		status = HttpStatusCode.Conflict;
			//	}

			//	return status;
			//};

			// On a successful POST to a new resource, return 201 Created
			// On a successful POST to an existing resource, return 200 OK (TBD - consider returning 409 CONFLICT)
			// Else, BadRequest
			Post["/v1/managers/{name}/{*}"] = p => {

				DynamicDictionary x = p;
				HttpStatusCode status = HttpStatusCode.BadRequest;

				#region hackUntilUsingBind
				var yo = x.ToDictionary();
				object args = null;
				string theDynamicDictionaryKey = "";
				if (yo.TryGetValue(theDynamicDictionaryKey, out args))
				{
					string paramString = Convert.ToString(args);
					string[] param = paramString.Split('&');

					if (param.Length > 0)
					{
						var properties = new Dictionary<string, string>();
						foreach (string y in param)
						{
							string[] nv = y.Split('=');
							if (nv.Length == 2)
							{
								properties.Add(nv[0], nv[1]);
							}
							else
							{
								// TODO - failure to parse an argument
							}
						}

						managers.AddAsync(p.name, properties);
						

						// TODO - if resource already exists, consider a 409 Conflict
						// But for now, return a 201 Created, even when the resource already exists
						// Not sure if handling a PUT instead of using POST to perform updates
						// TODO - read up on using POST for updates, or is PUT the only way to go.
						// For things like purchases or orders, sure, a POST always creates a unique resource, but 
						// when we are talking about entries for brokers, managers, take the "CreateOrUpdate" approach to using a POST
						status = HttpStatusCode.Created;
					}
				}
				#endregion

				return status;
			};

			// Re
			Get["/v1/managers"] = _ =>
			{
				return Response.AsJson(managers.GetMembers());
			};

			// TODO - for unit testing, create a generic Get handler that handles all gets
			Get["/v1/managers/{name}"] = parameters =>
			{
				string name = parameters.name;
				Dictionary<string, string> properties;
				if (managers.TryGet(name, out properties))
				{
					return Response.AsJson(properties);
				}
				else
				{
					return HttpStatusCode.NotFound;
				}
			};


			Delete["/v1/managers/{name}"] = p =>
			{
				HttpStatusCode status = HttpStatusCode.NotFound;

				if (managers.Contains(p.name))
				{
					managers.Delete(p.name);
					status = HttpStatusCode.OK;
				}

				return status;
			};
		}		
    };
    
}
