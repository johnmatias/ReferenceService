using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using StackExchange.Redis;

namespace ReferenceService.DataLayer.Redis
{
	// https://stackexchange.github.io/StackExchange.Redis/Basics
	// https://github.com/StackExchange/StackExchange.Redis/blob/master/docs/Basics.md
	
	public class RedisHashSet : IDbHashSet
	{
		private ConnectionMultiplexer redis;
		IDatabase db;

		private static RedisHashSet instance;

		public static RedisHashSet Instance
		{
			get
			{
				if (instance == null)
				{
					instance = new RedisHashSet();
				}
				return instance;
			}
		}

		public RedisHashSet()
		{
			Initialize();
		}

		public bool Initialize()
		{
			// connect to the default port
			redis = ConnectionMultiplexer.Connect("localhost");
			db = redis.GetDatabase();
			return true;
		}

		// Inserting results in persisting into a redis-set and into a redis-hashset
		// for instance, if inserting a "manager" named "JM" with attribute isNice=true
		// then this is created
		// set
		// --manager { "JM" }				// a set named "manager" containing member "JM"
		// hashset
		// -- manager:JM { {isNice:true} }  // hashset named "manager:JM" containing the attribute name value pair
		//
		// hmset <key> [field value ...]

		public async Task InsertAsync(string hashsetName, string key, Dictionary<string, string> dictionary)
		{
			var fields = Convert(dictionary);

			await db.SetAddAsync(hashsetName, key);
			await db.HashSetAsync(CreateKey(hashsetName,key), fields);
		}

		public void Insert(string hashsetName, string key, Dictionary<string, string> dictionary)
		{
			var fields = dictionary
				.Select(pair => new HashEntry(pair.Key, pair.Value))
				.ToArray();

			db.SetAdd(hashsetName, key);
			db.HashSet(CreateKey(hashsetName, key), fields);
		}

		public bool Delete(string hashsetName, string key)
		{
			db.SetRemove(hashsetName, key);
			
			// TODO - I want to delete *all* values, but the existing API requires that fields are returned.
			// But, it doesn't seem correct that I need to perform a read to get all fields in order to remove them...
			db.HashDelete(CreateKey(hashsetName, key), String.Empty);

			return true;  // Lame, but until I figure out delete.
		}

		/// <summary>
		/// Note, state is persisted into a redis-set and into a redis-hashset
		/// </summary>
		/// <param name="hashSetName"></param>
		/// <param name="member"></param>
		/// <returns></returns>
		public bool Contains(string hashSetName, string member)
		{
			return db.SetContains(hashSetName, member);
			//return db.HashValues(hashSetName).Contains(member);
		}

		public HashSet<string> GetMembers(string hashSetName)
		{
			var result = new HashSet<string>();
			var members = db.SetMembers(hashSetName);
			foreach (var x in members)
			{
				result.Add(x.ToString());
			}

			return result;
		}

		public async Task<HashSet<string>> GetMembersAsync(string hashSetName)
		{
			var result = new HashSet<string>();
			var members = await db.SetMembersAsync(hashSetName);
			return Convert(members);			
		}

		public Dictionary<string, Dictionary<string, string>> GetEverything(string hashSetName)
		{
			var all = new Dictionary<string, Dictionary<string, string>>();
			
			#region DoesntWork
			//var u1 = db.HashValues("manager");
			//var u2 = db.HashValues("manager:");
			//var u3 = db.HashValues("manager:john");
			//var a1 = db.HashScan("manager");
			//var a21 = db.HashScan("manager:");
			//var a201 = db.HashScan("0");
			//var a31 = db.HashScan("manager:john");
			//var aa1 = db.HashScan(string.Empty);
			//var aa21 = db.HashScan(string.Empty, "manager");
			//var aa31 = db.HashScan(string.Empty, "manager:john");
			//var k1 = db.HashKeys("manager");
			//var k2 = db.HashKeys("manager:");
			//var k3 = db.HashKeys("manager:john");

			//var xx = db.HashScan(string.Empty, pattern: "manager*");
			//var x1 = db.HashScan("0", pattern: "manager*");

			//var uniqueIdentifiers = db.HashValues(hashSetName);
			//var uniqueIdentifiers = db.HashValues(hashSetName);
			//var uniqueIdentifiers2 = db.HashKeys(hashSetName+ ":");
			#endregion

			// Until I can get SCAN or HSCAN to work, check the members of the set of the same namespace
			var uniqueIdentifiers = db.SetMembers(hashSetName);

			// TODO - there has got to be a batch method for this
			foreach (var val in uniqueIdentifiers)
			{
				var prop = GetInstance(CreateKey(hashSetName,val));
				all.Add(val, prop);
			}

			return all;
		}

		public Dictionary<string, string> GetInstance(string key)
		{
			var all = db.HashGetAll(key);

			Dictionary<string, string> x = new Dictionary<string, string>();
			foreach (var he in all)
			{
				x.Add(he.Name, he.Value);
			}

			return x;
		}


		private string CreateKey(string hashsetName, string key)
		{
			return hashsetName + ":" + key;
		}

		private HashEntry[] Convert(Dictionary<string, string> dictionary)
		{
			var fields = dictionary
						.Select(pair => new HashEntry(pair.Key, pair.Value))
						.ToArray();
			return fields;
		}

		private HashSet<string> Convert(RedisValue[] rv)
		{
			var result = new HashSet<string>();
			foreach (var member in rv)
			{
				result.Add(member.ToString());
			}
			return result;
		}

	}




}
