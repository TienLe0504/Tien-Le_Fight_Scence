using System;
using System.Collections.Generic;
using System.IO;
using System.Xml;
using UnityEngine;

public class LevelGenerator : BaseManager<LevelGenerator>
{
    [Header("Generator Settings")]
    [SerializeField] public int totalLevels = 10;

    [Header("Player Base Stats")]
    [SerializeField] public float playerBaseDamage = 15f;
    [SerializeField] public float playerBaseHealth = 100f;

    [Header("Output")]
    [SerializeField] public string outputFileName = "level_config.json";



    public LevelCollection GenerateAllLevels()
    {
        LevelCollection collection = new LevelCollection();
        int levelsPerMode = totalLevels / 3;
        int oneVoneLevels = levelsPerMode;
        int oneVmanyLevels = levelsPerMode;
        int manyVmanyLevels = totalLevels - (oneVoneLevels + oneVmanyLevels);
        int currentLevel = 1;
        float timeContinueAttack = oneVoneLevels;
        for (int i = 0; i < oneVoneLevels; i++)
        {
            LevelData level = new LevelData { levelNumber = currentLevel, gameMode = "1v1" };
            float maxDrag = 30f - (currentLevel*1.5f);

            ActorData enemy = new ActorData
            {
                health = 80f + (currentLevel * 10f),
                damage = 8f + (currentLevel * 1.5f),
                attackCooldown = Mathf.Max(0.3f, -0.6f * currentLevel + 1.8f),
                drag = Mathf.Max(25,maxDrag), 
                timeToMove = (15 + i*currentLevel*1.5f),
                timeToIdle =Mathf.Max(0.5f,oneVoneLevels -i*0.7f),
                canAttackContinuously = (currentLevel < 0.9f* oneVoneLevels),
                timeAttackContinue = Mathf.Max(0.5f, -0.75f * currentLevel + 2.25f)
            };
            level.enemies.Add(enemy);
            collection.levels.Add(level);
            currentLevel++;
        }
        timeContinueAttack = oneVmanyLevels;
        for (int i = 0; i < oneVmanyLevels; i++)
        {
            LevelData level = new LevelData { levelNumber = currentLevel, gameMode = "1vN" };
            int enemyCount = i + 2;
            float maxDrag = 32f - currentLevel;

            for (int j = 0; j < enemyCount; j++)
            {
                ActorData enemy = new ActorData
                {
                    health = 80f + (currentLevel * 8f),
                    damage = 7f + (currentLevel * 0.8f),
                    attackCooldown = Mathf.Max(0.4f, 1.9f - currentLevel * 0.3f),
                    drag = Mathf.Max(23f, maxDrag - 0.2f * j),
                    timeToMove = (15 + i * currentLevel * 1.5f),
                    timeToIdle = Mathf.Max(0.5f, oneVoneLevels - j * 0.7f),
                    canAttackContinuously = false,
                    timeAttackContinue = Mathf.Max(0.5f, -0.1f * currentLevel + 1.1f)
                };
                if (i <= oneVmanyLevels/2)
                {
                    enemy.canAttackContinuously = true;
                }
                level.enemies.Add(enemy);
            }
            collection.levels.Add(level);
            currentLevel++;
        }
        for (int i = 0; i < manyVmanyLevels; i++)
        {
            LevelData level = new LevelData { levelNumber = currentLevel, gameMode = "NvN" };
            float maxDrag = 35f - currentLevel;
            int allyCount = i + 2;
            for (int k = 0; k < allyCount; k++)
            {
                ActorData ally = new ActorData
                {
                    health = playerBaseHealth * 0.7f + currentLevel * 4f,
                    damage = playerBaseDamage * 0.8f,
                    attackCooldown = Mathf.Max(0.7f, -0.2667f * currentLevel + 3.3669f),
                    drag = Mathf.Max(24f, maxDrag - 0.2f * k), 
                    timeToMove = 18f,
                    timeToIdle = 3f,
                    canAttackContinuously = false,
                    timeAttackContinue = Mathf.Max(0.6f, -0.2f * currentLevel + 2.6f)
                };
                level.allies.Add(ally);
            }
            maxDrag = 34f - currentLevel;
            int enemyCount = i + 3;
            for (int j = 0; j < enemyCount; j++)
            {
                ActorData enemy = new ActorData
                {
                    health = 70f + (currentLevel * 10f),
                    damage = 12f + (currentLevel * 0.8f),
                    attackCooldown = Mathf.Max(0.4f, 0.7f - i * 0.1f),
                    drag = Mathf.Max(24f, maxDrag - 0.2f * j),
                    timeToMove = 18f,
                    timeToIdle = 3f,
                    canAttackContinuously = false,
                    timeAttackContinue = Mathf.Max(0.4f, 0.7f - i * 0.1f)
                };
                level.enemies.Add(enemy);
            }
            collection.levels.Add(level);
            currentLevel++;
        }

            LevelData level11 = new LevelData { levelNumber = currentLevel, gameMode = "25v25" };
            float drag = 24;
            int allyNumber = 24;
            float dragAllies = manyVmanyLevels;
            for (int k = 0; k < allyNumber; k++)
            {

                ActorData ally = new ActorData
                {
                    health = playerBaseHealth * 2.8f + currentLevel * 4f,
                    damage = playerBaseDamage * 1.8f,
                    attackCooldown = Mathf.Max(0.7f, (oneVmanyLevels + oneVoneLevels + manyVmanyLevels + 1) - currentLevel * 1.3f),
                    drag = Mathf.Max(20f, drag - 0.5f * k),
                    timeToMove = 18f,
                    timeToIdle = 3f,
                    canAttackContinuously = false,
                    timeAttackContinue = 0.7f
                };
            level11.allies.Add(ally);
            }

            int enemyNumber = 25;
            drag = 23f;
            
            float attackCooldown = 0.7f;
            for (int j = 0; j < enemyNumber; j++)
            {
                ActorData enemy = new ActorData
                {
                    health = 70f + (currentLevel * 15f),
                    damage = 14f + (currentLevel * 0.8f),
                    attackCooldown = Mathf.Max(0f, attackCooldown - j * 0.3f),
                    drag = Mathf.Max(20f, drag - 0.5f * j),
                    timeToMove = 18f,
                    timeToIdle = 3f,
                    canAttackContinuously = false,
                    timeAttackContinue = 0.45f
                };
            level11.enemies.Add(enemy);
            }
            collection.levels.Add(level11);
            currentLevel++;
        
        return collection;
    }

