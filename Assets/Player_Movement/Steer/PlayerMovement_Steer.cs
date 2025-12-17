using UnityEngine;

public class PlayerMovement_Steer : MonoBehaviour
{
    public float speed = 2f; // Speed of the player movement
    public float turnSpeed = 100f; // Speed of the player turning   
    public float gravity = -9.81f; // Gravity speed

    private CharacterController characterController;
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        float movementSpeed = Input.GetAxis("Vertical") * speed * Time.deltaTime;
        Vector3 movement = transform.forward * movementSpeed;
        movement.y += gravity * Time.deltaTime;
        characterController.Move(movement);
        float turn = Input.GetAxis("Horizontal") * turnSpeed * Time.deltaTime;
        transform.Rotate(0, turn, 0);
    }
}
