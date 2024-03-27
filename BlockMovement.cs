using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BlockMovement : MonoBehaviour
{

    public GridObject gridObject;
    public Vector2Int movingDirection = new(0, 0);
    public Vector2Int tryingDirection = new(0, 0);

    public bool moveSuccessed;

    void Start()
    {
        this.gridObject = this.GetComponent<GridObject>();
    }

    public bool Move(Vector2Int target, bool record = true)
    {
        void couldMove()
        {
            movingDirection = target;
            if (record) { GameManager.Instance.movedBlocks.Add(this); }
        }




        if ((this.gridObject.gridPosition + target).x < 1 ||
            (this.gridObject.gridPosition + target).y < 1 ||
            (this.gridObject.gridPosition + target).x > FindObjectOfType<GridMaker>().dimensions.x ||
            (this.gridObject.gridPosition + target).y > FindObjectOfType<GridMaker>().dimensions.y)
        { 
            return false; 
        }

        else
        {
            List<BlockMovement> matchingBlock = new();

            foreach (BlockMovement block in FindObjectsOfType<BlockMovement>())
            {
                if (block.gridObject.gridPosition + block.movingDirection == this.gridObject.gridPosition + target)
                {
                    if (block != this) { matchingBlock.Add(block); }
                    print(block);
                }
            }

            if (matchingBlock.Count > 0)
            {
                if (matchingBlock[0].CompareTag("Wall") || matchingBlock[0].CompareTag("Clingy"))
                {
                    print("There'sWall");
                    return false;
                }
                else
                {
                    if (matchingBlock[0].Move(target))
                    {
                        couldMove();
                        return true;
                    }
                    else
                    {
                        tryingDirection = target;
                        return false;
                    }
                }
            }
            else
            {
                couldMove();
                return true;
            }
        }
    }
}
