/**
 * @file OperationManager.cs
 * @brief 
 * @author G.Nagasato
 * @date 2021/04/02 作成
 */
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class OperationManager : MonoBehaviour
{
    // カメラ移動の速度
    public float m_speed = 20f;

    private bool scrollStartFlg = false; // スクロールが始まったかのフラグ
    private Vector2 scrollStartPos  =   new Vector2(); // スクロールの起点となるタッチポジション
    private static float SCROLL_DISTANCE_CORRECTION = 0.8f; // スクロール距離の調整

    private Vector2 touchPosition   =   new Vector2(); // タッチポジション初期化

    void Update()
    {
        // マウスホイールの回転値を変数 scroll に渡す
        float scroll = Input.GetAxis("Mouse ScrollWheel");

        // カメラの前後移動処理
        Camera.main.orthographicSize -= scroll * m_speed;


        if (Input.GetMouseButton(0))
        {
            touchPosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);

            scrollStartFlg = true;
            if (scrollStartPos.x == 0.0f && scrollStartPos.y == 0.0f)
            {
                // スクロール開始位置を取得
                scrollStartPos = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            }
            else
            {
                Vector2 touchMovePos = touchPosition;
                
                // 直前のタッチ位置との差を取得する
                float diffX = SCROLL_DISTANCE_CORRECTION * (touchMovePos.x - scrollStartPos.x);
                float diffY = SCROLL_DISTANCE_CORRECTION * (touchMovePos.y - scrollStartPos.y);

                Vector3 pos = this.transform.position;
                pos.x -= diffX;
                pos.y -= diffY;
                pos.z = -10;

                this.transform.position = pos;
                scrollStartPos = touchMovePos;
            }

        }
        else
        {
            // タッチを離したらフラグを落とし、スクロール開始位置も初期化する 
            scrollStartFlg = false;
            scrollStartPos = new Vector2();
        }
    }
}
