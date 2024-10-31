using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAttackController : MonoBehaviour
{
    [SerializeField] Animator swordAnim;
    [SerializeField]  KeyCode Attack;

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
        if (Input.GetKeyDown(Attack) && !isHitting)
        {
            swordAnim.SetTrigger("Hit");

        }

    }

}
