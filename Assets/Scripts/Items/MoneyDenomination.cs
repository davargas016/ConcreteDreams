using UnityEngine;
using System.Collections;
using System.Collections.Generic;


[CreateAssetMenu]
public class MoneyDenomination : ScriptableObject
{
    [SerializeField] private string DisplayName;
    [SerializeField] private int Value;

    public string displayName => DisplayName;
    public int value => Value;
}