using UnityEngine;


public class DragAndDropWithCursor : MonoBehaviour
{
    // Texturer för olika muspekare, ange i editorn
    public Sprite hoverCursorSprite;
    public Vector2 hoverCursorOffset = Vector2.zero; // Offset från lop-left för hover-cursor
    public Sprite grabCursorSprite;
    public Vector2 grabCursorOffset = Vector2.zero; // Offset från lop-left för grab-cursor

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

    void OnMouseEnter()
    {
        // https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Cursor.SetCursor.html
        Cursor.SetCursor(hoverCursorSprite.texture, hoverCursorOffset, CursorMode.Auto);
    }

    void OnMouseExit()
    {
        // https://docs.unity3d.com/6000.3/Documentation/ScriptReference/Cursor.SetCursor.html
        // Pass 'null' to the texture parameter to use the default system cursor.
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
    }
}
