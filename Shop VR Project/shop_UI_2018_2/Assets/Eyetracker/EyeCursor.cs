using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using aGlassDKII;

public class EyeCursor : MonoBehaviour {
	public GameObject cursor;
	float x, y;
    void Start()
    {
    }

	void Update () {
		//gaze.GetPos (lighter.gameObject, ref x, ref y);
        x = GameGaze.x;
        y = GameGaze.y;
		cursor.transform.localPosition = new Vector2 ((x - 0.5f) * 1512, (0.5f - y) * 1680);
	}
}
