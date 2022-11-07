using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AngleController : MonoBehaviour
{
    public float TurnRate;

    private float zRotation;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 rotation = transform.rotation.eulerAngles;
        if (Input.GetKey(KeyCode.Q))
        {
            zRotation += TurnRate * Time.deltaTime;
        }
        if (Input.GetKey(KeyCode.E))
        {
            zRotation -= TurnRate * Time.deltaTime;
        }

        rotation.z = Mathf.Clamp(zRotation, -90, 90);

        transform.rotation = Quaternion.Euler(rotation);
    }
}
