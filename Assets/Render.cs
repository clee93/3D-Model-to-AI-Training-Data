using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using System.IO;
using System;

public class Render : MonoBehaviour
{
    public Light dirLight;
    public Camera cam;
    public Camera camFiltered;
    public Transform pivot;
    public Transform pivot2;

    public int labelIndex = 0;

    // Start is called before the first frame update
    /*void Start()
    {
        //System.IO.File.WriteAllBytes(Application.dataPath + "/ImageSet/I" + ".png", RenderImg(cam, 500, 500));
        //System.IO.File.WriteAllBytes(Application.dataPath + "/ImageSet/F" + ".png", GenerateBoundingBox(camFiltered, 500, 500));
        //GenerateBoundingBox(camFiltered, 500, 500);
    }*/

    // Update is called once per frame

    [ContextMenu("Screenshot")]
    void StartScreenshot()
    {
        for (int j = 0; j < 360; j+=30) //10
        {
            pivot.localRotation = Quaternion.Euler(0, j, 0);

            for (int i = 0; i < 90; i+=15) //5
            {
                pivot2.localRotation = Quaternion.Euler(i, 0, 0);

                File.WriteAllBytes(Application.dataPath + "/ImageSet/" + (j.ToString())+"-"+ (i.ToString()) + ".png", RenderImg(cam, 500, 500));

                byte[] faBytes;
                string output;
                (faBytes, output) = GenerateBoundingBox(camFiltered, 500, 500);
                File.WriteAllBytes(Application.dataPath + "/ImageSet/DEBUG-" + (j.ToString()) + "-" + (i.ToString()) + ".png", faBytes);
                //print(output);
                WriteString(output, (j.ToString()) + " - " + (i.ToString()));
            }
            pivot2.localRotation = Quaternion.Euler(0, 0, 0);
        }
    }

    byte[] RenderImg(Camera camera, int resWidth, int resHeight)
    {
        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);
        byte[] bytes = screenShot.EncodeToPNG();
        //Debug.Log(screenShot.GetPixel(0, 0));


        return bytes;
    }
    (byte[], string) GenerateBoundingBox(Camera camera, int resWidth, int resHeight)
    {
        string output = "";

        RenderTexture rt = new RenderTexture(resWidth, resHeight, 24);
        camera.targetTexture = rt;
        Texture2D screenShot = new Texture2D(resWidth, resHeight, TextureFormat.ARGB32, false);
        camera.Render();
        RenderTexture.active = rt;
        screenShot.ReadPixels(new Rect(0, 0, resWidth, resHeight), 0, 0);
        camera.targetTexture = null;
        RenderTexture.active = null; // JC: added to avoid errors
        Destroy(rt);

        int[] smallXY = { resWidth-1, resHeight-1 };
        int[] largeXY = { 0, 0 };
        for (int x = 0; x < resWidth; x++)
        {
            for (int y = resHeight-1; y > 0; y--)
            {
                //Debug.Log();
                var p = screenShot.GetPixel(x, y);
                if (!(p.r == 0f & p.g == 1f & p.b == 0f))
                {
                    if (x  > largeXY[0])
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
        float middleX = ((largeXY[0] + smallXY[0]) / 2f) / resWidth;
        float middleY = ((largeXY[1] + smallXY[1]) / 2f) / resHeight;

        float width = (largeXY[0] - smallXY[0]) / (float)resWidth;
        float height = (largeXY[1] - smallXY[1]) / (float)resHeight;

        //Label_ID_1 X_CENTER_NORM Y_CENTER_NORM WIDTH_NORM HEIGHT_NORM
        output = labelIndex + " " + middleX.ToString() + " " + middleY.ToString() + " " + width.ToString() + " " + height.ToString();

        for (int x = smallXY[0]; x < largeXY[0]; x++)
        {
            screenShot.SetPixel(x, (resHeight - smallXY[1]), new Color(1,0,0));
            screenShot.SetPixel(x, (resHeight - largeXY[1]), new Color(1, 0, 0));
        }

        for (int y = smallXY[1]; y < largeXY[1]; y++)
        {
            screenShot.SetPixel(smallXY[0], (resHeight - y), new Color(1, 0, 0));
            screenShot.SetPixel(largeXY[0], (resHeight - y), new Color(1, 0, 0));
        }

        byte[] bytes = screenShot.EncodeToPNG();

        return (bytes, output);
    }

    public static void WriteString(string singleLine, string fileName)
    {
        string path = Application.dataPath + "/ImageSet/Labels/" + fileName + ".txt";
        //Write some text to the test.txt file
        StreamWriter writer = new StreamWriter(path, true);
        writer.WriteLine(singleLine);
        writer.Close();
        StreamReader reader = new StreamReader(path);
        //Print the text from the file
        Debug.Log(reader.ReadToEnd());
        reader.Close();
    }
}
