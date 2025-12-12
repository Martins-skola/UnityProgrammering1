using UnityEngine;

public class CollectObjectsInListByTag : MonoBehaviour
{
    
    GameObject[] objectArray;

    void Start()
    {
        objectArray = GameObject.FindGameObjectsWithTag("Collectible");
    }

    // Publik metod för att inaktivera alla objekt i listan
    public void HideAllCollectibles()
    {
        foreach (GameObject obj in objectArray)
        {
            obj.SetActive(false);
        }
    }

    // Publik metod för att aktivera alla objekt i listan
    public void ShowAllCollectibles()
    {
        foreach (GameObject obj in objectArray)
        {
            obj.SetActive(true);
        }
    }
}
