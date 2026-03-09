using UnityEngine;
using UnityEngine.SocialPlatforms.Impl;

public class UIScore : UICombo
{
    void Update()
    {
        if (player)
        {
            if (oldVal < player.score) animator.SetTrigger("Added");
            text.text = "" + player.score;
            oldVal = player.score;
        }
    }
}
