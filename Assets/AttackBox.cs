using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackBox : MonoBehaviour
{

    BoxCollider2D boxCol;
    public bool canDamage;
    // Start is called before the first frame update
    void Start()
    {
        boxCol = GetComponent<BoxCollider2D>();
        hitEnd();
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



    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Player" && collision.gameObject != transform.parent.gameObject&&canDamage)
        {
            hitEnd();
            HitsHealth currentHealth = collision.gameObject.GetComponent<HitsHealth>();
            currentHealth.Hit(transform.parent.gameObject);
        }
    }
}
