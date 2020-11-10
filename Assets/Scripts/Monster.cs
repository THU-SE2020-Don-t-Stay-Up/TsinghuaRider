using Newtonsoft.Json;
using System.Collections.Generic;
using System.IO;

/// <summary>
/// 怪物属性类，继承自LivingBase，存储角色所有基本属性
/// </summary>
public class Monster : LivingBase
{
    /// <summary>
    /// 视野角度
    /// </summary>
    public float ViewAngle { get; set; }
    /// <summary>
    /// 视野半径，玩家进入视野半径以内时追击玩家
    /// </summary>
    public float ViewRadius { get; set; }
    /// <summary>
    /// 击杀奖励
    /// </summary>
    public int Reward { get; set; }
    /// <summary>
    /// 难度
    /// </summary>
    public int Difficulty { get; set; }


    /// <summary>
    /// 加载怪物配置文件，游戏初始化时调用
    /// </summary>
    /// <returns>monster list</returns>
    public static List<Monster> LoadMonster()
    {
        string path = Settings.MONSTER_CONFIG_PATH;
        List<Monster> monsters = JsonSerializer.CreateDefault().Deserialize<List<Monster>>(new JsonTextReader(new StreamReader(path)));
        return monsters;
    }

    public static void SaveMonster(List<Monster> monsters)
    {
        string path = Settings.MONSTER_CONFIG_PATH;
        using (StreamWriter sw = new StreamWriter(path))
        {
            JsonSerializer.CreateDefault().Serialize(new JsonTextWriter(sw), monsters);
        }
    }
}

