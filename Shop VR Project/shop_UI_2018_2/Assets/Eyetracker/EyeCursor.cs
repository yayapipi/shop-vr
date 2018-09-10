using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using aGlassDKII;

public class EyeCursor : MonoBehaviour {
	GameGaze gaze;
	public GameObject cursor;
	float x, y;
    void Start()
    {
		gaze = GameObject.Find ("Gaze").GetComponent<GameGaze>();
    }

	void Update () {
		//gaze.GetPos (lighter.gameObject, ref x, ref y);
        x = aGlass.Instance.GetGazePoint().x;
        y = aGlass.Instance.GetGazePoint().y;
		cursor.transform.localPosition = new Vector2 ((x - 0.5f) * 1512, (0.5f - y) * 1680);
	}
}
