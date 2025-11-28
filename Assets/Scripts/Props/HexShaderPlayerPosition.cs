using UnityEngine;

public class HexShaderPlayerPosition : MonoBehaviour
{
    public Transform player;
    public Material hexMaterial;



    private void Start()
    {
        hexMaterial = GetComponent<MeshRenderer>().material;
    }

    private void Update()
    {
        hexMaterial.SetVector("_PlayerPosition", player.position);
    }
}
