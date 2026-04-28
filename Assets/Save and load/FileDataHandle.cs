using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using System.IO;

public class FileDataHandle 
{
    private string dataDirPath = "";
    private string datafileName = "";

    private bool encryptData = false;
    private string codeword = "YUI";

    public FileDataHandle(string _dataDirPath, string _datafileName , bool _encryptData)
    {

        this.dataDirPath = _dataDirPath;
        this.datafileName = _datafileName;
        this.encryptData = _encryptData;
    }

    public void Save(Gamedata _data)
    {
        string fullPath = Path.Combine(dataDirPath, datafileName);

        try
        {
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath));
            string dataToLoad = JsonUtility.ToJson(_data,true);

            if(encryptData)
            {
                dataToLoad = EncryptDecrypt(dataToLoad);
            }

            using(FileStream stream = new FileStream(fullPath, FileMode.Create))
            {
                using(StreamWriter writer = new StreamWriter(stream))
                {
                    writer.Write(dataToLoad);
                }
            }

        }
        catch (Exception e)
        {
            Debug.Log("Error on trying to save data to file:" + fullPath + "\n" + e);
        }
    }

    public Gamedata Load()
    {
        string fullPath = Path.Combine(dataDirPath, datafileName);
        Gamedata loadData = null;

        if(File.Exists(fullPath))
        {
            try
            {
                string dataToLoad = "";
                using (FileStream stream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(stream))
                    {
                        dataToLoad = reader.ReadToEnd();
                    }
                }
                if(encryptData)
                {
                    dataToLoad = EncryptDecrypt(dataToLoad);
                }


                loadData = JsonUtility.FromJson<Gamedata>(dataToLoad);
            }
            catch (Exception e) 
            {
                Debug.Log("Error:" + fullPath + "\n" + e);
            }
        }
        return loadData;
    }
    public void Delete()
    {
        string fullPath = Path.Combine(dataDirPath, datafileName);
        if (File.Exists(fullPath))
        {
            File.Delete(fullPath);
        }
    }

    private string EncryptDecrypt(string _data)
    {
        string modifiedData = "";
        for (int i = 0; i < _data.Length; i++)
        {
            modifiedData += (char)(_data[i] * codeword[i % codeword.Length]);
        }
        return modifiedData;
    }

}
