using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerCircleType : MonoBehaviour
{
    [SerializeField] Animator spriteAnim;
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


    bool keyReleased;
    bool holding;

    float keyHoldTime;
    bool attackAllowed;
    float holdTime;
    [SerializeField]
    float maxHoldtime;
    [SerializeField]
    float minHoldTime;

    bool keyPressed;
    [SerializeField]
    float keytoHoldRatio;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(attackKey)&& !keyPressed)
        {
            keyPressed = true;
            spriteAnim.SetTrigger("Hold");
        }

        if (Input.GetKey(attackKey))
        {
            keyHoldTime+=Time.deltaTime;
        }
        if (Input.GetKeyUp(attackKey))
        {
            keyReleased = true;
        }

        if (keyReleased && holding)
        {
            keyReleased = false;
            spriteAnim.SetTrigger("Attack");
            
        }
    }


    public void holdDone()
    {
        holding = true;
        spriteAnim.SetBool("isHolding", true);
    }
    public void attackDone()
    {
        holdTime = keytoHoldRatio * keyHoldTime;
        keyHoldTime = 0;
        holdTime = Mathf.Clamp(holdTime, minHoldTime, maxHoldtime);
        Invoke("stopHold", holdTime);
    }


    public void stopHold()
    {
        
        spriteAnim.SetBool("isHolding", false);
    }
    public void Attack()
    {
        

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
