using UnityEngine;

public class DamaingEnemy : Character
{
    // Start is called before the first frame update
    public override void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Constants.playerTag))
        {
            collision.gameObject.GetComponent<Character>().ApplyDamage(0.5f);
        }
    }
}
