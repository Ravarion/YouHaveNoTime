using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEditor;
using System.IO;
using UnityEngine.SceneManagement;

public class MapMaker : MonoBehaviour {

    public GameObject[] floorList;
    public GameObject[] wallList;
    public GameObject[] actorList;
    public GameObject[] hazardList;

    public RenderTexture[] renderTextures;
    public GameObject mapStudio;

    public TextAsset mapSpawnFile;
    public bool allowOverwrite = false;

    private GameController gameController;
    private GameObject[] levelStudios;

    void Start()
    {
        gameController = FindObjectOfType<GameController>();
        levelStudios = new GameObject[SceneManager.sceneCountInBuildSettings - gameController.scenesNotScored];

        if (SceneManager.GetActiveScene().buildIndex == 0)
        {
            LoadLevelSelectMaps();
            return;
        }

        string mapText = GetMapText(SceneManager.GetActiveScene());

        if(mapText != "")
        {
            CreateMap(mapText);
        }
        else
        {
            print("Could not load map");
        }

        if (FindObjectOfType<MovementController>() == null && FindObjectOfType<SpawnPlayer>() == null)
        {
            print("No Player Found.  Spawning Player.");
            GameObject newPlayer = SpawnPlayer();
            GameObject map = GetMap();
            newPlayer.transform.SetParent(map.transform);
        }
    }

    private MapPiece ReadMapChunk(string mapChunk)
    {
        MapPiece piece;
        int i = 0;

        piece.posX = int.Parse(mapChunk.Substring(i, 3));
        piece.posX *= (mapChunk.Substring(i + 3, 1) == "p") ? 1 : -1;
        i += 4;

        piece.posY = int.Parse(mapChunk.Substring(i, 3));
        piece.posY *= (mapChunk.Substring(i + 3, 1) == "p") ? 1 : -1;
        i += 4;

        piece.posZ = int.Parse(mapChunk.Substring(i, 3));
        piece.posZ *= (mapChunk.Substring(i + 3, 1) == "p") ? 1 : -1;
        i += 4;

        piece.rotX = int.Parse(mapChunk.Substring(i, 3));
        piece.rotX *= (mapChunk.Substring(i + 3, 1) == "p") ? 1 : -1;
        i += 4;

        piece.rotY = int.Parse(mapChunk.Substring(i, 3));
        piece.rotY *= (mapChunk.Substring(i + 3, 1) == "p") ? 1 : -1;
        i += 4;

        piece.rotZ = int.Parse(mapChunk.Substring(i, 3));
        piece.rotZ *= (mapChunk.Substring(i + 3, 1) == "p") ? 1 : -1;
        i += 4;

        piece.tileType = (TileType)int.Parse(mapChunk.Substring(i, 1));
        i += 1;

        piece.objName = mapChunk.Substring(i);

        return piece;
    } // End ReadMapChunk

    public GameObject CreateMap(string mapString)
    {
        // Destroy the current map so that we can start fresh
        DestroyMapObject();
        GameObject map = GetMap();

        map.transform.position.Set(0, 0, 0);

        string[] mapChunks = mapString.Split('/');
        for(int i = 0; i < mapChunks.Length - 1; i++)
        {
            MapPiece piece = ReadMapChunk(mapChunks[i]);
            GameObject[] tileList;
            switch(piece.tileType)
            {
                case TileType.FLOOR:
                    tileList = floorList;
                    break;
                case TileType.WALL:
                    tileList = wallList;
                    break;
                case TileType.ACTOR:
                    tileList = actorList;
                    break;
                case TileType.HAZARD:
                    tileList = hazardList;
                    break;
                default:
                    tileList = floorList;
                    break;
            }

            bool objectFound = false;
            foreach (GameObject gameObj in tileList)
            {
                if (gameObj.name == piece.objName)
                {
                    objectFound = true;
                    GameObject obj = Instantiate(gameObj, new Vector3(piece.posX, piece.posY, piece.posZ), Quaternion.identity);
                    obj.transform.rotation = Quaternion.Euler(piece.rotX, piece.rotY, piece.rotZ);
                    obj.transform.parent = map.transform;
                    break;
                }
            }
            if(!objectFound)
            {
                print("MapMaker cound not find: " + piece.objName);
            }
        }
        return map;
    } // End CreateMap

