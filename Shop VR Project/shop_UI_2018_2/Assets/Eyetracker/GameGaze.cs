using UnityEngine;
using UnityEngine.SceneManagement;
using aGlassDKII;

public class GameGaze : MonoBehaviour
{
    private int count;
    public static bool eyeclose;
    private bool eyeclose_last;
    public static bool eyeclick;

    void Start ()
	{
        print(aGlass.Instance.aGlassStart());
        eyeclose = false;
        eyeclose_last = false;
        eyeclick = false;
    }

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape) && (SceneManager.GetActiveScene ().buildIndex == 0 || SceneManager.GetActiveScene ().buildIndex == 1)) {
			Application.Quit ();
		}
        if (aGlass.Instance.GetEyeValid())
        {
            //print(aGlass.Instance.GetGazePoint().x);
            //print(aGlass.Instance.GetGazePoint().y);
            eyeclose_last = eyeclose;
            eyeclose = (aGlass.Instance.GetGazePoint().x <= 0 && aGlass.Instance.GetGazePoint().y <= 0);
            eyeclick = (!eyeclose && eyeclose_last);
        }
    }

	void OnDestroy ()
	{
        print(aGlass.Instance.aGlassStop());
    }

	public void GetPos (GameObject c, ref float cx, ref float cy)
	{
		if (aGlass.Instance.GetEyeValid()) {
			count = 0;
			if (!c.activeSelf) {
				c.SetActive (true);
			}
            cx = aGlass.Instance.GetGazePoint().x;
            cy = aGlass.Instance.GetGazePoint().y;
		} else {
			count++;
			if (count > 10 && c.activeSelf) {
				c.SetActive (false);
			}
		}
	}
}