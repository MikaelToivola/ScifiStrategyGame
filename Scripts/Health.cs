using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Health : MonoBehaviour {

    public float maxHealth = 100f;
    float currentHealth;
    public GameObject corpsePrefab;
    MasterController mController;
    BattleMouse bMouse;
    PlayerNavAgent navAgentScript;
    Sight sight;
    void Start () {
        currentHealth = maxHealth;

        navAgentScript = GetComponent<PlayerNavAgent>();
        sight = GetComponent<Sight>();
        mController = GameObject.FindGameObjectWithTag("MasterController").GetComponent<MasterController>();
        //bMouse = GameObject.FindGameObjectWithTag("CamCapsule").GetComponent<BattleMouse>();

    }

    void Update () {
		
	}

    public float GetCurrentHealth()
    {
        return currentHealth;
    }
    public void TakeDamage(float damage)
    {
        currentHealth -= damage;
        //Debug.Log(this.gameObject.name + " health = " + currentHealth);

        if (currentHealth < 1)
        {

            mController.RemoveSoldier(this.tag, this.gameObject);
            //bMouse.
            navAgentScript.Die();
            sight.Die();
            Debug.Log(this.gameObject.name + " is DEAD");
            Instantiate(corpsePrefab, transform.position, Quaternion.identity);//, this.transform
            //this.transform.GetChild(0).gameObject.SetActive(false);
            gameObject.SetActive(false);
            //Destroy(this.gameObject);
        }
        else
        {
            //Debug.Log(this.gameObject.name + " Still alive");
            //ToDo: Damage effects 
        }
    }
}
