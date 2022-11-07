using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public GameObject projectile;
    public float ThrowForce;
    public float ThrowCooldown;

    public Transform Orientation;

    public Transform BankAngle;

    private bool canThrow;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && canThrow)
        {
            canThrow = false;

            Throw();
        }
        if (!canThrow)
        {
            Invoke(nameof(ResetThrow), ThrowCooldown);
        }
    }

    private void Throw()
    {
        Vector3 rotation = Orientation.rotation.eulerAngles;
        rotation.z += BankAngle.rotation.eulerAngles.z;
        rotation.x -= 10;
        Instantiate(projectile, transform.position + Orientation.forward * 1.5f, Quaternion.Euler(rotation)).GetComponent<Rigidbody>().AddForce(Orientation.forward * ThrowForce, ForceMode.Impulse);
    }

    private void ResetThrow()
    {
        canThrow = true;
    }
}
