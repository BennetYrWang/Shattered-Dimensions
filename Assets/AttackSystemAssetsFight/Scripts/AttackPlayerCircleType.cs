using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerCircleType : MonoBehaviour
{
    [SerializeField] Animator swordAnim;
    [SerializeField] float damage;
    [SerializeField] Transform attackPoint;
    [SerializeField] string playerTag;
    [SerializeField] float attackRadius;

    [SerializeField] KeyCode attackKey;

    public string fightName;
    [System.NonSerialized]
    public bool isHitting;

    public int dimensionStreak;

    
    public Color winColor;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(attackKey))
        {
            Attack();
        }
    }




    void Attack()
    {
        swordAnim.SetTrigger("Hit");

        Collider2D[] hitPlayers =Physics2D.OverlapCircleAll(attackPoint.position, attackRadius);

        foreach( Collider2D player in hitPlayers)
        {
            if (player.gameObject != gameObject && player.CompareTag(playerTag))
            {
                HitsHealth currentHealth = player.gameObject.GetComponent<HitsHealth>();
                if (currentHealth != null)
                    currentHealth.Hit(gameObject);
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
