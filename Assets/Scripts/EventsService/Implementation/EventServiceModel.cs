using UnityEngine;

namespace EventsService
{
	public class EventServiceModel : MonoBehaviour, IEventServiceModel
	{
		[SerializeField] private int _cooldownBeforeSend;
		[SerializeField] private string _urlAnalyticsServer;

		private IEventPnkHandler _pnkHandler;
		private float _timer;

		private void Awake()
		{
			_pnkHandler = new EventPnkHandler(_urlAnalyticsServer, new JsonSerializer());
			_pnkHandler.SendAllEvents(TryUpdateTimer);
		}

		private void Update()
		{
			if (_timer <= 0)
			{
				return;
			}

			_timer -= Time.deltaTime;
			if (_timer <= 0)
			{
				_pnkHandler.SendAllEvents(TryUpdateTimer);
			}
		}

		public void TrackEvent(string type, string data) 
		{
			_pnkHandler.AddEventData(new EventDeliveryData(type, data));
			TryUpdateTimer();
		}
		
		private void TryUpdateTimer()
		{
			if (_cooldownBeforeSend <= 0)
			{
				_pnkHandler.SendAllEvents(TryUpdateTimer);
				return;
			}
			
			if (_timer <= 0)
			{
				_timer = _cooldownBeforeSend;
			}
		}
	}
}
