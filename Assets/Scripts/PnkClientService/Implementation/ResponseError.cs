namespace PnkClientService
{
	public class ResponseError : IResponseError
	{
		public string Massage { get; }
		public long Code { get; }

		public ResponseError(long code, string massage)
		{
			Code = code;
			Massage = massage;
		}
	}
}