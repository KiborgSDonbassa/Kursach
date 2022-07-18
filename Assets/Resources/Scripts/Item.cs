using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;



[CreateAssetMenu(fileName = "New Item", menuName = "Items", order = 1)]
public class Item : ScriptableObject
{
    public enum Types
    {
        nothing,
        armor,
        weapon
    }
    public int id_;
    public string name_;
    public Sprite icon_;
    public Types type;
}
    
