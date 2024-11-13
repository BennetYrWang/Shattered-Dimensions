using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class HitsHealth : MonoBehaviour
{
    int hitsLeft;

    [SerializeField]
    int maxHits;

    [SerializeField]
    Image[] hitsImages;

    
    [SerializeField] attackAnim attackAnim;

    [SerializeField] float killDelay;

    public GameObject pAttacker;


    // Start is called before the first frame update
    void Start()
    {
        hitsLeft = maxHits;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Hit(GameObject attacker)
    {
        hitsLeft--;
        
       
       
        if (hitsLeft >= 0)
        {
            attackAnim.DamageFlash();
            //updateUI();
        }

        if (hitsLeft == 0)
        {
            pAttacker = attacker;
            AttackGameManager.Instance.roundOver = true;
            Invoke("playerKill", killDelay);
            
        }
    }


    void playerKill()
    {
        AttackGameManager.Instance.roundOver = false;
        AttackGameManager.Instance.playerKilled(gameObject, pAttacker);
    }
    public void resetHealth()
    {
        hitsLeft = maxHits;
        attackAnim.setBack();
        //updateUI();
    }

    void updateUI()
    {
        for (int i = 0; i < hitsImages.Length; i++)
        {
            if (i > (hitsLeft - 1))
            {
                hitsImages[i].enabled = false;

            }
            else
            {
                hitsImages[i].enabled = true;
            }
        }
    }

    
}
