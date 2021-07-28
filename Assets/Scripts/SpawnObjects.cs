using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RespawnObjects
{
    public List<GameObject> objects = null;//Do not modify orignal array
    public List<Transform> initPos = null;
    protected List<GameObject> modifiedObj = null; //Copy contents of orignal array to shadow copy to modify the queue
    protected List<Transform> beginP = null;
    protected List<Transform> endP = null;

    public RespawnObjects()
    {
        objects = new List<GameObject>();
        initPos = new List<Transform>();
        modifiedObj = new List<GameObject>();
        beginP = new List<Transform>();
        endP = new List<Transform>();
    }

    public void copyObj_calc_BeginPEndP()
    {
        modifiedObj.Clear();
        beginP.Clear();
        endP.Clear();

        for (int i = 0; i < objects.Count; i++)
        {
            modifiedObj.Add(objects[i]);
            beginP.Add(objects[i].transform.GetChild(0));//Get child begin
            endP.Add(objects[i].transform.GetChild(1));//Get end child
        }
    }

    public virtual void UpdatePositionOnPassingEdgeofCam()
    {
        Transform currentObject = endP[0];
        Vector3 pos = Camera.main.WorldToViewportPoint(currentObject.position);

        if(pos.x < -0.1f)
        {           
            //Move the object infront queue
            GameObject cObj = modifiedObj[0];
            Transform f_begin = beginP[0];//Get begin child
            Transform f_end = endP[0];//Get end child

            modifiedObj.RemoveAt(0);
            beginP.RemoveAt(0);
            endP.RemoveAt(0);

            Transform l_end = endP[modifiedObj.Count - 1];

            Vector3 newObjPosition = l_end.position + (cObj.transform.position - f_begin.position);
            cObj.transform.position = new Vector3(newObjPosition.x, cObj.transform.position.y, cObj.transform.position.z);
            //newObjPosition;

            //Readd removed objects
            modifiedObj.Add(cObj);
            beginP.Add(f_begin);
            endP.Add(f_end);
        }
        //Debug.Log(currentObject.name + " : " + pos);
    }    

    public virtual void ResetAssetPositions()
    {
        copyObj_calc_BeginPEndP();

        if (initPos.Count != objects.Count)
            Debug.LogError("Init positions and object counts do not match, Recheck the assignments");
        for(int i = 0; i < initPos.Count; i++)
        {
            objects[i].transform.position = initPos[i].position;
        }
    }
}

[System.Serializable]
public class RespawnRandom:RespawnObjects
{
    public bool allowSpawnMultiple = false;

    private List<GameObject> pool = null;
    private List<Transform> endPpool = null;
    private List<Transform> beginPpool = null;

    public RespawnRandom()
    {
        pool = new List<GameObject>();
        endPpool = new List<Transform>();
        beginPpool = new List<Transform>();
    }

    public void SpawnSingle()
    {
        if(pool.Count != 0)
        {
            GameObject cObj = pool[0];
            Transform f_begin = beginPpool[0];//Get begin child
            Transform f_end = endPpool[0];//Get end child

            pool.RemoveAt(0);
            beginPpool.RemoveAt(0);
            endPpool.RemoveAt(0);

            if (modifiedObj.Count > 0)//If already spawned object there then add it after last spawned object
            {
                Transform l_end = endP[modifiedObj.Count - 1];

                Vector3 newObjPosition = l_end.position + (cObj.transform.position - f_begin.position);
                cObj.transform.position = new Vector3(newObjPosition.x, cObj.transform.position.y, cObj.transform.position.z);
            }
            else
            {
                Vector3 currentPos = Camera.main.WorldToViewportPoint(f_begin.position);
                Vector3 worldCoord = Camera.main.ViewportToWorldPoint(new Vector3(1.1f, currentPos.y, currentPos.z));

                Vector3 newObjPosition = worldCoord + (cObj.transform.position - f_begin.position);
                cObj.transform.position = new Vector3(newObjPosition.x, cObj.transform.position.y, cObj.transform.position.z);
            }

            //Readd removed objects
            modifiedObj.Add(cObj);
            beginP.Add(f_begin);
            endP.Add(f_end);
        }
    }

    public override void UpdatePositionOnPassingEdgeofCam()
    {
        if(modifiedObj.Count == 0)
        {
            if(allowSpawnMultiple)
            {
                int i = Random.Range(1, pool.Count + 1);
                //Debug.Log("Spawn " + i);
                for (int x = 0; x < i; x++)
                {
                    SpawnSingle();
                }
            }
            else
            {
                SpawnSingle();
            }
        }

        for (int i = 0; i < modifiedObj.Count; i++)
        {
            Transform currentObject = endP[i];
            Vector3 pos = Camera.main.WorldToViewportPoint(currentObject.position);

            if (pos.x < -0.1f)
            {
                pool.Add(modifiedObj[i]);
                endPpool.Add(endP[i]);
                beginPpool.Add(beginP[i]);

                modifiedObj.RemoveAt(i);
                endP.RemoveAt(i);
                beginP.RemoveAt(i);
            }
        }
    }

    public override void ResetAssetPositions()
    {
        base.ResetAssetPositions();

        pool.Clear();
        endPpool.Clear();
        beginPpool.Clear();
    }
}

public class SpawnObjects : MonoBehaviour
{
    public static SpawnObjects instance;
    public RespawnObjects[] respawns;

    public RespawnRandom[] randomRespawn;

    public void Start()
    {
        instance = this;

        ResetObjects();
    }

    public void Update()
    {
        if (GameControl.instance.GetGameState() != GameState.NewGame)
        {
            for (int i = 0; i < respawns.Length; i++)
            {
                respawns[i].UpdatePositionOnPassingEdgeofCam();
            }

            for (int i = 0; i < randomRespawn.Length; i++)
            {
                randomRespawn[i].UpdatePositionOnPassingEdgeofCam();
            }
        }
    }

    public void ResetObjects()
    {
        for (int i = 0; i < respawns.Length; i++)
        {
            respawns[i].ResetAssetPositions();
        }

        for (int i = 0; i < randomRespawn.Length; i++)
        {
            randomRespawn[i].ResetAssetPositions();
        }
    }
}
