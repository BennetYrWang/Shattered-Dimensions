using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackAnim : MonoBehaviour
{
    AttackPlayerCircleType playerAttack;

    // Start is called before the first frame update
    void Start()
    {
        playerAttack = transform.parent.gameObject.GetComponent<AttackPlayerCircleType>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void holdDone()
    {
        playerAttack.holdDone();
    }
    public void attackNow()
    {
        playerAttack.Attack();
    }

    public void attackDone()
    {
        playerAttack.attackDone();
    }
}
