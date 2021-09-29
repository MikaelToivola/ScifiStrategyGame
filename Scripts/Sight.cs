using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sight : MonoBehaviour {

    bool isAlive = true;

    public List<GameObject> enemiesInSight;

    public float shoulderHeigth = 1.5f;
    //public float gunLength = 0.5f;


    MasterController mController;

    LineRenderer laserPoiter;
    public GameObject gunPos;
    Transform gun_end;

    //float timerFull = 2f;
    float timer;
    public float fireRate = 8f;
    public float damage = 10f;
    public float accuracy = 90f;

    public GameObject muzzleflash;
    public GameObject hitEffect;
    public GameObject tracerEffect;
    Animator anim;
    string teamTag = "team0";
    string enemyTeamTag = "team1";
    AudioSource audio;
    public AudioClip gunShot;
    void Start () {
        mController = GameObject.FindGameObjectWithTag("MasterController").GetComponent<MasterController>();
        laserPoiter = transform.Find("LaserPointer").GetComponent<LineRenderer>();
        anim = transform.GetChild(0).GetComponent<Animator>();
        audio = GetComponent<AudioSource>();
        gun_end = gunPos.transform;


        teamTag = this.tag;
        if(teamTag == "team0") enemyTeamTag = "team1";
        else enemyTeamTag = "team0";
    }

    void Update()
    {
        if (isAlive)
        {
            ManageEnemyList();
            if (enemiesInSight.Count > 0)//ENEMIES IN SIGH. 
            {
                Aim();
            }
            else
            {
                timer = fireRate; //set first shot speed
                laserPoiter.enabled = false;
                anim.SetFloat("aimAngle", 0); // look straight
            }

        }
        else enemiesInSight.Clear();
    }

    public void Die()
    {
        isAlive = false;
    }

    void Aim()
    {
        //Line ORIGIN
        Vector3 aimPos = new Vector3(this.transform.position.x,
            this.transform.position.y + shoulderHeigth, this.transform.position.z );
            
        //Line destination
        
        Vector3 target = new Vector3(enemiesInSight[0].transform.position.x,
            enemiesInSight[0].transform.position.y + shoulderHeigth, enemiesInSight[0].transform.position.z);

        Vector3 dir = target - aimPos; //Direction Vector

        //Angle for AIM Animation Blend Tree
        float angle = Vector3.Angle(dir, transform.forward);

        Vector3 localTragetPos = this.transform.InverseTransformPoint(target);
        if (localTragetPos.x < 0) angle *= -1;

        anim.SetFloat("aimAngle", angle);

        if (teamTag == "team0")// only Player has lazersight
        {

            laserPoiter.enabled = true;
            laserPoiter.SetPosition(0, transform.InverseTransformPoint(gun_end.position));
            laserPoiter.SetPosition(1, transform.InverseTransformPoint(target));
            //Debug.DrawLine(aimPos, hit.point, Color.red);
        }

        timer -= Time.deltaTime;
        if (timer <= 0)
        {
            timer = fireRate * Random.Range(0.5f, 1f);
            
            Fire(target, dir.magnitude);
        }
    }

    void Fire(Vector3 targetPos, float range)
    {
        anim.SetTrigger("fire");//Play animation
        if(audio)audio.PlayOneShot(gunShot);

        Vector3 spawnPos = gun_end.position;//Spawn muzzleflash
        Quaternion rotationOfBarrel = gun_end.rotation;

        Instantiate(muzzleflash, spawnPos, rotationOfBarrel);
        if(Random.Range(0, 100f) > 75f) Instantiate(tracerEffect, spawnPos, rotationOfBarrel);


        //Calculate Hit chance
        //float roll = Random.Range(0, 100f) ;
        float prosent = Random.Range(0, 100f);
        //range accuracy
        Debug.Log("roll: " + prosent + " range: " + range + " " );

        if(range > 45f)
        {
            prosent -= 40f;
        }else if(range > 20f)
        {
            prosent -= 10f;
        }
        else
        {
            prosent += 50f;
        }
        Debug.Log("prosent " + prosent);


        if (prosent > 100f - accuracy)//Roll Hit chance
        {
            Instantiate(hitEffect, targetPos, Quaternion.identity);
            Health enemyHealth = enemiesInSight[0].GetComponent<Health>();
            enemyHealth.TakeDamage(damage);
            Debug.Log(this.name + " Hit");
        }
        else Debug.Log(this.name + " MISS");

    }

    void ManageEnemyList()
    {
        enemiesInSight.Clear();

        List<GameObject> enemies;
        if (teamTag == "team0") enemies = mController.enemies;
        else enemies = mController.playerSoldiers;
        
            
        if (enemies.Count < 1)  // list empty
        {
            //Debug.Log("All enemies down");
        }else
        {
            for (int i = 0; enemies.Count > i; i++)
            {
                //RAY ORIGIN
                Vector3 aimPos = new Vector3(this.transform.position.x,
                    this.transform.position.y + shoulderHeigth, this.transform.position.z);
                //RAY destination
                Vector3 target = new Vector3( enemies[i].transform.position.x, 
                    enemies[i].transform.position.y + shoulderHeigth, enemies[i].transform.position.z);

                Ray lineOfSight = new Ray(aimPos, target - aimPos);
                RaycastHit hit;

                if (Physics.Raycast(lineOfSight, out hit)){

                    if(hit.collider.gameObject.tag == enemyTeamTag )//or gam obj is null
                    {
                        GameObject enemy = hit.collider.gameObject;

                        //FIRST CHECK IF ENEMY IS ALREADY ON THE enemiesInSight LIST
                        bool enemyAlreadySeen = false;
                        for (int j = 0; enemiesInSight.Count > j; j++)
                        {
                            if (enemiesInSight[j] == enemy)
                            {
                                enemyAlreadySeen = true;
                            }
                        }
                        //ADD ENEMY TO enemiesInSight LIST
                        if (enemyAlreadySeen == false){
                            enemiesInSight.Add(hit.collider.gameObject);
                        
                        }
                    
                    }
                    else //REMOVE ENEMY FROM LIST
                    {
                        enemiesInSight.Remove(enemies[i]);

                        Debug.DrawLine(aimPos, hit.point, Color.green);
                    }

                }
            }
        }

    }
		
}

