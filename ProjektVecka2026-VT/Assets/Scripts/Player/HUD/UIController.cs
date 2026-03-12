using TMPro;
using UnityEngine;

public class UIController : UIFindPlayer
{

    RectTransform rectTransform;

    public override void OtherAwake()
    {
        rectTransform = GetComponent<RectTransform>();
    }
    // Update is called once per frame
    void Update()
    {
        if (player)
        {
            if (player.currentHp<=0)
            {
                rectTransform.position = new Vector3(300f, 0f, 0f);
            }
        }
    }
}
