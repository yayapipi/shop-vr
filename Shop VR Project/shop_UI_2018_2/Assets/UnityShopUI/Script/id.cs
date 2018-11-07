using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class id : MonoBehaviour {

    public int item_id;
    public Vector3 standard_size;
    public bool isSliced = false;
    public bool isColor = false;
    public bool re_saved = false; //turn true when obj is modified somewhere
    Texture original_texture = null;
    Texture color_texture = null;
}
