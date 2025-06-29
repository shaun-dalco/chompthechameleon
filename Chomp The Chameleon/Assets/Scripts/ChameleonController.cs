using UnityEngine;
using UnityEngine.Tilemaps;

public class ChameleonController : MonoBehaviour
{
    public Transform[] bodySegments; // 0 = tail, 1 = mid, 2 = head
    public Grid grid;
    public Tilemap solidTilemap;

    // Grid positions for each segment
    private Vector2Int[] segmentGridPos = new Vector2Int[3];
    private Vector2Int facing = Vector2Int.right;

    private Vector2Int queuedInput = Vector2Int.zero;
    public bool isInputFromUI = false;

    public Transform spawnPoint;


    void Start()
    {
        // Initial positions (head center, others left of it)
        /*segmentGridPos[2] = new Vector2Int(5, 5); // Head
        segmentGridPos[1] = new Vector2Int(4, 5); // Mid
        segmentGridPos[0] = new Vector2Int(3, 5); // Tail*/

        segmentGridPos[2] = new Vector2Int((int)spawnPoint.position.x, (int)spawnPoint.position.y);
        segmentGridPos[1] = new Vector2Int((int)spawnPoint.position.x -1, (int)spawnPoint.position.y);
        segmentGridPos[0] = new Vector2Int((int)spawnPoint.position.x -2, (int)spawnPoint.position.y);

        UpdateSegmentWorldPositions();
    }

    void Update()
    {
        bool moved = HandleInput();

        if (!moved)
        {
            ApplyGravity();
        }
    }
    

    public void OnArrowPressed(string direction)
    {
        isInputFromUI = true;

        switch (direction)
        {
            case "left":
                queuedInput = Vector2Int.left;
                facing = Vector2Int.left;
                GetComponent<ChameleonTongue>()?.CancelTongue();
                break;
            case "right":
                queuedInput = Vector2Int.right;
                facing = Vector2Int.right;
                GetComponent<ChameleonTongue>()?.CancelTongue();
                break;
            case "up":
                queuedInput = Vector2Int.up;
                GetComponent<ChameleonTongue>()?.CancelTongue();
                break;
            case "down":
                queuedInput = Vector2Int.down;
                GetComponent<ChameleonTongue>()?.CancelTongue();
                break;
            case "chomp":
                queuedInput = Vector2Int.zero;
                GetComponent<ChameleonTongue>().TongueButtonPressed();
                break;
        }
    }


    bool HandleInput()
    {
        Vector2Int inputDir = Vector2Int.zero;

        if (isInputFromUI)
        {
            inputDir = queuedInput;
            isInputFromUI = false; // reset after using
        }
        else
        {
            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                inputDir = Vector2Int.left;
                facing = Vector2Int.left;
                GetComponent<ChameleonTongue>()?.CancelTongue();
            }
            else if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                inputDir = Vector2Int.right;
                facing = Vector2Int.right;
                GetComponent<ChameleonTongue>()?.CancelTongue();
            }
            else if (Input.GetKeyDown(KeyCode.UpArrow))
            {
                inputDir = Vector2Int.up;
                GetComponent<ChameleonTongue>()?.CancelTongue();
            }
            else if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                inputDir = Vector2Int.down;
                GetComponent<ChameleonTongue>()?.CancelTongue();
            }
        }

        if (inputDir != Vector2Int.zero)
        {
            Vector2Int newHeadPos = segmentGridPos[2] + inputDir;

            if (IsPositionOccupiedByBody(newHeadPos))
                return false;

            if (!solidTilemap.HasTile((Vector3Int)newHeadPos))
            {
                AdvanceBody(newHeadPos);
                UpdateSegmentWorldPositions();
                return true;
            }
        }

        return false;
    }


    public bool IsPositionOccupiedByBody(Vector2Int pos)
    {
        return pos == segmentGridPos[0] || pos == segmentGridPos[1];
    }

    void ApplyGravity()
    {
        if (!IsAnySegmentSupported())
        {
            for (int i = 0; i < segmentGridPos.Length; i++)
            {
                segmentGridPos[i] += Vector2Int.down;
            }

            UpdateSegmentWorldPositions();
        }
    }

    bool IsAnySegmentSupported()
    {
        foreach (var seg in segmentGridPos)
        {
            if (IsSupportedBelow(seg))
                return true;
        }

        return false;
    }

    bool IsSupportedBelow(Vector2Int segmentPos)
    {
        Vector2Int below = segmentPos + Vector2Int.down;

        // Check solid tile
        if (solidTilemap.HasTile((Vector3Int)below))
            return true;

        // Check if peach exists below (support)
        Collider2D hit = Physics2D.OverlapPoint(GridToWorld(below));
        if (hit != null && hit.CompareTag("Peach"))
            return true;

        return false;
    }



    void AdvanceBody(Vector2Int newHeadGridPos)
    {
        // Shift tail → mid, mid → head
        segmentGridPos[0] = segmentGridPos[1];
        segmentGridPos[1] = segmentGridPos[2];
        segmentGridPos[2] = newHeadGridPos;
    }

    void UpdateSegmentWorldPositions()
    {
        for (int i = 0; i < bodySegments.Length; i++)
        {
            bodySegments[i].position = GridToWorld(segmentGridPos[i]);

            // Flip sprite if facing left
            Vector3 scale = bodySegments[i].localScale;
            scale.x = (facing == Vector2Int.left) ? -Mathf.Abs(scale.x) : Mathf.Abs(scale.x);
            bodySegments[i].localScale = scale;
        }
    }

    Vector3 GridToWorld(Vector2Int gridPos)
    {
        Vector3 worldPos = grid.CellToWorld((Vector3Int)gridPos);
        return worldPos + new Vector3(0.5f, 0.5f, 0); // center of tile
    }

    public Vector2Int GetHeadGridPos()
    {
        return segmentGridPos[2];
    }

    public Vector2Int GetFacing()
    {
        return facing;
    }

}
