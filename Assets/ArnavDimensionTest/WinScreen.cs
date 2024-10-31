using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class WinScreen : MonoBehaviour
{
    [SerializeField]
    TextMeshProUGUI winText;
    [SerializeField]
    Image background;
    // Start is called before the first frame update
    void Start()
    {
        gameObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void gameWon(string winnerName, Color winCol)
    {
        gameObject.SetActive(true);
        winText.text = winnerName + " Conquered over the dimesnions and their mind";
        background.color = winCol;
    }
}
