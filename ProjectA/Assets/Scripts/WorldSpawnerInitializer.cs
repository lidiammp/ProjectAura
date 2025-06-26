using System.Collections;
using System.Collections.Generic;
using UnityEngine;



public class WorldSpawnerInitializer : MonoBehaviour
{
    public Transform worldRootTransform;
    // Start is called before the first frame update
    void Start()
    {
        WorldSpawner.worldRoot = worldRootTransform;
    }

}

public static class WorldSpawner
{
    public static Transform worldRoot;

    public static GameObject Spawn(GameObject prefab, Vector3 position)
    {
        GameObject obj = GameObject.Instantiate(prefab, position, Quaternion.identity);
        if (worldRoot != null)
            obj.transform.SetParent(worldRoot, true);
        return obj;
    }
}
