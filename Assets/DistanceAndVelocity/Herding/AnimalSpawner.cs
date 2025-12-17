using UnityEngine;

public class AnimalSpawner : MonoBehaviour
{
    [Header("Spawn Settings")]
    [SerializeField] private GameObject animalPrefab;
    [SerializeField] private int numberOfAnimals = 15;
    [SerializeField] private float spawnRadius = 40f;
    [SerializeField] private Vector3 spawnCenter = Vector3.zero;
    
    [Header("Auto-Create Prefab")]
    [SerializeField] private bool autoCreatePrefab = true;
    [SerializeField] private Material animalMaterial;

    void Start()
    {
        if (animalPrefab == null && autoCreatePrefab)
        {
            animalPrefab = CreateAnimalPrefab();
        }
        
        if (animalPrefab != null)
        {
            SpawnAnimals();
        }
        else
        {
            Debug.LogError("Animal prefab is not assigned! Please assign a prefab or enable auto-create.");
        }
    }

    void SpawnAnimals()
    {
        for (int i = 0; i < numberOfAnimals; i++)
        {
            // Slumpmässig position inom spawn-radien
            Vector2 randomCircle = Random.insideUnitCircle * spawnRadius;
            Vector3 spawnPosition = spawnCenter + new Vector3(randomCircle.x, 0, randomCircle.y);
            
            // Spawna djuret
            GameObject animal = Instantiate(animalPrefab, spawnPosition, Quaternion.identity);
            animal.name = "Animal_" + i;
            
            // Sätt parent till denna spawner för organisation
            animal.transform.parent = transform;
            
            // Sätt slumpmässig rotation
            animal.transform.rotation = Quaternion.Euler(0, Random.Range(0f, 360f), 0);
        }
        
        Debug.Log($"Spawned {numberOfAnimals} animals in the scene!");
    }

    GameObject CreateAnimalPrefab()
    {
        // Skapa ett enkelt djur-prefab programmatiskt
        GameObject prefab = new GameObject("AnimalPrefab");
        
        // Lägg till kropp (Capsule)
        GameObject body = GameObject.CreatePrimitive(PrimitiveType.Capsule);
        body.name = "Body";
        body.transform.parent = prefab.transform;
        body.transform.localPosition = Vector3.zero;
        body.transform.localScale = new Vector3(0.8f, 0.6f, 0.8f);
        body.transform.localRotation = Quaternion.Euler(0, 0, 90);
        
        // Lägg till huvud (Sphere)
        GameObject head = GameObject.CreatePrimitive(PrimitiveType.Sphere);
        head.name = "Head";
        head.transform.parent = prefab.transform;
        head.transform.localPosition = new Vector3(0.7f, 0.3f, 0);
        head.transform.localScale = new Vector3(0.5f, 0.5f, 0.5f);
        
        // Lägg till AnimalBehavior script
        AnimalBehavior behavior = prefab.AddComponent<AnimalBehavior>();
        
        // Applicera material om det finns
        if (animalMaterial != null)
        {
            body.GetComponent<Renderer>().material = animalMaterial;
            head.GetComponent<Renderer>().material = animalMaterial;
        }
        else
        {
            // Skapa ett enkelt vitt material
            Material defaultMaterial = new Material(Shader.Find("Standard"));
            defaultMaterial.color = Color.white;
            body.GetComponent<Renderer>().material = defaultMaterial;
            head.GetComponent<Renderer>().material = defaultMaterial;
        }
        
        return prefab;
    }

    // Visualisera spawn-området i editor
    void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(spawnCenter, spawnRadius);
    }
}
