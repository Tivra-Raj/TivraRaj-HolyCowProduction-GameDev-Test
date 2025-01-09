using UnityEngine;

public class HomingEnemy : Character
{
    PlayerCharacter player;
    private bool isPlayerInRange = false;

    [SerializeField] private LayerMask raycastMask;
    [SerializeField] private float distanceThreshold = 6.7f;

    // Start is called before the first frame update
    public override void Start()
    {
        player = FindAnyObjectByType<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isPlayerInRange && player != null)
        {
            Vector2 direction = (player.transform.position - transform.position).normalized;
            RaycastHit2D hit = Physics2D.Raycast(transform.position, direction, Mathf.Infinity, raycastMask);

            Debug.DrawLine(transform.position, hit.point, Color.red);

            if (hit.collider != null)
            {
                if (!hit.collider.CompareTag(Constants.playerTag))
                {
                    Debug.Log("Hit Ground");
                }
                else
                {
                    float distance = Vector2.Distance(transform.position, player.transform.position);
                    //Debug.Log(distance);
                    if (distance > distanceThreshold)
                    {
                        //Debug.Log(distance);
                        transform.position = Vector2.MoveTowards(transform.position, player.transform.position, moveSpeed * Time.deltaTime);
                    }
                }
            }    
        }   
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.CompareTag(Constants.playerTag))
        {
            Debug.Log("player is in range");
            isPlayerInRange = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.CompareTag(Constants.playerTag))
        {
            Debug.Log("player is out of range");
            isPlayerInRange = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Constants.playerTag))
        {
            collision.gameObject.GetComponent<Character>().ApplyDamage(0.5f);
        }
    }
}