using UnityEngine;

public class Screenshot : MonoBehaviour
{
#if UNITY_EDITOR
    private static int count = 0;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            string name = string.Format("Screenshot_{0}.png", count++);
            ScreenCapture.CaptureScreenshot(name);
            print("Screenshot saved: " + name);
        }
    }
#endif
}
