/**
 * @file UIMenuManager.cs
 * @brief 
 * @author G.Nagasato
 * @date 2021/04/02 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;

public class UIMenuManager : MonoBehaviour
{
    [SerializeField] GameObject m_openAndCloseObject;
    [SerializeField] Sprite m_openDirectionImage;
    [SerializeField] Sprite m_closeDirectionImage;
    Button m_openAndCloseButton;
    Image m_openAndCloseImage;
    [SerializeField] GameObject m_menuObject;
    [SerializeField] TMP_InputField m_seedInputField;
    [SerializeField] TMP_InputField m_worldWidthInputField;
    [SerializeField] TMP_InputField m_worldHeightInputField;
    [SerializeField] TMP_InputField m_updateCountInputField;
    private WorldMapGenerator m_generator;

    private void Start()
    {
        m_openAndCloseButton = m_openAndCloseObject.GetComponent<Button>();
        m_openAndCloseImage = m_openAndCloseObject.GetComponent<Image>();
        m_openAndCloseImage.sprite = m_closeDirectionImage;
        //! メニューを閉じる処理をボタンに登録
        m_openAndCloseButton.onClick.AddListener(CloseMenu);

        m_generator = WorldMapGenerator.instance;

        m_seedInputField.text = "512";
        m_worldWidthInputField.text = "32";
        m_worldHeightInputField.text = "32";
        m_updateCountInputField.text = "21";

        GenerateMap();
    }

    /**
     * @name  OpenMenu
     * @brief メニューを開く処理
     */
    public void OpenMenu()
    {
        m_openAndCloseImage.sprite = m_closeDirectionImage;
        m_menuObject.transform.DOLocalMoveX(960, 0.5f).SetEase(Ease.OutCubic);
        m_openAndCloseButton.onClick.RemoveAllListeners();
        m_openAndCloseButton.onClick.AddListener(CloseMenu);
    }

    /**
     * @name  CloseMenu
     * @brief メニューを閉じる処理
     */
    public void CloseMenu()
    {
        m_openAndCloseImage.sprite = m_openDirectionImage;
        m_menuObject.transform.DOLocalMoveX(1710, 0.5f).SetEase(Ease.OutCubic);
        m_openAndCloseButton.onClick.RemoveAllListeners();
        m_openAndCloseButton.onClick.AddListener(OpenMenu);
    }

    /**
     * @name  GenerateMap
     * @brief WorldMapGeneratorのマップ生成処理を呼び出す処理
     */
    public void GenerateMap()
    {
        int seed        = TextToNumber(m_seedInputField.text);
        int width       = TextToNumber(m_worldWidthInputField.text);
        int height      = TextToNumber(m_worldHeightInputField.text);
        int updateCount = TextToNumber(m_updateCountInputField.text);
        m_generator.Generate(seed, width, height, updateCount);
    }

    /**
     * @name  TextToNumber
     * @brief 文字列を数字に変換する処理
     * @param[in] text 文字列
     * @return 文字列の数字が入っていた場合その数値、入っていない場合0を返す
     */
    private int TextToNumber(string text)
    {
        if ("" == text) return 0;

        return int.Parse(text);
    }
}
