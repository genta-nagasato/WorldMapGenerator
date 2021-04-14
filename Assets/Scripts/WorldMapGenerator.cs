/**
 * @file WorldMapGenerator.cs
 * @brief WorldMapGeneratorクラスの定義
 * @author G.Nagasato
 * @date 2021/04/02 作成
 */
using UnityEngine;

/**
 * @class WorldMapGenerator
 * @brief ワールドマップ作成クラス
 */
public class WorldMapGenerator : SingletonMonoBehaviour<WorldMapGenerator>
{
    [SerializeField] GameObject m_worldMapRoot;     //!< マップを生成するルートオブジェクト
    [SerializeField] GameObject[] m_tilePrefabs;    //!< タイルプレハブ
    private int[] m_mapData;    //!< マップ情報
    private int m_width;        //!< マップ横幅
    private int m_height;       //!< マップ縦幅

    private Vector2[] m_direction = new Vector2[]
    {
        new Vector2(1, 0),
        new Vector2(-1, 0),
        new Vector2(0, 1),
        new Vector2(0, -1),

    };  //! 4方向

    /**
     * @name  Generate
     * @brief ワールドマップの生成処理
     * @param[in] seed ランダムのシード値
     * @param[in] width 生成するマップの横幅
     * @param[in] height 生成するマップの縦幅
     * @param[in] updateCount ワールドマップの更新回数
     */
    public void Generate(int seed, int width, int height, int updateCount)
    {
        if (0 == width || 0 == height) return;

        //! 前に作成したタイルを削除
        foreach (Transform trans in m_worldMapRoot.transform)
        {
            GameObject.Destroy(trans.gameObject);
        }

        //! ランダムをシードで初期化
        Random.InitState(seed);

        m_width = width;
        m_height = height;

        m_mapData = new int[m_width * m_height];

        //! マップのふちを海で埋める
        for(int x = 0; x < m_width; x++)
        {
            m_mapData[x] = 0;
            m_mapData[(m_height - 1) * m_width + x] = 0;
        }
        for (int y = 0; y < m_height; y++)
        {
            m_mapData[y * m_width] = 0;
            m_mapData[y * m_width + m_width - 1] = 0;
        }

        //! ランダム代入
        for (int y = 1; y < m_height - 1; y++)
        {
            for(int x = 1; x < m_width - 1; x++)
            {
                int rand = Random.Range(0, m_tilePrefabs.Length);
                m_mapData[y * m_width + x] = rand;

            }
        }

        //! ワールドマップの更新
        for (int i = 0; i < updateCount; i++)
            UpdateWorldMap();

        //! 生成
        for (int y = 0; y < m_height; y++)
        {
            for (int x = 0; x < m_width; x++)
            {
                int num = y * m_width + x;
                GameObject tileObject = Instantiate(m_tilePrefabs[m_mapData[num]],new Vector3(x - m_width * 0.5f, -y + m_height * 0.5f, 0), Quaternion.identity);
                tileObject.transform.parent = m_worldMapRoot.transform;
            }
        }
    }

    /**
     * @name  UpdateWorldMap
     * @brief ワールドマップの更新処理
     */
    private void UpdateWorldMap()
    {
        //! タイルをランダム変更
        for (int y = 1; y < m_height - 1; y++)
        {
            for (int x = 1; x < m_width - 1; x++)
            {
                int num = y * m_width + x;

                int rand = Random.Range(0, 4);
                int dx = (int)m_direction[rand].x;
                int dy = (int)m_direction[rand].y;

                int tile = 0;
                if (x + dx <= 0 || m_width - 1 <= x + dx || y + dy <= 0 || m_height - 1 <= y + dy)
                    tile = 0;
                else
                    tile = m_mapData[(y + dy) * m_width + (x + dx)];

                m_mapData[y * m_width + x] = tile;
            }
        }

        //! 四方を同じタイル囲まれていたら同じタイルへ変更
        for (int y = 1; y < m_height - 1; y++)
        {
            for (int x = 1; x < m_width - 1; x++)
            {
                int tile = 0;
                int sameTile = -1;
                bool isSame = true;

                for (int i = 0; i < 4; i++)
                {
                    int dx = (int)m_direction[i].x;
                    int dy = (int)m_direction[i].y;

                    if (x + dx <= 0 || m_width - 1 <= x + dx || y + dy <= 0 || m_height - 1 <= y + dy)
                        tile = 0;
                    else
                        tile = m_mapData[(y + dy) * m_width + (x + dx)];

                    if (-1 == sameTile)
                        sameTile = tile;
                    else if (sameTile != tile)
                    {
                        isSame = false;
                        break;
                    }
                }

                if (isSame)
                {
                    m_mapData[y * m_width + x] = sameTile;
                }
            }
        }
    }
}
