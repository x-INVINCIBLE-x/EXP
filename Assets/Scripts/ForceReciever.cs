using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ForceReciever : MonoBehaviour
{
    [SerializeField] private CharacterController controller;

    private float verticalVelocity;

    void Update()
    {
        if (controller.isGrounded)
        {
            verticalVelocity = Physics.gravity.y * Time.deltaTime;
        }
        else
        {
            verticalVelocity += Physics.gravity.y * Time.deltaTime;
        }
    }

    public Vector3 Movement => Vector3.up * verticalVelocity;

    public void Jump(float jumpForce)
    {
        verticalVelocity += jumpForce;
    }

    internal void Reset()
    {
        verticalVelocity = 0f;
    }
}
