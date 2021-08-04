namespace EventsService
{
	public interface ISerializer
	{
		string Serialize<T>(T obj);
		T Deserialize<T>(string str);
		string GetContentTypeHeader();
	}
}