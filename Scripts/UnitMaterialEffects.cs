using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UnitMaterialEffects : MonoBehaviour
{

    public GameObject unitModel;
    bool toggle = false;

    //Color normal = new Color(255, 255, 255);
    //Color highlight = new Color(0, 0, 0);
    public Material soldierMat;
    public Material thermalMat;

    void Start()
    {
        //unitModel = transform.Find("soldier").gameObject;
        if(unitModel == null)
        {
            Debug.LogError(this.gameObject.name + "'s soldier model not found");
        }
    }

    // Update is called once per frame
    void Update()
    {
        
        if (Input.GetKeyDown(KeyCode.V))
        {
            toggle = !toggle;

            Material changeTo;
            if (toggle) changeTo = thermalMat;
            else changeTo = soldierMat;

            unitModel.GetComponent<SkinnedMeshRenderer>().material = changeTo;

        }
    }
}
