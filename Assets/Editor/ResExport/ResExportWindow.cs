using UnityEngine;
using UnityEditor;
using System.IO;
using System.Xml;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using Game;
/// <summary>
/// 导出图的origin是中心点，offset是当前图与父节点的偏移量，spriteResInfo.maps中是节点与当前图原点的偏移量
/// </summary>
public class ResExportWindow : EditorWindow {
	private static string CfgName = "index.html";
	private static string XmlHead = "<?xml version=\"1.0\" encoding=\"UTF-8\" ?>";
	private static string RootStart = "<root>";
	private static string RootEnd = "</root>";
    //只导出这些名字的身体部位
	private static List<string> PartNames = new List<string>{
		"body","arm","hand","head","ear","hairOverHead","hair","backHair","backHairBelowCap"
    };

	private static List<string> faceNames = new List<string>{
		"face"
	};

	//身体部位的挂载点名字(如果没有挂载点，那么它的中心点就是源点，有的话中心点就是挂载点的位置)//所有挂载点前加上map字符
    private static Dictionary<string, string> mapPartMount = new Dictionary<string, string> {
        {"arm","map_navel" },
        { "hand","map_navel"},
		{"head","map_neck"},
		{"ear","map_neck"},
        { "face","map_brow"},
        { "hairOverHead","map_brow"},
        { "hair","map_brow"},
        { "backHair","map_brow"},
        { "backHairBelowCap","map_brow"}
    };

	private static List<string> DefaultActionName = new List<string>{
		"stand2","walk2","jump"
	};

    private static List<string> DefaultOtherName = new List<string> {
        "default","smile"
    };

	private static Dictionary<AnimType,Vector2> MapAnimType = new Dictionary<AnimType, Vector2>{
		{AnimType.Base,new Vector2(2000,20000)},
		{AnimType.Face,new Vector2(20000,30000)},
		{AnimType.Hair,new Vector2(30000,40000)},
	};
	public enum GameUnit
	{
		Player,
		Monster,
		Npc
	}

	public enum AnimType
	{
		Base,
		Face,
        Hair
	}

	[MenuItem("Tools/ResExport _F1")]
	static void ShowResExportWindow()
	{
		ResExportWindow window = EditorWindow.GetWindowWithRect<ResExportWindow> (new Rect(200,200,600,400),true,"资源导入");
//		ResExportWindow window = EditorWindow.GetWindow<ResExportWindow>();
		window.Show ();
	}

	void EventListener()
	{
		Event e = Event.current;
		if (e.keyCode == KeyCode.Escape && e.type == EventType.KeyUp)
		{
			this.Close ();
		}
	}

	void OnGUI()
	{
		EventListener ();
		SelectFolder ();
		DrawContent ();
		//Test ();
	}

	private string folderName = "";
	private string rootFolderName;
	private GameUnit gameUnit = GameUnit.Player;
	private AnimType animType = AnimType.Base;
	void SelectFolder()
	{
		EditorGUILayout.BeginHorizontal ();
		EditorGUILayout.LabelField ("选择导出文件夹", GUILayout.ExpandWidth(false));
		EditorGUILayout.EndHorizontal ();
		using (var scope = new EditorGUILayout.HorizontalScope ())
		{
			EditorGUILayout.LabelField ("文件夹:", GUILayout.MaxWidth(60));
			folderName = EditorGUILayout.TextField (folderName,GUILayout.Width(300));
			if (GUILayout.Button ("选择", GUILayout.ExpandWidth(false)))
			{
				string dirPath = EditorUtility.OpenFolderPanel ("选择文件夹", folderName, "");
				if (!string.IsNullOrEmpty (dirPath))
				{
					folderName = dirPath;
				}
				if (!string.IsNullOrEmpty (folderName))
				{
					ParseHtml ();			
				}
			}
			gameUnit = (GameUnit)EditorGUILayout.EnumPopup ((System.Enum)gameUnit);
			animType = (AnimType)EditorGUILayout.EnumPopup ((System.Enum)animType);
		}
	}

