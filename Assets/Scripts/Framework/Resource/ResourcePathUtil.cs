using System;
using UnityEngine;
namespace Framework
{
    /// <summary>
    /// 资源的路径处理，在下载的时候使用真正的路径，暂时不考虑平台的问题
    /// </summary>
    public class ResourcePathUtil
    {
        public static string GetInResPath(string path,ResourceType resType, ResourceStorageType storageType)
        {
           
#if UNITY_EDITOR
            string resultPath = path;
            if (storageType == ResourceStorageType.UnKnow)
            {
                if (resType == ResourceType.DirectObject)
                {
                    storageType = ResourceStorageType.InResource;
                }
                else
                {
                    storageType = ResourceStorageType.OutResource;
                }
            }
            switch(storageType)
            {
                case ResourceStorageType.InResource:
                    int lastIndexPoint = resultPath.LastIndexOf('.');
                    if (lastIndexPoint != -1)
                    {
                        int lastIndexSlash = resultPath.LastIndexOf('/');
                        if ((lastIndexSlash != -1 && lastIndexSlash < lastIndexPoint) || (lastIndexSlash == -1))
                        {
                            resultPath = resultPath.Substring(0, lastIndexPoint);
                        }
                    }
                    break;
                case ResourceStorageType.OutResource:
                    resultPath = "file:///" + Application.dataPath + "/Resources/" + resultPath;
                    break;
                case ResourceStorageType.Remote:
                    CLog.LogError("can not support remote path!");
                    break;
            }
            return resultPath;
        }
#else
        CLog.LogError("Current Platform must be Editor!");
        return path;
#endif

    }
}
