using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceService.Data
{
	/// <summary>
	/// figure out why 
	/// </summary>
	//public interface IManager : IHash { };

	public interface IManager
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

		bool Delete(string name);
	}
}
