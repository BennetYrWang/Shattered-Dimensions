using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AttackPlayer : MonoBehaviour
{
    [SerializeField] Animator swordAnim;
 
    public bool isHitting;

    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space) && !isHitting)
        {
            swordAnim.SetTrigger("Hit");
            
            

        }
    }

    
}
