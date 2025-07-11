using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;
using Cinemachine;
public class GameManger : BaseManager<GameManger>
{
    [Header("Input")]
    public FloatingJoystick input;

    [Header("Camera")]
    public CinemachineVirtualCamera virtualCamera;

    [Header("Transforms")]
    public GameObject PlayerTranform;
    public GameObject EnemyTranform;
    public Transform heathTranform;

    [Header("Prefabs")]
    public Player PlayerPrefab;
    public Character enemyPrefab;
    public Character patterPrefab;

    [Header("Gameplay Positioning")]
    public Vector3 playerStartPosition = new Vector3(0f, 0.6f, -2f);
    public Vector3 enemyStartPosition = new Vector3(0f, 0.6f, 1.5f);
    public float distanceBetweenEntities = 0.4f;

    [Header("Spawn Configuration")]
    public int enemyCount = 13;
    public int partnerCount = 10;
    public int enemyRowCount = 8;
    public int parterRowCount = 6;

    [Header("Runtime Instances")]
    public Player playerCurrent;
    public List<Character> enemies = new List<Character>();
    public List<Character> teammates = new List<Character>();

    [Header("Health UI")]
    public HealthBarUI healthBarUI;
    public List<HealthBarUI> healthBarUIs = new List<HealthBarUI>();

    [Header("Level Data")]
    public LevelCollection allLevelData;
    public int levelNumber = 1;
    public string gameMode;

    [Header("Actor Data")]
    public List<ActorData> alliesData = new List<ActorData>();
    public List<ActorData> enemiesData = new List<ActorData>();

    [Header("Game State Flags")]
    public bool isPlayerGame = false;
    public bool isSend = false;
    public bool isIdle = false;
    public bool isBigModel = false;

    protected override void Awake()
    {
        base.Awake();
        InitializeCharacterPools(teammates, patterPrefab, PlayerTranform.transform);
        InitializeCharacterPools(enemies, enemyPrefab, EnemyTranform.transform);
        InitializeHealthBarPools(healthBarUIs, healthBarUI, heathTranform);
        LevelGenerator.Instance.SaveLevelsToFile();
        allLevelData = LevelGenerator.Instance.LoadAllLevelsFromFile(LevelGenerator.Instance.outputFileName);
        levelNumber = LevelGenerator.Instance.LoadLevelCurrent();
    }
    private void OnEnable()
    {
        ListenerManager.Instance.AddListener(EventType.StartGameWith25Models, IsBigModel);
        ListenerManager.Instance.AddListener(EventType.StartGame, StartGame);
        ListenerManager.Instance.AddListener(EventType.continueGame, PlayGameContinue);
        ListenerManager.Instance.AddListener(EventType.HomeScreen, HomeScreen);
        ListenerManager.Instance.AddListener(EventType.ReadyGo, OnReadyGoComplete);
    }
    private void OnDisable()
    {
        ListenerManager.Instance.RemoveListener(EventType.StartGame, StartGame);
        ListenerManager.Instance.RemoveListener(EventType.continueGame, PlayGameContinue);
        ListenerManager.Instance.RemoveListener(EventType.HomeScreen, HomeScreen);
        ListenerManager.Instance.RemoveListener(EventType.ReadyGo, OnReadyGoComplete);
        ListenerManager.Instance.RemoveListener(EventType.StartGameWith25Models, IsBigModel);
    }
    public void StartGame()
    {
        LoadLevel();
        isSend = false;
        ListenerManager.Instance.TriggerEvent(EventType.TurnOnUI);
        TurnOffHealthBarUI();
        SendDataUI();
        SpawPlayer();
        CreateEnemyFollowNumber(enemiesData.Count, enemyPrefab, EnemyTranform, enemyStartPosition, enemyRowCount, enemies, enemiesData);
        CreateEnemyFollowNumber(alliesData.Count, patterPrefab, PlayerTranform, playerStartPosition, parterRowCount, teammates, alliesData, true);

    }

    private void SpawPlayer()
    {
        if (playerCurrent == null)
            playerCurrent = Instantiate(PlayerPrefab, playerStartPosition, Quaternion.identity);
        playerCurrent.gameObject.SetActive(true);
        virtualCamera.gameObject.SetActive(true);
        virtualCamera.Follow = playerCurrent.transform;
        virtualCamera.LookAt = playerCurrent.transform;
        playerCurrent.InitializeInput(input);
        playerCurrent.InitializePlayer(LevelGenerator.Instance.playerBaseHealth, LevelGenerator.Instance.playerBaseDamage, playerStartPosition);
    }
    void InitializeCharacterPools(List<Character> character, Character prefab, Transform tranformPrefab)
    {
        for (int i = 0; i < 25; i++)
        {
            Character c = Instantiate(prefab);
            c.transform.SetParent(tranformPrefab);
            c.gameObject.SetActive(false);
            character.Add(c);
        }
    }
    void InitializeHealthBarPools(List<HealthBarUI> health, HealthBarUI prefab, Transform tranformPrefab)
    {
        for (int i = 0; i < 49; i++)
        {
            HealthBarUI healthBar = Instantiate(prefab);
            healthBar.transform.SetParent(tranformPrefab);
            healthBar.gameObject.SetActive(false);
            health.Add(healthBar);
        }
    }

