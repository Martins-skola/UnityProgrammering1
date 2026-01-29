using UnityEngine;

public class DestroyOnClick : MonoBehaviour
{
    private void OnMouseUp()
    {
        Destroy(gameObject);
    }
}
