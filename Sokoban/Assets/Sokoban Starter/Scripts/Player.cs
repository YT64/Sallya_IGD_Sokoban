using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.GraphicsBuffer;

public class Player : MonoBehaviour
{
    private GameObject[] ObjToBlock;
    private GameObject[] ObjToPush;
    private GameObject[] ObjToPull;
    private GameObject[] ObjToStick;
    private bool ReadyToMove;
    void Start()
    {
        ObjToBlock = GameObject.FindGameObjectsWithTag("Wall");
        ObjToPush = GameObject.FindGameObjectsWithTag("Smooth");
        ObjToPull = GameObject.FindGameObjectsWithTag("Clingy");
        ObjToStick = GameObject.FindGameObjectsWithTag("Sticky");

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


        Vector2 currentPosition = transform.position;


        Vector2 newPosition = currentPosition + direction;

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
            GameObject blockToPull = null;


            if (Blocked(transform.position, direction, out blockToPull))
            {
                if (blockToPull != null)
                {

                    if ((direction.x < 0 && blockToPull.transform.position.x > currentPosition.x) ||
                        (direction.x > 0 && blockToPull.transform.position.x < currentPosition.x) ||
                        (direction.y < 0 && blockToPull.transform.position.y > currentPosition.y) ||
                        (direction.y > 0 && blockToPull.transform.position.y < currentPosition.y))
                    {
                        if (!TryMoveBlock(blockToPull, direction))
                        {
                            print("Cannot move: Clingy block in the way");
                            return false;
                        }
                    }
                }
                else
                {
                    print("Other block in the way");
                    return false;
                }
            }


            float moveDistance = 0.5f;
            transform.Translate(direction * moveDistance);
            return true;
        }
    }

    public bool Blocked(Vector2 position, Vector2 direction, out GameObject blockToPull)
    {
        blockToPull = null;
        Vector2 newpos = position + direction;
    
        foreach (var obj in ObjToBlock)
        {
            if (obj.transform.position.x == newpos.x && obj.transform.position.y == newpos.y)
            {
                print("encounter wall");
                return true;
            }
        }
    
        foreach (var objToPush in ObjToPush)
        {
            if (objToPush.transform.position.x == newpos.x && objToPush.transform.position.y == newpos.y)
            {
                Push objtopush = objToPush.GetComponent<Push>();
                if (objtopush && objtopush.Move(direction))
                {
                    return false;
                }
                else
                {
                    return true;
                }
            }
        }
    
        foreach (var objToPull in ObjToPull)
            {
                Vector2 objPosition = objToPull.transform.position;
    

                Vector2 adjacentPosition = position + direction;
    

                float distance = Vector2.Distance(objPosition, position);
    
                if (Mathf.Approximately(distance, 1.0f)) 
                {
                if (objPosition == newpos)
                {
                    print("Cannot move: Clingy block in the way");
                    return true;
                }
                
                if ((direction.x < 0 && objPosition.x > position.x) ||
                        (direction.x > 0 && objPosition.x < position.x) ||
                        (direction.y < 0 && objPosition.y > position.y) ||
                        (direction.y > 0 && objPosition.y < position.y))
                    {
                        if (objToPull.transform.position.x == newpos.x && objToPull.transform.position.y == newpos.y)
                        {
                        print("Crush!");
                        return true;
                        }
                        else
                        {
                            print("Player moving towards or opposite to Clingy");
                            blockToPull = objToPull;
                            return true;
                        }
                    }
                    else
                    {
                        print("Player moving sideways next to Clingy");
                        return false;
                    }
                }
            }

        foreach (var objToStick in ObjToStick)
        {
            Vector2 objPosition = objToStick.transform.position;
            float distance = Vector2.Distance(objPosition, position);

            if (Mathf.Approximately(distance, 1.0f))
            {

                if (objPosition == newpos)
                {

                    if (!Blocked(objPosition, direction, out GameObject _))
                    {
                        print("Cannot move: Sticky block in the way");
                        return true;
                    }
                }


                if ((direction.x > 0 && objPosition.x == position.x + 1) ||
                    (direction.x < 0 && objPosition.x == position.x - 1) ||
                    (direction.y > 0 && objPosition.y == position.y + 1) ||
                    (direction.y < 0 && objPosition.y == position.y - 1))
                {
                    if (!Blocked(objPosition, direction, out GameObject _))
                    {
                        Push objtopush = objToStick.GetComponent<Push>();
                        print("Sticky Pushing");
                        return false;
                    }
                }

                else
                {
                    blockToPull = objToStick;
                    return true;
                }
            }
        }

        return false;
    }
    
    private bool TryMoveBlock(GameObject blockToPull, Vector2 direction)
    {
        Vector2 blockNewPos = (Vector2)blockToPull.transform.position + direction;
    

        if (blockNewPos.x >= -5f && blockNewPos.x <= 5f && blockNewPos.y >= -2.5f && blockNewPos.y <= 2.5f)
        {
            blockToPull.transform.Translate(direction * 0.5f);
            return true;
        }   
        else
        {
            return false;
    
        }
    }
}