    public void OnReadyGoComplete()
    {
        isPlayerGame = true;
    }
    public void IsBigModel()
    {
        isBigModel = true;
    }
    public void SendDataUI()
    {
        DataUI data = new DataUI { level = levelNumber, gameMode = gameMode };
        if (gameMode == CONST.GAME_MODE_25_MODEL)
        {
            data.level = 11;
        }
        ListenerManager.Instance.TriggerEvent<DataUI>(EventType.dataUI, data);
    }
    public void LoadLevel()
    {
        LevelData data = allLevelData.levels.Find(level => level.levelNumber == levelNumber);
        if (isBigModel)
        {
            data = allLevelData.levels.Find(level => level.levelNumber == CONST.BIG_MODEL);
        }
        if (data == null)
        {
            return;
        }
        gameMode = data.gameMode;
        alliesData = data.allies;
        enemiesData = data.enemies;
    }
    public void TakeDamagePlayerUI(float damage)
    {
        ListenerManager.Instance.TriggerEvent<float>(EventType.TakeDamage, damage);
    }
    public void IncreaseLevelNumber()
    {
        levelNumber += 1;
        LevelData data = allLevelData.levels.Find(level => level.levelNumber == levelNumber);
        if (data == null)
        {
            levelNumber = 1;
        }
        LevelGenerator.Instance.SaveToLevelCurrent(levelNumber);
    }
    public void PlayGameContinue()
    {
        DisableCharacterList(teammates);
        LoadLevel();
        StartGame();
    }
    public void DisableCharacterList(List<Character> listObject)
    {
        if (listObject.Count == 0)
            return;
        foreach (Character enemy in listObject)
        {
            enemy.gameObject.SetActive(false);
        }

    }
    public void TurnOffHealthBarUI()
    {
        foreach (HealthBarUI healthBar in healthBarUIs)
        {
            healthBar.gameObject.SetActive(false);
        }
    }

    public void WinGame()
    {
        if (isSend)
            return;
        isSend = true;
        TurnOffHealthBar();
        if (!isBigModel)
        {
            IncreaseLevelNumber();
        }
        isBigModel = false;
        virtualCamera.gameObject.SetActive(false);
        ListenerManager.Instance.TriggerEvent(EventType.winGame);
    }
    public void LoseGame()
    {
        if (isSend)
            return;
        isSend = true;
        TurnOffHealthBar();
        virtualCamera.gameObject.SetActive(false);
        ListenerManager.Instance.TriggerEvent(EventType.loseGame);
    }
    public void HomeScreen()
    {
        DisableCharacterList(enemies);
        DisableCharacterList(teammates);
        if (playerCurrent != null)
        {
            playerCurrent.gameObject.SetActive(false);
        }
    }
    public void TurnOffHealthBar()
    {
        if (healthBarUIs.Count == 0)
        {
            return;
        }
        foreach (HealthBarUI healthBar in healthBarUIs)
        {
            healthBar.gameObject.SetActive(false);
        }
    }
    public void CreateEnemyFollowNumber(int numberToSpawn, Character prefab, GameObject tranformParent, Vector3 position, int numerRowEnemy, List<Character> listObject, List<ActorData> actorsData, bool isPartent = false)
    {
        if (numberToSpawn <= 0)
        {
            return;
        }
        DisableCharacterList(listObject);
        if (numberToSpawn == 1 && !isPartent)
        {
            ActorData actorData = actorsData[0];
            float posX = 0;
            bool isCreateSuccess = CreateEnemyInPoolObject(listObject, position, ref posX, actorData);
            if (isCreateSuccess) return;
            Character enemy = Instantiate(prefab, position, Quaternion.identity);
            listObject.Add(enemy);
            HealthBarUI healthBar = CreateHealthBar(enemy);
            enemy.GetComponent<Character>().CreateEnemy(actorData.health, actorData.damage, actorData.attackCooldown, actorData.drag, actorData.timeToMove, actorData.timeToIdle, actorData.canAttackContinuously, healthBar, actorData.timeAttackContinue);
            enemy.transform.SetParent(tranformParent.transform);
            return;
        }

        int remaining = numberToSpawn;
        int rowCount = Mathf.CeilToInt((float)numberToSpawn / numerRowEnemy);
        float posXStart = CalculatePosX(numberToSpawn);
        for (int i = 0; i < rowCount; i++)
        {
            float posX = posXStart;
            float posZ = !isPartent ? position.z + i * distanceBetweenEntities : position.z - i * distanceBetweenEntities;
            if (isPartent)
            {
                posZ -= distanceBetweenEntities;
                //posX += disTanceBetweenEntity;

            }
            if (remaining > numerRowEnemy)
            {
                posX = SpawRow(posX, posZ, numerRowEnemy, prefab, tranformParent, position, listObject, actorsData);
                remaining -= numerRowEnemy;
            }
            else
            {
                posX = SpawRow(posX, posZ, remaining, prefab, tranformParent, position, listObject, actorsData);
            }
        }

    }

