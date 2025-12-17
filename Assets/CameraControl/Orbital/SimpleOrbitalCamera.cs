using UnityEngine;

/// <summary>
/// En orbital kamera som roterar runt ett angivet objekt.
/// Kontrolleras med mus för rotation.
/// </summary>
public class SimpleOrbitalCamera : MonoBehaviour
{
    [Tooltip("Objektet som kameran ska rotera runt")]
    public Transform target;

    [Tooltip("Rotationshastighet för musrörelse")]
    [Range(0.1f, 10f)]
    public float rotationSpeed = 2f;

    [Tooltip("Standardavstånd till målet")]
    [Range(1f, 50f)]
    public float distance = 10f;
    
    [Tooltip("Höjdvinkel i grader")]
    [Range(-89f, 89f)]
    public float elevation = 30f;
    
    // Privata variabler för rotation
    private float currentRotation = 0f;


    void Start()
    {             
        // Om inget mål är angivet, skapa ett varningsmeddelande
        if (target == null)
        {
            Debug.LogWarning("OrbitalCamera: Inget mål angivet! Ställ in 'target' i Inspector.");
        }
        
        UpdateCameraPosition();
    }

    void LateUpdate()
    {
        if (target == null)
            return;

        // Hämta musinmatning
        HandleMouseInput();
        
        // Uppdatera kamerans position
        UpdateCameraPosition();
    }

    private void HandleMouseInput()
    {

        float mouseX = Input.GetAxis("Mouse X");
        currentRotation += mouseX * rotationSpeed;
    }

    private void UpdateCameraPosition()
    {
        if (target == null)
            return;

        // Konvertera vinklar till radianer
        float rotationRad = currentRotation * Mathf.Deg2Rad;
        float elevationRad = elevation * Mathf.Deg2Rad;

        // Beräkna position i sfäriska koordinater
        float horizontalDistance = distance * Mathf.Cos(elevationRad);
        
        Vector3 offset = new Vector3(
            horizontalDistance * Mathf.Sin(rotationRad),
            distance * Mathf.Sin(elevationRad),
            horizontalDistance * Mathf.Cos(rotationRad)
        );

        // Sätt kamerans position och rotation
        transform.position = target.position + offset;
        transform.LookAt(target.position);
    }


    // Rita gizmos i editorn för att visualisera kamerans räckvidd
    void OnDrawGizmosSelected()
    {
        if (target == null)
            return;
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(target.position, transform.position);
    }
}
