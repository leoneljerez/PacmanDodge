using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Heap<H> where H : IHeapItem<H>
{
    H[] items;
    int currentItemCount;

    public Heap(int MaxSizeofHeap)
    {
        items = new H[MaxSizeofHeap];
    }

    public void Add(H item)
    {
        item.HeapIndex = currentItemCount;
        items[currentItemCount] = item;
        SortUp(item);
        currentItemCount++;
    }

    public H RemoveFirst()
    {
        H firstItem = items[0];
        currentItemCount--;
        items[0] = items[currentItemCount];
        items[0].HeapIndex = 0;
        SortDown(items[0]);
        return firstItem;
    }

    public void UpdateItem(H item)
    {
        SortUp(item);
    }

    public int Count
    {
        get
        {
            return currentItemCount;
        }
    }

    public bool Contains(H item)
    {
        return Equals(items[item.HeapIndex], item);
    }

    void SortDown(H item)
    {
        while (true)
        {
            int childIndexRight = item.HeapIndex * 2 + 2;
            int childIndexLeft = item.HeapIndex * 2 + 1;
            int swapIndex = 0;

            if(childIndexLeft < currentItemCount)
            {
                swapIndex = childIndexLeft;

                if(childIndexRight < currentItemCount)
                {
                    if(items[childIndexLeft].CompareTo(items[childIndexRight]) < 0)
                    {
                        swapIndex = childIndexRight;
                    }
                }

                if(item.CompareTo(items[swapIndex]) < 0)
                {
                    Swap(item, items[swapIndex]);
                }

                else
                {
                    return;
                }
            }
            else
            {
                return;
            }
        }
    }

    void SortUp(H item)
    {
        int parentIndex = (item.HeapIndex - 1) / 2;

        while (true)
        {
            H parentItem = items[parentIndex];
            if (item.CompareTo(parentItem) > 0)
            {
                Swap(item, parentItem);
            }
            else
            {
                break;
            }

            parentIndex = (item.HeapIndex - 1) / 2;
        }
    }

    void Swap(H itemA, H itemB)
    {
        items[itemA.HeapIndex] = itemB;
        items[itemB.HeapIndex] = itemA;
        int itemAIndex = itemA.HeapIndex;
        itemA.HeapIndex = itemB.HeapIndex;
        itemB.HeapIndex = itemAIndex;
    }
}

    public interface IHeapItem<H> : IComparable<H>
    {
        int HeapIndex
        {
            get;
            set;
        }
    }
