using System.Runtime.CompilerServices;
using UnityEngine;


public class CirculateAnimation : MonoBehaviour
{
    public float speed = 1f; // radianer per sekund
    public float radius = 0.75f; // avstånd från centerposition = radien på cirkeln
    public bool faceCenter = false; // om objektet ska roteras så att det alltid tittar mot centerpositionen

    private float currentAngle = 0f; // aktuell vinkel runt cirkeln i grader. Privat egenskap, behöver inte vara publik. Vi slumpar en startvinkel i Start() för att undvika att flera objekt börjar på samma position

    // Centerpositionen som objektet cirkulerar runt, vi sätter denna till objektets aktuella position i Start()
    // Om objektet även behöver röra sig på annat sätt, så får du lägga detta objekt som ett barnobjekt till ett tomt objekt och animera/röra det tomma objektet
    private Vector3 centerPosition; 

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
        centerPosition = transform.localPosition; // sätt centerpositionen till objektets lokala startposition (lokala, för att möjliggöra för objektet att vara ett barnobjekt till ett annat objekt som kan röra sig)
        currentAngle = Random.Range(0f, 2*Mathf.PI); // Radianer. Slumpar fram en startvinkel för att undvika att flera objekt börjar på samma position
    }

    void Update()
    {
        currentAngle = (currentAngle + speed * Time.deltaTime) % (2 * Mathf.PI); // håll vinkeln inom 0-2PI radianer. % är modulus-operatorn som ger resten vid division

        float xFromCenter = Mathf.Sin(currentAngle) * radius;
        float yFromCenter = Mathf.Cos(currentAngle) * radius;

        // Skapa en förflyttning-vektor för the wobble applicerad på vald axel
        // Värdet multipliceras sedan med hastigheten för att få rätt rotationshastighet
        Vector3 positionFromCenter = new Vector3(); // get vektorn (0,0,0) som standard

        if (axis == RotationAxis.X)
        {
            positionFromCenter = new Vector3(0, xFromCenter, yFromCenter);
        }
        else if(axis == RotationAxis.Y)
        {
            positionFromCenter = new Vector3(xFromCenter, 0, yFromCenter);
        }
        else if(axis == RotationAxis.Z)
        {
            positionFromCenter = new Vector3(xFromCenter, yFromCenter, 0);
        }

        // Ny position = centerPosition + positionFromCenter
        // Sätter objektets lokala position för att möjliggöra för objektet att vara ett barnobjekt till ett annat objekt som kan röra sig
        // transform.position sätter global position
        transform.localPosition = centerPosition + positionFromCenter;

        // Rotera objektet så att det tittar mot centerpositionen om faceCenter är true
        if (faceCenter)
        {
            transform.LookAt(centerPosition);
        }
    }
}
