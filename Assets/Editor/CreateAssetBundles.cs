using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using Newtonsoft.Json;
using UnityEditor;
using UnityEngine;

public class CreateAssetBundles : MonoBehaviour
{
    static string staticoutputPath = "Assets/AssetsBundles/IOS";
    static string bundlePath = staticoutputPath + "/Android";

    [MenuItem("Assets/Build AssetBundles")]
    private static void BuildAssetBundles()
    {
        try
        {
            BuildPipeline.BuildAssetBundles($"Assets/AssetsBundles/Android", BuildAssetBundleOptions.None, BuildTarget.Android);
            BuildPipeline.BuildAssetBundles($"Assets/AssetsBundles/IOS", BuildAssetBundleOptions.None, BuildTarget.iOS);
            //BuildPipeline.BuildAssetBundles($"Assets/AssetsBundles/Windows", BuildAssetBundleOptions.None, BuildTarget.StandaloneWindows);
            string[] bundleNames = AssetDatabase.GetAllAssetBundleNames();
            Debug.Log("AssetBundle Names: " + string.Join(", ", bundleNames));
            CreateCatalog(bundleNames);
        }
        catch (System.Exception e)
        {
            Debug.LogError(e);
        }
    }


    private static void CreateCatalog(string[] bundlesNames)
    {
        // 2. Calculate Hashes, Timestamps, and Sizes
        Dictionary<string, BundleInfo> bundleInfos = new Dictionary<string, BundleInfo>();

        foreach (string bundleName in bundlesNames)
        {
            string bundlePath = Path.Combine(staticoutputPath, bundleName); // Construct the full path
            byte[] bundleBytes = File.ReadAllBytes(bundlePath);

            using (MD5 md5 = MD5.Create()) // Or SHA256
            {
                byte[] hashBytes = md5.ComputeHash(bundleBytes);
                string hashString = System.BitConverter.ToString(hashBytes).Replace("-", "").ToLower(); // Convert to hex string
                string timestamp = File.GetCreationTime(bundlePath).ToString("o"); // ISO 8601 format
                long size = new FileInfo(bundlePath).Length; // Get the file size in bytes
                bundleInfos.Add(bundleName, new BundleInfo { Hash = hashString, Timestamp = timestamp, Size = size });
            }
        }

        // 3. Create and Write Manifest (JSON Example)
        string manifestPath = Path.Combine("Assets/AssetsBundles", "manifest.json");
        string manifestJson = JsonConvert.SerializeObject(new ManifestData { Bundles = bundleInfos }, Formatting.Indented);
        File.WriteAllText(manifestPath, manifestJson);

        AssetDatabase.Refresh(); // Important: Refresh the Unity Editor so it recognizes the new files
    }
}

[System.Serializable]
public class ManifestData
{
    public Dictionary<string, BundleInfo> Bundles;
}

[System.Serializable]
public class BundleInfo
{
    public string Hash;
    public string Timestamp;
    public long Size; // Size in bytes
}
