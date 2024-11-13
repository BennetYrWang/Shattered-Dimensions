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

    [SerializeField] Color normalCol, holdingCol;

    [SerializeField] SpriteRenderer mySpr;

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

    [SerializeField] AttackBox attackCol;
    // Start is called before the first frame update
    void Start()
    {
        
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
        attackCol.hitEnd();

        transform.parent.GetComponent<BennetWang.MovementSystem.PlayerMovementController>().inputAllowed = false ;
        holdTime = keytoHoldRatio * keyHoldTime;
        holdTime = Mathf.Clamp(holdTime, minHoldTime, maxHoldtime);

        Debug.Log(holdTime);
        Invoke("stopHold", holdTime);
    }


    public void stopHold()
    {
      
        keyHoldTime = 0;
        pressStart = false;
        holding = false;
        transform.parent.GetComponent<BennetWang.MovementSystem.PlayerMovementController>().inputAllowed = true;
        spriteAnim.SetBool("isHolding", false);
    }

    public void Attack()
    {
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
