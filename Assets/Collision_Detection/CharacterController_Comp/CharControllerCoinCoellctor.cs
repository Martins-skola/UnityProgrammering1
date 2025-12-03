using Unity.VisualScripting;
using UnityEngine;

/*
 * Detta script hanterar insamling av mynt för en karaktär som använder CharacterController-komponenten.
 * Vid kollision med ett objekt märkt som "Coin", ökar räknaren för insamlade mynt och inaktiverar myntobjektet.
 * Man kan också radera myntet (Destroy gameobject), men inaktivering är bättre för prestanda och mynten ska återanvändas senare.
 * Trillar man av banan eller när man samlat alla mynt, så aktiverar vi dom igen, så man kan börja om
 */


public class CharControllerCoinCoellctor : MonoBehaviour
{
    public int coinsCollected = 0;
    public int coinsToCollect = 3;

    private CharacterController charController;

    private GameObject[] coins;
    
    void Start()
    {
        charController = GetComponent<CharacterController>();
        coins = GameObject.FindGameObjectsWithTag("Coin");
    }


    void Update()
    {
        if (transform.position.y < -2)
        {
            Debug.Log("Falling! Resets coins and position"); // Positionen reset hanteras av förflyttnings-scriptet
            resetCoins();
        }
    }

    private void OnControllerColliderHit(ControllerColliderHit hit)
    {
        if (hit.gameObject.CompareTag("Coin"))
        {
            coinsCollected++;
            
            hit.gameObject.SetActive(false); // Inaktivera objektet istället för att radera det, bättre för prestanda om mynten ska återanvändas
            //Destroy(hit.gameObject); använd detta om du vill radera objketet du krockade med

            if (coinsCollected >= coinsToCollect)
            {
                Debug.Log("All coins collected!");
                resetCoins();
            }
            else
            {
                Debug.Log("Coins collected: " + coinsCollected);
            }
        }
    }

    private void resetCoins()
    {
        foreach (GameObject coin in coins)
        {
            Debug.Log("active");
            coin.SetActive(true);
        }
    }
}
