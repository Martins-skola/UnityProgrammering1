using UnityEngine;

public class SpawnWithRigidBody : MonoBehaviour
{
    public Transform spawnpoint;
    public GameObject prefabToSpawn;
    void Start()
    {
        if(spawnpoint == null)
        {
            Debug.LogError("Du har inte angiviet ett (tomt) gameobjekt som ska fungera som spawnpoint.");
        }
        if (prefabToSpawn == null)
        {
            Debug.LogError("Du har inte angivet vilken prefab somn ska spawnas in.");
        }
    }

    public void Spawn()
    {
        GameObject obj = Instantiate(prefabToSpawn, spawnpoint.position, spawnpoint.rotation);
        if (obj.GetComponent<Rigidbody>() == null)
        {
            obj.AddComponent<Rigidbody>();
        }
        if (obj.GetComponent<DestroyOnClick>() == null)
        {
            obj.AddComponent<DestroyOnClick>();
        }
    }
}
