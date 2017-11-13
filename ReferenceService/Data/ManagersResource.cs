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
	/// Managers Resource.
	/// Currently utilizes stackExchange to persist to redis.
	/// Maybe extended to also persist to MS SQL
	/// </summary>
	public class ManagersResource : IManager
	{
		private static IManager instance;

		public static IManager Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new ManagersResource();
				}
				return instance;
			}
		}

		public ManagersResource()
		{
		}

		public HashSet<string> GetMembers()
		{
			return RedisHashSet.Instance.GetMembers(HashGroupName);
		}

		public HashSet<string> GetMembersAsync()
		{
			return RedisHashSet.Instance.GetMembers(HashGroupName);
		}

		public bool TryGet(string name, out Dictionary<string, string> properties)
		{
			properties = RedisHashSet.Instance.GetInstance(CreateKey(name));
			return properties.Count > 0;
		}

		/// <summary>
		/// This is awful in that state for the managers is kept in two places - in the set and in the hashSet
		/// But, until I figure out how to quickly search members of a resource 
		/// (for instance, all hashsets representing "managers", and not searcing all "hashsets"
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool Contains(string name)
		{
			return RedisHashSet.Instance.Contains(HashGroupName, name);
		}

		public void Add(string name, Dictionary<string, string> properties)
		{
			RedisHashSet.Instance.Insert(HashGroupName, name, properties);
		}

		public async Task AddAsync(string name, Dictionary<string, string> properties)
		{			
			await RedisHashSet.Instance.InsertAsync(HashGroupName, name, properties);
		}

		/// <summary>
		/// return false if resource dNote, this doesn't attempt to distinguish 
		/// </summary>
		/// <param name="name"></param>
		/// <returns></returns>
		public bool Delete(string member)
		{
			return RedisHashSet.Instance.Delete(HashGroupName,member);
		}

		public string HashGroupName { get { return "manager"; } }

		public string CreateKey(string uniqueIdentifier)
		{
			return HashGroupName + ":" + uniqueIdentifier;
		}
	}

}

