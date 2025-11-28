using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class ShockWave : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private ProceduralTorus proceduralTorus;
    [SerializeField] private Transform player;
    [SerializeField] private SphereCollider hitCollider;
    [SerializeField] private ShockWaveCollider colliderScript;

    [Header("Expansion Variables")]
    [SerializeField] private float maxSize;
    [SerializeField] private float minSize;
    [Space]
    [SerializeField] private float lifeTime;
    public float speed;


    void Start()
    {
        if (proceduralTorus == null)
            proceduralTorus = gameObject.GetComponent<ProceduralTorus>();

        if (player == null)
            player = GameObject.FindGameObjectWithTag("Player").transform;

        StartCoroutine(Expansion());
    }

    public IEnumerator Expansion()
    {
        float elapsed = 0f;
        minSize = proceduralTorus.majorRadius;

        while (elapsed < lifeTime)
        {
            proceduralTorus.majorRadius = Mathf.Lerp(minSize, maxSize, elapsed / lifeTime);
            
            UpdateColliderPosition();

            elapsed += Time.deltaTime * speed;
            yield return null;
        }

        Destroy(gameObject);
    }

    private void UpdateColliderPosition()
    {
        if (player == null || hitCollider == null)
            return;

        Vector3 playerFlat = new Vector3(player.position.x, transform.position.y, player.position.z);
        Vector3 dirToPlayer = (playerFlat - transform.position).normalized;

        Vector3 torusEdge = transform.position + dirToPlayer * proceduralTorus.majorRadius;

        hitCollider.transform.position = torusEdge;
    }
}
