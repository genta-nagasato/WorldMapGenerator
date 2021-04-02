/**
 * @file   SingletonMonoBehaviour.cs
 * @brief  Monobehaviour用シングルトンクラス定義
 * @author G.Nagasato
 * @date 2021/04/02 作成
 */
using System;
using UnityEngine;
using UnityEngine.SceneManagement;

/**
 * @class SingletonMonoBehaviour
 * @brief Monobehaviour用シングルトンクラス
 */
public abstract class SingletonMonoBehaviour<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T com;               //!< コンポーネント
    private static bool instantiated;   //!< 初期化フラグ
    private static bool dontDestroy;    //!< 非破壊オブジェクトフラグ

    //! インスタンス
    public static T instance
    {
        get
        {
            //! 既に存在するか確認し、存在したらコンポーネントを返す
            var type = typeof(T);
            var objects = FindObjectsOfType<T>();
            if (objects.Length > 0) {
                instance = objects[0];
                if (objects.Length > 1)
                {
                    for (var i = 1; i < objects.Length; ++i)
                    {
                        DestroyImmediate(objects[i].gameObject);
                    }
                }
                instantiated = true;
                return com;
            }

            if (instantiated)
            { 
                //! 初期化済みの場合コンポーネントを返す
                return com;
            }

            //! プレハブから読み込み
            try
            {
                var gameObject = Instantiate(Resources.Load<GameObject>($"Prefabs/{typeof(T).ToString()}")) as GameObject;
                if (gameObject != null)
                {
                    gameObject.name = typeof(T).ToString();
                    instance = gameObject.GetComponent<T>();
                    if (!instantiated)
                    {
                        instance = gameObject.AddComponent<T>();
                    }
                    return instance;
                }
            }
            catch (System.ArgumentException e)
            {
                Debug.LogWarning(e);
            }

            return null;
        }

        private set
        {
            com = value;
            instantiated = value != null;
        }
    }

    public bool dontDestroyOnLoad
    {
        get
        {
            return dontDestroy;
        }
        set
        {
            dontDestroy = value;

            if (dontDestroy)
            {
                DeleteSameObject();
            }
            else
            {
                SceneManager.MoveGameObjectToScene(this.gameObject, SceneManager.GetActiveScene());
            }

        }
    }

    /**
     * @brief 
     * @return 
     */
    protected void DeleteSameObject()
    {
        var objects = FindObjectsOfType<T>();
        if (objects.Length > 1)
        {
            for (var i = 1; i < objects.Length; ++i)
            {
                DestroyImmediate(objects[i].gameObject);
            }
        }
        else if(objects.Length == 0)
        {
            DontDestroyOnLoad(this.gameObject);
        }
    }
}