using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using UnityEngine;

class PoolSystem
{
    private static PoolSystem _instance;
    public static PoolSystem Instance => _instance == null ? _instance = new PoolSystem() : _instance;


    public GameObject[] prefabs;
    private GameObject[] poolObjects;
    private List<BaseObject>[] allocatedObjects, readyToAllocateObjects;



    public void InitilazePool(GameObject LetterPrefab, GameObject staticTilePrefab)
    {
        prefabs = new GameObject[(int)ObjectType.ObjectTypeCount];
        prefabs[(int)ObjectType.LETTER] = LetterPrefab;
        prefabs[(int)ObjectType.STACK_OBJECT] = staticTilePrefab;

        InitilazeLists();
        InitilazePoolObjects();
        CreateObjects();
    }


    public T Allocate<T>() where T : BaseObject
    {
        T tile;
        int type;


        type = (int)Utils.ObjectInfo.AnalyzeType<T>();
        tile = (T)readyToAllocateObjects[type][0];
        readyToAllocateObjects[type].RemoveAt(0);
        allocatedObjects[type].Add(tile);
        return tile;
    }

    public void Deallocate<T>(BaseObject tile) where T : BaseObject
    {
        int type;

        type = (int)Utils.ObjectInfo.AnalyzeType<T>();
        if (allocatedObjects[type].Contains(tile))
        {
            readyToAllocateObjects[type].Add(tile);
            allocatedObjects[type].Remove(tile);
        }
    }

    public ReadOnlyCollection<T> Allocate<T>(int count) where T : BaseObject
    {
        BaseObject tile;
        int type;

        type = (int)Utils.ObjectInfo.AnalyzeType<T>();
        for (int i = 0; i < count; i++)
        {
            tile = readyToAllocateObjects[type][0];
            readyToAllocateObjects[type].RemoveAt(0);
            allocatedObjects[type].Add(tile);
        }
        return GetSafeAllocatedObjects<T>();
    }

    public void Deallocate<T>() where T : BaseObject
    {
        int type;

        type = (int)Utils.ObjectInfo.AnalyzeType<T>();
        while (allocatedObjects[type].Count > 0)
        {
            readyToAllocateObjects[type].Add(allocatedObjects[type][0]);
            allocatedObjects[type].RemoveAt(0);
        }
    }

    public ReadOnlyCollection<T> GetSafeAllocatedObjects<T>() where T : BaseObject
    {
        int type;

        type = (int)Utils.ObjectInfo.AnalyzeType<T>();
        return allocatedObjects[type].OfType<T>().ToList().AsReadOnly();
    }




    private void InitilazeLists()
    {
        allocatedObjects = new List<BaseObject>[(int)ObjectType.ObjectTypeCount];
        for(int i = 0; i < (int)ObjectType.ObjectTypeCount; i++) allocatedObjects[i] = new List<BaseObject>();

        readyToAllocateObjects = new List<BaseObject>[(int)ObjectType.ObjectTypeCount];
        for(int i = 0; i < (int)ObjectType.ObjectTypeCount; i++) readyToAllocateObjects[i] = new List<BaseObject>();
    }

    private void InitilazePoolObjects() {
        poolObjects = new GameObject[(int)ObjectType.ObjectTypeCount + 1];

        poolObjects[(int)ObjectType.ObjectTypeCount] = new GameObject("PoolObject");
        poolObjects[(int)ObjectType.ObjectTypeCount].transform.SetParent(GameObject.Find("GameObject").transform);

        for (int i = 0; i < (int)ObjectType.ObjectTypeCount; i++)
        {
            poolObjects[i] = new GameObject(((ObjectType)i).ToString().ToLower() + "s");
            poolObjects[i].transform.SetParent(poolObjects[(int)ObjectType.ObjectTypeCount].transform);
        }
    }

    private void CreateObjects()
    {
        int totalTileCount;
        int longestWordLength;

        totalTileCount = LevelManager.Instance.MaxLetterCountPerLevel;
        for (int i = 0; i < totalTileCount; i++) CreateObject(ObjectType.LETTER);

        longestWordLength = WordManager.Instance.LongestWordLength;
        for (int i = 0; i < longestWordLength; i++) CreateObject(ObjectType.STACK_OBJECT);
    }

    private void CreateObject(ObjectType type)
    {
        GameObject gameObject;
        BaseObject tile;


        gameObject = GameObject.Instantiate(prefabs[(int)type]);
        gameObject.transform.SetParent(poolObjects[(int)type].transform);

        tile = gameObject.GetComponent<BaseObject>();
        readyToAllocateObjects[(int)type].Add(tile);
        tile.TurnOffObject();
    }
}
