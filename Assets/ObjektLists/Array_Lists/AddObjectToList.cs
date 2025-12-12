using Unity.VisualScripting;
using UnityEditor;
using UnityEngine;

public class ObjectList : MonoBehaviour
{
    
    GameObject[] objectArray;

        

    public void CreateObject()
    {

    }
    
    // Metod för att inaktivera alla objekt i listan
    void AddObject(GameObject gameobject)
    {
        /* Ska du själv lägga till objekt i listor, och inte får "färdiga" listor från Unity, så är det bättre att använda List istället för Array.
         * List har inbyggda metoder för att lägga till och ta bort objekt, medan Array är av fast storlek och kräver mer manuellt arbete för att ändra storlek.
         * Se annat kodexempel som använder List istället för Array för att hantera objektlistor.
         */

        // Skapar en temporär array med det nya objektet, då array objekt bara kan lägga till andra arrayer, utan att behöva ändra storlek manuellt
        GameObject[] tempArray = new GameObject[] { gameobject };
        // Lägger till den temporära arrayen med objektet i den ursprungliga arrayen
        objectArray.AddRange(tempArray);
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
