using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameDateManager 
{
    /*public const string CHICKEN_NUMBER = "鸡肉数量";
    public const string PORK_NUMBER = "猪肉数量";
    public const string EGG_NUMBER = "鸡蛋数量";
    public const string RICE_NUMBER = "米数量";
    public const string TOMATO_NUMBER = "番茄数量";
    public const string POTATO_NUMBER = "土豆数量";
    public const string GREENPEPPER_NUMBER = "青椒数量";
    public const string MUSHROOM_NUMBER = "香菇数量";
    public const string EGGPLANT_NUMBER = "茄子数量";
    public const string FUNGUS_NUMBER = "木耳数量";*/

    //鸡肉数量
    public int CHICKEN_NUMBER = 0;
    //猪肉数量
    public int PORK_NUMBER = 0;
    //鸡蛋数量
    public int EGG_NUMBER = 0;
    //米数量
    public int RICE_NUMBER = 0;
    //番茄数量
    public int TOMATO_NUMBER = 0;
    //土豆数量
    public int POTATO_NUMBER = 0;
    //青椒数量
    public int GREENPEPPER_NUMBER = 0;
    //香菇数量
    public int MUSHROOM_NUMBER = 0;
    //茄子数量
    public int EGGPLANT_NUMBER = 0;
    //木耳数量
    public int FUNGUS_NUMBER = 0;

    //金币数量
    public int SPECIES_NUMBER = 100;

    //关卡进度
    public int BRAAIER_GRADE_1 = 0;
    public int BRAAIER_GRADE_2 = 0;
    public int BRAAIER_GRADE_3 = 0;
    public int LevelNumber = 1;

    //顾客等待时间
    private float[] patience = { 60, 55, 10 };

    public static GameDateManager instance;
    public static GameDateManager Instance
    {
        get
        {
            if(instance == null)
            {
                instance = new GameDateManager();
            }
            return instance;
        }
    }

    public float GetpatienceTimer(int levelNumber)
    {
        return patience[levelNumber - 1];
    }
}
