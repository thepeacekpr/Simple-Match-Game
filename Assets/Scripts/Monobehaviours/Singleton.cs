using System.Collections;
using System.Collections.Generic;

using UnityEngine;

public class Singleton<T> : MonoBehaviour
{
    public static T Instance;

    private void Awake()
    {
        if (Instance != null)
        {
            Debug.Log($"Instance already assigned");
            return;
        }

        Instance ??= gameObject.GetComponent<T>();
    }

    private void OnDestroy()
    {
        if (Instance != null)
        {
            Destroy(gameObject);
        }
    }
}
