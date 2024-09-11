using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

public class MouseLock : MonoBehaviour
{
    private bool isLocked = false;
    void Update()
    {
        if(Input.GetKeyDown(KeyCode.Q))
            isLocked = !isLocked;

        if(Input.GetKeyUp(KeyCode.Escape))
            isLocked = false;

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
