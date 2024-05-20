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
                Debug.Log("Operación cancelada");
            else
            {
                Texture2D tex = null;
                if (File.Exists(path))
                {
                    var fileData = File.ReadAllBytes(path);
                    tex = new Texture2D(2, 2);
                    tex.LoadImage(fileData);
                }

                if (tex != null)
                {
                    // Modifica la textura según tus necesidades
                    int nuevoAncho = tex.width / 2; // Por ejemplo, reducir el ancho a la mitad
                    int nuevoAlto = tex.height / 2; // Por ejemplo, reducir el alto a la mitad
                    Texture2D texModificada = new Texture2D(nuevoAncho, nuevoAlto);
                    Graphics.ConvertTexture(tex, texModificada);

                    // Crea un Sprite a partir de la textura modificada
                    var sprite = Sprite.Create(texModificada, new Rect(0, 0, texModificada.width, texModificada.height),
                        new Vector2(0.5f, 0.5f));
                    myImageComponent.sprite = sprite;

                    // Convierte la textura modificada de nuevo a PNG o JPG
                    byte[] imageBytes = texModificada.EncodeToPNG(); // o usa EncodeToJPG()

                    // Obtiene la ruta del directorio de almacenamiento interno
                    string pathToSave = Application.persistentDataPath + "/Pictures/PhotoLeap/";

                    // Verifica y crea directorios si es necesario
                    if (!Directory.Exists(pathToSave))
                    {
                        Directory.CreateDirectory(pathToSave);
                    }

                    // Genera un nombre de archivo único basado en la fecha y hora actual
                    string nombreArchivo = "Photoleap_imagen_modificada_" + DateTime.Now.ToString("yyyyMMddHHmmss") + ".jpg";
                    var filePath = Path.Combine(pathToSave, nombreArchivo);

                    // Guarda los bytes de la imagen en un archivo
                    File.WriteAllBytes(filePath, imageBytes);
                    Debug.Log("Imagen modificada guardada en: " + filePath);

                    // Exporta el archivo
                    var permission = NativeFilePicker.ExportFile(filePath,
                        (success) => Debug.Log("Archivo exportado: " + success));
                    Debug.Log("Resultado del permiso: " + permission);
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