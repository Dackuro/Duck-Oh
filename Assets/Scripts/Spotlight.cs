using UnityEngine;

public class Spotlight : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Transform player;

    void Update()
    {
        transform.LookAt(player);
    }
}
