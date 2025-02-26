using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class DownloadAssetsBundles : MonoBehaviour
{
    private enum AssetBundleType
    {
        TEXTURE2D,
        GAMEOBJECT,
        AUDIOCLIP,
        UNKNOWN

    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        StartCoroutine(DownloadBundles());
    }

    private IEnumerator DownloadBundles()
    {
        GameObject go = new GameObject();

        string uri = "https://gs://kleatest-70c98.firebasestorage.app/Windows";

        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(uri))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Downloaded asset bundle successfully.");
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                if (bundle != null)
                {
                    bundle.LoadAllAssets();
                    GameObject[] assets = bundle.LoadAllAssets<GameObject>();
                    foreach (GameObject asset in assets)
                    {
                        Instantiate(asset);
                    }
                }

                www.Dispose();
            }
        }


        yield return null;
    }

    private IEnumerator DownloadAssetsType(Callback<dynamic, AssetBundleType> callback, string assetBundleName = "")
    {
        dynamic assetBundleLoad = null;
        AssetBundleType assetBundleType = AssetBundleType.UNKNOWN;

        string uri = "https://gs://kleatest-70c98.firebasestorage.app/Windows/" + assetBundleName;

        using (UnityWebRequest www = UnityWebRequestAssetBundle.GetAssetBundle(uri))
        {
            yield return www.SendWebRequest();

            if (www.result == UnityWebRequest.Result.ConnectionError || www.result == UnityWebRequest.Result.ProtocolError)
            {
                Debug.LogError(www.error);
            }
            else
            {
                Debug.Log("Downloaded asset bundle successfully.");
                AssetBundle bundle = DownloadHandlerAssetBundle.GetContent(www);
                assetBundleLoad = bundle.LoadAllAssets();
                www.Dispose();
            }
        }


        yield return null;
    }

}


