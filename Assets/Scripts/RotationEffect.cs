using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RotationEffect : MonoBehaviour
{
    [SerializeField] public float roationSpeed = 35f;
    private int dir = -1;

    public void Setup(int dir)
    {
        this.dir = dir;
    }

    void Update()
    {
        transform.Rotate(Vector3.forward, dir * roationSpeed * Time.deltaTime);
    }
}
