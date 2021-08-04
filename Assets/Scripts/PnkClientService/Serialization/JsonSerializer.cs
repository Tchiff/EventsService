using System;
using System.Collections.Generic;
using System.Text;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using Newtonsoft.Json.Serialization;

namespace EventsService
{
	public class JsonSerializer : ISerializer
	{
		private static JsonSerializerSettings _jsonSerializeSettings;
		private JsonSerializerSettings JsonSerializeSettings
		{
			get
			{
				if (_jsonSerializeSettings == null)
				{
					_jsonSerializeSettings = new JsonSerializerSettings
					{
						ContractResolver = new DefaultContractResolver()
					};
				}
				return _jsonSerializeSettings;
			}
		}

		private string ObjectToJsonString(object obj)
		{
			return JsonConvert.SerializeObject(obj, Formatting.None, JsonSerializeSettings);
		}
		
		public string Serialize<T>(T obj)
		{
			if (obj != null)
			{
				return ObjectToJsonString(obj);
			}
			throw new AggregateException($"json is Empty!");
		}
		
		public T Deserialize<T>(string str)
		{
			return JsonConvert.DeserializeObject<T>(str, JsonSerializeSettings);
		}

		public string GetContentTypeHeader()
		{
			return "application/json";
		}
	}
}