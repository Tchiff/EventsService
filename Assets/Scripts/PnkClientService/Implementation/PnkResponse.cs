namespace PnkClientService
{
	public class PnkResponse<T> : IPnkResponse<T>
	{
		public T Data { get; private set; }
		private IResponseError _error;

		public bool TryGetErrorInfo(out IResponseError error)
		{
			error = _error;
			return _error != null;
		}
		
		public void SetData(T data)
		{
			Data = data;
		}

		public void SetError(IResponseError error)
		{
			_error = error;
		}
	}
}