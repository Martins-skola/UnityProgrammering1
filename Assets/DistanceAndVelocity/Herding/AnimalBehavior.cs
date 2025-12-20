using UnityEngine;



public class AnimalBehavior : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 2f;
    [SerializeField] private float moveFriction = 1f; // Sakta ner rörelsen över tid, andel av moveSpeed 0-1 (0.7 betyder att 70% av hastigheten behålls varje frame)

    [Header("Flee Settings")]
    [SerializeField] private float fleeDetectionRadius = 10f;
    [SerializeField] private float panicRadius = 5f;
    [SerializeField] private float fleeSpeedMultiplier = 1.2f;
    [SerializeField] private float panicSpeedMultiplier = 2f;

    [Header("Idle Beavior")]
    [Range(0f, 1f)]
    [SerializeField] private float idleMoveness = 0.5f; // Hur ofta djuret byter mellan idle och rörelse (0-1), högre värde = mer rörelse
    [Range(0f, 10f)]
    [SerializeField] private float stillMinTime = 1f; // minsta tid i idle-läge, sekunder
    [Range(0f, 10f)]
    [SerializeField] private float stillMaxTime = 3f;
    [Range(0f, 10f)]
    [SerializeField] private float moveMinTime = 0.5f;
    [Range(0f, 10f)]
    [SerializeField] private float moveMaxTime = 2f;

    enum AnimalState
    {
        Still,
        Moving,
        Fleeing
    }

    private Transform playerTransform;
    private Vector3 velocity;
    private AnimalState state;
    private float idleTimer;

    void Start()
    {
        // Hitta spelaren (antag att spelaren har tag "Player")
        GameObject player = GameObject.FindGameObjectWithTag("Player");
        if (player != null)
        {
            playerTransform = player.transform;
        }

        // Sätter initial idle state och -tid
        ResetIdle();
    }

    void Update()
    {
        // Om ingen spelare hittad, gör inget
        if (playerTransform == null) return;

        float distanceToPlayer = Vector3.Distance(transform.position, playerTransform.position);
        
        // Kolla om spelaren är idle eller flyr (inom flyktavstånd)
        if (distanceToPlayer < fleeDetectionRadius)
        {
            // Flee från spelaren
            FleeFromPlayer(distanceToPlayer);
        }
        else
        {
            // Idla

            // Kollar om vi kommer ur flee-läge, i så fall skapa ny idle-sekvens
            if (state == AnimalState.Fleeing)
            {
                ResetIdle(); // Ställer state och timer för idle/rörelse
            }
            else
            { // Fortsätt med idla eller reset idla om timer gått ut
                idleTimer -= Time.deltaTime;

                if (idleTimer <= 0)
                {
                    ResetIdle();
                }
            }
        }

        // Applicera rörelse
        transform.position += velocity * Time.deltaTime;
               
        // Rotera djuret i rörelsens riktning om det rör sig
        if (velocity.magnitude > 0.1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(velocity.normalized);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * 5f);
        }
    }

    // Börjar en idle-sekvens
    private void ResetIdle()
    {
        float moveness = Random.Range(0f, 1f);
        if (moveness < idleMoveness)
        {
            StartMoving();
        } else {             
            StartStill();
        }
    }

    void FleeFromPlayer(float distanceToPlayer)
    {
        // Har vi hamnat här, så är vi i flee-läge
        state = AnimalState.Fleeing;

        // Beräkna riktning bort från spelaren
        Vector3 directionFromPlayer = (transform.position - playerTransform.position).normalized;

        // Se till att djuret rör sig endast på XZ-planet
        directionFromPlayer.y = 0;

        // Om flee eller panic
        // FleeFromPlayer körs bara om inom fleeDetectionRadius, så fleeRadius behöver inte kollas
        float fleeSpeed;
        if (distanceToPlayer < panicRadius) // om flee men inte panik 
        {
            // Panik - maximal hastighet
            fleeSpeed = moveSpeed * panicSpeedMultiplier;
        }
        else
        {
            // Flee - normal flykthastighet
            fleeSpeed = moveSpeed * fleeSpeedMultiplier;
        }

        // Sätt hastighet i flykt-riktning
        velocity = directionFromPlayer * fleeSpeed;
    }

    void StartStill()
    {
        state = AnimalState.Still;
        idleTimer = Random.Range(stillMinTime, stillMaxTime);
        velocity = Vector3.zero;
    }

    void StartMoving()
    {
        state = AnimalState.Moving;
        idleTimer = Random.Range(moveMinTime, moveMaxTime);

        // Slumpmässig riktning på XZ-planet (3D)
        float randomAngle = Random.Range(0f, 360f) * Mathf.Deg2Rad;
        Vector3 randomDirection = new Vector3(Mathf.Cos(randomAngle), 0, Mathf.Sin(randomAngle));
        
        velocity = randomDirection * moveSpeed;
    }

    // Debug GUI för att visa state och hastighet - endast för utveckling och en animal i scenen
    private void OnGUI()
    {
        //GUI.Label(new Rect(10, 10, 300, 20), "Animal State: " + state);
        //GUI.Label(new Rect(10, 20, 300, 20), "Velocity: " + velocity.magnitude);
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
    }
}
