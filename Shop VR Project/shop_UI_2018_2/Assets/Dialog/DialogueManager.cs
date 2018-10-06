using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DialogueManager : MonoBehaviour {
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
            new Dialogue("Shop", new string[] {"open the shop"}),
            new Dialogue("Inventory", new string[] { "open the inventory" }),
            new Dialogue("View", new string[] { "rotate view" }),
            new Dialogue("Setting", new string[] { "open setting menu" }),
            new Dialogue("Scale", new string[] { "scale the object" }),
            new Dialogue("Rotate", new string[] { "rotate the object" }),
            new Dialogue("Put back", new string[] { "put back the object" }),
            new Dialogue("Draw", new string[] { "draw on the object" }),
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
	public void StartDialogue (int index)
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
	void EndDialogue()
	{
		animator.SetBool("IsOpen", false);
	}
    */

}
