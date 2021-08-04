using System;
using System.Text;
using EventsService;
using UnityEngine.Networking;

namespace PnkClientService
{
	public abstract class PnkHandler
	{
		public class EmptyClass
		{
		}
		
		protected abstract ISerializer _serializer { get; }
		
		protected void SendRequest<TRequestType, T>(string url, TRequestType data, Action<IPnkResponse<T>> callback,
			string webRequestMethod = UnityWebRequest.kHttpVerbPOST) where T : new()
		{
			byte[] byteData = GetByteData(data);
			var request = new PnkRequest<T>(url, byteData, webRequestMethod, _serializer);
			request.Send(callback);
		}
		
		protected void SendRequest<TRequestType>(string url, TRequestType data, Action<IPnkResponse<EmptyClass>> callback,
			string webRequestMethod = UnityWebRequest.kHttpVerbPOST)
		{
			byte[] byteData = GetByteData(data);
			var request = new PnkRequest<EmptyClass>(url, byteData, webRequestMethod, _serializer);
			request.Send(callback);
		}

		private byte[] GetByteData<T>(T data)
		{
			string str = _serializer.Serialize(data);
			return Encoding.UTF8.GetBytes(str);
		}
		
	}
}