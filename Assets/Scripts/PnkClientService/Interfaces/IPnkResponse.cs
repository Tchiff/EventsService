namespace PnkClientService
{
	public interface IPnkResponse<T>
	{
		T Data { get; }
		bool TryGetErrorInfo(out IResponseError error);
	}
}