    public bool SaveMap(string pFileName)
    {
        GameObject map = GetMap();
        MapPieceData[] mapDatas = map.GetComponentsInChildren<MapPieceData>();
        string toWrite = "";
        for (int i = 0; i < mapDatas.Length; i++)
        {
            mapDatas[i].UpdateData();
            MapPiece piece = mapDatas[i].mapPiece;
            toWrite += Mathf.Abs(piece.posX).ToString("D3");
            toWrite += piece.posX >= 0 ? "p" : "n";
            toWrite += Mathf.Abs(piece.posY).ToString("D3");
            toWrite += piece.posY >= 0 ? "p" : "n";
            toWrite += Mathf.Abs(piece.posZ).ToString("D3");
            toWrite += piece.posZ >= 0 ? "p" : "n";
            toWrite += Mathf.Abs(piece.rotX).ToString("D3");
            toWrite += piece.rotX >= 0 ? "p" : "n";
            toWrite += Mathf.Abs(piece.rotY).ToString("D3");
            toWrite += piece.rotY >= 0 ? "p" : "n";
            toWrite += Mathf.Abs(piece.rotZ).ToString("D3");
            toWrite += piece.rotZ >= 0 ? "p" : "n";
            toWrite += (int)piece.tileType;
            toWrite += piece.objName;
            toWrite += "/";
        }
        //gameController.SendDebugText("About to Save Map.");
#if UNITY_ANDROID || UNITY_WEBGL
        gameController.SendDebugText("Saving to PlayerPrefs: " + "Assets/Resources/" + pFileName);
        PlayerPrefs.SetString("Assets/Resources/" + pFileName, toWrite);
#endif
#if UNITY_EDITOR
        pFileName = "Assets/Resources/" + pFileName;
#endif
#if !UNITY_EDITOR && !UNITY_ANDROID && !UNITY_WEBGL
        pFileName = "You Have No Time_Data/" + pFileName;
#endif
#if UNITY_EDITOR || (!UNITY_ANDROID && !UNITY_WEBGL)
        gameController.SendDebugText("Saving to File.");
        if (!File.Exists(pFileName) || allowOverwrite)
        {
            StreamWriter file = new StreamWriter(pFileName);
            file.WriteLine(toWrite);
            file.Close();
            print("Saved " + pFileName);
            gameController.SendDebugText("Saved to File.");

            // If we're in the editor, refresh so that we can see the new file
#if UNITY_EDITOR
            AssetDatabase.Refresh();
#endif
        }
        else
        {
            gameController.SendDebugText("Failed to save to File.");
            print(pFileName + " already exists");
            return false;
        }
#endif
        return true;
    } // End SaveMap

    public void DestroyMapObject()
    {
        DestroyImmediate(GetMap());
    }

    public string GetMapText(Scene scene, bool save)
    {
        return GetMapText(scene.name, save);
    }

    public string GetMapText(Scene scene)
    {
        return GetMapText(scene.name, false);
    }

    public string GetMapText(string levelPath)
    {
        return GetMapText(levelPath, false);
    }

    public string GetMapText(string levelPath, bool save)
    {
        // Remove any folder names, and remove the .unity at the end
        string mapName = GetMapName(levelPath);
        if (save)
        {
            SaveMap(mapName);
        }
        string mapText = "";
#if UNITY_ANDROID || UNITY_WEBGL
        gameController.SendDebugText("Loading from PlayerPrefs: " + "Assets/Resources/" + mapName + ".txt");
        mapText = PlayerPrefs.GetString("Assets/Resources/" + mapName + ".txt");
#endif
#if !UNITY_ANDROID && !UNITY_WEBGL
        if (mapText == "")
        {
            if (File.Exists("You Have No Time_Data/" + mapName + ".txt"))
            {
                StreamReader file = new StreamReader("You Have No Time_Data/" + mapName + ".txt");
                mapText = file.ReadLine();
                file.Close();
            }
        }
#endif
        if (mapText == "")
        {
            TextAsset txtAsset = (TextAsset)Resources.Load(mapName, typeof(TextAsset));
            if (txtAsset != null)
            {
                mapText = txtAsset.text;
            }
        }
        return mapText;
    }

