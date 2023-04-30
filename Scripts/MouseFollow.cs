using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseFollow : MonoBehaviour
{
public Transform followPlayer;
private Transform cameraTransform;

public Vector3 playerOffset;
public float moveSpeed = 10f;

private void Start() 
{
    cameraTransform = transform;    
}

public void Settarget(Transform newTransformTarget)
{
    followPlayer = newTransformTarget;
}

private void LateUpdate() 
{
    if (followPlayer != null)
    
        cameraTransform.position = Vector3.Lerp(cameraTransform.position, followPlayer.position + playerOffset, moveSpeed * Time.deltaTime);
    
}

}
