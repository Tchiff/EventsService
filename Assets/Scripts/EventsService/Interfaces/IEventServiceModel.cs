namespace EventsService
{
	public interface IEventServiceModel
	{
		void TrackEvent(string type, string data);
	}
}