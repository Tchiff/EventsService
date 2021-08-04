using System;
using System.Collections.Generic;
using UnityEngine;

namespace EventsService
{
	public class LocalEventsLoader : IEventsLoader
	{
		private const string KEY_SAVE_DATA = "saved_events_data";
		private const string UNLOKED = "unlocked";

		private readonly string _prefix;
		
		private string _save;

		public LocalEventsLoader(string prefix)
		{
			_prefix = prefix;
		}

		public void SaveEvent(EventDeliveryData eventData)
		{
			var save = LoadData();
			save = string.IsNullOrEmpty(save) 
				? $"{eventData.EventType}_{eventData.Data}_{UNLOKED}"
				: $"{eventData.EventType}_{eventData.Data}_{UNLOKED};{save}";
			SaveData(save);
		}

		public List<EventDeliveryData> LoadEventsAndLock(string lockId, bool isIgnoreLocked)
		{
			if (string.IsNullOrEmpty(lockId))
			{
				throw new ArgumentException($"lockId cannot be empty!");
			}
			
			var events = new List<EventDeliveryData>();
			var save = LoadData();
			var strEvents = save.Split(';');
			string newStr = "";
			for (int i = 0; i < strEvents.Length; i++)
			{
				var props = strEvents[i].Split('_');
				if (props.Length == 3 && (isIgnoreLocked || props[2].Contains(UNLOKED)))
				{
					events.Add(new EventDeliveryData(props[0],props[1]));
					newStr = Combine(newStr, $"{props[0]}_{props[1]}_{lockId}");
				}
				else
				{
					newStr = Combine(newStr, strEvents[i]);
				}
			}
			SaveData(newStr);
			return events;
		}

		public void RemoveEvents(string lockId)
		{
			var save = LoadData();
			var strEvents = save.Split(';');
			string newStr = "";
			for (int i = 0; i < strEvents.Length; i++)
			{
				var props = strEvents[i].Split('_');
				if (props.Length == 3 && (string.IsNullOrEmpty(lockId) || !props[2].Contains(lockId)))
				{
					newStr = Combine(newStr, strEvents[i]);
				}
			}
			SaveData(newStr);
		}

		public void UnlockedEvents(string lockId)
		{
			var save = LoadData();
			var strEvents = save.Split(';');
			string newStr = "";
			for (int i = 0; i < strEvents.Length; i++)
			{
				var props = strEvents[i].Split('_');
				if (props.Length == 3 && (string.IsNullOrEmpty(lockId) || props[2].Contains(lockId)))
				{
					newStr = Combine(newStr, $"{props[0]}_{props[1]}_{UNLOKED}");
				}
				else
				{
					newStr = Combine(newStr, strEvents[i]);
				}
			}
			SaveData(newStr);
		}

		private string Combine(string original, string add)
		{
			return string.IsNullOrEmpty(original) 
				? add
				: $"{original};{add}";
		}

		private string LoadData()
		{
			return string.IsNullOrEmpty(_save) ? PlayerPrefs.GetString(GetSaveKey(), "") : _save;
		}

		private void SaveData(string save)
		{
			_save = save;
			PlayerPrefs.SetString(GetSaveKey(), _save);
		}

		private string GetSaveKey()
		{
			return $"{_prefix}{KEY_SAVE_DATA}";
		}
	}
}