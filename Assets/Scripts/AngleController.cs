using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleController : MonoBehaviour
{
    public float TurnRate;

    public Transform VerticalLine;
    public Transform HorizontalLine;

    private PlayerController script;

    // Start is called before the first frame update
    void Start()
    {
        script = GameObject.Find("Player").GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 horizontal = new Vector3(1, 1, script.BankAngle.x * 0.25f);
        Vector3 vertical = new Vector3(1, 1, script.BankAngle.y * 0.5f);

        HorizontalLine.localScale = horizontal;
        VerticalLine.localScale = vertical;
    }
}
