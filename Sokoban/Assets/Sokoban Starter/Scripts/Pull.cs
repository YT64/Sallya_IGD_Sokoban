using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Pull : MonoBehaviour
{
    private GameObject[] ObjToBlock;
    private GameObject[] ObjToPush;
    private GameObject[] ObjToPull;

    void Start()
    {
        ObjToBlock = GameObject.FindGameObjectsWithTag("Wall");
        ObjToPush = GameObject.FindGameObjectsWithTag("Smooth");
        ObjToPull = GameObject.FindGameObjectsWithTag("Sticky");
    }

    // Update is called once per frame
    void Update()
    {

    }

    public bool Move(Vector2 direction)
    {
        if (ObjToBlocked(transform.position, direction))
        {
            return false;
        }
        else
        {
            transform.Translate(direction);
            return true;
        }
    }

    public bool ObjToBlocked(Vector3 position, Vector2 direction)
    {
        Vector2 newpos = new Vector2(position.x, position.y) + direction;

        foreach (var obj in ObjToBlock)
        {
            if (obj.transform.position.x == newpos.x && obj.transform.position.y == newpos.y)
            {
                return true;
            }
        }

        foreach (var objToPush in ObjToPush)
        {
            if (objToPush.transform.position.x == newpos.x && objToPush.transform.position.y == newpos.y)
            {
                return true;
            }
        }
        return false;

    }
}
