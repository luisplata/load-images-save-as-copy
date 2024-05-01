using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class SelectImageFromPlugin : MonoBehaviour
{
    private string pdfFileType;
    public Image myImageComponent;

    void Start()
    {
        pdfFileType = NativeFilePicker.ConvertExtensionToFileType("png, jpg, jpeg, bmp, gif, tiff, tga, exif, raw"); // Returns "application/pdf" on Android and "com.adobe.pdf" on iOS
        Debug.Log("images's MIME/UTI is: " + pdfFileType);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            // Don't attempt to import/export files if the file picker is already open
            if (NativeFilePicker.IsFilePickerBusy())
                return;

            if (Input.mousePosition.x < Screen.width / 3)
            {
                // Pick a PDF file
                NativeFilePicker.Permission permission = NativeFilePicker.PickFile((path) =>
                {
                    if (path == null)
                        Debug.Log("Operation cancelled");
                    else
                    {
                        Debug.Log("Picked file: " + path);
                        Texture2D tex = null;
                        byte[] fileData;

                        if (File.Exists(path))
                        {
                            fileData = System.IO.File.ReadAllBytes(path);
                            tex = new Texture2D(2, 2);
                            tex.LoadImage(fileData); //..this will auto-resize the texture dimensions.
                        }

                        // Convert the Texture2D to a Sprite
                        Sprite sprite = Sprite.Create(tex, new Rect(0, 0, tex.width, tex.height), new Vector2(0.5f, 0.5f));

                        // Set the sprite to the Image component
                        myImageComponent.sprite = sprite;
                    }
                }, new string[] { pdfFileType });

                Debug.Log("Permission result: " + permission);
            }
        }
    }

}
