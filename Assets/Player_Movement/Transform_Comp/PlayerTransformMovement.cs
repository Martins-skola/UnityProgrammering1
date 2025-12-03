using UnityEngine;

public class PlayerTransformMovement : MonoBehaviour
{
    public float moveSpeed = 3f; // spelarens förflyttningshastighet i Unity-enhet/sekund. Public för att kunna ändras i Inspector-fönstret

    private Vector3 startPoint; // En egenskap att spara startpositionen i, för att kunna återställa positionen, t.ex. vid fall utanför banan
    
    void Start()
    {
        startPoint = transform.position; // Spara startpositionen när spelet startar
    }


    void Update()
    {
        Vector3 moveDistance = new Vector3(); // En vektor för att lagra rörelseavståndet denna frame

        // Hämta inmatning från tangentbordet och beräkna rörelseavståndet
        // moveSpeed: egenskapen med spelarens förflyttningshastighet/sekund (förändras av nedan)
        // Använd "Horizontal" och "Vertical" axlarna som standard i Unity Input Manager, -1 till 1 för riktning på respektive axel, 0 för ingen rörelse
        // Multiplicera med Time.deltaTime för att göra rörelsen framerate-oberoende
        moveDistance.x = moveSpeed * Input.GetAxis("Horizontal") * Time.deltaTime; // Horisontell input-rörelse (sidled)
        moveDistance.z = moveSpeed * Input.GetAxis("Vertical") * Time.deltaTime;   // Vertikal input-rörelse (vi låter den styra djupled)
        // Transform-komponentens rörelsehantering gör ingen kollionskontroll, så ingen idé att lägga till gravitation på y-axeln ovan (som för CharController)

        // Vi flyttar spelaren genom att uppdatera Transform-komponentens position med Translate-metoden, som tar en vektor med rörelseavståndet
        // När Transform-komponenten används för att flytta så görs ingen kollisionskontroll, så spelaren kan passera genom andra objekt.
        // Om kollisionskontroll behövs, får vi flytta med andra komponenter som gör kollisionskkontroll - eller skriva egen kollisionskod.
        // Unity ger oss en "färdig" transform-variabel med en referens till Transform-komponenten och dess egenskaper och metoder, då komponenten alltid finns på GameObjecten
        transform.Translate(moveDistance);
    }
}
