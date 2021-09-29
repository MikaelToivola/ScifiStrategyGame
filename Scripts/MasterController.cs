using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MasterController : MonoBehaviour {

    public List<GameObject> playerSoldiers;
    public List<GameObject> enemies;
    public List<GameObject> soldierList;
    Canvas canvas;
    GameObject winnText;
    GameObject lossText;

    float nextLevelTimer;

    void Start () {

        Debug.Log("scene "+ SceneManager.GetActiveScene().buildIndex + " / " + SceneManager.sceneCountInBuildSettings);


        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();

        winnText = canvas.transform.Find("Victory").gameObject;
        lossText = canvas.transform.Find("Failure").gameObject;
        nextLevelTimer = 5f;

        foreach (GameObject go in GameObject.FindGameObjectsWithTag("team0"))
        {
            //FIRST CHECK IF ENEMY IS ALREADY ON THE  LIST
            bool enemyAlreadySeen = false;
            for (int j = 0; playerSoldiers.Count > j; j++)
            {
                if (playerSoldiers[j] == go)
                {
                    enemyAlreadySeen = true;
                }
            }
            //ADD TO  LIST
            if (enemyAlreadySeen == false)
            {
                playerSoldiers.Add(go);
            }


        }
        foreach (GameObject go in GameObject.FindGameObjectsWithTag("team1"))
        {
            //FIRST CHECK IF ENEMY IS ALREADY ON THE  LIST
            bool enemyAlreadySeen = false;
            for (int j = 0; enemies.Count > j; j++)
            {
                if (enemies[j] == go)
                {
                    enemyAlreadySeen = true;
                }
            }
            //ADD TO  LIST
            if (enemyAlreadySeen == false)
            {
                enemies.Add(go);
            }
        }
    }
	
	void Update () {

        if (winnText.activeSelf)
        {
            nextLevelTimer = nextLevelTimer - 1f * Time.deltaTime;
            if(nextLevelTimer < 0)
            {
                int nextScene = SceneManager.GetActiveScene().buildIndex +1;
                if(nextScene >= SceneManager.sceneCountInBuildSettings)
                {
                    nextScene = 0;
                }
                SceneManager.LoadScene(nextScene);
            }
        }

    }
    void GetList(string team, List<GameObject> list)
    {
        soldierList.Clear();
        
        list = soldierList;
        
    }

    public void RemoveSoldier(string team, GameObject deadSoldier)
    {
        Debug.Log("REMOVE " + deadSoldier);
        if(team == "team1")
        {
            enemies.Remove(deadSoldier);
            if(enemies.Count == 0)
            {
                Debug.Log("Winner chicken dinner");
                winnText.SetActive(true);
                //canvas.GetComponent<PauseMenu>().PauseGame();
            }

        }else
        {
            playerSoldiers.Remove(deadSoldier);
            if (playerSoldiers.Count == 0)
            {
                Debug.Log("All units KIA");
                lossText.SetActive(true);
                canvas.GetComponent<PauseMenu>().PauseGame();
            }
        }

    }
}
