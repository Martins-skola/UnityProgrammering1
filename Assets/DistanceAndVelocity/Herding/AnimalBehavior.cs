using UnityEngine;

public class AnimalBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float idleMinTime = 1f;
    [SerializeField] private float idleMaxTime = 3f;
    [SerializeField] private float moveMinTime = 0.5f;
    [SerializeField] private float moveMaxTime = 2f;
    [SerializeField] private float randomMoveSpeed = 2f;
    
    [Header("Flee Settings")]
    [SerializeField] private float fleeDetectionRadius = 10f;
    [SerializeField] private float panicRadius = 5f;
    [SerializeField] private float normalFleeSpeed = 5f;
    [SerializeField] private float panicFleeSpeed = 10f;
    
    [Header("Boundaries")]
    [SerializeField] private float boundaryRadius = 50f;
    [SerializeField] private Vector3 centerPoint = Vector3.zero;
    
    private Transform playerTransform;
    private Vector3 velocity;
    private float currentIdleTime;
    private bool isIdle = true;
    private float stateTimer;

    void Start()
    {
        // Hitta spelaren (antag att spelaren har tag "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }
        
        // Sätt initial idle-tid
        stateTimer = Random.Range(idleMinTime, idleMaxTime);
    }

    void Update()
    {
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        // Kolla om spelaren är inom flyktavstånd
        if (distanceToPlayer < fleeDetectionRadius)
        {
            FleeFromPlayer(distanceToPlayer);
        }
        else
        {
            RandomBehavior();
        }
        
        // Applicera rörelse
        transform.position += velocity * Time.deltaTime;
        
        // Håll djuret inom gränser
        EnforceBoundaries();
        
        // Rotera djuret i rörelsens riktning om det rör sig
        if (velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
        
        // Sakta ner hastigheten gradvis
        velocity *= 0.95f;
    }

    void FleeFromPlayer(float distanceToPlayer)
    {
        // Beräkna riktning bort från spelaren
        Vector3 directionFromPlayer = (transform.position - playerTransform.position).normalized;
        
        // Ju närmare spelaren, desto snabbare flykt
        float fleeSpeed;
        if (distanceToPlayer < panicRadius)
        {
            // Panik - maximal hastighet
            fleeSpeed = panicFleeSpeed;
        }
        else
        {
            // Gradvis ökning av hastighet beroende på avstånd
            float panicFactor = (fleeDetectionRadius - distanceToPlayer) / (fleeDetectionRadius - panicRadius);
            fleeSpeed = Mathf.Lerp(normalFleeSpeed, panicFleeSpeed, panicFactor);
        }
        
        // Sätt hastighet i flykt-riktning
        velocity = directionFromPlayer * fleeSpeed;
        
        // Reset timers när djuret flyr
        isIdle = false;
        stateTimer = Random.Range(idleMinTime, idleMaxTime);
    }

    void RandomBehavior()
    {
        stateTimer -= Time.deltaTime;
        
        if (stateTimer <= 0)
        {
            if (isIdle)
            {
                // Byt från idle till rörelse (30% chans)
                if (Random.value < 0.3f)
                {
                    StartMoving();
                }
                else
                {
                    StartIdle();
                }
            }
            else
            {
                // Byt från rörelse till idle (70% chans för idle)
                if (Random.value < 0.7f)
                {
                    StartIdle();
                }
                else
                {
                    StartMoving();
                }
            }
        }
    }

    void StartIdle()
    {
        isIdle = true;
        velocity = Vector3.zero;
        stateTimer = Random.Range(idleMinTime, idleMaxTime);
    }

    void StartMoving()
    {
        isIdle = false;
        
        // Slumpmässig riktning på XZ-planet (3D)
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 randomDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle));
        
        velocity = randomDirection * randomMoveSpeed;
        stateTimer = Random.Range(moveMinTime, moveMaxTime);
    }

    void EnforceBoundaries()
    {
        Vector3 offset = transform.position - centerPoint;
        float distance = offset.magnitude;
        
        if (distance > boundaryRadius)
        {
            // Studsa tillbaka mot centrum
            Vector3 directionToCenter = -offset.normalized;
            velocity = directionToCenter * velocity.magnitude;
            
            // Flytta tillbaka inom gränsen
            transform.position = centerPoint + offset.normalized * boundaryRadius;
        }
    }

    // Visualisera räckvidden i editor
    void OnDrawGizmosSelected()
    {
        // Flee detection radius
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, fleeDetectionRadius);
        
        // Panic radius
        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, panicRadius);
        
        // Boundaries
        Gizmos.color = Color.blue;
        Gizmos.DrawWireSphere(centerPoint, boundaryRadius);
    }
}
