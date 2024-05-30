using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MouseLock : MonoBehaviour
{
    private bool isLocked = true;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.L))
            isLocked = !isLocked;

        if (isLocked)
        {
            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
        }
        else
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
    }
}
