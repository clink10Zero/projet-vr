using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using static ItemProperties;

public class Inventory : MonoBehaviour
{
    private static int INVENTORY_ROWS = 4;// barre inclu
    private static int INVENTORY_ROWS_SIZE = 10;

    [SerializeField] private Item[] itemsStart;
    [SerializeField] private Button[] barre = new Button[INVENTORY_ROWS_SIZE];
    [SerializeField] private HandItem hand;

    (Item,int)[,] itemList = new (Item,int)[INVENTORY_ROWS, INVENTORY_ROWS_SIZE];
    int currentItemRow = 0;
    int currentItemPosition = 0;

    public void setCurrent(int indice)
    {
        currentItemPosition = indice;
        hand.selecter = itemList[0, indice].Item1;
    }


    public void Start()
    {
        for(int i = 0; i < 2; i ++)
        {
            itemList[0, i] = (itemsStart[i], 0);
        }
    }

    public void Update()
    {
        for(int i = 0; i < 10; i++)
        {
            if (currentItemPosition == i)
                barre[i].GetComponent<Image>().color = Color.yellow;
            else
                barre[i].GetComponent<Image>().color = Color.white;

            if (itemList[0, i].Item1 != null)
                barre[i].GetComponentInChildren<Text>().text = itemList[0, i].Item1.name;
            else
                barre[i].GetComponentInChildren<Text>().text = "";
        }
    }

    bool addItem(Item item)
    {
        (int,int) pos = findFirstFreePosition(item);
        if (pos == (INVENTORY_ROWS, INVENTORY_ROWS_SIZE))
        {
            return false;
        }
        itemList[pos.Item1,pos.Item2] = (item, itemList[pos.Item1, pos.Item2].Item2++);
        return true;
    }

    void moveItem(int newInventoryRow, int newPosition)
    {
        if (itemList[newInventoryRow,newPosition].Item1.itemName == itemList[currentItemRow, currentItemPosition].Item1.itemName && itemList[newInventoryRow,newPosition].Item2 < stackable[itemList[newInventoryRow,newPosition].Item1.itemName])// un stack de l'objet est déjà présent et non complet
        {
            if (itemList[newInventoryRow,newPosition].Item2 + itemList[currentItemRow, currentItemPosition].Item2 > stackable[itemList[newInventoryRow,newPosition].Item1.itemName])//si la somme est supérieure à la taille max de la stack 
            {
                itemList[newInventoryRow,newPosition].Item2 = stackable[itemList[newInventoryRow,newPosition].Item1.itemName];
                itemList[currentItemRow,currentItemPosition].Item2 -= itemList[newInventoryRow,newPosition].Item2 + itemList[currentItemRow,currentItemPosition].Item2 - stackable[itemList[newInventoryRow,newPosition].Item1.itemName];
            }
            else
            {
                itemList[newInventoryRow,newPosition].Item2 += itemList[currentItemRow,currentItemPosition].Item2;
                itemList[currentItemRow,currentItemPosition] = (null, 0);
            }
        }
        else
        {
            (Item, int) tmpItem = (null, 0);
            if (itemList[newInventoryRow,newPosition].Item1.itemName != itemList[currentItemRow,currentItemPosition].Item1.itemName && itemList[newInventoryRow,newPosition].Item2 != 0)//il y a un objet différent
            {
                tmpItem = itemList[newInventoryRow,newPosition];
            }
            itemList[newInventoryRow,newPosition] = itemList[currentItemRow,currentItemPosition];
            itemList[currentItemRow,currentItemPosition] = tmpItem;
        }
    }

    (int,int) findFirstFreePosition(Item item)
    {
        for(int i=0; i< INVENTORY_ROWS; i++)
        {
            for(int j=0;j < INVENTORY_ROWS_SIZE; j++)
            {
                if (itemList[i,j].Item2 == 0 || (itemList[i,j].Item1.itemName == item.itemName && itemList[i,j].Item2 < stackable[item.itemName]))
                {
                    return (i, j);
                }
            }
        }
        return (INVENTORY_ROWS, INVENTORY_ROWS_SIZE);

    }
}
