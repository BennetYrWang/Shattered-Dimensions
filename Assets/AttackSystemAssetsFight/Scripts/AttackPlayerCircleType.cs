using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerCircleType : MonoBehaviour
{
    [SerializeField] Animator swordAnim;
    [SerializeField] float damage;
    [SerializeField] Transform attackPoint;
    [SerializeField] LayerMask playerLayer;
    [SerializeField] float attackRadius;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Attack();
        }
    }

    void Attack()
    {
        swordAnim.SetTrigger("Hit");

        Collider2D[] hitPlayers =Physics2D.OverlapCircleAll(attackPoint.position, attackRadius, playerLayer);

        foreach( Collider2D player in hitPlayers)
        {
            if (player.gameObject != gameObject)
            {
                Health currentHealth = player.gameObject.GetComponent<Health>();
                if(currentHealth!=null)
                currentHealth.Damage(damage);
            }
           
        }


    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }
}
