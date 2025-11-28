using UnityEngine;

public class CameraSwing : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float rotationSpeed = 0.4f;
    [SerializeField] public float rotationAmount = 1.5f;

    private Vector3 startEuler;

    private void Start()
    {
        startEuler = transform.localEulerAngles;
    }
    private void Update()
    {
        // Smooth breathing motion (tilt up/down)
        float xTilt = Mathf.Sin(Time.time * rotationSpeed) * rotationAmount;
        float yTilt = Mathf.Cos(Time.time * rotationSpeed) * rotationAmount;

        // Apply the tilt relative to the original rotation
        transform.localEulerAngles = new Vector3(startEuler.x + xTilt, startEuler.y + xTilt, startEuler.z);
    }
}