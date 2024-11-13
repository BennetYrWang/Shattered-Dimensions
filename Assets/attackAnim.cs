using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class attackAnim : MonoBehaviour
{
    AttackPlayerCircleType playerAttack;
    SpriteRenderer mySpr;

    [SerializeField] float flashDuration = 0.2f;
    [SerializeField] int numflashes = 4;

    Color prevCol;
    // Start is called before the first frame update
    void Start()
    {
        
        playerAttack = transform.parent.gameObject.GetComponent<AttackPlayerCircleType>();
        mySpr = GetComponent<SpriteRenderer>();
        prevCol = mySpr.color;
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

    public void DamageFlash()
    {
        StartCoroutine(FlashRedRepeatedly());
    }

    private IEnumerator FlashRedRepeatedly()
    {
        
        for (int i = 0; i < numflashes; i++)
        {
            // Set the color to red
            mySpr.color = Color.red;

            // Wait for half of the flash duration
            yield return new WaitForSeconds(flashDuration / 2);

            // Revert to the original color
            mySpr.color = Color.black;

            // Wait for the other half of the flash duration
            yield return new WaitForSeconds(flashDuration / 2);
        }

        mySpr.color = Color.clear;
        
    }

    public void setBack()
    {
        mySpr.color = prevCol;
    }
}