	private Dictionary<string,string> infoMap = new Dictionary<string, string> ();
	private List<string> listAction = new List<string> ();
	private List<bool> listSelectAction = new List<bool>();
	private Dictionary<string,List<ActionKeys>> actionToInfos = new Dictionary<string, List<ActionKeys>> ();
	private void ParseHtml()
	{
		if (!Directory.Exists (folderName))
			return;
		int index = folderName.LastIndexOf ('/');
		rootFolderName = folderName.Substring (index + 1, folderName.Length - index - 1);
		if (string.IsNullOrEmpty (rootFolderName))
		{
			Debug.Log ("rootFolderName is null!");
			return;
		}
        if(rootFolderName.IndexOf('.') != -1)
        {
            rootFolderName = rootFolderName.Split('.')[0];
        }
		int animTypeValue = int.Parse (rootFolderName);
		bool hasAnimType = false;
		foreach (var item in MapAnimType)
		{
			if (animTypeValue >= item.Value.x && animTypeValue < item.Value.y)
			{
				animType = item.Key;
				hasAnimType = true;
				break;
			}
		}
		if (!hasAnimType)
		{
			Debug.LogError ("can not find animType with " + rootFolderName);
			return;
		}
		string cfgPath = folderName + "/" + CfgName;
		if (!File.Exists (cfgPath))
		{
			Debug.Log ("can not find "+cfgPath);
			return;
		}
		string originContent = File.ReadAllText (cfgPath);
		string xmlContent = XmlHead + RootStart + originContent + RootEnd;
		XmlDocument xml = new XmlDocument ();
		xml.LoadXml (xmlContent);
		XmlNodeList trList = xml.SelectNodes("/root/table/tr");
		infoMap.Clear ();
		listAction.Clear ();
		actionToInfos.Clear ();
		foreach (XmlNode trNode in trList)
		{
			XmlNodeList tdNodes = trNode.ChildNodes;
			string key = tdNodes.Item (0).InnerText;
			XmlNode valueNode = tdNodes.Item (1);
			string value;
			XmlNode childNode = valueNode.ChildNodes.Item (0);
			if (childNode.Attributes != null && childNode.Attributes ["src"] != null)
			{
				value = childNode.Attributes ["src"].Value;
			}
			else
			{
				value = valueNode.InnerText;
			}
			infoMap.Add (key, value);

			string[] strs = key.Split ('.');
            //判断是否是默认动作，将第0帧加入
            bool isDefaultAction = key.StartsWith("default.") || key.StartsWith("backDefault.");
            if (strs == null || (strs.Length < 3 && !isDefaultAction))
				continue;
			List<ActionKeys> tempList;
			actionToInfos.TryGetValue (strs [0], out tempList);
			if (tempList == null)
			{
				tempList = new List<ActionKeys> ();
				actionToInfos.Add (strs [0], tempList);
			}
            if(isDefaultAction)
            {
                string[] tempStrArr = new string[strs.Length + 1];
                tempStrArr[0] = strs[0];
                tempStrArr[1] = "0";
                for (int i = 2; i < tempStrArr.Length; i++)
                {
                    tempStrArr[i] = strs[i - 1];
                }
                strs = tempStrArr;
            }
			ActionKeys actionkeys = new ActionKeys (key,strs);
			tempList.Add (actionkeys);
			if(listAction.Contains(strs[0]))
				continue;
			listAction.Add (strs[0]);
			listSelectAction.Add (DefaultActionName.Contains(strs[0]) || DefaultOtherName.Contains(strs[0]));
		}
		Debug.Log ("infoMap:"+infoMap.Count);
		string path = "./test.xml";
		if (File.Exists (path))
		{
			File.Delete (path);
		}
		StreamWriter writer = File.CreateText (path);
		foreach (var item in infoMap)
		{
			writer.WriteLine (item.Key + ":" + item.Value);
		}
		writer.Flush ();
		writer.Close ();
		writer.Dispose ();
	}

	private Vector2 scrollPos;
	private bool isSelectAll;
	private bool isInverse;

