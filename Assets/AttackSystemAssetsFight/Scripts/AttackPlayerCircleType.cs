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

    [SerializeField] Color normalCol, holdingCol;
    bool keyReleased;
    bool holding;

    float keyHoldTime;
    bool attackAllowed;
    float holdTime;
    [SerializeField]
    float maxHoldtime;
    [SerializeField]
    float minHoldTime;

    bool pressStart;
    [SerializeField]
    float keytoHoldRatio;

    [SerializeField] BoxCollider2D attackCol;
    // Start is called before the first frame update
    void Start()
    {
        attackCol.enabled = false;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(attackKey) && !holding && !pressStart )
        {
            pressStart = true;
            keyReleased = false;
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
        attackCol.enabled = false;

        holdTime = keytoHoldRatio * keyHoldTime;
        holdTime = Mathf.Clamp(holdTime, minHoldTime, maxHoldtime);

        Invoke("stopHold", holdTime);
    }


    public void stopHold()
    {
        keyHoldTime = 0;
        pressStart = false;
        holding = false;
        spriteAnim.SetBool("isHolding", false);
    }

    public void Attack()
    {
        attackCol.enabled = true;

    }

    private void OnDrawGizmosSelected()
    {
        if (attackPoint == null)
        {
            return;
        }
        Gizmos.DrawWireSphere(attackPoint.position, attackRadius);
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if(collision.gameObject.tag=="Player"&& gameObject != collision.gameObject)
        {
            HitsHealth currentEnemy = collision.gameObject.GetComponent<HitsHealth>();
            currentEnemy.Hit(gameObject);
        }
    }


}
