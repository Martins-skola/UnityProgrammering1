using Unity.VisualScripting;
using UnityEngine;

/*
 * Detta script hanterar insamling av mynt för en karaktär som använder CharacterController-komponenten.
 * Vid kollision med ett objekt märkt som "Coin", ökar räknaren för insamlade mynt och inaktiverar myntobjektet.
 * Man kan också radera myntet (Destroy gameobject), men inaktivering är bättre för prestanda och mynten ska återanvändas senare.
 * Trillar man av banan eller när man samlat alla mynt, så aktiverar vi dom igen, så man kan börja om
 */


public class CharControllerCoinCollector : MonoBehaviour
{
    [HideInInspector] // Dölj publika egenskapen nedan i inspektorn, vi sätter värdet själva i koden
    public int coinsCollected = 0; // Räknare för insamlade mynt. Publik, så andra komponenter kan läsa värdet
    [HideInInspector] // Dölj publika egenskapen nedan i inspektorn, vi sätter värdet i Start()
    public int coinsToCollect = 0; // Antal mynt som behövs för att insamlingen ska anses klar. Publik, så andra komponenter kan läsa värdet. Vi sätter värdet i Start()

    private CharacterController charController; // Referens till CharacterController-komponenten

    private GameObject[] coins; // Array för att lagra referenser till alla mynt i scenen

    void Start()
    {
        charController = GetComponent<CharacterController>(); // Hämtar CharacterController-komponenten
        coins = GameObject.FindGameObjectsWithTag("Coin"); // Hämta alla mynt i scenen och lagra dem i arrayen
        coinsToCollect = coins.Length; // Sätt antalet mynt som ska samlas in baserat på hur många mynt som finns i coins-arrayen
    }


    void Update()
    {
        if (transform.position.y < -2)
        {
            Debug.Log("Falling! Resets coins and position"); // Positionen reset hanteras av förflyttnings-scriptet
            resetCoins();
        }
    }

    // OnTriggerEnter används för att upptäcka kollisioner med trigger-colliders
    // Den anropas automatiskt av både CharacterController- och RigidBody(fysik)-komponenten när spelaren kolliderar med en trigger-collider
    void OnTriggerEnter(Collider other)
    {
        // Om trigger-krock med coin
        if (other.gameObject.CompareTag("Coin"))
        {
            coinsCollected++;

            other.gameObject.SetActive(false); // Inaktivera objektet istället för att radera det, bättre för prestanda om mynten ska återanvändas
            //Destroy(hit.gameObject); använd detta om du vill radera objketet du krockade med
        }
    }

    // Metoden är public för att andra komponenter ska kunna göra reset coins.
    public void resetCoins()
    {
        foreach (GameObject coin in coins)
        {
            coin.SetActive(true);
        }
        coinsCollected = 0;
    }

    // Skriver debug-text på skärmen om antalet samlade mynt
    void OnGUI()
    {
        
        GUI.Label(new Rect(10, 10, 100, 20), "Samlade mynt: " + coinsCollected.ToString() + "/" + coinsToCollect.ToString());
    }
}
