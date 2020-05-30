using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DetectOnGround : MonoBehaviour
{
    public Collider Collider;
    public PlayerController PlayerController;

    void OnTriggerEnter(Collider collider)
    {
        Debug.Log("OnCollisionEnter");
        PlayerController.IsGrounded = true;
    }
}
