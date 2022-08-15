using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BelowWaterCannonBall : MonoBehaviour
{
    [SerializeField] private GameObject despawn;

    private void FixedUpdate()
    {
        Debug.Log(this.name);

        if (transform.position.y < despawn.transform.position.y)
        {
            Debug.Log(this.name);
            Destroy(this);
        }
    }
}
