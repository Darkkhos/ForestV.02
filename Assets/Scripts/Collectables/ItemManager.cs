using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance;
    public SOInt fruit;
    public TextMeshProUGUI uiTextCollectFruit;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        else
            Destroy(gameObject);
    }

    private void Start()
    {
        Reset();
    }
    private void Reset()
    {
        fruit.value = 0;
    }

    public void AddFuits(int amount = 1)
    {
        fruit.value += amount;
    }
}
