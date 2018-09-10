using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GetWatch : MonoBehaviour
{
    private bool clickable;
    private Animator animator;
    private GameObject item;
    private GameObject UI_text;

    void Start()
    {
        animator = GetComponent<Animator>();
        clickable = false;
        item = this.gameObject.transform.GetChild(0).gameObject;
        UI_text = this.gameObject.transform.GetChild(1).gameObject;
    }

    void FixedUpdate()
    {

    }

    // Update is called once per frame
    void Update () {
		
	}

    public void Watch()
    {
        animator.SetBool("Watch", true);
        //clickable = false;

        //Debug.Log("Watch\tclickable = " + clickable);
    }

    public void Unwatch()
    {
        animator.SetBool("Watch", false);
        clickable = false;

        //Action
        item.GetComponent<Renderer>().material.color = new Color(1f, 1f, 1f);
        UI_text.SetActive(false);

        //Debug.Log("UnWatch\tclickable = " + clickable);
    }

    //Call by animation
    public void AllowClick()
    {
        //player must look the object while Clicking 
        if (animator.GetBool("Watch"))
        {
            clickable = true;

            //Action
            item.GetComponent<Renderer>().material.color = new Color(1f, 0.82f, 0.45f);
            UI_text.SetActive(true);

            //Debug.Log("AllowClick");
        }
    }

    public void Click()
    {
        //Action
        if (clickable)
            item.GetComponent<Animation>().Play("cube");

        //Debug.Log("Click\tclickable = " + clickable);
    }
}
