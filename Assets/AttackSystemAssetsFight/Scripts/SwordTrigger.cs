using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrigger : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField]
    AttackPlayer playerAttack;
    bool hitOn;

    Collider2D hitCollider;
    // Start is called before the first frame update
    void Start()
    {
        hitCollider = GetComponent<Collider2D>();
        hitCollider.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject != transform.parent.gameObject)
        {
            Health currentHealth= collision.gameObject.GetComponent<Health>();
            currentHealth.Damage(damage);
        }
       
    }

    public void hitStart()
    {
        hitCollider.enabled = true;
        hitOn = true;
        playerAttack.isHitting = true;

    }

    public void hitEnd()
    {
        hitCollider.enabled = false;
        hitOn = false;
        playerAttack.isHitting = false;

    }
}
