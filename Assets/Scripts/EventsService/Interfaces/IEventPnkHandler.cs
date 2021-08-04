using System;

namespace EventsService
{
	public interface IEventPnkHandler
	{
		void AddEventData(EventDeliveryData data);
		void SendAllEvents(Action onError);
	}
}