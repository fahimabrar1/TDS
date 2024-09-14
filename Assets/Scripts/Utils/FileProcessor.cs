using System;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Threading.Tasks;
using UnityEngine;

public class FileProcessorr<T> where T : class
{
    /// <summary>
    /// Saves the data to a file asynchronously.
    /// </summary>
    /// <param name="fileName">The name of the file to save.</param>
    /// <param name="data">The data to save.</param>
    public void OnSaveAsync(string fileName, T data)
    {
        string path = Application.persistentDataPath + "/" + fileName + GameConstants.SAVE_FILE_EXTENSION;


        BinaryFormatter bf = new();
        using FileStream stream = new(path, FileMode.OpenOrCreate);

        string json = JsonUtility.ToJson(data);
        bf.Serialize(stream, json);

        MyDebug.Log(json);
    }



    /// <summary>
    /// Saves the data to a file asynchronously.
    /// </summary>
    /// <param name="fileName">The name of the file to save.</param>
    /// <param name="data">The data to save.</param>
    public void OnSaveAsync(T data)
    {
        string path = Application.persistentDataPath + "/" + typeof(T).Name.ToLower() + GameConstants.SAVE_FILE_EXTENSION;


        BinaryFormatter bf = new();
        using FileStream stream = new(path, FileMode.OpenOrCreate);

        string json = JsonUtility.ToJson(data);
        bf.Serialize(stream, json);

        MyDebug.Log(json);
    }

    /// <summary>
    /// Loads data from a file into the specified object asynchronously.
    /// </summary>
    /// <param name="fileName">The name of the file to load.</param>
    /// <param name="target">The object to populate with loaded data.</param>
    public async Task OnLoadAsync(string fileName, T target)
    {
        string path = Application.persistentDataPath + "/" + fileName + GameConstants.SAVE_FILE_EXTENSION;

        // Check if the file exists before attempting to load
        if (File.Exists(path))
        {
            await Task.Run(() =>
            {
                BinaryFormatter bf = new();
                using FileStream stream = new(path, FileMode.Open);
                try
                {
                    string json = (string)bf.Deserialize(stream);
                    JsonUtility.FromJsonOverwrite(json, target);
                    MyDebug.Log(json);
                }
                catch (Exception e)
                {
                    MyDebug.LogError("Failed to load data: " + e.Message);
                }
            });
        }
    }



    /// <summary>
    /// Loads data from a file into the specified object asynchronously.
    /// </summary>
    /// <param name="fileName">The name of the file to load.</param>
    /// <param name="target">The object to populate with loaded data.</param>
    public async Task OnLoadAsync(T target)
    {
        string path = Application.persistentDataPath + "/" + typeof(T).Name.ToLower() + GameConstants.SAVE_FILE_EXTENSION;
        // Check if the file exists before attempting to load
        if (File.Exists(path))
        {
            await Task.Run(() =>
            {
                BinaryFormatter bf = new();
                using FileStream stream = new(path, FileMode.Open);
                try
                {
                    string json = (string)bf.Deserialize(stream);
                    JsonUtility.FromJsonOverwrite(json, target);
                    MyDebug.Log(json);
                }
                catch (Exception e)
                {
                    MyDebug.LogError("Failed to load data: " + e.Message);
                }
            });
        }
    }

}

