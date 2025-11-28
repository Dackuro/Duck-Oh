using UnityEngine;

public class CameraManager : MonoBehaviour
{
    [Header("References")]
    public Transform player;
    public Transform mainSpeaker;

    [Header("Settings")]
    [Range(0f, 1f)] public float playerBias = 0.7f; // 0 = Enemy || 1 = Player

    private Vector3 targetPoint;


    void Update()
    {
        if (player == null || mainSpeaker == null) return;

        targetPoint = Vector3.Lerp(mainSpeaker.position, player.position, playerBias);
        
        transform.LookAt(targetPoint);    
    }
}
