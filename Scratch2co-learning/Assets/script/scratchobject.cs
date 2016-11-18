using UnityEngine;
using System.Collections;

public class scratchobject : MonoBehaviour {
    
    public GameObject sp_control;
    public int obj_mem;
    public int obj_id;
    public float obj_x;
    public float obj_y;
    public float obj_z; //size
    public float obj_r; //round
    public float obj_d; //drection
    public int obj_disp;
    public Sprite obj_sprite;
    float obj_rx = 0f;
    float obj_ry = 0f;
    float obj_sizex;
    float obj_sizey;
    float[] screen = new float[4];
    float[,] obj_edge = new float[4,2];
    SpriteRenderer thisrender;
    socketio sp_con;
    bool ch_sprite= false;
    // Use this for initialization
    void Start () {
        thisrender = gameObject.GetComponent<SpriteRenderer>();
        sp_control = GameObject.Find("connect_server");
        sp_con = sp_control.GetComponent<socketio>();
        obj_sizex = thisrender.bounds.size.x;
        obj_sizey = thisrender.bounds.size.y;
        switch (obj_mem)
        {
            case 0:
                screen[0] = sp_con.scsize_x * -1;
                screen[1] = sp_con.scsize_y;
                screen[2] = 0f;
                screen[3] = 0f;
                break;
            case 1:
                screen[0] = 0f;
                screen[1] = sp_con.scsize_y;
                screen[2] = 0f;
                screen[3] = sp_con.scsize_x;
                break;
            case 2:
                screen[0] = sp_con.scsize_x * -1;
                screen[1] = 0f;
                screen[2] = sp_con.scsize_y * -1;
                screen[3] = 0f;
                break;
            case 3:
                screen[0] = 0f;
                screen[1] = 0f;
                screen[2] = sp_con.scsize_y * -1;
                screen[3] = sp_con.scsize_x * -1;
                break;
        }
        screen[0] = screen[0] * sp_con.disp_x / sp_con.scsize_x;    //左の限界座標 - Scratch系
        screen[1] = screen[1] * sp_con.disp_y / sp_con.scsize_y;    //上の限界座標 - Scratch系
        screen[2] = screen[2] * sp_con.disp_y / sp_con.scsize_y;    //下の限界座標 - Scratch系
        screen[3] = screen[3] * sp_con.disp_x / sp_con.scsize_x;    //右の限界座標 - Scratch系
    }
	
	// Update is called once per frame
	void Update () {
	    if(ch_sprite == false && obj_sprite)
        {
            thisrender.sprite = obj_sprite;
            this.transform.localScale = new Vector3(obj_z, obj_z, 1);
            ch_sprite = true;
        }
        obj_x = sp_con.obj_x[obj_mem, obj_id];
        obj_y = sp_con.obj_y[obj_mem, obj_id];
        obj_z = sp_con.obj_z[obj_mem, obj_id];
        obj_r = sp_con.obj_r[obj_mem, obj_id];
        obj_d = sp_con.obj_d[obj_mem, obj_id];
        obj_disp = sp_con.obj_disp[obj_mem, obj_id];
        Vector3 obj_pos = transform.position;
        obj_pos.x = obj_x * sp_con.scsize_x / sp_con.disp_x;    //Unity系座標に変換
        obj_pos.y = obj_y * sp_con.scsize_y / sp_con.disp_y;    //Unity系座標に変換
        transform.position = obj_pos;
        transform.localScale = new Vector3(obj_z, obj_z, 1);
        transform.rotation = Quaternion.Euler(obj_rx, obj_ry, obj_r);
        var color = thisrender.color;
        if (obj_disp >0)
        {          
            color.a = 1f;
        }
        else
        {
            color.a = 0f;
        }
        thisrender.color = color;
        obj_sizex = thisrender.bounds.size.x * sp_con.disp_x / sp_con.scsize_x;
        obj_sizey = thisrender.bounds.size.y * sp_con.disp_y / sp_con.scsize_y;
        obj_edge[0, 0] = obj_x - obj_sizex * Mathf.Cos(obj_r * Mathf.Deg2Rad);          //オブジェクトの左上x座標 - scratch系
        obj_edge[0, 1] = obj_y + obj_sizey * Mathf.Sin(obj_r * Mathf.Deg2Rad);          //オブジェクトの左上y座標 - scratch系
        obj_edge[1, 0] = obj_x + obj_sizex * Mathf.Cos((90f - obj_r) * Mathf.Deg2Rad);  //オブジェクトの右上x座標 - scratch系
        obj_edge[1, 1] = obj_y + obj_sizey * Mathf.Sin((90f - obj_r) * Mathf.Deg2Rad);  //オブジェクトの右上y座標 - scratch系
        obj_edge[2, 0] = obj_x - obj_sizex * Mathf.Cos((90f - obj_r) * Mathf.Deg2Rad);  //オブジェクトの左下x座標 - scratch系
        obj_edge[2, 1] = obj_y - obj_sizey * Mathf.Sin((90f - obj_r) * Mathf.Deg2Rad);  //オブジェクトの左下y座標 - scratch系
        obj_edge[3, 0] = obj_x + obj_sizex * Mathf.Cos(obj_r * Mathf.Deg2Rad);          //オブジェクトの右下x座標 - scratch系
        obj_edge[3, 1] = obj_y - obj_sizey * Mathf.Sin(obj_r * Mathf.Deg2Rad);          //オブジェクトの右下y座標 - scratch系
    }
}
