using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using UnityEngine;

namespace EventsService
{
	public class EventDeliveryData : ICloneable
	{
		[JsonProperty("type")] 
		public string EventType;
		[JsonProperty("data")] 
		public string Data;

		public EventDeliveryData(string type, string data)
		{
			EventType = type;
			Data = data;
		}

		public object Clone()
		{
			return new EventDeliveryData(EventType, Data);
		}
	}
}
