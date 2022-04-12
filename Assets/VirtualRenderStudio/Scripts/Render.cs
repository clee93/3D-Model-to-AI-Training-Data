using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using System;
using UnityEditor;
using Random = UnityEngine.Random;
using UnityEngine.Rendering.HighDefinition; //REMOVE THIS LINE FOR NON-HDR

public enum LabelType
{
    NoLabel,
    ResizeToLabel,
    YOLOUsingDarknet,
    YOLOUsingTensorFlow,
    PascalVOC,
    Keras
}

public class Render : MonoBehaviour
{
    private string prefixFolder = Environment.GetFolderPath(Environment.SpecialFolder.MyPictures);
    [Header("General References")]
    public Camera cam;
    public Camera camFiltered;
    private HDAdditionalCameraData camFilteredHD;  //REMOVE THIS LINE FOR NON-HDR
    public Transform pivot;
    public Transform pivot2;
    public Transform pivot3;
    public ReflectionProbe probe;

    [Header("Folder References")]
    public Transform lightingProfiles;
    public Transform enviormentProfiles;
    public Transform modelFolder;

    [Header("Configuration")]
    [Header("---------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------------")]

    // public int labelIndex = 0;
    public bool renderDebugImages;
    [Header("Output Resolution")]
    public Vector2Int outputResolution = new Vector2Int(500,500);
    public LabelType labelType;
    [Header("Number of Angles Captured per Axis")]
    //[Range(1, 360)]
    [Space]
    [Range(1, 50)]
    public int horizontalSpin = 12; //30
    [Range(0, 45)]
    public float horizontalSpinRandomVariance = 0;
    //[Range(1, 90)]
    [Space]
    [Range(1, 50)]
    public int verticalRotation = 6; //15
    [Range(0, 45)]
    public float verticalRotationRandomVariance = 0;
    //[Range(1, 360)]
    [Space]
    [Range(1, 50)]
    public int tilt = 1;
    [Range(0, 45)]
    public float tiltRandomVarance = 0;
    [Space]
    [Range(1, 20)]
    public float zoom = 10;

