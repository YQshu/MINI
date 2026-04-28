using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemDrop : MonoBehaviour
{
    [SerializeField] private int possibleItemDrop;
    [SerializeField] private ItemData[] possibleDrop;
    private List<ItemData> dropList = new List<ItemData>();

    [SerializeField] private GameObject itemPrefab;


    public virtual void GenerateDrop()
    {
        for(int i = 0; i < possibleDrop.Length; i++) 
        {
            if(Random.Range(0, 100) <= possibleDrop[i].dropChance)
            {
                dropList.Add(possibleDrop[i]);
            }
        }

        if (dropList.Count == 0) return;

        for (int i = 0; i < possibleItemDrop; i++)
        {
            if (dropList.Count == 0) break; // 列表空了就终止循环
            ItemData randomItem = dropList[Random.Range(0, dropList.Count)];

            dropList.Remove(randomItem);
            DropItem(randomItem);
        }

    }

    protected void DropItem(ItemData _itemData)
    {
        if (itemPrefab == null || _itemData == null) return;
        GameObject newDrop = Instantiate(itemPrefab, transform.position, Quaternion.identity);

        Vector2 randomVelocity = new Vector2(Random.Range(-5, 5), Random.Range(12, 20));

        newDrop.GetComponent<ItemObject>().SetupItem(_itemData,randomVelocity);
    }


}
