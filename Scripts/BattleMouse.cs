using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class BattleMouse : MonoBehaviour {

	GameObject selectedObject;
	List <GameObject> selectedObjects;

    //private bool mouse_over = false;
    GameObject target;
	Vector2 direction;

	public Vector3 moveTarget;
	public GameObject moveTargetPrefab;

	float mouseHeldDownTime;
	public float crossHareAppearTime = 0.5f;
	public float minFireTime = 2f;
	public GameObject crossHare;
	LineRenderer line;

    Canvas canvas;
    UImanager uiManager;
    GameObject unitHUD;
	bool freezeTime = false;
    MasterController masterController;
	// Use this for initialization
	void Start () {
		selectedObjects = new List<GameObject>();
        masterController = GameObject.Find("MasterController").GetComponent<MasterController>();
        canvas = GameObject.Find("Canvas").GetComponent<Canvas>();
        uiManager = canvas.GetComponent<UImanager>();
        unitHUD = canvas.transform.GetChild(0).gameObject;
        Time.timeScale = 1;
    }
	
	// Update is called once per frame
	void Update () {
		if (Input.GetKeyDown (KeyCode.Space) && freezeTime == false) {
			Time.timeScale = 0.05f;// SLOW TIME
			freezeTime = true;
		}else if (Input.GetKeyDown (KeyCode.Space) && freezeTime == true) {
			Time.timeScale = 1;// UN SLOW TIME
			freezeTime = false;
		}

		if (Input.GetMouseButtonDown (1)) {
            unitHUD.SetActive(false);
            ClearSelection();
                        
		}
        
        if(!uiManager.MouseOver()) MyRaycast(); //Only raycast when not over UI

        if(unitHUD.activeSelf && selectedObject != null) UpdateUI();
	}

    public void ClearSelection()
    {
        for (int i = 0; i < selectedObjects.Count; i++)
        {

            selectedObjects[i].transform.Find("Highlight").gameObject.SetActive(false);
        }
        selectedObject = null;
        selectedObjects.Clear();
    }

    public void CycleSoldiers(int indexMove)
    {
        ClearSelection();
        List<GameObject> playerSoldiers = masterController.playerSoldiers;
        if (selectedObject == null)
        {
            selectedObject = playerSoldiers[0];
        }
        else
        {
            int index = playerSoldiers.IndexOf(selectedObject);
            index += indexMove;

            if (index >= playerSoldiers.Count) index = 0;//Jump to the beginning of the list
            else if (index < 0) index = playerSoldiers.Count - 1;
            Debug.Log("playerSoldiers.Count= " + playerSoldiers.Count);
            Debug.Log("index= " + index);
            selectedObject = playerSoldiers[index];
        }
    }

    void UpdateUI()
    {
        GameObject info = unitHUD.transform.GetChild(0).gameObject;
        Health unitHealth = selectedObject.GetComponent<Health>();
        //Debug.Log("Unit: " + selectedObject.name + " " + unitHealth.GetCurrentHealth() + "/" + unitHealth.maxHealth);
        info.GetComponent<Text>().text = "Unit: " + selectedObject.name + "\n" + unitHealth.GetCurrentHealth() + "/" + unitHealth.maxHealth;
    }

    void MyRaycast()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hitInfo;
        if (Physics.Raycast(ray, out hitInfo))
        {
            GameObject ourHitObject = hitInfo.collider.gameObject;

            if (ourHitObject.GetComponent<PlayerNavAgent>() != null)
            {
                MouseOver_playerNavAgent(ourHitObject);
            }
            else if (ourHitObject.tag == "terrain" && Input.GetMouseButtonDown(0) && selectedObject != null)
            {
                if (selectedObject.tag == "team0")
                {
                    moveTarget = new Vector3(hitInfo.point.x, hitInfo.point.y, hitInfo.point.z);
                    //Debug.Log ("hit terrain point " + moveTarget);

                    selectedObject.GetComponent<PlayerNavAgent>().moveTarget = moveTarget;

                    Instantiate (moveTargetPrefab,moveTarget,Quaternion.identity);
                }
            }
            
        }

        //FIRE Aim 
        /*if (selectedObject != null && selectedObject.tag == "team0")
        {
            GameObject crossHare = selectedObject.transform.Find("CrossHare").gameObject;

            //Mouse release
            if (Input.GetMouseButtonUp(0) && mouseHeldDownTime >= minFireTime)
            {
                //selectedObject.GetComponent<ShipAttack> ().FireBroadside();
                mouseHeldDownTime = 0;
                crossHare.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            }
            else if (Input.GetMouseButtonUp(0) && mouseHeldDownTime < minFireTime)
            {
                mouseHeldDownTime = 0;
                crossHare.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = false;
            }

            //Mouse Held Down
            if (Input.GetMouseButton(0))
            {
                mouseHeldDownTime += Time.deltaTime;
                if (mouseHeldDownTime > crossHareAppearTime)
                {

                    selectedObject.GetComponent<PlayerNavAgent>().isAiming = true;

                    /*line = selectedObject.GetComponent<LineRenderer>();
                    line.SetPosition(0, Vector3.zero);
                    line.SetPosition(1, crossHare.transform.localPosition);*/
                    /*
                    crossHare.transform.GetChild(0).gameObject.GetComponent<MeshRenderer>().enabled = true;
                    crossHare.transform.position = new Vector3(hitInfo.point.x, 0, hitInfo.point.z);
                }
            }
            else
            {
                mouseHeldDownTime = 0;
            }
        }*/
    }

	void MouseOver_playerNavAgent (GameObject ourHitObject){

		if (Input.GetMouseButtonDown (0) && Input.GetKey (KeyCode.LeftShift) && ourHitObject.tag == "team0") {//multiple select
			selectedObject = ourHitObject;
			selectedObjects.Add (selectedObject);

            selectedObject.transform.Find("Highlight").gameObject.SetActive(true);
        }
        else if(Input.GetMouseButtonDown(0) && ourHitObject.tag == "team0")//single select
        { 
            ClearSelection();
			selectedObject = ourHitObject;
            selectedObjects.Add (selectedObject);

            selectedObject.transform.Find("Highlight").gameObject.SetActive(true);
            unitHUD.SetActive(true);
            

        }
        else if(Input.GetMouseButtonDown(0) && ourHitObject.tag == "team1"){
			
			if(selectedObject != null){

				for (int i = 0; i < selectedObjects.Count; i++) {
					Debug.Log ("selectedObjects [" + i + "] = " + selectedObjects [i].name);

					PlayerNavAgent Script = selectedObjects [i].GetComponent<PlayerNavAgent> ();

                    Debug.Log("Target " + ourHitObject.name + "(function not written)");

				}



			}

		}
	}
}
