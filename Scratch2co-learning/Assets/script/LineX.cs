using UnityEngine;
using System.Collections;

public class LineX : MonoBehaviour {
    public get_cam_pos Getcampos;
	// Use this for initialization
	void Start () {
        
    }
	
	// Update is called once per frame
	void Update () {
        LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
        // 線の幅
        renderer.SetWidth(0.1f, 0.1f);
        // 頂点の数
        renderer.SetVertexCount(2);
        // 頂点を設定
        renderer.SetPosition(0, new Vector3(Getcampos.ScreenTopLeft.x * -1f, 0f, 0f));
        renderer.SetPosition(1, new Vector3(Getcampos.ScreenTopLeft.x, 0f, 0f));
    }
}
