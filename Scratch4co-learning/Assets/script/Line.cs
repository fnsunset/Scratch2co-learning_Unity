using UnityEngine;
using System.Collections;

public class Line : MonoBehaviour
{
    public Vector3 line0;
    public Vector3 line1;
    socketio line_con;
    public int line_id;
    // Use this for initialization
    void Start()
    {
        GameObject line_con_obj = GameObject.Find("connect_server");
        line_con = line_con_obj.GetComponent<socketio>();
    }

    // Update is called once per frame
    void Update()
    {
        if (line_con.flame || (line_id != 1 && line_id != 4))
        {
            LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
            // 線の幅
            renderer.SetWidth(0.1f, 0.1f);
            // 頂点の数
            renderer.SetVertexCount(2);
            // 頂点を設定
            renderer.SetPosition(0, line0);
            renderer.SetPosition(1, line1);
        }else
        {
            LineRenderer renderer = gameObject.GetComponent<LineRenderer>();
            // 線の幅
            renderer.SetWidth(0f, 0f);
            // 頂点の数
            renderer.SetVertexCount(2);
            // 頂点を設定
            renderer.SetPosition(0, line0);
            renderer.SetPosition(1, line1);
        }
    }
}