    public string GetMapName(string levelPath)
    {
        string mapName = levelPath.Split('/')[levelPath.Split('/').Length - 1].Split('.')[0];
        return mapName;
    }

    private void LoadLevelSelectMaps()
    {
        GameObject levelSelectStudios = GameObject.Find("LevelSelectStudios");
        for(int i = gameController.scenesNotScored; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            int studioIndex = i - gameController.scenesNotScored;
            levelStudios[studioIndex] = Instantiate(mapStudio);
            levelStudios[studioIndex].transform.position = new Vector3(500 + 100 * i, 0, 500);
            levelStudios[studioIndex].transform.SetParent(levelSelectStudios.transform);
            levelStudios[studioIndex].GetComponentInChildren<Camera>().targetTexture = renderTextures[studioIndex];

            GameObject levelMap = CreateMap(GetMapText(SceneUtility.GetScenePathByBuildIndex(i)));
            levelMap.tag = "Untagged";
            levelMap.transform.SetParent(levelStudios[studioIndex].transform.GetChild(3));

            Transform[] transforms = levelMap.GetComponentsInChildren<Transform>();
            float totalX = 0;
            float totalZ = 0;
            foreach (Transform child in levelMap.GetComponentsInChildren<Transform>())
            {
                totalX += child.position.x;
                totalZ += child.position.z;
            }
            float avgX = totalX / transforms.Length;
            float avgZ = totalZ / transforms.Length;
            Vector3 centerPoint = new Vector3(avgX, 0, avgZ);

            levelMap.transform.position = new Vector3(500 + 100 * i - centerPoint.x, 0, 500 - centerPoint.z);
        }
    }

    private GameObject SpawnPlayer()
    {
        GameObject playerPrefab = null;
        foreach (GameObject gameObject in actorList)
        {
            if (gameObject.GetComponent<SpawnPlayer>())
            {
                playerPrefab = gameObject;
            }
        }
        GameObject newPlayer = Instantiate(playerPrefab, new Vector3(1, 1, 1), Quaternion.identity);
        return newPlayer;
    }

    private GameObject GetMap()
    {
        GameObject map = GameObject.FindGameObjectWithTag("Map");
        if (map == null)
        {
            map = new GameObject("Map");
            map.tag = "Map";
        }
        return map;
    }
}

public enum TileType
{
    FLOOR,
    WALL,
    ACTOR,
    HAZARD
}

[System.Serializable]
public struct MapPiece
{
    public int posX;
    public int posY;
    public int posZ;
    public int rotX;
    public int rotY;
    public int rotZ;
    public TileType tileType;
    public string objName;

    MapPiece(int pX, int pY, int pZ, int rX, int rY, int rZ, TileType type, string name)
    {
        posX = pX;
        posY = pY;
        posZ = pZ;
        rotX = rX;
        rotY = rY;
        rotZ = rZ;
        tileType = type;
        objName = name;
    }
}
#if UNITY_EDITOR
[CustomEditor(typeof(MapMaker))]
public class MapMakerEditor : Editor
{
    string savePath = "";

    public void OnEnable()
    {
        savePath = "MapFiles/" + SceneManager.GetActiveScene().name + ".txt";
    }

    public override void OnInspectorGUI()
    {
        DrawDefaultInspector();

        MapMaker mapMaker = (MapMaker)target;
        if (GUILayout.Button("Spawn Level"))
        {
            mapMaker.CreateMap(mapMaker.GetMapText(SceneManager.GetActiveScene()));
        }
        if (GUILayout.Button("Spawn From File"))
        {
            mapMaker.CreateMap(mapMaker.mapSpawnFile.text);
        }
        if(GUILayout.Button("Destroy Map"))
        {
            mapMaker.DestroyMapObject();
        }
        if (GUILayout.Button("Save To File"))
        {
            mapMaker.SaveMap(savePath);
        }
        GUIContent savePathLabelContent = new GUIContent("Save Path", "Root location is Assets Folder");
        GUILayout.Label(savePathLabelContent);
        savePath = GUILayout.TextField(savePath);
    }
}
#endif