using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class GameStatusUI : MonoBehaviour
{

    TextMeshProUGUI text;

    
    // Start is called before the first frame update
    void Awake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void setText(string Color, int dimensLeft)
    {
        text.text = dimensLeft + "/" + DimensionManager.Instance.totalDimensions + " Dimensions colored " + Color;
    }
}
