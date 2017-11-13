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
	/// TODO figure out why deriving the IManager interface from IHash isn't working
	/// Ideally, resources can derive from IHash when desirable
	/// </summary>
	[Obsolete]
	public interface IHash
	{
		/// <summary>
		/// Name for the hash group.  Only required for Redis
		/// </summary>
		string HashGroupName { get; }
		string CreateKey(string uniqueIdentifier);

		HashSet<string> GetMembers();

		bool TryGet(string name, out Dictionary<string, string> properties);

		bool Contains(string name);

		void Add(string name, Dictionary<string, string> properties);
		
		Task AddAsync(string name, Dictionary<string, string> properties);

		bool TryDelete(string name);
	}
}

