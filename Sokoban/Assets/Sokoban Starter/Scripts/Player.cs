using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private GameObject[] ObjToBlock;
    private GameObject[] ObjToPush;
    private GameObject[] ObjToPull;

    private bool ReadyToMove;
    void Start()
    {
        ObjToBlock = GameObject.FindGameObjectsWithTag("Wall");
        ObjToPush = GameObject.FindGameObjectsWithTag("Smooth");
        ObjToPull = GameObject.FindGameObjectsWithTag("Sticky");
    }


    void Update()
    {
        Vector2 moveinput = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveinput.Normalize();

        if (moveinput.sqrMagnitude > 0.5)
        {
            if (ReadyToMove)
            {
                ReadyToMove = false;
                Move(moveinput);
            }
        }
        else
        {
            ReadyToMove = true;
        }
    }

    public bool Move(Vector2 direction) 
    {
        if (Mathf.Abs(direction.x) < 0.5)
        {
            direction.x = 0;
        }
        else 
        {
            direction.y = 0;
        }
        direction.Normalize();

        if (Blocked(transform.position, direction))
        {
            return false;
        }
        else 
        {
            float moveDistance = 0.5f; 
            transform.Translate(direction * moveDistance);
            return true;
        }
    }

    public bool Blocked(Vector3 position, Vector2 direction) 
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
                Push objtopush = objToPush.GetComponent<Push>();
                if(objtopush && objtopush.Move(direction))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
        return false;

    }
}
