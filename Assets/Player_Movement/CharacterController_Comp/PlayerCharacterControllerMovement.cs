using UnityEngine;

public class PlayerCharacterControllerMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // spelarens förflyttningshastighet i Unity-enhet/sekund. Public för att kunna ändras i Inspector-fönstret

    // En gravitationshastighet nedåt (-y)
    // CharacterController-komponenten gör krockkollision och tillåter ramper och steg. Vi behöver nedåthastighet för att komma ner igen, när inget kolliderar underifrån (eller falla av banan)
    public float gravity = -5f; // public för justering i Inspector-fönstret

    private Vector3 startPoint; // En egenskap att spara startpositionen i, för att kunna återställa positionen, t.ex. vid fall utanför banan

    // Vi behöver en egenskap för en referens till CharacterController-komponenten på samma GameObject som detta script sitter på, för att kunna flytta spelaren med den
    private CharacterController charController;

    void Start()
    {
        startPoint = transform.position; // Spara startpositionen när spelet startar

        // Hämta referens till CharacterController-komponenten på samma GameObject
        // Om gameobjectet inte har en CharacterController-komponent, så genererar nedan ett felmeddelande i konsolen
        charController = GetComponent<CharacterController>(); 
    }


    void Update()
    {
        Vector3 moveDistance = new Vector3(); // En vektor för att lagra rörelseavståndet denna frame

        // Se om nedan i exempel-scenen om förflyttning med Transform-komponenten
        moveDistance.x = moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime; // Horisontell input-rörelse (sidled)
        moveDistance.z = moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;   // Vertikal input-rörelse (vi låter den styra djupled)
        // Transform-komponentens rörelsehantering gör ingen kollionskontroll, så ingen idé att lägga till gravitation på y-axeln ovan (som för CharController)

        // Applicerar gravitation på y-axeln (nedåt) - det är en fusk-gravitation med en godtydklig nedåt hastighet
        moveDistance.y = gravity * Time.deltaTime;

        // Vi flyttar spelaren genom att uppdatera Transform-komponentens position med Translate-metoden, som tar en vektor med rörelseavståndet
        // När Transform-komponenten används för att flytta så görs ingen kollisionskontroll, så spelaren kan passera genom andra objekt.
        // Om kollisionskontroll behövs, får vi flytta med andra komponenter som gör kollisionskkontroll - eller skriva egen kollisionskod.
        // Unity ger oss en "färdig" transform-variabel med en referens till Transform-komponenten och dess egenskaper och metoder, då komponenten alltid finns på GameObjecten
        charController.Move(moveDistance);

        // Återställ spelaren till startpositionen om den faller under en viss y-nivå (t.ex. utanför banan)
        if (transform.position.y < -5f)
        {
            transform.position = startPoint; // Återställ positionen till startpunkten
        }
    }

    // En publik metod för att återställa spelarens position till startpunkten, kan anropas från andra komponenter
    // Vi kan inte flytta med CharacterController-komponenten direkt, eftersom den krocktestar flytten och inte tillåter "teleportering" av objektet
    // Men kan heller inte bara använda Transform-komponenten direkt, eftersom CharacterController-komponenten kan motverka flytten med sin egen logik
    public void ResetPosition()
    {
        charController.enabled = false;   // Stänger temporärt av CharacterController-komponenten, så den inte motverkar Transform-flytten med sin flytt-logik. 
        transform.position = startPoint;  // Byter position på objektet med Transform-komponenten
        charController.enabled = true;    // Slår på CharacterController-komponenten igen
    }
}
