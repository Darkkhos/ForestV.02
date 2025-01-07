using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemCollectableFruit : ItemCollectableBase
{
    protected override void OnCollect()
    {
        base.OnCollect();
        ItemManager.Instance.AddFuits();
    }
}
