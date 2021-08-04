using System;
using System.Collections.Generic;
using Newtonsoft.Json;
using PnkClientService;

namespace EventsService
{
	public class EventPnkHandler : PnkHandler, IEventPnkHandler
	{
		private class EventsDeliveryData
		{
			[JsonProperty("events")]
			public List<EventDeliveryData> Events;
		}
		
		protected override ISerializer _serializer { get; }

		private string _url;
		private readonly EventsDeliveryData _sendData = new EventsDeliveryData();
		private readonly IEventsLoader _eventsLoader = new LocalEventsLoader("");
		
		private int _sendCount;

		public EventPnkHandler(string url, ISerializer serializer)
		{
			_url = url;
			_serializer = serializer;
			_eventsLoader.UnlockedEvents(string.Empty);
		}

		public void AddEventData(EventDeliveryData data)
		{
			_eventsLoader.SaveEvent(data);
		}

		public void SendAllEvents(Action onError)
		{
			_sendData.Events = _eventsLoader.LoadEventsAndLock(_sendCount.ToString(),false);
			if (_sendData.Events.Count > 0)
			{
				int id = _sendCount;
				SendRequest(_url, _sendData,  response => HandleResponse(response, onError, id));
				_sendCount++;
			}
		}

		private void HandleResponse(IPnkResponse<EmptyClass> response, Action onError, int id)
		{
			if (response.TryGetErrorInfo(out var error))
			{
				_eventsLoader.UnlockedEvents(id.ToString());
				onError?.Invoke();
			}
			else
			{
				_eventsLoader.RemoveEvents(id.ToString());
			}
		}
	}
}