using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Movement Settings")]
    [SerializeField] private float moveSpeed = 5f;
    [SerializeField] private float rotationSpeed = 10f;
    
    [Header("Camera Settings")]
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private bool useCameraRelativeMovement = true;
    
    private CharacterController characterController;
    private Vector3 moveDirection;

    void Start()
    {
        // Försök hitta CharacterController, annars lägg till en
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            characterController = gameObject.AddComponent<CharacterController>();
        }
        
        // Om ingen kamera är tilldelad, använd huvudkameran
        if (cameraTransform == null)
        {
            cameraTransform = Camera.main.transform;
        }
        
        // Se till att spelaren har tag "Player"
        if (gameObject.tag != "Player")
        {
            gameObject.tag = "Player";
        }
    }

    void Update()
    {
        HandleMovement();
    }

    void HandleMovement()
    {
        // Hämta input
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");
        
        Vector3 inputDirection = new Vector3(horizontal, 0, vertical).normalized;
        
        if (inputDirection.magnitude >= 0.1f)
        {
            // Beräkna rörelsens riktning baserat på kameran eller världen
            Vector3 targetDirection;
            
            if (useCameraRelativeMovement && cameraTransform != null)
            {
                // Kamera-relativ rörelse
                Vector3 cameraForward = cameraTransform.forward;
                Vector3 cameraRight = cameraTransform.right;
                
                // Ignorera Y-komponent för att hålla rörelsen horisontell
                cameraForward.y = 0;
                cameraRight.y = 0;
                cameraForward.Normalize();
                cameraRight.Normalize();
                
                targetDirection = cameraForward * vertical + cameraRight * horizontal;
            }
            else
            {
                // Världsrelativ rörelse
                targetDirection = inputDirection;
            }
            
            // Flytta spelaren
            moveDirection = targetDirection.normalized * moveSpeed;
            characterController.Move(moveDirection * Time.deltaTime);
            
            // Rotera spelaren mot rörelsens riktning
            if (targetDirection != Vector3.zero)
            {
                Quaternion targetRotation = Quaternion.LookRotation(targetDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, rotationSpeed * Time.deltaTime);
            }
        }
        
        // Applicera gravitation om CharacterController används
        if (!characterController.isGrounded)
        {
            moveDirection.y -= 9.81f * Time.deltaTime;
            characterController.Move(new Vector3(0, moveDirection.y, 0) * Time.deltaTime);
        }
    }
}