    public void SaveLevelsToFile()
    {
        LevelCollection allLevels = GenerateAllLevels();
        string json = JsonUtility.ToJson(allLevels, true);
        string path = Path.Combine(Application.persistentDataPath, outputFileName);
        string directory = Path.GetDirectoryName(path);

        if (!Directory.Exists(directory))
        {
            Directory.CreateDirectory(directory);
        }

        File.WriteAllText(path, json);
        Debug.Log("Đã lưu level vào: " + path);
    }

    public void SaveToLevelCurrent(int levelNumber)
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, CONST.PATH_LEVEL_COLLECTION);
            File.WriteAllText(filePath, levelNumber.ToString());
        }
        catch (System.Exception e)
        {
        }
    }
    public int LoadLevelCurrent()
    {
        try
        {
            string filePath = Path.Combine(Application.persistentDataPath, CONST.PATH_LEVEL_COLLECTION);
            Debug.Log("filePath: " + filePath);
            if (!File.Exists(filePath))
            {
                SaveToLevelCurrent(1); 
                return 1;
            }

            string content = File.ReadAllText(filePath);
            if (int.TryParse(content, out int level))
                return level;
            else
            {
                SaveToLevelCurrent(1);
                return 1;
            }
        }
        catch (System.Exception e)
        {
            return 1;
        }
    }
    public LevelCollection LoadAllLevelsFromFile(string fileName)
    {
        string path = Path.Combine(Application.persistentDataPath, fileName);

        if (File.Exists(path))
        {
            try
            {
                string jsonContent = File.ReadAllText(path);
                LevelCollection levelCollection = JsonUtility.FromJson<LevelCollection>(jsonContent);
                return levelCollection;
            }
            catch (Exception e)
            {
                Debug.LogError("Lỗi khi đọc level file: " + e.Message);
                return null;
            }
        }
        else
        {
            Debug.LogWarning("Không tìm thấy file level tại: " + path);
            return null;
        }
    }

}

[Serializable]
public class LevelData
{
    public int levelNumber;
    public string gameMode;
    public List<ActorData> allies; 
    public List<ActorData> enemies; 

    public LevelData()
    {
        allies = new List<ActorData>();
        enemies = new List<ActorData>();
    }
}

[Serializable]
public class ActorData
{
    public float health;
    public float damage;
    public float attackCooldown;
    public float drag;
    public float timeToMove;
    public float timeToIdle;
    public float timeAttackContinue;
    public bool canAttackContinuously;
}

[Serializable] 
public class LevelCollection
{
    public List<LevelData> levels;

    public LevelCollection()
    {
        levels = new List<LevelData>();
    }
}
public class DataUI
{
   public int level;
   public string gameMode;
}