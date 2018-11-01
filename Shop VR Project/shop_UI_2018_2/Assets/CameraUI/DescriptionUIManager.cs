using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DescriptionUIManager : MonoBehaviour {
    [Header("OBJECT")]
    public GameObject notificationObject;
    private Animator notificationAnimator;
    private Text title;
    private Text description;

    private Coroutine Coroutine;

    private Dialogue[] dialogue;

    private Queue<string> sentences;

    // Use this for initialization
    void Start()
    {
        sentences = new Queue<string>();

        dialogue = new Dialogue[] {
            //Base panel
    /*0*/   new Dialogue("Shop", new string[] {"open the shop"}),
    /*1*/   new Dialogue("View", new string[] { "rotate view" }),
    /*2*/   new Dialogue("Setting", new string[] { "open setting menu" }),
    /*3*/   new Dialogue("Inventory", new string[] { "open the inventory" }),
            //Model panel
    /*4*/   new Dialogue("Properties", new string[] { "open object properties menu" }),
    /*5*/   new Dialogue("View", new string[] { "rotate view" }),
    /*6*/   new Dialogue("Put back", new string[] { "put back the object" }),
    /*7*/   new Dialogue("Special Function", new string[] { "open speacial function panel" }),
            //Speacial panel
   /*8*/    new Dialogue("Paint", new string[] { "paint color on the object" }),
   /*9*/    new Dialogue("Auto Align", new string[] { "auto align the object position" }),
   /*10*/   new Dialogue("Slice", new string[] { "slicing the model of object" }),
   /*11*/   new Dialogue("Auto Scale", new string[] { "auto scaling the object size" }),

             //Select Tooltips
   /*12*/    new Dialogue("Object1", new string[] { "select object 1" }),
   /*13*/    new Dialogue("Object2", new string[] { "select object 2" }),
   /*13*/    new Dialogue("Select Fail", new string[] { "Please select two different object" })
        };

        notificationAnimator = notificationObject.GetComponent<Animator>();
        title = notificationObject.transform.Find("Title").GetComponent<Text>();
        description = notificationObject.transform.Find("Description").GetComponent<Text>();
    }
    /*
    public void ShowNotification()
    {
        //notificationObject.SetActive(true);
        titleObject.text = titleText;
        descriptionObject.text = descriptionText;

        notificationAnimator.Play(animationNameIn);

    }
    */
	public void StartDescription (int index)
	{
		title.text = dialogue[index].name;
        notificationAnimator.SetBool("IsOpen", true);
        notificationAnimator.Play("Fade Effect");

        sentences.Clear();

		foreach (string sentence in dialogue[index].sentences)
		{
			sentences.Enqueue(sentence);
		}

		DisplayNextSentence();
	}

	public void DisplayNextSentence ()
	{
        if (sentences.Count == 0)
		{
            notificationAnimator.SetBool("IsOpen", false);
            return;
		}

		string sentence = sentences.Dequeue();
        if(Coroutine != null)
        {
            StopCoroutine(Coroutine);
        }
        Coroutine = StartCoroutine(TypeSentence(sentence));
	}

	IEnumerator TypeSentence (string sentence)
	{
		description.text = "";

        yield return new WaitForSeconds(0.2f);

        foreach (char letter in sentence.ToCharArray())
		{
			description.text += letter;
			yield return null;
		}
        yield return new WaitForSeconds(2f);
        DisplayNextSentence();
    }

    /*
	void EndDescription()
	{
		animator.SetBool("IsOpen", false);
	}
    */

}
