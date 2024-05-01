using System;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SeleccionarYMostrarImagen : MonoBehaviour
{
    public Image myImageComponent;

    private void Start()
    {
#if UNITY_ANDROID
        //RequestPermissionAsynchronously();
#endif
    }

    public void ChooseFile()
    {
#if UNITY_ANDROID
        string[] fileTypes = { "image/*" };

        NativeFilePicker.PickFile(path =>
        {
            if (path == null)
                Debug.Log("Operation cancelled");
            else
            {
                Texture2D tex = null;
                if (System.IO.File.Exists(path))
                {
                    var fileData = System.IO.File.ReadAllBytes(path);
                    tex = new Texture2D(2, 2);
                    tex.LoadImage(fileData);
                }
                if (tex != null)
                {
                    var sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));
                    myImageComponent.sprite = sprite;
                    
                    // Convert the modified Texture2D back to PNG or JPG
                    byte[] imageBytes = tex.EncodeToPNG(); // or use EncodeToJPG()

                    // Write the image bytes to a file
                    var filePath = Path.Combine(Application.temporaryCachePath, "modifiedImage.png"); // or "modifiedImage.jpg"
                    File.WriteAllBytes(filePath, imageBytes);

                    // Export the file
                    var permission = NativeFilePicker.ExportFile(filePath, (success) => Debug.Log("File exported: " + success));
                    Debug.Log("Permission result: " + permission);
                }
            }
        }, fileTypes);
#endif
    }
    
    // Example code doesn't use this function but it is here for reference. It's recommended to ask for permissions manually using the
    // RequestPermissionAsync methods prior to calling NativeFilePicker functions
    private async void RequestPermissionAsynchronously(bool readPermissionOnly = false)
    {
        NativeFilePicker.Permission permission = await NativeFilePicker.RequestPermissionAsync(readPermissionOnly);
        Debug.Log("Permission result: " + permission);
    }
}
