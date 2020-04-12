using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

public class StorageHandler
{
    /// <summary>
    ///     Serialize an object to the devices File System.
    /// </summary>
    /// <param name="objectToSave">The Object that will be Serialized.</param>
    /// <param name="fileName">Name of the file to be Serialized.</param>
    public void SaveData(object objectToSave, string fileName)
    {
        // Add the File Path together with the files name and extension.
        // We will use .bin to represent that this is a Binary file.
        string fullFilePath = Application.persistentDataPath + "/" + fileName + ".bin";
        // We must create a new formatter to Serialize with.
        BinaryFormatter formatter = new BinaryFormatter();
        // Create a streaming path to our new file location.
        FileStream fileStream = new FileStream(path: fullFilePath, mode: FileMode.Create);
        // Serialize the object to the File Stream
        formatter.Serialize(serializationStream: fileStream, graph: objectToSave);
        // Finally Close the FileStream and let the rest wrap itself up.
        fileStream.Close();
    }

    /// <summary>
    ///     Deserialize an object from the FileSystem.
    /// </summary>
    /// <param name="fileName">Name of the file to deserialize.</param>
    /// <returns>Deserialized Object</returns>
    public object LoadData(string fileName)
    {
        string fullFilePath = Application.persistentDataPath + "/" + fileName + ".bin";
        // Check if our file exists, if it does not, just return a null object.
        if (!File.Exists(path: fullFilePath)) return null;
        BinaryFormatter formatter = new BinaryFormatter();
        FileStream fileStream = new FileStream(path: fullFilePath, mode: FileMode.Open);
        object obj = formatter.Deserialize(serializationStream: fileStream);
        fileStream.Close();
        // Return the un-cast untyped object.
        return obj;

    }
}