	private List<string> exportData = new List<string> ();
	void DrawContent()
	{
		using (var horiztalScope = new GUILayout.HorizontalScope ())
		{
			GUILayout.Label("所有动作:",GUILayout.ExpandWidth(false));
			bool tempSelect = isSelectAll;
			isSelectAll = GUILayout.Toggle (isSelectAll,"选择所有",GUILayout.ExpandWidth(false));
			if (tempSelect != isSelectAll)
			{
				for (int i = 0; i < listSelectAction.Count; i++)
				{
					listSelectAction [i] = isSelectAll;
				}
			}
			tempSelect = isInverse;
			isInverse = GUILayout.Toggle (isInverse,"反选所有",GUILayout.ExpandWidth(false));
			if (tempSelect != isInverse && isInverse)
			{
				for (int i = 0; i < listSelectAction.Count; i++)
				{
					listSelectAction [i] = !listSelectAction[i];
				}
			}
			if(GUILayout.Button("导出已选择",GUILayout.ExpandWidth(false)))
			{
				exportData.Clear ();
				for (int i = 0; i < listSelectAction.Count; i++)
				{
					if (listSelectAction [i])
					{
						exportData.Add(listAction[i]);
					}
				}
				ExportData (exportData);
			}
		}
		using(var scrollScope = new EditorGUILayout.ScrollViewScope(scrollPos,GUILayout.Width(this.position.width),GUILayout.Height(300)))
		{
			scrollPos = scrollScope.scrollPosition;
			for (int i = 0; i < listAction.Count; i++)
			{
				EditorGUILayout.BeginHorizontal ();
				listSelectAction[i] = GUILayout.Toggle (listSelectAction[i],listAction[i],GUILayout.Width(500));
				if (GUILayout.Button ("导出",GUILayout.ExpandWidth(false)))
				{
					ExportData (new List<string>{listAction[i]});
				}
				EditorGUILayout.EndHorizontal ();
			}
		}
	}

	private List<int> tempIntList = new List<int>();
	private void ExportData(List<string> actionNames)
	{
		ActionResScriptObj resObj = GetResObj (rootFolderName);
		//删除当前img目录
		if (AssetDatabase.DeleteAsset ("Assets/Resources/Action/"+ gameUnit.ToString()+"/" + animType.ToString() + "/img/" + rootFolderName))
		{
			Debug.Log ("删除Assets/Resources/Action/"+ gameUnit.ToString()+"/" + animType.ToString() + "/img/"+ rootFolderName+"目录成功");
		}
		if (resObj.actionInfos == null)
			resObj.actionInfos = new SerializableDictionaryActionResInfo ();
		for (int i = 0; i < actionNames.Count; i++)
		{
			string actionName = actionNames[i];
			List<ActionKeys> tempList;
			actionToInfos.TryGetValue (actionName,out tempList);
			if (tempList == null)
			{
				Debug.Log ("can not find actionName's infos:"+actionName);
				return;
			}
			//计算总帧数
			int totalFrames = 0;
			tempIntList.Clear ();
			for (int j = 0; j < tempList.Count; j++)
			{
				int curFrame = int.Parse (tempList [j].splitKeys [1]);
				tempList [j].curFrame = curFrame;
				if (!tempIntList.Contains (curFrame))
				{
					totalFrames++;
					tempIntList.Add (curFrame);
				}
			}

			ActionResInfo actionResInfo;
			resObj.actionInfos.TryGetValue (actionName, out actionResInfo);
			if (actionResInfo == null)
			{
				actionResInfo = new ActionResInfo ();
				resObj.actionInfos.Add (actionName, actionResInfo);
			}
			actionResInfo.frameInfos = new FrameResInfo[totalFrames];
			for (int j = 0; j < totalFrames; j++) 
			{
				actionResInfo.frameInfos[j] = new FrameResInfo ();
				actionResInfo.frameInfos [j].frameResInfos = new SerializableDictionarySpriteResInfo ();
				ExportFrameData (tempList, j, actionResInfo.frameInfos[j]);
			}
		}
		EditorUtility.SetDirty (resObj);
		AssetDatabase.SaveAssets();
		AssetDatabase.Refresh (ImportAssetOptions.ForceUpdate);
       
    }

