using UnityEngine;
using System.Collections;

public class get_cam_pos : MonoBehaviour {
    public Vector3 ScreenTopLeft;
    public Vector3 ScreenBottomRight;
    // Use this for initialization
    private Camera _mainCamera;

    void Start()
    {
        // カメラオブジェクトを取得します
        GameObject obj = GameObject.Find("Main Camera");
        _mainCamera = obj.GetComponent<Camera>();

        // 座標値を出力
        Debug.Log(getScreenTopLeft().x + ", " + getScreenTopLeft().y);
        Debug.Log(getScreenBottomRight().x + ", " + getScreenBottomRight().y);
    }

    public Vector3 getScreenTopLeft()
    {
        // 画面の左上を取得
        Vector3 topLeft = _mainCamera.ScreenToWorldPoint(Vector3.zero);
        // 上下反転させる
        topLeft.Scale(new Vector3(1f, -1f, 1f));
        ScreenTopLeft = topLeft;
        return topLeft;
    }

    public Vector3 getScreenBottomRight()
    {
        // 画面の右下を取得
        Vector3 bottomRight = _mainCamera.ScreenToWorldPoint(new Vector3(Screen.width, Screen.height, 0.0f));
        // 上下反転させる
        bottomRight.Scale(new Vector3(1f, -1f, 1f));
        ScreenBottomRight = bottomRight;
        return bottomRight;
    }

    // Update is called once per frame
    void Update () {
	
	}
}
