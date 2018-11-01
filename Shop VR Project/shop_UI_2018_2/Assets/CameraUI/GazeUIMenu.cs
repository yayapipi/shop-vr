using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class GazeUIMenu : MonoBehaviour {

    [System.Serializable]
    public class eventButton
    {
        public Sprite ButtonIcon;
        public UnityEvent OnClick = new UnityEvent();
        public UnityEvent OnHoverEnter = new UnityEvent();
        public UnityEvent OnHoverExit = new UnityEvent();
        public UnityEvent OnHover = new UnityEvent();
    }

    public eventButton[] buttons = new eventButton[3];
    private bool[] buttonOnHover = new bool[3];
    
    // Use this for initialization
    void Start ()
    {
		for(int i = 0; i < 4; i++)
        {
            buttonOnHover[i] = false;
        }
	}
	
	// Update is called once per frame
	void Update ()
    {
	    for(int i = 0; i < 4; i++)
        {
            if (buttonOnHover[i])
            {
                buttons[i].OnHover.Invoke();
            }
        }
	}

    public void pointerEnter(int index)
    {
        buttons[index].OnHoverEnter.Invoke();
        buttonOnHover[index] = true;
    }

    public void pointerExit(int index)
    {
        buttons[index].OnHoverExit.Invoke();
        buttonOnHover[index] = false;
    }

    public void pointerClick(int index)
    {
        buttons[index].OnClick.Invoke();
    }
}
