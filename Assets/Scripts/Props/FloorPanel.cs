using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FloorPanel : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Material materialInstance;

    [Header("Colors")]
    [SerializeField] private List<Color> colors = new List<Color>();
    [SerializeField] private Color currentColor;
    [SerializeField] private Color newColor;

    [Header("Variables")]
    [SerializeField] float changeDelay;
    [SerializeField] float minSpeed;
    [SerializeField] float maxSpeed;



    private void Start()
    {
        materialInstance = GetComponent<Renderer>().material;
        materialInstance.EnableKeyword("_EMISSION");

        changeDelay = Random.Range(minSpeed, maxSpeed);

        StartCoroutine(SwitchColor());
    }

    private IEnumerator SwitchColor()
    {
        while (true)
        {
            while (newColor == currentColor)
                newColor = colors[Random.Range(0, colors.Count)];

            // Material
            materialInstance.color = newColor;

            materialInstance.SetColor("_EmissionColor", newColor);

            currentColor = newColor;

            yield return new WaitForSeconds(changeDelay);
        }
    }
}
