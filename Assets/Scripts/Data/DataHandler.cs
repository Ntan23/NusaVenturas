using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;
using UnityEditor;

public class DataHandler
{
    public string keyString;
    public string ivString;
    private string dataPath = "";
    private string dataName = "";
    private bool useAESEncryption = false;

    public DataHandler(string dataPath, string dataName, bool useAESEncryption)
    {
        this.dataPath = dataPath;
        this.dataName = dataName;
        this.useAESEncryption = useAESEncryption;
    }

    public GameData Load() 
    {
        string fullPath = Path.Combine(dataPath,dataName);

        GameData loadedData = null;

        if (File.Exists(fullPath)) 
        {
            try 
            {
                // load the serialized data from the file
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath,FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }

                // optionally decrypt the data
                if (useAESEncryption) 
                {
                    keyString = Security.DecryptKey(PlayerPrefs.GetString("??"));
                    ivString = Security.DecryptIV(PlayerPrefs.GetString("!!"));

                    byte[] decrypted = File.ReadAllBytes(fullPath);
                    dataToLoad = Security.DecryptUsingAES(decrypted,keyString,ivString);

                    keyString = Security.RandomKeyGenerator();
                    ivString = Security.RandomIVGenerator();

                    PlayerPrefs.SetString("??",Security.EncryptKey(keyString));
                    PlayerPrefs.SetString("!!",Security.EncryptIV(ivString));
                }

                loadedData = JsonUtility.FromJson<GameData>(dataToLoad);
            }
            catch(Exception e) 
            {
                Debug.LogError("Error occured when trying to load data from file: " +fullPath + "\n" + e);
            }
        }

        return loadedData;
    }

    public void Save(GameData data) 
    {
        // use Path.Combine to account for different OS's having different path separators
        string fullPath = Path.Combine(dataPath, dataName);
        
        try 
        {
            // create the directory the file will be written to if it doesn't already exist
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));

            // serialize the game data into Json
            string dataToStore = JsonUtility.ToJson(data,true);
            
            // optionally encrypt the data
            // if (useXOREncryption)
            // {
            //     dataToStore = Security.EncryptDecryptUsingXOR(dataToStore);
            //     // write the serialized data to the file
            //     using (FileStream stream = new FileStream(fullPath,FileMode.Create))
            //     {
            //         using (StreamWriter writer = new StreamWriter(stream)) 
            //         {
            //             writer.Write(dataToStore);
            //         }
            //     }
            // }
            
            if (useAESEncryption)
            {
                keyString = Security.DecryptKey(PlayerPrefs.GetString("??"));
                ivString = Security.DecryptIV(PlayerPrefs.GetString("!!"));

                // Debug.Log("Encrypt Key String : " + keyString);
                // Debug.Log("Encrypt IV String : " + ivString);
                byte[] encrypted = Security.EncryptUsingAES(dataToStore,keyString,ivString);
                File.WriteAllBytes(fullPath,encrypted);
            }
        }
        catch (Exception e) 
        {
            Debug.LogError("Error occured when trying to save data to file: " + fullPath + "\n" + e);
        }
    }

    public void DeleteData()
    {
        Debug.Log("Delete");
        string fullPath = Path.Combine(dataPath, dataName);
        File.Delete(fullPath);
        
        #if UNITY_EDITOR
		UnityEditor.AssetDatabase.Refresh();
		#endif
    }
}
