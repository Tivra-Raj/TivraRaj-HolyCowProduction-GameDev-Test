using System.Collections;
using UnityEngine;

public class Boomerang : Weapon
{
    Vector3 startPosition;
    [SerializeField]
    Transform endPosition;
    [SerializeField]
    float moveSpeed;
    [SerializeField]
    float rotateSpeed;

    Vector3 initialPosition;
    Vector3 finalPosition;


    float lerpAlpha;
    float timeCount;

    bool isReverse;
    bool isMoving;
    // Start is called before the first frame update
    void Start()
    {
        startPosition = transform.position;
        initialPosition = startPosition;
        finalPosition = endPosition.position;
    }

    // Update is called once per frame
    void Update()
    {
        if (isMoving)
        {
            UpdateLocation();
        }
    }

    public override void Fire(Transform shootpoint, float range, bool isPlayer)
    {
        //TODO Boomerang audio
        base.Fire(shootpoint, range, isPlayer);
        isMoving = true;
        canFire = false;

    }
    void UpdateLocation()
    {
        lerpAlpha = timeCount * moveSpeed;
        if (lerpAlpha < 1)
        {
            if (!isReverse)
            {
                transform.position = Vector3.Lerp(initialPosition, finalPosition, lerpAlpha);
            }
            else
            {
                transform.position = Vector3.Lerp(finalPosition, initialPosition, lerpAlpha);
            }
        }
        else
        {
            timeCount = 0;
            if (isReverse)
            {
                isMoving = false;
                isReverse = false;
                StartCoroutine(ApplyCoolDown());
            }
            else
            {
                isReverse = true;
            }
        }

        ///I have made first boomerang rotate in hierarchy using animator please notice it///
        transform.Rotate(0, 0, rotateSpeed);

        timeCount = timeCount + Time.deltaTime;
    }
    IEnumerator ApplyCoolDown()
    {
        yield return new WaitForSeconds(coolDown);
        canFire = true;
    }
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.collider.CompareTag(Constants.playerTag))
        {
            collision.gameObject.GetComponent<Character>().ApplyDamage(1);
        }
    }
}
