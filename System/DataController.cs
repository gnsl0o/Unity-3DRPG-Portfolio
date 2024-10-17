using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using System.Linq;
using System;
using UnityEngine.SceneManagement;

public class DataController : MonoBehaviour
{
    static GameObject _container;
    static GameObject Container
    {
        get { return _container; }
    }

    static DataController _instance;
    public static DataController Instance
    {
        get
        {
            if (!_instance)
            {
                _container = new GameObject();
                _container.name = "DataController";
                _instance = _container.AddComponent(typeof(DataController)) as DataController;
                DontDestroyOnLoad(_container);
            }
            return _instance;
        }
    }

    private GameData _gameData;
    public GameData gameData
    {
        get
        {
            if (_gameData == null)
            {
                _gameData = new GameData();
            }
            return _gameData;
        }
    }

    private int currentSlot = 0;

    private string GetFilePath(int slot)
    {
        return Path.Combine(Application.persistentDataPath, $"gamedata_slot{slot}.json");
    }

    public void LoadGameData(int slot)
    {
        currentSlot = slot;
        string filePath = GetFilePath(slot);

        if (File.Exists(filePath))
        {
            Debug.Log("�ҷ����� ����");
            string fromJsonData = File.ReadAllText(filePath);
            _gameData = JsonUtility.FromJson<GameData>(fromJsonData);
            LoadingScreen.LoadScene(_gameData.currentScene);
        }
        else
        {
            Debug.Log("���ο� ���� ����");
            _gameData = new GameData();
        }
    }

    public void ApplyLoadedData()
    {
        // ��� ISaveable ��ü�� ã�� ������ �ε�
        foreach (ISaveable saveable in FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>())
        {
            saveable.LoadData(_gameData);
        }
    }

    public void SaveGameData(int slot)
    {
        currentSlot = slot;
        string filePath = GetFilePath(slot);

        if (_gameData == null)
        {
            _gameData = new GameData();
        }

        _gameData.currentScene = SceneManager.GetActiveScene().name;

        foreach (ISaveable saveable in FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>())
        {
            saveable.SaveData(_gameData);
        }

        string toJsonData = JsonUtility.ToJson(gameData, true);
        File.WriteAllText(filePath, toJsonData);
    }

    private void OnApplicationQuit()
    {
        SaveGameData(currentSlot);
    }

    private IEnumerator LoadSceneAndRestoreState(string sceneName)
    {
        // ���� �񵿱������� �ε��մϴ�.
        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);
        while (!asyncLoad.isDone)
        {
            yield return null;
        }

        // ���� �ε�� �� ��� ISaveable ��ü�� LoadData �޼��带 ȣ���Ͽ� ���� ���¸� �����մϴ�.
        foreach (ISaveable saveable in FindObjectsOfType<MonoBehaviour>().OfType<ISaveable>())
        {
            saveable.LoadData(_gameData);
        }
    }
}