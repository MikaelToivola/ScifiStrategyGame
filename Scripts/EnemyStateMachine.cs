using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyStateMachine : MonoBehaviour
{
    Sight sight;
    PlayerNavAgent navAgent;

    public float attackDistance = 20f;

    private int closestEnemy = 0;
    float shortestDist = 100f;
    private EnemyState _currentState;
    // Start is called before the first frame update
    void Start()
    {
        sight = GetComponent<Sight>();
        navAgent = GetComponent<PlayerNavAgent>();
        
    }

    // Update is called once per frame
    void Update()
    {
        //Debug.Log(_currentState);
        switch (_currentState)
        {
            case EnemyState.Patrol:
            {
                    //Debug.Log("Patrolling");
                if (sight.enemiesInSight.Count != 0) {
                    _currentState = EnemyState.Chase;
                    return;
                }

                if (navAgent.moveTarget == null) GetDestination();

                break; // if needed ??
            }
            case EnemyState.Chase:
            {
                //Debug.Log("Chase is on");

                if (sight.enemiesInSight.Count == 0)
                {
                    _currentState = EnemyState.Patrol;
                    return;
                }
                ClosestEnemy();//loop enemiesVisible[0] for closest one

                if (shortestDist < attackDistance)
                {
                    _currentState = EnemyState.Fire;
                    return;
                }

                navAgent.moveTarget = sight.enemiesInSight[closestEnemy].transform.position;

                break;
            }
            case EnemyState Fire:
            {
                //Debug.Log("EnemyState = Fire");
                navAgent.moveTarget = transform.position;

                if (sight.enemiesInSight.Count == 0)
                {
                    _currentState = EnemyState.Patrol;
                    return;
                }
                ClosestEnemy();
                if (shortestDist > attackDistance)
                {
                    _currentState = EnemyState.Chase;
                    return;
                }

                break;
            }
        }
    }

    public enum EnemyState
    {
        Patrol,
        Chase,
        Fire
    }

    void GetDestination()
    {
        Debug.Log("No Patrol Path");
        //Get Closest Patroll point in Loop
    }

    void ClosestEnemy()
    {
        for (int i = 0; i < sight.enemiesInSight.Count; i++)
        {
            Vector3 targetPos = sight.enemiesInSight[i].transform.position;
            float dist = Vector3.Distance(targetPos, transform.position);
            if (dist < shortestDist)
            {
                shortestDist = dist;
                closestEnemy = i;
            }
        }
    }
}