    public void StartScreenshot()
    {
        if (!Application.isPlaying)
        {
            Debug.LogError("Application must be in playmode.");
            return;
        }
        SetLayerRecursively(modelFolder.gameObject, LayerMask.NameToLayer("RenderObject"));
        cam.transform.localPosition = new Vector3(cam.transform.localPosition.x, cam.transform.localPosition.y, -1 * zoom);
        camFilteredHD = camFiltered.GetComponent<HDAdditionalCameraData>();  //REMOVE THIS LINE FOR NON-HDR
        StartCoroutine(ScreenshotCoroutine());
        //ScreenshotCoroutine();
    }
    IEnumerator ScreenshotCoroutine()
    //public void ScreenshotCoroutine()
    {
        // Bookkeeping Variables
        int lightingCount = 0;
        int enviormentCount = 0;
        int modelCount = -1;

        // Create Directories
        Debug.Log("Checking Directories");

        CreateDirectories(prefixFolder + "\\ImageSet\\");

        switch (labelType)
        {
            case LabelType.YOLOUsingDarknet:
                CreateDirectories(prefixFolder + "\\ImageSet\\DataSet");
                //CreateDirectories(prefixFolder + "\\ImageSet\\Labels");
                break;
            case LabelType.YOLOUsingTensorFlow:
            case LabelType.PascalVOC:
                CreateDirectories(prefixFolder + "\\ImageSet\\DataSet");
                break;
            case LabelType.Keras:
            case LabelType.ResizeToLabel:
                foreach (Transform model in modelFolder)
                    CreateDirectories(prefixFolder + "\\ImageSet\\" + model.name);
                break;
            default:
                break;
        }
     
        //yield break;

        // Iterate through all predefined conditions
        Debug.Log("Rendering...");
        foreach (Transform model in modelFolder)
        {
            foreach (Transform model2 in modelFolder)
            {
                model2.gameObject.SetActive(false);
            }
            model.gameObject.SetActive(true);
            modelCount++;
        
            // Write Class Names
            if (labelType == LabelType.YOLOUsingDarknet || labelType == LabelType.YOLOUsingTensorFlow)
                WriteString(model.name, prefixFolder + "\\ImageSet\\dataset.names");

            Debug.Log("Rendering Model ( " + modelCount + " of " + modelFolder.childCount + " )");
            yield return new WaitForSeconds(1);

            foreach (Transform evnior in enviormentProfiles)
            {
                foreach (Transform evnior2 in enviormentProfiles)
                {
                    evnior2.gameObject.SetActive(false);
                }
                evnior.gameObject.SetActive(true);
                enviormentCount++;

                foreach (Transform lighting in lightingProfiles)
                {
                    foreach (Transform lighting2 in lightingProfiles)
                    {
                        lighting2.gameObject.SetActive(false);
                    }
                    lighting.gameObject.SetActive(true);
                    lightingCount++;
                    probe.RenderProbe();

                    for (int j = 0; j < 360; j += (360 / horizontalSpin)) //10
                    {
                        pivot.localRotation = Quaternion.Euler(0, j + Random.Range((-1 * horizontalSpinRandomVariance), horizontalSpinRandomVariance), 0);

                        for (int i = 0; i < 90; i += (90 / verticalRotation)) //5
                        {
                            pivot2.localRotation = Quaternion.Euler(i + Random.Range((-1 * verticalRotationRandomVariance), verticalRotationRandomVariance), 0, 0);

                            for (int z = 0; z < 360; z += (360 / tilt)) //10
                            {
                                pivot3.localRotation = Quaternion.Euler(0, 0, z + Random.Range((-1 * tiltRandomVarance), tiltRandomVarance));
                                string fileSuffix = "LABEL" + modelCount + "E" + enviormentCount + "L" + lightingCount.ToString() + "_x" + (j.ToString()) + "y" + (i.ToString()) + "z" + (z.ToString());
                                byte[] imgBytes;
                                //
                                byte[] faBytes;
                                string output;
                                int[] bounding;
                                //
                                string imgOutputPath;

                                Texture2D imgTexture;
                                (imgBytes, imgTexture) = RenderImg(cam, outputResolution.x, outputResolution.y);

                                //////////////////////////////////////////////////////////////////////////////////////////////////
                                if (labelType == LabelType.ResizeToLabel || labelType == LabelType.Keras)
                                    imgOutputPath = prefixFolder + "\\ImageSet\\" + model.name + "\\" + fileSuffix + ".jpg";
                                else
                                    imgOutputPath = prefixFolder + "\\ImageSet\\DataSet\\" + fileSuffix + ".jpg";

                                File.WriteAllBytes(imgOutputPath, imgBytes);
                                //////////////////////////////////////////////////////////////////////////////////////////////////
                                switch (labelType)
                                {
                                    case LabelType.YOLOUsingDarknet:
                                    case LabelType.YOLOUsingTensorFlow:

                                        (faBytes, output, bounding) = GenerateBoundingBox(outputResolution.x, outputResolution.y, labelType);
                                        
                                        if (labelType == LabelType.YOLOUsingDarknet) 
                                        {
                                            string labelPath = prefixFolder + "\\ImageSet\\DataSet\\" + fileSuffix + ".txt";
                                            string trainPath = prefixFolder + "\\ImageSet\\dataset_train.txt";

                                            WriteString(modelCount + " " + output, labelPath);
                                            WriteString(imgOutputPath, trainPath);
                                        }
                                        else if (labelType == LabelType.YOLOUsingTensorFlow) 
                                        {
                                            string labelPath = prefixFolder + "\\ImageSet\\dataset_train.txt";
                                            WriteString(imgOutputPath + " " + output + "," + modelCount, labelPath); 
                                        }

                                        if (renderDebugImages)
                                            File.WriteAllBytes(prefixFolder + "\\ImageSet\\DataSet\\DEBUG-" + fileSuffix + ".png", faBytes);
                                        break;
                                    case LabelType.PascalVOC:
                                        Debug.LogError("Pascal VOC not yet implemented.");
                                        break;
                                    case LabelType.ResizeToLabel:
                                        (faBytes, output, bounding) = GenerateBoundingBox(outputResolution.x, outputResolution.y, labelType);

                                        imgBytes = CropImg(imgBytes, bounding[0], bounding[1], bounding[2], bounding[3], outputResolution.x, outputResolution.y);

                                        if (renderDebugImages)
                                            File.WriteAllBytes(prefixFolder + "\\ImageSet\\DEBUG-" + fileSuffix + ".png", faBytes);
                                        break;
                                    default:
                                        break;
                                }
                                //////////////////////////////////////////////////////////////////////////////////////////////////
                            }

                            pivot3.localRotation = Quaternion.Euler(0, 0, 0);

                        }
                        pivot2.localRotation = Quaternion.Euler(0, 0, 0);
                    }
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                    GC.Collect();
                    yield return new WaitForEndOfFrame();
                }
            }
        }

        /*if (labelType == LabelType.YOLOUsingDarknet)
        {
            string trainPath = prefixFolder + "\\ImageSet\\dataset_train.txt";
            string data = "classes = " + modelCount.ToString() +"\n" +
                          "train = " + prefixFolder + "\\ImageSet\\dataset_train.txt\n" +
                          "names = " + prefixFolder + "\\ImageSet\\dataset.names\n" +
                          "backup = backup\\\n" +
            WriteString(modelCount + " " + output, labelPath);
            WriteString(imgOutputPath, trainPath);
        }*/

        Debug.Log("Done");
        Debug.Log("Training Data can be found at: " + prefixFolder + "\\ImageSet\\");
    }

