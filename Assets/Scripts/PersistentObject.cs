using UnityEngine;
using System.Collections.Generic;

public class PersistentObject : MonoBehaviour
{
    [SerializeField] private string persistentId;

    private static Dictionary<string, GameObject> persistentInstances = new Dictionary<string, GameObject>();

    private void Awake()
    {
        if (string.IsNullOrEmpty(persistentId))
        {
            persistentId = gameObject.name;
        }

        if (persistentInstances.ContainsKey(persistentId))
        {
            if (persistentInstances[persistentId] != this.gameObject)
            {
                Destroy(gameObject);
            }
        }
        else
        {
            persistentInstances[persistentId] = this.gameObject;
            DontDestroyOnLoad(gameObject);
        }
    }
}
