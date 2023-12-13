//credit: https://forum.unity.com/threads/how-to-save-manually-save-a-png-of-a-camera-view.506269/
using System.IO;
using UnityEngine;

public class SR_RenderCamera : MonoBehaviour
{
    public int FileCounter = 0;

    private void LateUpdate()
    {
        if (Input.GetKeyDown(KeyCode.F9))
        {
            CamCapture();
        }
    }

    void CamCapture()
    {
        Camera Cam = GetComponent<Camera>();

        RenderTexture currentRT = RenderTexture.active;
        RenderTexture.active = Cam.targetTexture;

        Cam.Render();

        Texture2D Image = new Texture2D(Cam.targetTexture.width, Cam.targetTexture.height);
        Image.ReadPixels(new Rect(0, 0, Cam.targetTexture.width, Cam.targetTexture.height), 0, 0);
        Image.Apply();
        RenderTexture.active = currentRT;

        var Bytes = Image.EncodeToPNG();
        Destroy(Image);

        File.WriteAllBytes(Application.dataPath + "/CameraPreviews/" + FileCounter + ".png", Bytes);
        FileCounter++;
    }

}
