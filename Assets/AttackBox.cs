using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{

    BoxCollider2D boxCol;
    public bool canDamage;
    [SerializeField] float hitDist;
    [SerializeField] float verticalMove;
    [SerializeField] ParticleSystem playerHit;
    [SerializeField] ParticleSystem groundHit;

    bool hitOther;
    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        canDamage = false;
        boxCol.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void hitStart()
    {
        canDamage = true;
        boxCol.enabled = true;
        


    }

    public void hitEnd()
    {
        canDamage = false;
        boxCol.enabled = false;

        if (!hitOther)
            groundHit.Play();
        else
            playerHit.Play();

        hitOther = false;
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject != transform.parent.gameObject&&canDamage)
        {
            hitOther = true;
            hitEnd();
            HitsHealth currentHealth = collision.gameObject.GetComponent<HitsHealth>();
            currentHealth.Hit(transform.parent.gameObject);

            int dir = gameObject.transform.parent.GetComponent<BennetWang.MovementSystem.PlayerActor>().FacingRight ? 1 : -1;
            collision.gameObject.transform.parent.GetComponent<BennetWang.MovementSystem.PlayerMovementController>().TryMovePlayerHorizontally(hitDist*dir );
            collision.gameObject.GetComponent<Rigidbody2D>().position -= collision.gameObject.GetComponent<BennetWang.MovementSystem.PlayerActor>().GetGravityDirection()*verticalMove;



        } 
    }
}
