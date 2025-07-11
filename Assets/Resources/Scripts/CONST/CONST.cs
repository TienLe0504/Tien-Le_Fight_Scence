using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CONST 
{
    public const string TAG_PLayer = "Player";
    public const string TAG_ENEMY = "Enemy";
    public const string PATH_LEVEL_CONFIG = "level_config.json";
    public const string PATH_LEVEL_COLLECTION = "level_collection.json";
    public const string VAR_FLOAT_COUNT_KEY = "VarFloatCount";
    public const string IDLE = "Idle";
    public const string WALK = "Walk";
    public const string JUMP = "Jump";
    public const string ATTACK = "Attack";
    public const string INJURY = "Injury";
    public const string KNOCK_OUT = "KnockOut";
    public const string WIN = "Win";
    public const float SPEED_CHARACTER = 2f;
    public const float ROTATION_SPEED_CHARACTER = 10f;
    public const float ATTACK_COMBO_STEP = 0.5f;
    public const float ATTACK_COMBO_WINDOW_PLAYER = 1.5f;
    public const float ATTACK_COMBO_WINDOW_CHARACTER = 1f;
    public const string GAME_MOD = "GameMode";
    public const string LEVEL_MOD = "Level";
    public const int BIG_MODEL = 11;
    public const string GAME_MODE_25_MODEL = "25v25";
    public const string VICTORY = "VICTORY!";
    public const string DEFEAT = "DEFEAT!";
}
