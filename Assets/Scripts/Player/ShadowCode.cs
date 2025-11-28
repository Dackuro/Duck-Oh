using UnityEngine;

public class ShadowCode : MonoBehaviour
{
    [Header("Values")]
    [SerializeField] private float yPosition;

    private void Start()
    {
        yPosition = transform.position.y;
    }

    private void Update()
    {
        Vector3 newPosition = transform.position;
        newPosition.y = yPosition;

        transform.position = newPosition;
    }
}