    void CreateDirectories(string directory)
    {
        if (!Directory.Exists(directory))
        {
            var folder = Directory.CreateDirectory(directory);
        }
    }

    (byte[], Texture2D) RenderImg(Camera camera, int resWidth, int resHeight)
    {
        //camera.enabled = true;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null; // Just In Case, to avoid errors
        byte[] bytes = screenShot.EncodeToJPG();

        //Debug.Log(screenShot.GetPixel(0, 0));
        //camera.enabled = false;
        Destroy(rt);
        Destroy(screenShot);
        return (bytes, screenShot);
    }
    byte[] CropImg(byte[] img, int startX, int startY, int width, int height, int resWidth, int resHeight)
    {
        Texture2D screenShot = new Texture2D(resWidth, resHeight);
        screenShot.LoadImage(img);
        Texture2D screenShotCropped = new Texture2D(width, height);
        screenShotCropped.SetPixels(screenShot.GetPixels(startX, startY, width, height));
        screenShotCropped.Apply();

        byte[] bytes = screenShotCropped.EncodeToJPG();

        Destroy(screenShot);
        Destroy(screenShotCropped);
        return bytes;
    }
    (byte[], string, int[]) GenerateBoundingBox(int resWidth, int resHeight, LabelType labelFormat)
    {
        Camera camera = camFiltered;
        string output = "";
        int[] boundingBox = new int[4];
        //camera.enabled = true;
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        camera.clearFlags = CameraClearFlags.Color;

        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
        camera.backgroundColor = Color.green;
        camFilteredHD.backgroundColorHDR = Color.green;  //REMOVE THIS LINE FOR NON-HDR
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

        Texture2D screenShotAlt = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
        camera.backgroundColor = Color.red;
        camFilteredHD.backgroundColorHDR = Color.red;  //REMOVE THIS LINE FOR NON-HDR
        camera.Render();
        RenderTexture.active = rt;
        screenShotAlt.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);

        camera.targetTexture = null;
        RenderTexture.active = null; // Just In Case, to avoid errors

        // Calculate 2D Bounding Box From Image
        int[] smallXY = { resWidth-1, resHeight-1 };
        int[] largeXY = { 0, 0 };
        for (int x = 0; x < resWidth; x++)
        {
            for (int y = resHeight-1; y > 0; y--)
            {
                //Debug.Log();
                var p = screenShot.GetPixel(x, y);
                var p2 = screenShotAlt.GetPixel(x, y);
                if ((p.r == 0f & p.g == 1f & p.b == 0f) & (p2.r == 1f & p2.g == 0f & p2.b == 0f))
                {
                    if (renderDebugImages)
                        screenShot.SetPixel(x, y, new Color(0, 0, 0, 0));
                }
                else
                {
                    if (x > largeXY[0])
                    {
                        largeXY[0] = x;
                    }
                    if ((resHeight - y) > largeXY[1])
                    {
                        largeXY[1] = (resHeight - y);
                    }

                    if (x < smallXY[0])
                    {
                        smallXY[0] = x;
                    }
                    if ((resHeight - y) < smallXY[1])
                    {
                        smallXY[1] = (resHeight - y);
                    }
                }
            }
        }
        //

