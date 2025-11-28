using UnityEngine;

public class SpectatorMovement : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float angle;

    [Header("Speed")]
    [SerializeField] private int speed;
    [SerializeField] private int minSpeed;
    [SerializeField] private int maxSpeed;

    [Header("Offset")]
    [SerializeField] private int rotation;
    [SerializeField] private int minRotation;
    [SerializeField] private int maxRotation;

    [Header("Inverted")]
    [SerializeField] private float yRotation;
    [SerializeField] private bool shouldRotate;


    void Start()
    {
        speed = Random.Range(minSpeed, maxSpeed);
        rotation = Random.Range(minRotation, maxRotation);

        yRotation = shouldRotate ? 180f : 0f;
    }

    private void Update()
    {
        angle = Mathf.Sin(Time.time * speed) * rotation;
        transform.localRotation = Quaternion.Euler(angle, yRotation, 0f);
    }
}
