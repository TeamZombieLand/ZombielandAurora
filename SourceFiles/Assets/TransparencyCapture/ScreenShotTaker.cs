using UnityEngine;
using System.IO;

public class ScreenShotTaker : MonoBehaviour
{
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
        {
            ScreenCapture.CaptureScreenshot(Application.dataPath + "/car.png");
        }
    }



}