using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class shoot : MonoBehaviour
{
    [SerializeField] private GameObject cannonBall;
    [SerializeField] private float ballSpeed;
    [SerializeField] private Transform POF;
    [SerializeField] private Transform despawn;

    public List<GameObject> balllz = new List<GameObject>();
    private List<GameObject> balllzToRemove = new List<GameObject>();


    void Update()
    {
        if (Input.GetButtonDown("Fire1"))
        {
            fireCannon();
        }
        removeBall();
    }

    private void fireCannon()
    {
        GameObject ball = Instantiate(cannonBall, POF.position, Quaternion.identity);
        Rigidbody rb = ball.AddComponent<Rigidbody>();
        rb.velocity = ballSpeed * POF.forward;

        balllz.Add(ball);

    }

    private void removeBall()
    {
        int i = 0;
        //Debug.Log(balllz.Count);
        foreach (GameObject ball in balllz)
        { 
            if(ball.transform.position.y < despawn.position.y) 
            {
                balllz[i].SetActive(false);
                balllz[i] = null;
                Destroy(balllz[i]);
            }
            i++;
        }
        balllz.RemoveAll(x => x == null);

        
        
    }

}
