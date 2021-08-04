using System.Collections.Generic;

namespace EventsService
{
	public interface IEventsLoader
	{
		void SaveEvent(EventDeliveryData eventData);
		List<EventDeliveryData> LoadEventsAndLock(string lockId, bool isIgnoreLocked);
		void RemoveEvents(string lockId);
		void UnlockedEvents(string lockId);
	}
}