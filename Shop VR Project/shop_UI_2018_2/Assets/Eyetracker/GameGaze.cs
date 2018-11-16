using UnityEngine;
using UnityEngine.SceneManagement;
using aGlassDKII;

public class GameGaze : MonoBehaviour
{
    private int count;
    private bool eyeClose;
    private bool eyeClose_last;

    //Eye tracker events
    public delegate void EyeTrackerEventManager();
    public static event EyeTrackerEventManager EyeClose;
    public static event EyeTrackerEventManager EyeOpen;
    public static event EyeTrackerEventManager EyeBack;
    public static float x, y;

    void Start ()
	{
        print(aGlass.Instance.aGlassStart());
        eyeClose = false;
        eyeClose_last = false;
    }

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape) && (SceneManager.GetActiveScene ().buildIndex == 0 || SceneManager.GetActiveScene ().buildIndex == 1)) {
			Application.Quit ();
		}
        if (aGlass.Instance.GetEyeValid())
        {
            x = aGlass.Instance.GetGazePoint().x;
            y = aGlass.Instance.GetGazePoint().y;
            eyeClose_last = eyeClose;
            eyeClose = (x <= 0 && y <= 0);
            
            if (eyeClose && !eyeClose_last)
            {
                if (EyeClose != null)
                    EyeClose();
            }
            else if (!eyeClose && eyeClose_last)
            {
                if (EyeOpen != null)
                    EyeOpen();
            }
        }
    }

    public void StartEyeBackEvent()
    {
        if (EyeBack != null)
            EyeBack();
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