	private Dictionary<string,SpriteResInfo> _mapSpriteInfos = new Dictionary<string, SpriteResInfo> ();
	private void ExportFrameData(List<ActionKeys> list,int frame,FrameResInfo frameResInfo)
	{
		_mapSpriteInfos.Clear ();
		for (int i = 0; i < list.Count; i++)
		{
			ActionKeys actionKey = list[i];
			if (actionKey.curFrame == frame)
			{
				//导出所有图片到预设里
				ExportFrameResData (actionKey, frame, frameResInfo);
                ////导出其他动作资源
                //ExportFrameOtherData(actionKey, frame);

            }
		}
        //处理一些数据（调整sprite的中心点，一些map点的转换）
        if(frameResInfo.frameResInfos != null)
        {
            int count = frameResInfo.frameResInfos.ListKeys.Count;
            for (int i = 0; i < count; i++)
            {
                string partName = frameResInfo.frameResInfos.ListKeys[i];
                SpriteResInfo spriteResInfo = frameResInfo.frameResInfos.ListValues[i];
                Sprite sprite = spriteResInfo.sprite;
                //将所有坐标转换为相对于做下角的坐标
                spriteResInfo.origin.y = (sprite.rect.height / 100f) - spriteResInfo.origin.y;
                int mapCount = spriteResInfo.maps.ListKeys.Count;
				for (int j = 0; j < mapCount; j++)
                {
                    Vector2 pos = spriteResInfo.maps.ListValues[j];
					spriteResInfo.maps.ListValues [j] = new Vector2 (pos.x,-pos.y);
                }
                string mountName = null;
                mapPartMount.TryGetValue(partName, out mountName);
				Vector2 pivot = spriteResInfo.origin;
                if (string.IsNullOrEmpty(mountName))
                {
					spriteResInfo.offset = Vector2.zero; 
                }
                else
                {
					spriteResInfo.offset = -spriteResInfo.maps[mountName];
                    spriteResInfo.maps.Remove(mountName);
                }
                pivot.x = pivot.x / (sprite.rect.width/100f);
                pivot.y = pivot.y / (sprite.rect.height/100f); 
                string path = AssetDatabase.GetAssetPath(sprite);
                TextureImporter textureImporter = AssetImporter.GetAtPath(path) as TextureImporter;
                TextureImporterSettings tis = new TextureImporterSettings();
                textureImporter.ReadTextureSettings(tis);
                tis.spriteAlignment = 9; //customer  
                tis.spritePivot = pivot;

                textureImporter.SetTextureSettings(tis);
                AssetDatabase.ImportAsset(path);
                

            }
        }
        
	}

	private void ExportFrameResData(ActionKeys actionKey,int frame,FrameResInfo frameResInfo)
	{
		string partName = actionKey.splitKeys [2];
        switch(partName)
        {
            case "delay":
			frameResInfo.delay = (int.Parse(infoMap[actionKey.key]) / 1000f);
                break;
            default:
				//face特殊处理
			if ((animType != AnimType.Face && PartNames.Contains(partName)) || (animType == AnimType.Face && faceNames.Contains(partName)))
                {
                    if(actionKey.splitKeys.Length >= 3)
                    {
                        SpriteResInfo spriteResInfo;
                        if (frameResInfo.frameResInfos.ContainsKey(partName))
                        {
                            spriteResInfo = frameResInfo.frameResInfos[partName];
                        }
                        else
                        {
                            spriteResInfo = new SpriteResInfo();
                            spriteResInfo.maps = new SerializableDictionarySpriteMap();
                            frameResInfo.frameResInfos.Add(partName, spriteResInfo);
                        }

                        if (actionKey.splitKeys.Length == 3)
                        {
							string resValue = infoMap[actionKey.key];
							string resPath = "";
							if (resValue.EndsWith (".png"))
							{
								resPath = folderName + "/" + resValue;
								if (string.IsNullOrEmpty(resPath))
								{
									Debug.Log("key is " + actionKey.key + " can not find Res path");
									return;
								}
								
							}
							//这个特殊写
							else if(resValue.StartsWith("../../../00002000/"))
							{
								string namePrev = resValue.Substring ("../../../00002000/".Length);
								namePrev = namePrev.Replace ('/', '.');
								Debug.Log ("../../../00002000/:"+namePrev);
								infoMap.TryGetValue (namePrev, out resPath);
								if (string.IsNullOrEmpty(resPath))
								{
									Debug.Log("key is " + namePrev + " can not find Res path[../../../00002000]");
									return;
								}
								resPath = folderName + "/" + resPath;
								
								//这里将key为namePrev的SpriteResInfo找出来
								InitSpriteInfo(spriteResInfo,namePrev);
							}
							else if(resValue.StartsWith("../../"))
							{
								string namePrev = resValue.Substring ("../../".Length);
								namePrev = namePrev.Replace ('/', '.');
								infoMap.TryGetValue (namePrev, out resPath);
								Debug.Log("../../: " + namePrev);
								if (string.IsNullOrEmpty(resPath))
								{
									Debug.Log("key is " + namePrev + " can not find Res path[../../]");
									return;
								}
								resPath = folderName + "/" + resPath;
								
								//这里将key为namePrev的SpriteResInfo找出来
								InitSpriteInfo(spriteResInfo,namePrev);
							}
							else if(resValue.StartsWith("../"))
							{
								string namePrev = resValue.Substring (3);
								namePrev = namePrev.Replace ('/', '.');
								namePrev = actionKey.splitKeys [0]+"." + namePrev;
								infoMap.TryGetValue (namePrev, out resPath);
								if (string.IsNullOrEmpty(resPath))
								{
									Debug.Log("key is " + namePrev + " can not find Res path[../]");
									return;
								}
								resPath = folderName + "/" + resPath;
								
								//这里将key为namePrev的SpriteResInfo找出来
								InitSpriteInfo(spriteResInfo,namePrev);
							}
							else
							{
								Debug.LogError ("can not support format:actionKey:"+actionKey.key+",value:"+resValue+" to analy");
								return;
							}	
							if (string.IsNullOrEmpty(resPath))
							{
								return;
							}
							string savePath = ImportPng(resPath);
							if (string.IsNullOrEmpty(savePath))
							{
								Debug.Log("import png " + resPath + " fail!");
								return;
							}
							Sprite sprite = (Sprite)AssetDatabase.LoadAssetAtPath(savePath, typeof(Sprite));
							spriteResInfo.sprite = sprite;
                        }
                        else if (actionKey.splitKeys.Length > 3)
                        {
                            switch (actionKey.splitKeys[3])
                            {
                                case "origin":
                                    string originStr = infoMap[actionKey.key];
                                    string[] originArr = originStr.Split(',');
                                    spriteResInfo.origin = new Vector2(float.Parse(originArr[0]) / 100,float.Parse(originArr[1]) / 100);
                                    break;
                                case "z":
                                    spriteResInfo.z = infoMap[actionKey.key];
                                    break;
                                case "group":
                                    spriteResInfo.group = infoMap[actionKey.key];
                                    break;
                                case "map":
                                    if(actionKey.splitKeys.Length > 4)
                                    {
                                        string vecValue = infoMap[actionKey.key];
                                        string[] vecValueArr = vecValue.Split(',');
										//所有挂载点前加上map字符
                                        spriteResInfo.maps.Add("map_"+actionKey.splitKeys[4], new Vector2(float.Parse(vecValueArr[0]) / 100,float.Parse(vecValueArr[1])/100));
                                    }
                                    break;
                            }
                        }
                    }
                    
                }
                break;

        }
       
	}

