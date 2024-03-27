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

        // Get the current position
        Vector2 currentPosition = transform.position;

        // Calculate the new position
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
                    // If the player is moving away from the Clingy block, try to move the block along with the player
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

            // If movement is allowed, move the player
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
    
                // Calculate the position adjacent to the player in the direction of movement
                Vector2 adjacentPosition = position + direction;
    
                // Calculate the distance between the player and the block
                float distance = Vector2.Distance(objPosition, position);
    
                // Check if the player is adjacent to the block
                if (Mathf.Approximately(distance, 1.0f)) // Assuming 1.0f is the distance when the player is adjacent
                {
                if (objPosition == newpos)
                {
                    print("Cannot move: Clingy block in the way");
                    return true;
                }
                // If the player is adjacent and moving towards or opposite to the block, allow the block to move
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
    
            return false;
    }
    
    private bool TryMoveBlock(GameObject blockToPull, Vector2 direction)
    {
        Vector2 blockNewPos = (Vector2)blockToPull.transform.position + direction;
    
        // Check if the new position of the block is within the boundaries and not blocked
        if (blockNewPos.x >= -5f && blockNewPos.x <= 5f && blockNewPos.y >= -2.5f && blockNewPos.y <= 2.5f)
        {
            // Move the block
            blockToPull.transform.Translate(direction * 0.5f);
            return true;
        }   
        else
        {
            // The block cannot be moved
            return false;
    
        }
    }
}
