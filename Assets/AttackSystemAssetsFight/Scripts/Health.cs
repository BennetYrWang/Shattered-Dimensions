using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Health : MonoBehaviour
{
   float health;

    [SerializeField]
    float maxHealth;

    [SerializeField]
    TextMeshProUGUI healthText;

    



    // Start is called before the first frame update
    void Start()
    {
        health = maxHealth;
        Damage(0);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void Damage(float damage)
    {
        health -= damage;
        healthText.text = health.ToString();
    }

    
}