	private void InitSpriteInfo(SpriteResInfo spriteResInfo,string key)
	{
		SpriteResInfo tempSpriteResInfo;
		_mapSpriteInfos.TryGetValue (key, out tempSpriteResInfo);
		if (tempSpriteResInfo != null)
		{
			CopySpriteResInfoWithoutSprite (spriteResInfo, tempSpriteResInfo);
			return;
		}
		else
		{
			tempSpriteResInfo = new SpriteResInfo ();
			tempSpriteResInfo.maps = new SerializableDictionarySpriteMap ();
			_mapSpriteInfos.Add (key, tempSpriteResInfo);
		}
		foreach (var item in infoMap)
		{
			if (item.Key.StartsWith (key) && item.Key != key)
			{
				string infoKey = item.Key.Substring (key.Length + 1);
				string[] infoKeyArr = infoKey.Split ('.');
				switch (infoKeyArr[0])
				{
				case "origin":
					string originStr = item.Value;
					string[] originArr = originStr.Split(',');
					tempSpriteResInfo.origin = new Vector2(float.Parse(originArr[0]) / 100,float.Parse(originArr[1]) / 100);
					break;
				case "z":
					tempSpriteResInfo.z = item.Value;
					break;
				case "group":
					tempSpriteResInfo.group = item.Value;
					break;
				case "map":
					if(infoKeyArr.Length > 1)
					{
						string vecValue = item.Value;
						string[] vecValueArr = vecValue.Split(',');
						//所有挂载点前加上map字符
						tempSpriteResInfo.maps.Add("map_"+infoKeyArr[1], new Vector2(float.Parse(vecValueArr[0]) / 100,float.Parse(vecValueArr[1])/100));
					}
					break;
				}
			}
		}
		CopySpriteResInfoWithoutSprite (spriteResInfo, tempSpriteResInfo);
	}

