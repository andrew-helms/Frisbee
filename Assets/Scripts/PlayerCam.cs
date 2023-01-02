using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCam : MonoBehaviour
{
    public float XSens;
    public float YSens;

    [SerializeField] private Transform Orientation;

    [SerializeField] private RunManager runManager;
    [SerializeField] private ThrowManager throwManager;

    private float xRotation;
    private float yRotation;

    // Start is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (!throwManager.AimLock && !runManager.Paused)
        {
            float mouseX = Input.GetAxisRaw("Mouse X") * Time.deltaTime * XSens;
            float mouseY = Input.GetAxisRaw("Mouse Y") * Time.deltaTime * YSens;

            yRotation += mouseX;
            xRotation -= mouseY;

            xRotation = Mathf.Clamp(xRotation, -90, 90);

            transform.rotation = Quaternion.Euler(xRotation, yRotation, 0f);
            Orientation.rotation = Quaternion.Euler(0, yRotation, 0f);
        }
    }

    public void UnLock()
    {
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
    }

    public void Lock()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
    }
}
