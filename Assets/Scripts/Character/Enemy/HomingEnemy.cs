using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEngine.RuleTile.TilingRuleOutput;

public class HomingEnemy : Character
{
    PlayerCharacter player;
    // Start is called before the first frame update
    void Start()
    {
        player = FindAnyObjectByType<PlayerCharacter>();
    }

    // Update is called once per frame
    void Update()
    {
        transform.position = Vector2.MoveTowards(transform.position,player.transform.position,moveSpeed*Time.deltaTime);
    }
}
