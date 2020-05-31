using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Collider))]
public class DetectOnGround : MonoBehaviour
{
    public PlayerController PlayerController;
    public float YLockCooldownInSeconds = 1.0f;

    private int _numActiveCollisions = 0;
    private float _elapsedTime = 0f;
    private bool _yLockOnCooldown = false;

    void FixedUpdate()
    {
        _elapsedTime += Time.deltaTime;
        if (_numActiveCollisions == 0)
            UnlockY();

        if (_yLockOnCooldown && _elapsedTime > YLockCooldownInSeconds)
            _yLockOnCooldown = false;

        if (!_yLockOnCooldown && _numActiveCollisions > 0)
            PlayerController.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezePositionY | RigidbodyConstraints.FreezeRotation;
    }

    public void UnlockY()
    {
        _elapsedTime = 0f;
        _yLockOnCooldown = true;
        PlayerController.GetComponent<Rigidbody>().constraints = RigidbodyConstraints.FreezeRotation;
    }

    void OnTriggerEnter(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Blocks"))
        {
            PlayerController.IsGrounded = true;
            _numActiveCollisions += 1;
        }
    }

    void OnTriggerExit(Collider collider)
    {
        if (collider.gameObject.layer == LayerMask.NameToLayer("Blocks"))
            _numActiveCollisions -= 1;
    }
}
