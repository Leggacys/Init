using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;


public class LoadDressable : MonoBehaviour
{

    public List<string> keys = new List<string>() { "default" };

    public TextMeshProUGUI text;

    AsyncOperationHandle<IList<GameObject>> loadHandle;


    void Start()
    {

    }

    private void OnLoadCompleted(AsyncOperationHandle<IList<GameObject>> handle)
    {
        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            text.text = ("Successfully loaded all assets.");
            Debug.Log("Successfully loaded all assets from remote server.");
        }
        else if (handle.Status == AsyncOperationStatus.Failed)
        {
            text.text = ($"Failed to load assets: {handle.OperationException?.Message}");
            Debug.LogError($"Failed to load assets: {handle.OperationException}");
            if (handle.OperationException != null)
            {
                Debug.LogError(handle.OperationException.StackTrace);
            }
        }
        else if (handle.Status == AsyncOperationStatus.None)
        {
            text.text = ("Operation status is None.");
        }
    }


}
