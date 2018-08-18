using UnityEngine;
using System.Collections;

[System.Serializable]
public class CreatureData
{
    public string Name;
    public int HitPoints;
    public int SlashingDR;
    public int BludgeonDR;
    public int PiercingDR;

    public override string ToString()
    {
        string data = "Name: " + Name + System.Environment.NewLine + "Hit Points: " + HitPoints + System.Environment.NewLine
            + "Damage reduction from slashing weapons: " + SlashingDR + System.Environment.NewLine
            + "Damage reduction from bludgeoning weapons: " + BludgeonDR + System.Environment.NewLine
            + "Damage reduction from piercing weapons: " + PiercingDR + System.Environment.NewLine;

        return data;
    }
}
