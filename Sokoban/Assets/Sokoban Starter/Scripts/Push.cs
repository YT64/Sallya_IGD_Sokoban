using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Push : MonoBehaviour
{
    private GameObject[] ObjToBlock;
    private GameObject[] ObjToPush;
    public GridObject gridObject;
    private GameObject[] ObjToPull;
    private GameObject[] ObjToStick;

    void Start()
    {
        ObjToBlock = GameObject.FindGameObjectsWithTag("Wall");
        ObjToPush = GameObject.FindGameObjectsWithTag("Smooth");
        ObjToPull = GameObject.FindGameObjectsWithTag("Clingy");
        ObjToStick = GameObject.FindGameObjectsWithTag("Sticky");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public bool Move(Vector2 direction) 
    {
        // Get the current position
        Vector2 currentPosition = transform.position;

        // Calculate the new position
        Vector2 newPosition = currentPosition + direction;

        Debug.Log("New position: " + newPosition); ;

        float minX = -5f; 
        float maxX = 5f; 
        float minY = -2.5f;
        float maxY = 2.5f; 

        if (newPosition.x < minX || newPosition.x > maxX || newPosition.y < minY || newPosition.y > maxY)
        {
            print("Cannot move: out of bounds");
            return false;
        }
        else 
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

        foreach (var objToPush in ObjToPull)
        {
            if (objToPush.transform.position.x == newpos.x && objToPush.transform.position.y == newpos.y)
            {
                return true;
            }
        }
        foreach (var objToStick in ObjToStick)
        {
            if (objToStick.transform.position.x == newpos.x && objToStick.transform.position.y == newpos.y)
            {

                return true;
            }
        }
        return false;

    }
}
