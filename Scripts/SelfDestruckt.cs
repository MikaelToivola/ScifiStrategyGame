using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelfDestruckt : MonoBehaviour { 

    public float timer = 5f;
    float timerLeft;

    // Start is called before the first frame update
    void Start()
    {
        timerLeft = timer;
    }

    // Update is called once per frame
    void Update()
    {
        timerLeft -= 1f * Time.deltaTime;

        if(timerLeft < 0)
        {
            Destroy(this.gameObject);
        }
    }
}
