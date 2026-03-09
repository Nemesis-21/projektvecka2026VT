//Edgar, 2026-03-09
using UnityEngine;

public class UIHealthbar : UIFindPlayer
{
    void Update()
    {
        if (player) animator.SetFloat("Hp", 1f - (player.currentHp / player.maxhp));
    }
}
