using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwordTrigger : MonoBehaviour
{
    [SerializeField] float damage;
    [SerializeField]
    AttackPlayer playerAttack;
    bool hitOn;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }


    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject != transform.parent.gameObject &&hitOn)
        {
            Health currentHealth= collision.gameObject.GetComponent<Health>();
            currentHealth.Damage(damage);
        }
       
    }

    public void hitStart()
    {
        hitOn = true;
        playerAttack.isHitting = true;
    }

    public void hitEnd()
    {
        hitOn = false;
        playerAttack.isHitting = false;

    }
}
