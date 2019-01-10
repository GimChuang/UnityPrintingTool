using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public static class FileIOUtility
{

    public enum FileExtension { PNG, JPG }

    public static string GenerateFileName(string _prefix, int _index, FileExtension _extension)
    {
        string ext = "";
        switch (_extension)
        {
            case FileExtension.PNG:
                ext = ".png";
                break;
            case FileExtension.JPG:
                ext = ".jpg";
                break;
        }

        return string.Format("{0}_{1}_{2}{3}",
                              _prefix,
                              _index.ToString("00000"),
                              System.DateTime.Now.ToString("MM-dd_HH-mm-ss"),
                              ext);
    }

    public static void SaveImage(Texture2D _texture, string _path, string _fileName, FileExtension _extension)
    {
        try
        {
            string fullPath = _path + "\\" + _fileName;

            byte[] bytes = new byte[2];
            switch (_extension)
            {
                case FileExtension.PNG:
                    bytes = _texture.EncodeToPNG();
                    break;
                case FileExtension.JPG:
                    bytes = _texture.EncodeToJPG();
                    break;
            }

            System.IO.File.WriteAllBytes(fullPath, bytes);

            Debug.Log("Image saved to: " + fullPath);
        }
        catch (Exception e)
        {
            Debug.LogError("Failed to save image.");
            Debug.LogError(e.Message);
        }
    }

    public static bool DoesPathExist(string _path)
    {
        bool isExist = Directory.Exists(_path);
        if (isExist)
        {
            return true;
        }
        else
        {
            Debug.LogError("Path Not Exists: " + _path);
            return false;
        }
    }

    public static bool IsFileLocked(string _filePath)
    {
        FileInfo fileInfo = new FileInfo(_filePath);

        FileStream stream = null;

        try
        {
            stream = fileInfo.Open(FileMode.Open, FileAccess.ReadWrite, FileShare.None);
        }
        catch (IOException e)
        {
            //the file is unavailable because it is:
            //still being written to
            //or being processed by another thread
            //or does not exist (has already been processed)
            Debug.Log(e.Message);
            return true;
        }
        finally
        {
            if (stream != null)
                stream.Close();
        }

        //file is not locked
        return false;
    }
}
