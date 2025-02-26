using System.IO;
using UnityEngine;

public class Load : MonoBehaviour
{
    public string path = "Assets/AssetsBundles/Android";

    private GameObject avatar;

    [ContextMenu("Load Mesh")]
    public void LoadMesh()
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(path, "mesh"));

        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }

        avatar = myLoadedAssetBundle.LoadAsset<GameObject>("KleaV4");
        Instantiate(avatar);
    }

    [ContextMenu("Load Texture")]
    public void LoadTexture()
    {
        var myLoadedAssetBundle = AssetBundle.LoadFromFile(Path.Combine(path, "texture"));

        if (myLoadedAssetBundle == null)
        {
            Debug.Log("Failed to load AssetBundle!");
            return;
        }
    }

    [ContextMenu("Reapply Shaders")]
    public void ReapplyShaders()
    {
        Renderer[] renderers = avatar.GetComponentsInChildren<Renderer>();
        Material[] materials;
        string[] shaders;

        foreach (var rend in renderers)
        {
            materials = rend.sharedMaterials;
            shaders = new string[materials.Length];

            for (int i = 0; i < materials.Length; i++)
            {
                shaders[i] = materials[i].shader.name;
            }

            for (int i = 0; i < materials.Length; i++)
            {
                materials[i].shader = Shader.Find(shaders[i]);
            }
        }
    }
}
