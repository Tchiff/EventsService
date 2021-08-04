using System;
using System.Collections;
using EventsService;
using UnityEngine;
using UnityEngine.Networking;

namespace PnkClientService
{
	public class PnkRequest<T>
		where T : new()
	{
		private readonly string _url;
		private readonly byte[] _data;
		private readonly string _webRequestMethod;
		private readonly ISerializer _serializer;
		
		private Action<PnkResponse<T>> _responseCallback;

		public PnkRequest(string url, byte[] data, string webRequestMethod, ISerializer serializer)
		{
			_url = url;
			_data = data;
			_webRequestMethod = webRequestMethod;
			_serializer = serializer;
		}
		
		public void Send(Action<PnkResponse<T>> responseCallback = null)
		{
			_responseCallback = responseCallback;
			CoroutineManager.Instance.StartCoroutine(Request());
		}

		private IEnumerator Request()
		{
			using (UnityWebRequest www = new UnityWebRequest(_url, _webRequestMethod))
			{
				www.SetRequestHeader("Content-Type", _serializer.GetContentTypeHeader());
				www.downloadHandler = new DownloadHandlerBuffer();
				if (_webRequestMethod != UnityWebRequest.kHttpVerbGET)
				{
					www.uploadHandler = new UploadHandlerRaw(_data);
				}

				yield return www.SendWebRequest();
				
				string wwwError = www.error;
				string wwwText = www.downloadHandler.text;
				long code = www.responseCode;

				bool isError = code != 200;
				bool isResponse = !string.IsNullOrEmpty(wwwText);

				var response = new PnkResponse<T>();
				if (isError)
				{
					string message = $"Request: code = {code} for url {_url}\n{wwwError}";
					response.SetError(new ResponseError(code, message));
					Debug.LogError(message);
				}
				else if(isResponse)
				{
					try
					{
						var responseData = _serializer.Deserialize<T>(wwwText);
						response.SetData(responseData);
					}
					catch (Exception e)
					{
						string message = $"Cannot parse response: {wwwText}!\n{e.Message}";
						response.SetError(new ResponseError(code, message));
						Debug.LogError(message);
					}
				}
				
				_responseCallback(response);
			}
		}
	}
}