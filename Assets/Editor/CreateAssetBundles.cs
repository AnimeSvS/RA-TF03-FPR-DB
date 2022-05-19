// referencias
using UnityEditor;
using System.IO;
using UnityEngine;

public class CreateAssetBundles
{
    //atributo que indicara laubicacion del directorio
    public static string assetBundleDirectory = "Assets/AssetBundles/";

    [MenuItem("Assets/Build AssetBundles")]

    //funcion para verificacion de carpeta
    static void BuildAllAssetBundles()
    {

        //si existe alguna carpeta de assets, lo deja ahi , y si no lo creara
        if (Directory.Exists(assetBundleDirectory))
        {
            Directory.Delete(assetBundleDirectory, true);
        }

        Directory.CreateDirectory(assetBundleDirectory);

        //create bundles for all platform (use IOS for editor support on MAC but must be on IOS build platform)
        ///BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.iOS);
        ///AppendPlatformToFileName("IOS");
        ///Debug.Log("IOS bundle created...");
        
        //crea paquetes para la plataforma android
        BuildPipeline.BuildAssetBundles(assetBundleDirectory, BuildAssetBundleOptions.None, BuildTarget.Android);
        AppendPlatformToFileName("Android");
        Debug.Log("Creando paquete de Android...");

        RemoveSpacesInFileNames();

        AssetDatabase.Refresh();
        Debug.Log("Completado");
    }


    //funcion de espaciado en nombre de modelo 3d
    static void RemoveSpacesInFileNames()
    {
        foreach (string path in Directory.GetFiles(assetBundleDirectory))
        {
            string oldName = path;
            string newName = path.Replace(' ', '-');
            File.Move(oldName, newName);
        }
    }

    //funcion para nombrar bien los archivos 3D
    static void AppendPlatformToFileName(string platform)
    {
        foreach (string path in Directory.GetFiles(assetBundleDirectory))
        {
            //get filename
            string[] files = path.Split('/');
            string fileName = files[files.Length - 1];

            //delete files we dont need
            if (fileName.Contains(".") || fileName.Contains("Bundle"))
            {
                File.Delete(path);
            }
            else if (!fileName.Contains("-"))
            {
                //append platform to filename
                FileInfo info = new FileInfo(path);
                info.MoveTo(path + "-" + platform);
            }
        }
    }
}