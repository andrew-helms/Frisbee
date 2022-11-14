using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleController : MonoBehaviour
{
    [SerializeField] private float TurnRate;

    [SerializeField] private Transform VerticalLine;
    [SerializeField] private Transform HorizontalLine;

    [SerializeField] private ThrowManager throwManager;

    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 horizontal = new Vector3(1, 1, throwManager.BankAngle.x * 0.25f);
        Vector3 vertical = new Vector3(1, 1, throwManager.BankAngle.y * 0.5f);

        HorizontalLine.localScale = horizontal;
        VerticalLine.localScale = vertical;
    }
}
