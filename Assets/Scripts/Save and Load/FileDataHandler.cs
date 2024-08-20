using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using UnityEngine;

public class FileDataHandler
{
    private string dataDirPath;
    private string dataFilename;
    
    private const string CodeWord = "vanodev";
    private bool encryptData;
    
    public FileDataHandler(string path, string filename, bool encryptData)
    {
        this.dataDirPath = path;
        this.dataFilename = filename;
        this.encryptData = encryptData;
    }
    
    public void Save(GameData data)
    {
        var fullPath = Path.Combine(dataDirPath, dataFilename);

        try
        {
            string dataText = JsonUtility.ToJson(data, true);
            
            if (encryptData)
                dataText = Encrypt(dataText);
            
            Directory.CreateDirectory(Path.GetDirectoryName(fullPath)!);

            using (FileStream fileStream = new FileStream(fullPath, FileMode.Create))
            {
                using (StreamWriter writer = new StreamWriter(fileStream))
                {
                    writer.Write(dataText);
                }
            }
        }
        catch (Exception e)
        {
           Debug.LogError("Error saving data to file: " + fullPath + "\n" + e);
        }
    }
    
    public bool HasData()
    {
        var fullPath = Path.Combine(dataDirPath, dataFilename);
        return File.Exists(fullPath);
    }
    
    public GameData Load()
    {
        var fullPath = Path.Combine(dataDirPath, dataFilename);

        try
        {
            if (File.Exists(fullPath))
            {
                using (FileStream fileStream = new FileStream(fullPath, FileMode.Open))
                {
                    using (StreamReader reader = new StreamReader(fileStream))
                    {
                        string dataText = reader.ReadToEnd();
                        
                        if (encryptData)
                        {
                            if (!TryDecrypt(dataText, out dataText))
                            {
                                Debug.LogError("Error decrypting data from file: " + fullPath);
                                return new GameData();
                            }
                        }
                        
                        return JsonUtility.FromJson<GameData>(dataText);
                    }
                }
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error loading data from file: " + fullPath + "\n" + e);
        }

        return new GameData();
    }
    
    public void Delete()
    {
        var fullPath = Path.Combine(dataDirPath, dataFilename);

        try
        {
            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
            }
        }
        catch (Exception e)
        {
            Debug.LogError("Error deleting file: " + fullPath + "\n" + e);
        }
    }

    #region Encription

    const int Iterations = 1000;

    private string Encrypt(string plainText)
    {
        if (plainText == null)
        {
            throw new ArgumentNullException("plainText");
        }

        if (string.IsNullOrEmpty(CodeWord))
        {
            throw new ArgumentNullException("password");
        }

        // create instance of the DES crypto provider
        var des = new DESCryptoServiceProvider();

        // generate a random IV will be used a salt value for generating key
        des.GenerateIV();

        // use derive bytes to generate a key from the password and IV
        var rfc2898DeriveBytes = new Rfc2898DeriveBytes(CodeWord, des.IV, Iterations);

        // generate a key from the password provided
        byte[] key = rfc2898DeriveBytes.GetBytes(8);

        // encrypt the plainText
        using (var memoryStream = new MemoryStream())
        using (var cryptoStream = new CryptoStream(memoryStream, des.CreateEncryptor(key, des.IV), CryptoStreamMode.Write))
        {
            // write the salt first not encrypted
            memoryStream.Write(des.IV, 0, des.IV.Length);

            // convert the plain text string into a byte array
            byte[] bytes = Encoding.UTF8.GetBytes(plainText);

            // write the bytes into the crypto stream so that they are encrypted bytes
            cryptoStream.Write(bytes, 0, bytes.Length);
            cryptoStream.FlushFinalBlock();

            return Convert.ToBase64String(memoryStream.ToArray());
        }
    }

    private bool TryDecrypt(string cipherText, out string plainText)
    {
        // its pointless trying to decrypt if the cipher text
        // or password has not been supplied
        if (string.IsNullOrEmpty(cipherText) ||
            string.IsNullOrEmpty(CodeWord))
        {
            plainText = "";
            return false;
        }

        try
        {
            byte[] cipherBytes = Convert.FromBase64String(cipherText);

            using (var memoryStream = new MemoryStream(cipherBytes))
            {
                // create instance of the DES crypto provider
                var des = new DESCryptoServiceProvider();

                // get the IV
                byte[] iv = new byte[8];
                memoryStream.Read(iv, 0, iv.Length);

                // use derive bytes to generate key from password and IV
                var rfc2898DeriveBytes = new Rfc2898DeriveBytes(CodeWord, iv, Iterations);

                byte[] key = rfc2898DeriveBytes.GetBytes(8);

                using (var cryptoStream =
                       new CryptoStream(memoryStream, des.CreateDecryptor(key, iv), CryptoStreamMode.Read))
                using (var streamReader = new StreamReader(cryptoStream))
                {
                    plainText = streamReader.ReadToEnd();
                    return true;
                }
            }
        }
        catch (Exception ex)
        {
            // TODO: log exception
            Console.WriteLine(ex);

            plainText = "";
            return false;
        }
    }

    #endregion
}
