using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IUpdatable
{
    void Update();
}

public class UpdateHandler : MonoBehaviour {

    static UpdateHandler instance;

    List<IUpdatable> list = new List<IUpdatable>();
    bool isDirty;

    private void Awake()
    {
        if(instance != null) {
            Destroy(gameObject);
            return;
        }
        instance = this;
        isDirty = false;
    }

    // Update is called once per frame
    void Update () {
        for (int i = 0; i < list.Count; i++) {
            if (list[i] == null)
            {
                isDirty = true;
                continue;
            }
            list[i].Update();
        }
	}

    public void RefreshList()
    {
        if (isDirty)
        {
            int count = list.RemoveAll(i => i == null);
            Debug.LogFormat("Removed {0} objects from list.", count);
            isDirty = false;
        }
    }

    public static void AddUpdateObject(IUpdatable obj)
    {
        instance.list.Add(obj);
    }

    public static void RemoveUpdateObject(IUpdatable obj)
    {
        instance.list.Remove(obj);
    }
}
