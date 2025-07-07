using UnityEngine;
using UnityEngine.Tilemaps;
using System.Collections.Generic;


public class ChameleonController : MonoBehaviour
{
    public Grid grid;
    public Tilemap solidTilemap;

    // Grid positions for each segment
    private Vector2Int facing = Vector2Int.right;

    private Vector2Int queuedInput = Vector2Int.zero;
    public bool isInputFromUI = false;

    public Transform spawnPoint;

    public List<Transform> bodySegments = new List<Transform>();
    private List<Vector2Int> segmentGridPos = new List<Vector2Int>();

    public GameObject bodySegmentPrefab;

    public int pendingGrowths = 0;




    void Start()
    {
        segmentGridPos.Add(new Vector2Int((int)spawnPoint.position.x - 2, (int)spawnPoint.position.y));
        segmentGridPos.Add(new Vector2Int((int)spawnPoint.position.x - 1, (int)spawnPoint.position.y));
        segmentGridPos.Add(new Vector2Int((int)spawnPoint.position.x, (int)spawnPoint.position.y));

        // Create visual segments for each grid position
        /*foreach (var pos in segmentGridPos)
        {
            GameObject newSegment = Instantiate(bodySegmentPrefab, GridToWorld(pos), Quaternion.identity);
            bodySegments.Add(newSegment.transform);
        }*/

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
            Vector2Int newHeadPos = segmentGridPos[segmentGridPos.Count - 1] + inputDir;

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
        for (int i = 0; i < segmentGridPos.Count - 1; i++) // exclude head
        {
            if (segmentGridPos[i] == pos)
                return true;
        }
        return false;
    }

    void ApplyGravity()
    {
        if (!IsAnySegmentSupported())
        {
            /*for (int i = 0; i < segmentGridPos.Length; i++)
            {
                segmentGridPos[i] += Vector2Int.down;
            }*/

            for (int i = 0; i < segmentGridPos.Count; i++)
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
        if (hit != null && (hit.CompareTag("Peach") || hit.CompareTag("Watermelon")))
            return true;

        return false;
    }



    void AdvanceBody(Vector2Int newHeadGridPos)
    {
        if (pendingGrowths > 0)
        {
            // Save old head position
            Vector2Int oldHead = segmentGridPos[segmentGridPos.Count - 1];

            // Insert old head as new body segment
            segmentGridPos.Insert(segmentGridPos.Count - 1, oldHead);

            // Create visual body segment at old head's position
            GameObject newSegment = Instantiate(bodySegmentPrefab, GridToWorld(oldHead), Quaternion.identity);
            bodySegments.Insert(bodySegments.Count - 1, newSegment.transform);

            // Replace head with new head position
            segmentGridPos[segmentGridPos.Count - 1] = newHeadGridPos;

            pendingGrowths--;
        }
        else
        {
            // Follow-the-leader logic: tail follows up toward head
            for (int i = 0; i < segmentGridPos.Count - 1; i++)
            {
                segmentGridPos[i] = segmentGridPos[i + 1];
            }

            // Head moves to new position
            segmentGridPos[segmentGridPos.Count - 1] = newHeadGridPos;
        }


    }





    void UpdateSegmentWorldPositions()
    {
        if (segmentGridPos.Count != bodySegments.Count)
        {
            Debug.LogError("Mismatch between segmentGridPos and bodySegments. This will break movement visuals.");
            return;
        }

        for (int i = 0; i < bodySegments.Count; i++)
        {
            bodySegments[i].position = GridToWorld(segmentGridPos[i]);

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
        return segmentGridPos[segmentGridPos.Count - 1];
    }


    public Vector2Int GetFacing()
    {
        return facing;
    }


    public void Grow()
    {
        Debug.Log("Grow called");
        pendingGrowths++;
    }




}
