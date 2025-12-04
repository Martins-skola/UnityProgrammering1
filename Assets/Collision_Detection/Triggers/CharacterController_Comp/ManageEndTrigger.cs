using UnityEngine;

// Detta script hanterar aktivering (och inaktivering) av ett gameobject med en trigger som marker banans slut om spelaren krockar med det
public class ManageEndTrigger : MonoBehaviour
{
    // En egenskap med referens till ett gameobject med en trigger-markerad collider och med tag 'Finish'. Sätts i inspectorn i edtorn.
    public Collider endTrigger;

    // En egenskap med referens till player-objektets CoinsCollector-komponent, för att här kunna ha koll på insamlade mynt.
    // Då detta skript ska sitta på samma objekt (player), kan vi sätta denna i Start() med GetComponent<>().
    private CharControllerCoinCollector coinCollector;

    // En egenskap med referens till player-objektets PlayerCharacterControllerMovement-komponent, så att vi kan återställa splearen till startposition med komponentens ResetPostion()-metod.
    // Då detta skript ska sitta på samma objekt (player), kan vi sätta denna i Start() med GetComponent<>().
    private PlayerCharacterControllerMovement movement;

    void Start()
    {
        if (!coinCollector)
        {
            Debug.LogError("EndTrigger: Referens till ett objekt med end-trigger inte satt i inspector!");
        }
        // När scenen startar inaktiverar vi obektet triggern så att det inte syns vid start
        // Alla colliders har egenskapen gameObject som leder till objektet trigger-collidern ligger på
        endTrigger.gameObject.SetActive(false);

        coinCollector = GetComponent<CharControllerCoinCollector>();
        movement = GetComponent<PlayerCharacterControllerMovement>();
    }

    // Update is called once per frame
    void Update()
    {
        if (coinCollector.coinsCollected >= coinCollector.coinsToCollect)
        {
            // Aktivera trigger-objektet när spelaren samlat alla mynt
            endTrigger.gameObject.SetActive(true);
        }
        else
        {
            // Inaktivera igen objektet (igen) om mynten inte är insamlade, t.ex. om spelaren dör/ramlar och mynten återställs
            endTrigger.gameObject.SetActive(false);
        }
    }

    // OnTriggerEnter används för att upptäcka kollisioner med trigger-colliders
    // Den anropas automatiskt av både CharacterController- och RigidBody(fysik)-komponenten när spelaren kolliderar med en trigger-collider
    void OnTriggerEnter(Collider other)
    {
        Debug.Log("End trigger");
        // Om trigger-krock med endtrigger-objektet (som har 'Finish'-taggen)
        // Markerar banans slut, vi återställer i detta fall banan, istället för att gå vidare till en annan bana, genom att t.ex. ladda en ny scen.
        if (other.gameObject.CompareTag("Finish"))
        {
            coinCollector.resetCoins();
            movement.ResetPosition();
        }
    }
}
