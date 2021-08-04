using UnityEngine;

public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
	private static T _instance = null;

	public static T Instance
	{
		get
		{
			if (_instance != null)
			{
				return _instance;
			}

			return CreateSingleton();
		}
	}

	protected void Awake()
	{
		if (_instance == null)
		{
			_instance = GetComponent<T>();
			DontDestroyOnLoad(_instance.gameObject);
			Init();
		}
		else if (this != _instance)
		{
			Destroy(gameObject);
		}
	}

	protected virtual void Init()
	{
	}

	private static T CreateSingleton()
	{
		var obj = new GameObject(typeof(T).ToString());
		return obj.AddComponent<T>();
	}
}

public abstract class SingletonMonoBehaviour<IT, T> : SingletonMonoBehaviour<T> where T : MonoBehaviour, IT
{
	public new static IT Instance => SingletonMonoBehaviour<T>.Instance;
}