using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Floater : MonoBehaviour
{
    [SerializeField] private Rigidbody body;
    [SerializeField] private float depthBeforeSubmerssion = 1f;
    [SerializeField] private float dispAmnt = 3f;
    [SerializeField] private int numOfFloatingPoints = 1;
    [SerializeField] private float waterDrag = 0.99f;
    [SerializeField] private float angularDrag = 0.5f;

    private void FixedUpdate()
    {
        body.AddForceAtPosition(Physics.gravity / numOfFloatingPoints, transform.position, ForceMode.Acceleration);
        float waveHeight = waveManager.instance.GetWaveHeight(transform.position.x, transform.position.z) + waveManager.instance.transform.position.y;
        if (transform.position.y < waveHeight)
        {
            float dispMultiplier = Mathf.Clamp01((waveHeight - transform.position.y) / depthBeforeSubmerssion) * dispAmnt;
            body.AddForceAtPosition(new Vector3(0f, Mathf.Abs(Physics.gravity.y) * dispMultiplier, 0f), transform.position, ForceMode.Acceleration);
            body.AddForce(dispMultiplier * -body.velocity * waterDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
            body.AddTorque(dispMultiplier * -body.angularVelocity * angularDrag * Time.fixedDeltaTime, ForceMode.VelocityChange);
        }
    }
}
