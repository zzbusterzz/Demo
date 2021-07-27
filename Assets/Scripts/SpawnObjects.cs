using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class RespawnObjects
{
    public List<GameObject> objects = null;//Do not modify orignal array
    public List<Transform> initPos = null;
    private List<GameObject> modifiedObj = null; //Copy contents of orignal array to shadow copy to modify the queue
    private List<Transform> beginP = null;
    private List<Transform> endP = null;

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

    public void UpdatePositionOnPassingEdgeofCam()
    {
        Transform currentObject = endP[0];
        Vector3 pos = Camera.main.WorldToViewportPoint(currentObject.position);

        if(pos.x < -0.1f)
        {
            //Move the object infront queue
            GameObject cObj = objects[0];
            Transform f_begin = beginP[0];//Get begin child
            Transform f_end = endP[0];//Get end child

            objects.RemoveAt(0);
            beginP.RemoveAt(0);
            endP.RemoveAt(0);

            GameObject lastObject = objects[objects.Count - 1];
            Transform l_end = endP[objects.Count - 1];//Get begin child

            Vector3 newObjPosition = l_end.position + (cObj.transform.position - f_begin.position);
            cObj.transform.position = newObjPosition;

            //Readd removed objects
            objects.Add(cObj);
            beginP.Add(f_begin);
            endP.Add(f_end);

        }
        //Debug.Log(currentObject.name + " : " + pos);
    }

    public void ResetAssetPositions()
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

public class SpawnObjects : MonoBehaviour
{
    public static SpawnObjects instance;
    public RespawnObjects[] respawns;
    
    public void Start()
    {
        instance = this;

        ResetObjects();
    }

    public void Update()
    {
        for(int i=0; i < respawns.Length; i++) 
        {
            respawns[i].UpdatePositionOnPassingEdgeofCam();
        }        
    }

    public void ResetObjects()
    {
        for (int i = 0; i < respawns.Length; i++)
        {
            respawns[i].ResetAssetPositions();
        }
    }
}
