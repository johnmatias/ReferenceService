using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ReferenceService.DataLayer.Redis
{

	// TODO - add more public methods from RedisHashSet
	public interface IDbHashSet
	{
		bool Initialize();

		void Insert(string hashsetName, string key, Dictionary<string, string> dictionary);
		
		Task InsertAsync(string hashsetName, string key, Dictionary<string, string> dictionary);

		/// <summary>
		/// return true if resource deleted
		/// returns false if resource not found to be deleted
		/// TBD: what to do on failure to delete?
		/// </summary>
		/// <returns></returns>
		bool Delete(string hashsetName, string key);

		/// <summary>
		/// Checks for the existence of a member in the redis-set named hashsetName.  
		/// </summary>
		/// <param name="hashSetName"></param>
		/// <param name="member"></param>
		/// <returns></returns>
		bool Contains(string hashSetName, string member);
	}
}
