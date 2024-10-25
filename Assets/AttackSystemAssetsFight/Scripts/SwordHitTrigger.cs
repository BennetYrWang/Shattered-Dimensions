using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordHitTrigger : MonoBehaviour
{
    [SerializeField] float damage;
    PlayerAttackController playerAttack;
    

    Collider2D hitCollider;
    // Start is called before the first frame update
    void Start()
    {
        playerAttack = transform.parent.GetComponent<PlayerAttackController>();
        hitCollider = GetComponent<Collider2D>();
        hitCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
       
        if ( collision.gameObject.tag== "Player" && collision.gameObject != transform.parent.gameObject)
        {

            HitsHealth currentHealth= collision.gameObject.GetComponent<HitsHealth>();
            currentHealth.Hit(transform.parent.gameObject);
        }
       
    }

    public void hitStart()
    {
        hitCollider.enabled = true;
        playerAttack.isHitting = true;
      

    }

    public void hitEnd()
    {
        hitCollider.enabled = false;

        playerAttack.isHitting = false;


    }
}
