using UnityEditor;
using UnityEngine;

public class BuildiOSAssetBundles : MonoBehaviour
{
    [InitializeOnLoadMethod]
    static void SetupResourcesBuild()
    {
        UnityEditor.iOS.BuildPipeline.collectResources += CollectResources;
    }

    static UnityEditor.iOS.Resource[] CollectResources()
    {
        return new UnityEditor.iOS.Resource[]
        {
            new UnityEditor.iOS.Resource( "objects", "Assets/AssetBundlesIOS/Objects" ).AddOnDemandResourceTags( "objects" ),
        };
    }

    [MenuItem("Bundle/Build iOS AssetBundle")]
    static void BuildAssetBundles()
    {
        var options = BuildAssetBundleOptions.None;

        bool shouldCheckODR = EditorUserBuildSettings.activeBuildTarget == BuildTarget.iOS;

#if UNITY_TVOS
            shouldCheckODR |= EditorUserBuildSettings.activeBuildTarget == BuildTarget.tvOS;
#endif

        if (shouldCheckODR)
        {
#if ENABLE_IOS_ON_DEMAND_RESOURCES
            if (PlayerSettings.iOS.useOnDemandResources)
                options |= BuildAssetBundleOptions.UncompressedAssetBundle;
#endif

#if ENABLE_IOS_APP_SLICING
            options |= BuildAssetBundleOptions.UncompressedAssetBundle;
#endif
        }

        BuildPipeline.BuildAssetBundles("Assets/AssetBundlesIOS/ObjectsIOS", options, EditorUserBuildSettings.activeBuildTarget);
    }

}
