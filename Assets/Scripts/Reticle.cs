using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Reticle : MonoBehaviour
{
    private RectTransform reticle;

    [SerializeField] private float size = 75;
    [SerializeField] private float length = 25;
    [SerializeField] private float width = 2;
    [SerializeField] private RectTransform top;
    [SerializeField] private RectTransform right;
    [SerializeField] private RectTransform bottom;
    [SerializeField] private RectTransform left;

    [SerializeField] private Color color;
    [SerializeField] private List<Image> lines;

    // Start is called before the first frame update
    private void Start()
    {
        reticle = GetComponent<RectTransform>();

        reticle.sizeDelta = new Vector2(size, size);

        top.sizeDelta = new Vector2(width, length);
        right.sizeDelta = new Vector2(length, width);
        left.sizeDelta = new Vector2(length, width);
        bottom.sizeDelta = new Vector2(width, length);

        lines.ForEach(line => line.color = color); ;
    }
}