        // Generate Data Labels
        float[] outputVariables = new float[4];
        char seperator = ' ';

        switch (labelFormat)
        {
            case LabelType.YOLOUsingDarknet:
                outputVariables[0] = ((largeXY[0] + smallXY[0]) / 2f) / resWidth; // Middle X
                outputVariables[1] = ((largeXY[1] + smallXY[1]) / 2f) / resHeight; // Middle Y

                outputVariables[2] = (largeXY[0] - smallXY[0]) / (float)resWidth;// / width;
                outputVariables[3] = (largeXY[1] - smallXY[1]) / (float)resHeight;// / height;
                seperator = ' ';
                break;
            case LabelType.YOLOUsingTensorFlow:
                outputVariables[0] = smallXY[0]; // Start X
                outputVariables[1] = smallXY[1]; // Start Y
                outputVariables[2] = largeXY[0]; // End X
                outputVariables[3] = largeXY[1]; // End Y
                seperator = ',';
                break;
            default:
                break;
        }

        for (int i = 0; i < outputVariables.Length; i++)
            output += outputVariables[i].ToString() + seperator;
        output = output.TrimEnd(seperator, ' ');
        //

        // bounding box for image resizing & cropping

        boundingBox[0] = smallXY[0];
        boundingBox[1] = resHeight - largeXY[1]; //smallXY[1]
        boundingBox[2] = Mathf.RoundToInt(largeXY[0] - smallXY[0]);
        boundingBox[3] = Mathf.RoundToInt(largeXY[1] - smallXY[1]);

        if (renderDebugImages)
        {
            for (int x = smallXY[0]; x < largeXY[0]; x++)
            {
                screenShot.SetPixel(x, (resHeight - smallXY[1]), new Color(1, 0, 0));
                screenShot.SetPixel(x, (resHeight - largeXY[1]), new Color(1, 0, 0));
            }

            for (int y = smallXY[1]; y < largeXY[1]; y++)
            {
                screenShot.SetPixel(smallXY[0], (resHeight - y), new Color(1, 0, 0));
                screenShot.SetPixel(largeXY[0], (resHeight - y), new Color(1, 0, 0));
            }
        }
        //

        byte[] bytes = screenShot.EncodeToPNG();
        //camera.enabled = false;
        Destroy(rt);
        Destroy(screenShot);
        Destroy(screenShotAlt);
        return (bytes, output, boundingBox);
    }

    public void WriteString(string singleLine, string path)
    {
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(singleLine);
        writer.Close();
        StreamReader reader = new StreamReader(path);
        reader.Close();
    }

    public void OverridreWriteString(string singleLine, string path)
    {
        File.WriteAllText(path, singleLine);
    }

    void SetLayerRecursively(GameObject obj, int newLayer)
    {
        if (obj == null)
        {
            return;
        }

        obj.layer = newLayer;

        foreach (Transform child in obj.transform)
        {
            if (child == null)
            {
                continue;
            }
            SetLayerRecursively(child.gameObject, newLayer);
        }
    }
    // Editor Functions
    public long EstimateTotalSizeInMB()
    {
        return ((long)CalculateTotalImages() * (long)outputResolution.x * (long)outputResolution.y * (long)4) / (long)1000000;
    }
    public int CalculateTotalImages()
    {
        int md = modelFolder.childCount;
        return CalculateImagesPerModel() * md;
    }
    public int CalculateImagesPerModel()
    {
        int env = enviormentProfiles.childCount;
        int lt = lightingProfiles.childCount;
        if (env == 0)
            env = 1;
        if (lt == 0)
            lt = 1;
        return horizontalSpin * verticalRotation * tilt * env * lt;
    }
    //
}
