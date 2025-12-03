using System.Runtime.CompilerServices;
using UnityEngine;


public class AutoRotateAnimation : MonoBehaviour
{
    public float speed = 120f; // Rotationshastighet i grader per sekund (publik, justerbar i inspektorn)
    
    public enum RotationAxis // Enum (en datatyps-lista) för axlarna. Behöver vara publik för att inspektorn ska kunna använda datatypen för att visa gränssnitt
    {
        X,
        Y,
        Z
    }

    // Publik, Välj rotationaxel i inspektorn (Y förvalt).
    // Dettta är objektets lokala axel, så beroende på objektets ursprungliga rotation i scenen kan rotationsaxel vara svår att förutse, man får prova
    public RotationAxis axis = RotationAxis.Y; 

    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        // Skapa en vektor baserat på vald axel. värdet är 1 på den valda axeln och 0 på de andra
        // Värdet multipliceras sedan med hastigheten för att få rätt rotationshastighet
        Vector3 rotationAxis = new Vector3(); // get vektorn (0,0,0) som standard

        if (axis == RotationAxis.X)
        {
            rotationAxis = new Vector3(1, 0, 0);
        }
        else if(axis == RotationAxis.Y)
        {
            rotationAxis = new Vector3(0, 1, 0);
        }
        else if(axis == RotationAxis.Z)
        {
            rotationAxis = new Vector3(0, 0, 1);
        }

        // Rotera objektet runt den valda axeln med den angivna hastigheten
        // rotationAxis*speed*Time.deltaTime ger en ny vektor där speed*Time.deltaTime har multiplicerats med varje axel-värde i vektorn
        transform.Rotate(rotationAxis * speed * Time.deltaTime);
    }
}
