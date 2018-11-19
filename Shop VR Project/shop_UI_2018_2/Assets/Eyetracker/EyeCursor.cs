using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using aGlassDKII;

public class EyeCursor : MonoBehaviour {
	public GameObject cursor;
    public GameObject cursor2;
    public Canvas cursorCanvas;
	float x, y;
    void Start()
    {
        cursorCanvas = cursor.transform.parent.GetComponent<Canvas>();
    }

	void Update () {
        //gaze.GetPos (lighter.gameObject, ref x, ref y);
        x = GameGaze.x;
        y = GameGaze.y;
        float dis = Vector3.Dot(MainController.currentPointerCamera.transform.forward, (MainController.Instance("EyeCursor").rayHitPos - MainController.currentPointerCamera.transform.position));
        if (dis > 0)
            cursorCanvas.planeDistance = dis;

        cursor.transform.localPosition = new Vector2((x - 0.5f) * 1512, (0.5f - y) * 1680);
        if (cursor2)
            cursor2.transform.localPosition = new Vector2((x - 0.5f) * 1512, (0.5f - y) * 1680);
    }
}
