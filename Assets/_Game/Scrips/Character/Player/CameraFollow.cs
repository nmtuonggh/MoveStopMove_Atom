using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraFollow : MonoBehaviour
{
    [SerializeField] private Camera camera;
    [SerializeField] private Vector3 offset;

    private void LateUpdate()
    {
        camera.transform.position = transform.position + offset;
    }
}
