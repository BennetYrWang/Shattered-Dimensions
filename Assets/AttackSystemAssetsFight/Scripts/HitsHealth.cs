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
            for (int i = 0; i < hitsImages.Length; i++)
            {
                if (i > (hitsLeft-1))
                {
                    hitsImages[i].enabled = false;

                }
            }
        }

        if (hitsLeft == 0)
        {
            AttackGameManager.Instance.playerKilled(gameObject, attacker);
        }
    }

    
}
