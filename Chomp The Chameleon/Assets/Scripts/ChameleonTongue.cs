using System.Collections;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ChameleonTongue : MonoBehaviour
{
    public Grid grid;
    public Tilemap solidTilemap;
    public ChameleonController chameleon;

    public int maxTongueDistance = 6;
    private bool tongueExtended = false;
    private GameObject latchedPeach;
    private Vector2Int tongueTargetPos;
    public LineRenderer tongueLine;

    private bool animatingTongue = false;
    private float tongueAnimTime = 0f;
    public float tongueRollDuration = 0.15f;
    private Vector3 tongueStart;
    private Vector3 tongueEnd;

    public Sprite defaultHead;
    public Sprite openHead;
    public SpriteRenderer head;

    public Transform tongueStartPos;
    private bool playingHeadAnim = false;

    public SoundManager soundManager;



    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!tongueExtended)
            {
                TryExtendTongue();
                soundManager.PlaySound("tongue");
            }
            else
            {
                TryPullPeach();
            }
        }

        if (tongueExtended)
        {
            TryAutoEatPeach();
        }

        CheckHeadCollisionWithPeach(); 
        UpdateTongueVisual();

        if(!playingHeadAnim) {
            if(tongueExtended) {
                head.sprite = openHead;
            } else {
                head.sprite = defaultHead;
            }
        }
    }

    public void TongueButtonPressed() {
        if (!tongueExtended)
        {
            TryExtendTongue();
            soundManager.PlaySound("tongue");
        }
        else
        {
            TryPullPeach();
        }
    }

    void CheckHeadCollisionWithPeach()
    {
        Vector2Int headPos = chameleon.GetHeadGridPos();

        // Get all colliders overlapping this tile
        Collider2D[] hits = Physics2D.OverlapBoxAll(GridToWorld(headPos), new Vector2(0.8f, 0.8f), 0f);

        foreach (var hit in hits)
        {
            if (hit != null && hit.CompareTag("Peach"))
            {
                Destroy(hit.gameObject);

                if (hit.gameObject == latchedPeach)
                {
                    latchedPeach = null;
                    tongueExtended = false;
                    tongueLine.positionCount = 0;
                }

                Debug.Log("PEACH EATEEEEEEN");

                StartCoroutine(ChompAnimation());
                return;
            }
            if(hit != null && hit.CompareTag("Watermelon"))
            {
                Destroy(hit.gameObject);

                if (hit.gameObject == latchedPeach)
                {
                    latchedPeach = null;
                    tongueExtended = false;
                    tongueLine.positionCount = 0;
                }

                Debug.Log("GROWWW");
                GetComponent<ChameleonController>().Grow();

                StartCoroutine(ChompAnimation());
                return;
            }
        }
    }


    IEnumerator ChompAnimation()
    {
        playingHeadAnim = true;
        head.sprite = openHead;
        soundManager.PlaySound("chomp");
        yield return new WaitForSeconds(0.1f);
        head.sprite = defaultHead;
        playingHeadAnim = false;
    }


    void TryExtendTongue()
    {
        Vector2Int headPos = chameleon.GetHeadGridPos();
        Vector2Int dir = chameleon.GetFacing();
        Vector2Int checkPos = headPos;

        for (int i = 1; i <= maxTongueDistance; i++)
        {
            checkPos += dir;
            Debug.Log("Checking tile: " + checkPos);

            if (solidTilemap.HasTile((Vector3Int)checkPos) || chameleon.IsPositionOccupiedByBody(checkPos))
            {
                Debug.Log("Blocked at " + checkPos);
                break;
            }

            Collider2D hit = Physics2D.OverlapCircle(GridToWorld(checkPos), 0.1f);
            if (hit != null)
            {
                Debug.Log("Hit something: " + hit.name);
            }

            if (hit != null && (hit.CompareTag("Peach") || hit.CompareTag("Watermelon")))
            {
                tongueExtended = true;
                latchedPeach = hit.gameObject;
                tongueTargetPos = WorldToGrid(latchedPeach.transform.position);

                //tongueStart = GridToWorld(headPos);
                tongueStart = tongueStartPos.position;
                tongueEnd = latchedPeach.transform.position;
                tongueAnimTime = 0f;
                animatingTongue = true;

                Debug.Log("Hit peach: " + hit.name);
                // TODO: Animate tongue flick here
                return;
            }
        }

        tongueExtended = false;
        latchedPeach = null;
    }

    void UpdateTongueVisual()
    {
        if (tongueLine == null) return;

        if (!tongueExtended || latchedPeach == null)
        {
            tongueLine.positionCount = 0;
            animatingTongue = false;
            return;
        }

        Vector3 headWorld = GridToWorld(chameleon.GetHeadGridPos());
        Vector3 peachWorld = latchedPeach.transform.position;

        if (animatingTongue)
        {
            tongueAnimTime += Time.deltaTime;
            float t = Mathf.Clamp01(tongueAnimTime / tongueRollDuration);
            Vector3 animatedEnd = Vector3.Lerp(tongueStart, tongueEnd, t);

            tongueLine.positionCount = 2;
            tongueLine.SetPosition(0, tongueStart);
            tongueLine.SetPosition(1, animatedEnd);

            if (t >= 1f)
            {
                animatingTongue = false; // done animating
            }
        }
        else
        {
            // Draw full tongue
            tongueLine.positionCount = 2;
            tongueLine.SetPosition(0, tongueStartPos.position);
            tongueLine.SetPosition(1, peachWorld);
        }
    }



    void TryPullPeach()
    {
        if (latchedPeach == null) return;

        Vector2Int peachPos = WorldToGrid(latchedPeach.transform.position);
        Vector2Int headPos = chameleon.GetHeadGridPos();
        Vector2Int pullDir = headPos - peachPos;

        if (pullDir.x != 0)
            pullDir = new Vector2Int(pullDir.x > 0 ? 1 : -1, 0);
        else if (pullDir.y != 0)
            pullDir = new Vector2Int(0, pullDir.y > 0 ? 1 : -1);
        else
            return; // already overlapping

        Vector2Int nextPos = peachPos + pullDir;

        // Stop if blocked
        if (solidTilemap.HasTile((Vector3Int)nextPos) || chameleon.IsPositionOccupiedByBody(nextPos))
            return;

        Collider2D other = Physics2D.OverlapPoint(GridToWorld(nextPos));
        if (other != null && (other.CompareTag("Peach") || other.CompareTag("Watermelon")))
            return;

        // Move peach
        latchedPeach.transform.position = GridToWorld(nextPos);
        tongueTargetPos = nextPos;

        // Check if peach was pulled into head
        if (nextPos == headPos)
        {
            EatLatchedPeach();
        }
    }

    void TryAutoEatPeach()
    {
        if (latchedPeach == null) return;

        Vector2Int peachPos = WorldToGrid(latchedPeach.transform.position);
        Vector2Int headPos = chameleon.GetHeadGridPos();

        if (peachPos == headPos)
        {
            EatLatchedPeach();
        }
    }

    void EatLatchedPeach()
    {
        if(latchedPeach.tag == "Watermelon") {
            GetComponent<ChameleonController>().Grow();
        }
        Destroy(latchedPeach);
        latchedPeach = null;
        tongueExtended = false;
        tongueLine.positionCount = 0;
        animatingTongue = false;


        // TODO: Add eating animation or sound
    }

    public void CancelTongue()
    {
        tongueExtended = false;
        latchedPeach = null;
        tongueLine.positionCount = 0;
        animatingTongue = false;

    }


    Vector3 GridToWorld(Vector2Int gridPos)
    {
        return grid.CellToWorld((Vector3Int)gridPos) + new Vector3(0.5f, 0.5f, 0);
    }

    Vector2Int WorldToGrid(Vector3 worldPos)
    {
        Vector3Int cell = grid.WorldToCell(worldPos);
        return new Vector2Int(cell.x, cell.y);
    }

    public bool IsTongueExtended()
    {
        return tongueExtended;
    }
}
