using UnityEngine;
using System.Collections;

public class Movement : MonoBehaviour {

    public GameObject lcontrol;
    public GameObject rcontrol;
    public GameObject head;

    public float speed_move =6;
    public float speed_rotate =1;

    float angle=0;
    float len;
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
    void Update()
    {
        //Movement

        //Detect the Sign of direction
        if (lcontrol.GetComponent<SteamVR_TrackedController>().dirX != 0)
        {
            if (lcontrol.GetComponent<SteamVR_TrackedController>().dirX > 0 && lcontrol.GetComponent<SteamVR_TrackedController>().dirY > 0)
            {
                angle = Mathf.Atan(lcontrol.GetComponent<SteamVR_TrackedController>().dirY / lcontrol.GetComponent<SteamVR_TrackedController>().dirX) * 180 / Mathf.PI;
            }
            if (lcontrol.GetComponent<SteamVR_TrackedController>().dirX < 0 && lcontrol.GetComponent<SteamVR_TrackedController>().dirY > 0)
            {
                angle = 90 + Mathf.Atan(-lcontrol.GetComponent<SteamVR_TrackedController>().dirX / lcontrol.GetComponent<SteamVR_TrackedController>().dirY) * 180 / Mathf.PI;
            }
            if (lcontrol.GetComponent<SteamVR_TrackedController>().dirX < 0 && lcontrol.GetComponent<SteamVR_TrackedController>().dirY < 0)
            {
                angle = 180 + Mathf.Atan(lcontrol.GetComponent<SteamVR_TrackedController>().dirY / lcontrol.GetComponent<SteamVR_TrackedController>().dirX) * 180 / Mathf.PI;
            }
            if (lcontrol.GetComponent<SteamVR_TrackedController>().dirX > 0 && lcontrol.GetComponent<SteamVR_TrackedController>().dirY < 0)
            {
                angle = 270 + Mathf.Atan(-lcontrol.GetComponent<SteamVR_TrackedController>().dirX / lcontrol.GetComponent<SteamVR_TrackedController>().dirY) * 180 / Mathf.PI;
            }
        }
        else {
            if (lcontrol.GetComponent<SteamVR_TrackedController>().dirY >= 0)
            {
              angle = 90;
            }
            else
            {
                angle = 270;
            }
        }

        //Find The Length 
        len = Mathf.Sqrt(Mathf.Pow(lcontrol.GetComponent<SteamVR_TrackedController>().dirX, 2) + Mathf.Pow(lcontrol.GetComponent<SteamVR_TrackedController>().dirY, 2));

        //Find The Angle 
        angle = angle - head.GetComponent<Transform>().rotation.eulerAngles.y + transform.rotation.eulerAngles.y;
        angle = angle / 180 * Mathf.PI;  //change rad to degree

        transform.Translate(len * Mathf.Cos(angle) * speed_move * Time.deltaTime, 0, len * Mathf.Sin(angle) * speed_move * Time.deltaTime);
        //transform.Translate(lcontrol.GetComponent<SteamVR_TrackedController>().dirX * speed_move * Time.deltaTime, 0, lcontrol.GetComponent<SteamVR_TrackedController>().dirY * speed_move * Time.deltaTime);


        //Rotation
        Vector3 newRotation = new Vector3(0, rcontrol.GetComponent<SteamVR_TrackedController>().dirX * speed_rotate, 0);
        transform.Rotate(newRotation);
    }
}
