using System.Collections;
using UnityEngine;

public class CoroutineManager : SingletonMonoBehaviour<ICoroutineManager, CoroutineManager>, ICoroutineManager
{
	public new Coroutine StartCoroutine(IEnumerator coroutine)
	{
		return base.StartCoroutine(coroutine);
	}

	public new void StopCoroutine(IEnumerator coroutine)
	{
		base.StopCoroutine(coroutine);
	}

	public new void StopAllCoroutines()
	{
		base.StopAllCoroutines();
	}
}
