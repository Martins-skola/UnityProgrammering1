using UnityEngine;

/// <summary>
/// En orbital kamera som roterar runt ett angivet objekt.
/// Kontrolleras med mus för rotation och zoom.
/// </summary>
public class AdvancedOrbitalCamera : MonoBehaviour
{
    [Header("Mål")]
    [Tooltip("Objektet som kameran ska rotera runt")]
    public Transform target;

    [Header("Rotation")]
    [Tooltip("Rotationshastighet för musrörelse")]
    [Range(0.1f, 10f)]
    public float rotationSpeed = 2f;

    [Header("Avstånd")]
    [Tooltip("Standardavstånd till målet")]
    [Range(1f, 50f)]
    public float distance = 10f;
    
    [Tooltip("Minimalt avstånd")]
    [Range(1f, 100f)]
    public float minDistance = 2f;
    
    [Tooltip("Maximalt avstånd")]
    [Range(1f, 100f)]
    public float maxDistance = 20f;
    
    [Tooltip("Zoomhastighet med mushjulet")]
    [Range(0.1f, 5f)]
    public float zoomSpeed = 2f;

    [Header("Höjdvinkel")]
    [Tooltip("Använd en fast höjdvinkel istället för muskontroll")]
    public bool useFixedElevation = false;
    
    [Tooltip("Fast höjdvinkel i grader (om useFixedElevation är aktiverad)")]
    [Range(-89f, 89f)]
    public float fixedElevation = 30f;
    
    [Tooltip("Minimal höjdvinkel i grader")]
    [Range(-89f, 89f)]
    public float minElevation = -60f;
    
    [Tooltip("Maximal höjdvinkel i grader")]
    [Range(-89f, 89f)]
    public float maxElevation = 80f;
    
    [Tooltip("Höjdvinkelhastighet för musrörelse")]
    [Range(0.1f, 10f)]
    public float elevationSpeed = 2f;

    // Privata variabler för rotation
    private float currentRotation = 0f;
    private float currentElevation = 30f;
    private float currentDistance;

    void Start()
    {
        // Sätt startvärden
        currentDistance = distance;
        
        if (useFixedElevation)
        {
            currentElevation = fixedElevation;
        }
        
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
        // Rotation med höger musknapp eller musens x-rörelse
        if (Input.GetMouseButton(1) || Input.GetMouseButton(0))
        {
            float mouseX = Input.GetAxis("Mouse X");
            currentRotation += mouseX * rotationSpeed;
            
            // Hantera höjdvinkel (om inte fast)
            if (!useFixedElevation)
            {
                float mouseY = Input.GetAxis("Mouse Y");
                currentElevation -= mouseY * elevationSpeed;
                currentElevation = Mathf.Clamp(currentElevation, minElevation, maxElevation);
            }
            else
            {
                currentElevation = fixedElevation;
            }
        }
        
        // Zoom med mushjulet
        float scroll = Input.GetAxis("Mouse ScrollWheel");
        if (scroll != 0f)
        {
            currentDistance -= scroll * zoomSpeed;
            currentDistance = Mathf.Clamp(currentDistance, minDistance, maxDistance);
        }
    }

    private void UpdateCameraPosition()
    {
        if (target == null)
            return;

        // Konvertera vinklar till radianer
        float rotationRad = currentRotation * Mathf.Deg2Rad;
        float elevationRad = currentElevation * Mathf.Deg2Rad;

        // Beräkna position i sfäriska koordinater
        float horizontalDistance = currentDistance * Mathf.Cos(elevationRad);
        
        Vector3 offset = new Vector3(
            horizontalDistance * Mathf.Sin(rotationRad),
            currentDistance * Mathf.Sin(elevationRad),
            horizontalDistance * Mathf.Cos(rotationRad)
        );

        // Sätt kamerans position och rotation
        transform.position = target.position + offset;
        transform.LookAt(target.position);
    }

    // Hjälpmetod för att ändra målet under körning
    public void SetTarget(Transform newTarget)
    {
        target = newTarget;
    }

    // Hjälpmetod för att ändra avstånd programmatiskt
    public void SetDistance(float newDistance)
    {
        currentDistance = Mathf.Clamp(newDistance, minDistance, maxDistance);
    }

    // Hjälpmetod för att återställa kameran
    public void ResetCamera()
    {
        currentRotation = 0f;
        currentElevation = useFixedElevation ? fixedElevation : 30f;
        currentDistance = distance;
        UpdateCameraPosition();
    }

    // Rita gizmos i editorn för att visualisera kamerans räckvidd
    void OnDrawGizmosSelected()
    {
        if (target == null)
            return;

        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(target.position, minDistance);
        
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(target.position, maxDistance);
        
        Gizmos.color = Color.green;
        Gizmos.DrawLine(target.position, transform.position);
    }
}
