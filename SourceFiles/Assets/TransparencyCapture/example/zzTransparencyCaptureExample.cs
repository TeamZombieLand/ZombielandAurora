using System.Collections;
using UnityEngine;

public class zzTransparencyCaptureExample:MonoBehaviour
{
    public Texture2D capturedImage;
    public Transform cameraTransform;

    void Start()
    {
        lastMousePosition = Input.mousePosition;
    }

    public IEnumerator capture()
    {
        //capture whole screen
        Rect lRect = new Rect(0f,0f,Screen.width,Screen.height);
        if(capturedImage)
            Destroy(capturedImage);

        yield return new WaitForEndOfFrame();
        //After Unity4,you have to do this function after WaitForEndOfFrame in Coroutine
        //Or you will get the error:"ReadPixels was called to read pixels from system frame buffer, while not inside drawing frame"
        zzTransparencyCapture.captureScreenshot("Tutorial" +  Random.Range(0,99999).ToString()+".png");
    }

    Vector3 lastMousePosition;
    public void Update()
    {
        if (Input.GetKeyDown(KeyCode.C))
            StartCoroutine(capture());
        if (Input.GetKeyDown(KeyCode.S))
            Destroy(capturedImage);

        //Update camera position
        
    }

   
}