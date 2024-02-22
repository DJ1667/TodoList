using UnityEngine;
using System.Collections;

public abstract class MonoSingletonBase <T> : MonoBehaviour where T :  MonoBehaviour
{
	private static T instance;
	
	public static T Instance
	{
		get
		{
			if(instance == null)
			{
				instance = GameObject.FindObjectOfType<T>();
				if(instance == null)
				{
					GameObject instanceObj = new GameObject();
					instanceObj.name = "(Singleton)" + typeof(T).ToString();
					instance = instanceObj.AddComponent<T>();
				}
			}
			
			return instance;
		}
	}
}