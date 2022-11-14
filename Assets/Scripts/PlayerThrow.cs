using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerThrow : MonoBehaviour
{
    [SerializeField] private GameObject projectile;
    [SerializeField] private float ThrowForce;
    [SerializeField] private float ThrowCooldown;
    [SerializeField] private float SpinForce;

    [SerializeField] private Transform camOrientation;

    [SerializeField] private PlayerCam cam;

    [SerializeField] private ThrowManager throwManager;

    // Start is called before the first frame update
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        MyInput();
        
        if (throwManager.AimLock)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * cam.XSens;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * cam.YSens;

            throwManager.BankAngle.x += mouseX * .01f;
            throwManager.BankAngle.y += mouseY * .01f;

            throwManager.BankAngle.x = Mathf.Clamp(throwManager.BankAngle.x, -1, 1);
            throwManager.BankAngle.y = Mathf.Clamp(throwManager.BankAngle.y, -0.5f, 0.5f);
        }
    }

    private void Throw()
    {
        Vector3 rotation = camOrientation.rotation.eulerAngles;
        rotation.z -= throwManager.BankAngle.x * 90; //bank = horizontal mouse
        rotation.x += throwManager.BankAngle.y * 90; //pitch = vertical mouse
        Rigidbody disc = Instantiate(projectile, transform.position + camOrientation.forward * 1.5f, Quaternion.Euler(rotation)).GetComponent<Rigidbody>();
        disc.AddForce(camOrientation.forward * ThrowForce, ForceMode.Impulse);
        disc.AddTorque(disc.transform.up * SpinForce, ForceMode.Impulse);
    }

    private void MyInput()
    {
        if (Input.GetKeyDown(KeyCode.Mouse0) && throwManager.HoldingDisc)
        {
            throwManager.StartThrow();
        }
        if (Input.GetKeyUp(KeyCode.Mouse0) && throwManager.HoldingDisc)
        {
            throwManager.EndThrow();
            Throw();
            throwManager.BankAngle.Set(0, 0, 0);
        }
    }
}
