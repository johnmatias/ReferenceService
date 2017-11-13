using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using StackExchange.Redis;
using ReferenceService.DataLayer.Redis;

namespace ReferenceService.Data
{

	/// <summary>
	/// This stub uses a .NET Dictionary to store state
	/// It can be used to test HTTP routing as it doesn't rely on a persistance story
	/// </summary>
	public class ManagersResourceStub : IManager
	{
		private Dictionary<string, Dictionary<string, string>> data;
		private static IManager instance;

		public static IManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ManagersResourceStub();
				}
				return instance;
			}
		}

		public ManagersResourceStub()
		{
			data = new Dictionary<string, Dictionary<string, string>>();
		}

		public HashSet<string> GetMembers()
		{
			var result = new HashSet<string>();
			data.Keys.Select(keys => result.Add(keys));
			return result;
		}
		public Dictionary<string, Dictionary<string, string>> GetAll()
		{
			return new Dictionary<string, Dictionary<string, string>>(data);
		}

		public bool TryGet(string name, out Dictionary<string, string> properties)
		{
			properties = null;
			return data.TryGetValue(name, out properties);
		}

		public bool Contains(string name)
		{
			return data.ContainsKey(name);
		}

		public void Add(string name, Dictionary<string, string> properties)
		{
			data[name] = new Dictionary<string, string>(properties);
		}

		public async Task AddAsync(string name, Dictionary<string, string> properties)
		{
			Task x = Task.Run( () => {
				data[name] = new Dictionary<string, string>(properties);
			});

			await x;
		}

		public bool Delete(string name)
		{
			return data.Remove(name);
		}

		public string HashGroupName { get { return "manager"; } }

		public string CreateKey(string uniqueIdentifier)
		{
			return HashGroupName + ":" + uniqueIdentifier;
		}
	}

}