    private float CalculatePosX(int numberToSpawn)
    {
        float posXStart;
        if (numberToSpawn > enemyRowCount)
        {
            posXStart = -(enemyRowCount / 2) * distanceBetweenEntities;
        }
        else
        {
            posXStart = -(numberToSpawn / 2) * distanceBetweenEntities;
        }

        return posXStart;
    }

    private float SpawRow(float posX, float posZ, int numberEnemy, Character prefab, GameObject tranformParrent, Vector3 position, List<Character> listObject, List<ActorData> actorsData)
    {
        for (int j = 0; j < numberEnemy; j++)
        {
            ActorData actorData = actorsData[j % actorsData.Count];
            bool isCreateSuccess = CreateEnemyInPoolObject(listObject, new Vector3(posX, position.y, posZ), ref posX, actorData);
            if (!isCreateSuccess)
            {
                Character enemy = Instantiate(prefab, new Vector3(posX, position.y, posZ), Quaternion.identity);
                listObject.Add(enemy);
                enemy.transform.SetParent(tranformParrent.transform);
                posX = GenerateAttribute(ref posX, enemy, actorData);
            }
        }

        return posX;
    }

    private float GenerateAttribute(ref float posX, Character enemy, ActorData actorData)
    {
        HealthBarUI healthBar = CreateHealthBar(enemy);
        enemy.GetComponent<Character>().CreateEnemy(actorData.health, actorData.damage, actorData.attackCooldown, actorData.drag, actorData.timeToMove, actorData.timeToIdle, actorData.canAttackContinuously, healthBar, actorData.timeAttackContinue);
        posX += distanceBetweenEntities;
        return posX;
    }

    private HealthBarUI CreateHealthBar(Character enemy)
    {
        HealthBarUI healtBarObject = CreateHealthBarPool(enemy);
        if (healtBarObject == null)
        {
            healtBarObject = Instantiate(healthBarUI, heathTranform.position, Quaternion.identity);
            healthBarUIs.Add(healtBarObject);
            healtBarObject.transform.SetParent(heathTranform);
        }
        return healtBarObject;
    }

    public HealthBarUI CreateHealthBarPool(Character enemy)
    {
        if (healthBarUIs.Count == 0)
        {
            return null;
        }
        foreach (HealthBarUI healthBar in healthBarUIs)
        {
            if (!healthBar.gameObject.activeSelf)
            {
                healthBar.gameObject.SetActive(true);
                healthBar.transform.SetParent(heathTranform);
                return healthBar;
            }
        }
        return null;
    }

    public bool CreateEnemyInPoolObject(List<Character> listObject, Vector3 pos, ref float posX, ActorData actorData)
    {
        if (listObject.Count == 0)
            return false;
        foreach (Character item in listObject)
        {
            if (!item.gameObject.activeSelf)
            {
                item.gameObject.SetActive(true);
                item.transform.position = pos;
                posX = GenerateAttribute(ref posX, item, actorData);
                return true;
            }
        }
        return false;
    }


    public Entity GetPosPosition(EntityType entityType, Vector3 pos)
    {
        List<Character> listObject = GetListObject(entityType);
        if (listObject == null)
        {
            return null;
        }
        Character nearestEnemy = null;
        float minDistance = float.MaxValue;

        foreach (Character enemy in listObject)
        {
            if (enemy.gameObject.activeSelf && enemy.isAlive)
            {
                float distance = Vector3.Distance(pos, enemy.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    nearestEnemy = enemy;
                }
            }
        }

        if (nearestEnemy != null)
        {
            if (entityType == EntityType.Player && playerCurrent.isAlive)
            {
                float distanceToPlayer = Vector3.Distance(pos, playerCurrent.transform.position);
                float distanceToEnemy = Vector3.Distance(pos, nearestEnemy.transform.position);
                if (distanceToPlayer < distanceToEnemy)
                {
                    return playerCurrent;
                }
            }
            return nearestEnemy;
        }
        return null;
    }
    public List<Character> GetListObject(EntityType type)
    {
        if (type == EntityType.Player)
        {
            return teammates;
        }
        if (type == EntityType.Enemy)
        {
            return enemies;
        }
        return null;
    }
    public bool IsWinBattle()
    {
        foreach (Character enemy in enemies)
        {
            if (enemy.isAlive && enemy.gameObject.activeSelf)
            {
                return false;
            }
        }
        return true;
    }
    public void ChangeCamera()
    {
        foreach (Character c in teammates)
        {
            if (c.isAlive && c.gameObject.activeSelf)
            {
                virtualCamera.LookAt = c.gameObject.transform;
                virtualCamera.Follow = c.gameObject.transform;
                c.hasCamera = true;
                return;
            }
        }
        return;
    }
}
