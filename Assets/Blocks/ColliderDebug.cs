using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderDebug : MonoBehaviour
{
    void OnCollisionEnter(Collision collision)
    {
        Debug.Log($"{name} collided with {collision.gameObject.name}");
    }
    void OnTriggerEnter(Collider collider)
    {
        Debug.Log($"{name} collided with {collider.gameObject.name}");
    }
}
