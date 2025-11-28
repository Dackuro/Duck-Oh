using UnityEngine;

public class DiscoBall : MonoBehaviour
{
    [Header("Variables")]
    [SerializeField] private float speed;
    [SerializeField] private float angle;


    private void Update()
    {
        transform.Rotate(0f, angle * Time.deltaTime * speed, 0f);
    }
}
