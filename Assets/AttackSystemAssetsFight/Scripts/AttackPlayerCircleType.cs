using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayerCircleType : MonoBehaviour
{
    [SerializeField] Animator spriteAnim;
    [SerializeField] Transform attackPoint;
    [SerializeField] string playerTag;
    [SerializeField] float attackRadius;

    [SerializeField] KeyCode attackKey;
    
    public string fightName;
    [System.NonSerialized]
    public bool isHitting;

    public int dimensionStreak;

    
    public Color winColor;

    [SerializeField] Color holdingCol, hitCol;
    public Color normalCol;
    [SerializeField] SpriteRenderer mySpr;

    bool keyReleased;
    bool holding;

    float keyHoldTime;
    bool attacked;
    float holdTime;
    [SerializeField]
    float maxHoldtime;
    [SerializeField]
    float minHoldTime;

    bool pressStart;
    [SerializeField]
    float keytoHoldRatio;

    [SerializeField] AttackBox attackCol;

    [SerializeField] float downForce;
    // Start is called before the first frame update
    void Start()
    {
        normalCol = mySpr.color;
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(attackKey) && !holding  )
        {
            if (!pressStart)
            {
                pressStart = true;
                keyReleased = false;
                spriteAnim.SetTrigger("Hold");
            }
            else if(pressStart && !attacked)
            {
                keyReleased = false;
            }
            
        }

        if (Input.GetKey(attackKey)&&pressStart&&!holding)
        {
            keyHoldTime+=Time.deltaTime;
        }
        if (Input.GetKeyUp(attackKey)&&pressStart)
        {
            keyReleased = true;
        }

        if (keyReleased && holding &&!attacked)
        {
            attacked = true;
            keyReleased = false;
            spriteAnim.SetTrigger("Attack");
            
        }
    }


    public void holdDone()
    {
        mySpr.color = holdingCol;
        holding = true;
        spriteAnim.SetBool("isHolding", true);
        
    }

    public void attackDone()
    {
        
        attackCol.hitEnd();
        mySpr.color = hitCol;
        mySpr.color = normalCol;
        transform.parent.GetComponent<BennetWang.MovementSystem.PlayerMovementController>().inputAllowed = false ;
        holdTime = keytoHoldRatio * keyHoldTime;
        holdTime = Mathf.Clamp(holdTime, minHoldTime, maxHoldtime);

        Debug.Log(holdTime);
        Invoke("stopHold", holdTime);
    }


    public void stopHold()
    {
        attacked=false;
        mySpr.color = normalCol;
        keyHoldTime = 0;
        pressStart = false;
        holding = false;
        transform.parent.GetComponent<BennetWang.MovementSystem.PlayerMovementController>().inputAllowed = true;
        spriteAnim.SetBool("isHolding", false);
    }

    public void Attack()
    {
        downForce = spriteAnim.GetBool("inAir") ? 4 : 1;
        //GetComponent<Rigidbody2D>().position += GetComponent<BennetWang.MovementSystem.PlayerActor>().GetGravityDirection() * downForce;
        attackCol.hitStart();
       
    }

   
    //private void OnTriggerEnter2D(Collider2D collision)
    //{
    //    Debug.Log(gameObject.name);

    //    if (collision.gameObject.tag == "Player" && collision.gameObject != gameObject)
    //    {


    //        HitsHealth currentEnemy = collision.gameObject.GetComponent<HitsHealth>();
    //        if (currentEnemy != null && currentEnemy != gameObject.GetComponent<HitsHealth>())
    //        {

    //            currentEnemy.Hit(gameObject);
    //            attackCol.enabled = false;

    //        }

    //    }
    //}


}
