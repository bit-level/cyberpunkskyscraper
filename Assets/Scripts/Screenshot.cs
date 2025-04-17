using UnityEngine;
using UnityEditor;
using System.IO;

public class ScreenshotTool : EditorWindow
{
    [MenuItem("Tools/Screenshot")]
    private static void TakeScreenshot()
    {
        string folderPath = "Screenshots";
        if (!Directory.Exists(folderPath))
        {
            Directory.CreateDirectory(folderPath);
        }

        string timestamp = System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss");
        string fileName = $"Screenshot_{timestamp}.png";
        string fullPath = Path.Combine(folderPath, fileName);

        ScreenCapture.CaptureScreenshot(fullPath);
        Debug.Log($"Screenshot saved to: {fullPath}");
    }
}