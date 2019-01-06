using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CoroutineHandler : MonoBehaviour {

    public static CoroutineHandler Instance { get { return instance; } }

    static CoroutineHandler instance;

    private void Awake()
    {
        if(instance != null) {
            Destroy(gameObject);
            return;
        }

        instance = this;
    }

    public static void Start(IEnumerator coroutine)
    {
        instance.StartCoroutine(coroutine);
    }
}
