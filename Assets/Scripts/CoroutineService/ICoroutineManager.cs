using System.Collections;
using UnityEngine;

public interface ICoroutineManager
{
	Coroutine StartCoroutine(IEnumerator coroutine);
	void StopCoroutine(IEnumerator coroutine);
	void StopAllCoroutines();
}