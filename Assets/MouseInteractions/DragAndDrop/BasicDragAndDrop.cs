using UnityEngine;

public class DragAndDrop : MonoBehaviour
{
    private bool isDragging = false;
    private float yPosition; // Sparar objektets ursprungliga Y-höjd

    void Start()
    {
        // Spara objektets Y-position så den inte ändras när vi drar
        yPosition = transform.position.y;
    }

    void Update()
    {
        // Om vi håller nere musknappen och drar objektet
        if (isDragging)
        {
            // Skapa en ray från kameran genom musens position
            Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);

            // Beräkna var i världen musen pekar på XZ-planet
            // Vi använder Y-positionen för att veta vilken höjd planet är på
            float distanceToPlane = (yPosition - ray.origin.y) / ray.direction.y;
            Vector3 worldPosition = ray.origin + ray.direction * distanceToPlane;

            // Flytta objektet till den nya positionen (behåller Y-höjden)
            transform.position = new Vector3(worldPosition.x, yPosition, worldPosition.z);
        }
    }

    // Unity anropar denna när musen klickar på objektets collider
    void OnMouseDown()
    {
        isDragging = true;
    }

    // Unity anropar denna när musknappen släpps
    void OnMouseUp()
    {
        isDragging = false;
    }
}
