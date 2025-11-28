using UnityEngine;

public class DeathZone : MonoBehaviour
{
    [Header("Audio")]
    [SerializeField] private AudioClip deathClip;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            GameManager.instance.EndGame();

            AudioManager.instance.PlaySFXClip(deathClip, "DeathSound AudioSource", other.transform, 0.5f);
        }
    }
}
