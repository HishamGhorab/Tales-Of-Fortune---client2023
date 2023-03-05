using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ship
{
    public string id;
    public GameObject shipPrefab;

    public enum ShipType {Sloop, Baghlah, Dhow, Fanchuan, WarBrigrate, WarFrigrate}
    public ShipType shipType;

    public int shipHealth;
    public int baseDamage;

}
