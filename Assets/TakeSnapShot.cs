using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TakeSnapShot : MonoBehaviour
{
    //[SerializeField] Button takeSnap;

    //resolution of the image
    [SerializeField] int resWidth = 1920;
    [SerializeField] int resHeight = 1080;
    // [SerializeField] Text path;
    // [SerializeField] GameObject PlacementIndicator;
    public static bool takingPicture = false;

    //checks if the button is pressed
    // private bool takesnapshot = false;
    private void Start()
    {
        resWidth = Screen.width;
        resHeight = Screen.height;
    }

    //this function creates the name of the file
    public static string screenShotName(int width, int height)
    {
        return string.Format("{0}/screen_{1}X{2}_{3}.png",
            Application.persistentDataPath,
            width,
            height,
            System.DateTime.Now.ToString("yyyy-MM-dd_HH-mm-ss"));
    }

    public void takepicture()
    {
        takingPicture = true;
        //PlacementIndicator.SetActive(false);
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        Camera.main.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.RGB24, false);
        Camera.main.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        screenShot.Apply();
        Camera.main.targetTexture = null;
        RenderTexture.active = null;
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        string fileName = screenShotName(resWidth, resHeight);
        NativeGallery.SaveImageToGallery(bytes, Application.productName, fileName, null);
        //System.IO.File.WriteAllBytes(fileName, bytes);
        // path.text = Application.persistentDataPath;
        // PlacementIndicator.SetActive(true);

        //ScreenCapture.CaptureScreenshot(screenShotName(resWidth, resHeight), 4);
    }
    private void Update()
    {
        if (takingPicture)
        {
            StartCoroutine(istakingPicture());
        }
    }

    IEnumerator istakingPicture()
    {
        yield return new WaitForSeconds(1.5f);
        takingPicture = false;
    }
}