using System.Collections;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine;


public class PlayerNavAgent : MonoBehaviour {

    bool isAlive = true;
    //public int TeamNumber = 0;
	public bool isAiming = false; //Manual Aim
	NavMeshAgent navAgent;
	Animator anim;
    Sight sightScript;
	public Vector3 moveTarget;

    //List<Vector3> path;
    LineRenderer pathRender;

    public float turnSpeed = 1f;

    void Start () {
		navAgent = GetComponent<NavMeshAgent> ();
        sightScript = GetComponent<Sight>();
        if (sightScript == null){
            Debug.LogError("NO Sight Script");
        };
        moveTarget = transform.position;
		anim = transform.GetChild (0).GetComponent<Animator> ();

        pathRender = transform.Find("PathRender").GetComponent<LineRenderer>();

    }

    void Update () {
        if (isAlive)
        {
		    if (navAgent.hasPath) {//IS MOVING
			    anim.SetFloat ("speed", 1);
                if(this.tag == "team0") RenderPath();
		    } else {
			    anim.SetFloat("speed", 0);
                anim.SetFloat("walkAngle", 0);
            }

		    if (moveTarget != null) {
			    navAgent.destination = moveTarget;

                // Angle for side step Animation Blend Tree
                Vector3 dir = moveTarget - this.transform.position;
                float angle = Vector3.Angle(transform.forward, dir);

                Vector3 localTragetPos = this.transform.InverseTransformPoint(moveTarget);
                if (localTragetPos.x < 0) angle *= -1;

                anim.SetFloat("walkAngle", angle);
            }
            else
            {
                anim.SetFloat("walkAngle", 0);
            }

            if (sightScript.enemiesInSight.Count > 0) {//ENEMIES IN SIGH. 
            
			    navAgent.updateRotation = false;

                Rotate();
		    } else {
			    navAgent.updateRotation = true;

		    }
        }
	}

    public void Die()
    {
        isAlive = false;
        navAgent.destination = transform.position;
    }

    void Rotate() //TURN TO FACE FIRST SEEN ENEMY
    {
        Vector3 targetDir = sightScript.enemiesInSight[0].transform.position - this.transform.position;

        // The step size is equal to speed times frame time.
        float step = turnSpeed * Time.deltaTime;

        Vector3 newDir = Vector3.RotateTowards(transform.forward, targetDir, step, 0.0f);
        Debug.DrawRay(this.transform.position, newDir, Color.yellow);

        // Move our position a step closer to the target.
        transform.rotation = Quaternion.LookRotation(newDir);

        
    }

    void RenderPath()
    {
        //path[0] = transform.position;

        pathRender.SetPosition(0, transform.InverseTransformPoint(transform.position));
        pathRender.SetPosition(1, transform.InverseTransformPoint(moveTarget));

    }


}
