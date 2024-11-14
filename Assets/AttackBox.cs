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
    [SerializeField] GameObject trail;

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
            float distance = (transform.parent.position - collision.gameObject.transform.position).magnitude;
            Vector2 direction = -(trail.transform.position - collision.gameObject.transform.position).normalized;

            RaycastHit2D[] hits = Physics2D.RaycastAll(trail.transform.position, direction, distance);


            foreach (RaycastHit2D hit in hits)
            {
               
                if (hit.collider.gameObject.CompareTag("LandScape") && hit.collider.gameObject.name.Substring(0, 1) == "G")
                {
                    
                    return;
                }
            }

            

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
