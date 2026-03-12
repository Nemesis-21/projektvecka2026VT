using UnityEngine;
using TMPro;

public class UICombo : UIFindPlayer
{
    public TextMeshProUGUI text;
    public int oldVal=0;
    public override void OtherAwake()
    {
        text = GetComponent<TextMeshProUGUI>();
    }

    void Update()
    {
        if (player)
        {
            if (oldVal < player.comboCounter) animator.SetTrigger("Added");
            text.text = player.comboCounter + "X";
            oldVal = player.comboCounter;
        }
    }
}
