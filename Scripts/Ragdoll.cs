using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ragdoll : MonoBehaviour
{
    public float force;
    public Vector3 direction ;
    Rigidbody rig;
    float rand;

    void Start()
    {
        rig = this.GetComponent<Rigidbody>();
        //direction = new Vector3(0, 1, 0);//Random.Range(-1, 0)
        rand = Random.Range(2, -1);
        Debug.Log("ragdoll " + this.name + " force: " + force + " + " + rand);

        rig.AddRelativeForce(direction * force * rand );

        //direction = new Vector3(-5, 0, 0 );

        //rig.AddForceAtPosition(Vector3.forward * force, direction);
    }


}
