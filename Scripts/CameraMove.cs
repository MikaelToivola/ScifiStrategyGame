using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{

    public float movementEdge = 7; // mouse distance from screen edge, that starts to move the camera. (screen / movementEdge)
    public float minZoom = 5f;
    public float maxZoom = 30f;
    public float rotateSpeed = 1f;
    public float moveSpeed = 0.8f;
    public List<float> MapLimitsEWNS; // RIGHT, LEFT, UP, DOWN

    // Use this for initialization
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        Move();
        Turn();
        Zoom();
    }

    void Zoom()
    {
        if (Input.mouseScrollDelta.y != 0)
        {

            float newHeight = transform.position.y - Input.mouseScrollDelta.y;

            if (newHeight < minZoom)
            {
                newHeight = minZoom;
            }
            else if (newHeight > maxZoom)
            {
                newHeight = maxZoom;
            }
            transform.position = new Vector3(transform.position.x, newHeight, transform.position.z);

        }
    }
    void Turn()
    {
        if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.Rotate(0, -rotateSpeed, 0);//-90 degrees
        }
        else if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.Rotate(0, rotateSpeed, 0);
        }
    }
    void Move()
    {

        if (Input.mousePosition.x > Screen.width - Screen.width / movementEdge && Input.mousePosition.x < Screen.width)
        {   //Mouse at RIGHT edge of screen
            float speedMultiplier = (Screen.width / movementEdge - (Screen.width - Input.mousePosition.x)) / 100;//Speed based on mouse positions distance from move edge
            transform.Translate(Vector3.right * speedMultiplier * moveSpeed);

        }
        else if (Input.mousePosition.x < Screen.width / movementEdge && Input.mousePosition.x > 0)//Mouse at LEFT edge of screen
        {
            float speedMultiplier = (Screen.width / movementEdge - Input.mousePosition.x) / 100;
            transform.Translate(Vector3.left * speedMultiplier * moveSpeed);

        }
        if (Input.mousePosition.y > Screen.height - Screen.height / movementEdge && Input.mousePosition.y < Screen.height)//Mouse at TOP edge of screen
        {
            float speedMultiplier = (Screen.height / movementEdge - (Screen.height - Input.mousePosition.y)) / 100;
            transform.Translate(Vector3.forward * speedMultiplier * moveSpeed);


        }
        else if (Input.mousePosition.y < Screen.height / movementEdge && Input.mousePosition.y > 0) //Mouse at BOTTOM edge of screen
        {

            float speedMultiplier = (Screen.height / movementEdge - Input.mousePosition.y) / 100;
            transform.Translate(Vector3.back * speedMultiplier * moveSpeed);

        }

        //IF Camera out of game area
        if (this.transform.position.x > MapLimitsEWNS[0] ) 
        {
            transform.position = new Vector3(MapLimitsEWNS[0], transform.position.y, transform.position.z);
        }

        if (this.transform.position.x < MapLimitsEWNS[1])
        {
            transform.position = new Vector3(MapLimitsEWNS[1], transform.position.y, transform.position.z);
        }

        if (this.transform.position.z > MapLimitsEWNS[2]) 
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, MapLimitsEWNS[2]);
        }

        if (this.transform.position.z < MapLimitsEWNS[3]) 
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, MapLimitsEWNS[3]);
        }
    }
}
