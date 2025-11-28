using UnityEngine;

public class ShockWaveCollider : MonoBehaviour
{
    [SerializeField] private Transform parent;
    [SerializeField] private bool hasHit;


    private void Start()
    {
        hasHit = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        if (hasHit)
            return;

        if (other.transform.CompareTag("Player"))
        {
            PlayerGeneral playerGeneral = other.transform.parent.GetComponentInChildren<PlayerGeneral>();
            playerGeneral.CONFIGURATION.SetPush(parent.position);

            hasHit = true;
        }
    }
}
