using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

class Settings
{
    public const string MONSTER_CONFIG_PATH = "./Assets/monsters.json";
}

class Global
{
    public static List<Monster> monsters;

}

[Flags]
public enum Status
{
    None = 0x0,
    Slow = 0x1,
    Poison = 0x2,
    Cold = 0x4,
    Fierce = 0x8
}