	private void CopySpriteResInfoWithoutSprite(SpriteResInfo spriteResInfo,SpriteResInfo tempSpriteResInfo)
	{
		spriteResInfo.group = tempSpriteResInfo.group;
		spriteResInfo.origin = tempSpriteResInfo.origin;
		spriteResInfo.z = tempSpriteResInfo.z;
		spriteResInfo.maps.Clear ();
		foreach (var item in tempSpriteResInfo.maps)
		{
			spriteResInfo.maps.Add (item.Key, new Vector2(item.Value.x,item.Value.y));
		}
	}

	private string GetResPath(string key)
	{
		string resValue = infoMap[key];
		if (resValue.EndsWith (".png"))
		{
			return folderName + "/" + resValue;
		}
		else
		{
			Debug.LogError ("暂不支持可连接的路径");
			return "";
		}
	}

	private string ImportPng(string path)
	{
		if (!File.Exists (path))
			return "";
		string saveDir = Application.dataPath + "/Resources/Action/" + gameUnit.ToString()+"/" + animType.ToString() + "/img/" + rootFolderName;
		if (!Directory.Exists(saveDir))
		{
			Directory.CreateDirectory (saveDir);
			Debug.Log ("创建目录:"+saveDir);
			AssetDatabase.ImportAsset (saveDir);
		}
		string fileName = GetFileName(path);
		string copyPath = saveDir + "/" + fileName;

		try
		{
			File.Copy (path,copyPath , true);
			Debug.Log("importPath:"+copyPath);
			string importPath = GetImportSpritePath(path);
			AssetDatabase.ImportAsset (importPath,ImportAssetOptions.ForceUpdate);
			return importPath;
		}catch(System.Exception e)
		{
			return "";
		}
	}

    private string GetFileName(string path)
    {
        int nameIndex = path.LastIndexOf('/');
        return path.Substring(nameIndex + 1, path.Length - nameIndex - 1);
    }

    private string GetImportSpritePath(string path)
    {
		return "Assets/Resources/Action/" + gameUnit.ToString()+"/" + animType.ToString() + "/img/" + rootFolderName + "/" + GetFileName(path);
    }


	private ActionResScriptObj GetResObj(string resName)
	{
		string dir = "/Resources/Action/" + gameUnit.ToString() + "/" + animType.ToString() + "/prefab/";

		string realDir = Application.dataPath + dir;
		Debug.Log ("realDir:"+realDir);
		if (!Directory.Exists(realDir))
		{
			Directory.CreateDirectory (realDir);
			Debug.Log ("创建目录:"+realDir);
			AssetDatabase.ImportAsset (realDir);
		}
		string[] guids = AssetDatabase.FindAssets (resName + " t:Object", new string[]{ "Assets"+dir }); 
		ActionResScriptObj obj;
		//删除资源
		if (guids != null && guids.Length > 0)
		{
			string resPath = AssetDatabase.GUIDToAssetPath (guids [0]);
			AssetDatabase.DeleteAsset (resPath);
		}
		Debug.Log ("can not find res");
		obj = ScriptableObject.CreateInstance<ActionResScriptObj> (); 
		AssetDatabase.CreateAsset (obj, "Assets/Resources/Action/" + gameUnit.ToString() + "/" + animType.ToString() + "/prefab/" + resName + ".asset");
		return obj;
	}

	//void Test()
	//{
	//	if (GUILayout.Button ("创建资源"))
	//	{
	//		ActionResScriptObj obj = GetResObj (rootFolderName);
	//		obj.actionInfos = new SerializableDictionaryActionResInfo ();
	//		for (int i = 0; i < 2; i++)
	//		{
	//			ActionResInfo actionInfo = new ActionResInfo (); 
	//			FrameResInfo frameInfo = new FrameResInfo ();
	//			frameInfo.frameResInfos = new SerializableDictionarySpriteResInfo ();
	//			frameInfo.frameResInfos.Add ("body", null);
	//			actionInfo.frameInfos = new FrameResInfo[]{ frameInfo, frameInfo };
	//			obj.actionInfos.Add ("test" + i, actionInfo);
	//		}
	//		AssetDatabase.SaveAssets (); 
	//	}
	//}

	public class ActionKeys
	{
		public string key{get;private set;}
		public string[] splitKeys{get;private set;}
		public int curFrame{ get; set;}
		public ActionKeys(string key,string[] splitKeys)
		{
			this.key = key;
			this.splitKeys = splitKeys;
		}
	}
}

