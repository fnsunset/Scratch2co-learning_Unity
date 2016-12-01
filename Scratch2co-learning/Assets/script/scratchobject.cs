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
    public float obj_sizex;
    public float obj_sizey;
    float[] screen = new float[4];
    public float[,] obj_edge = new float[4,2];
    SpriteRenderer thisrender;
    CircleCollider2D thiscollision;
    socketio sp_con;
    bool ch_sprite= false;
    int count = 0;
    int wait = 0;
    // Use this for initialization
    void Start () {
        thisrender = gameObject.GetComponent<SpriteRenderer>();
        thiscollision = gameObject.GetComponent<CircleCollider2D>();
        thiscollision.radius = 3.0f;
        sp_control = GameObject.Find("connect_server");
        sp_con = sp_control.GetComponent<socketio>();
        obj_sizex = thisrender.bounds.size.x;
        obj_sizey = thisrender.bounds.size.y;
        setscreen(true);
        wait = Random.Range(20, 50);
    }
	
	// Update is called once per frame
	void Update () {
        
        if (ch_sprite == false && obj_sprite)
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
        setscreen(sp_con.flame);
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
        obj_sizex = thisrender.bounds.size.x * sp_con.disp_x / sp_con.scsize_x / 2;
        obj_sizey = thisrender.bounds.size.y * sp_con.disp_y / sp_con.scsize_y / 2;
        obj_edge[0, 0] = sp_con.obj_x[obj_mem, obj_id] - obj_sizex * Mathf.Cos(obj_r * Mathf.Deg2Rad);          //オブジェクトの左上x座標 - scratch系
        obj_edge[0, 1] = sp_con.obj_y[obj_mem, obj_id] + obj_sizey * Mathf.Sin(obj_r * Mathf.Deg2Rad);          //オブジェクトの左上y座標 - scratch系
        obj_edge[1, 0] = sp_con.obj_x[obj_mem, obj_id] + obj_sizex * Mathf.Cos((90f - obj_r) * Mathf.Deg2Rad);  //オブジェクトの右上x座標 - scratch系
        obj_edge[1, 1] = sp_con.obj_y[obj_mem, obj_id] + obj_sizey * Mathf.Sin((90f - obj_r) * Mathf.Deg2Rad);  //オブジェクトの右上y座標 - scratch系
        obj_edge[2, 0] = sp_con.obj_x[obj_mem, obj_id] - obj_sizex * Mathf.Cos((90f - obj_r) * Mathf.Deg2Rad);  //オブジェクトの左下x座標 - scratch系
        obj_edge[2, 1] = sp_con.obj_y[obj_mem, obj_id] - obj_sizey * Mathf.Sin((90f - obj_r) * Mathf.Deg2Rad);  //オブジェクトの左下y座標 - scratch系
        obj_edge[3, 0] = sp_con.obj_x[obj_mem, obj_id] + obj_sizex * Mathf.Cos(obj_r * Mathf.Deg2Rad);          //オブジェクトの右下x座標 - scratch系
        obj_edge[3, 1] = sp_con.obj_y[obj_mem, obj_id] - obj_sizey * Mathf.Sin(obj_r * Mathf.Deg2Rad);          //オブジェクトの右下y座標 - scratch系

        if (Mathf.Min(obj_edge[0, 0], obj_edge[1, 0], obj_edge[2, 0], obj_edge[3, 0]) < screen[0])
        {
            sp_con.obj_d[obj_mem, obj_id] = 180f - obj_d;
            sp_con.obj_x[obj_mem, obj_id] += screen[0] - Mathf.Min(obj_edge[0, 0], obj_edge[1, 0], obj_edge[2, 0], obj_edge[3, 0]);
        }
        if (Mathf.Max(obj_edge[0, 0], obj_edge[1, 0], obj_edge[2, 0], obj_edge[3, 0]) > screen[3])
        {
            sp_con.obj_d[obj_mem, obj_id] = 180f - obj_d;
            sp_con.obj_x[obj_mem, obj_id] += screen[3] - Mathf.Max(obj_edge[0, 0], obj_edge[1, 0], obj_edge[2, 0], obj_edge[3, 0]);
        }
        if (Mathf.Min(obj_edge[0, 1], obj_edge[1, 1], obj_edge[2, 1], obj_edge[3, 1]) < screen[2])
        {
            sp_con.obj_d[obj_mem, obj_id] = 360f - obj_d;
            sp_con.obj_y[obj_mem, obj_id] += screen[2] - Mathf.Min(obj_edge[0, 1], obj_edge[1, 1], obj_edge[2, 1], obj_edge[3, 1]);
        }
        if (Mathf.Max(obj_edge[0, 1], obj_edge[1, 1], obj_edge[2, 1], obj_edge[3, 1]) > screen[1])
        {
            sp_con.obj_d[obj_mem, obj_id] = 360f - obj_d;
            sp_con.obj_y[obj_mem, obj_id] += screen[1] - Mathf.Max(obj_edge[0, 1], obj_edge[1, 1], obj_edge[2, 1], obj_edge[3, 1]);
        }
        count++;
        if(count > wait)
        {
            count = 0;
            wait = Random.Range(20, 50);
            if (obj_disp > 0)
            {
                sp_con.send_pos(obj_mem, obj_id, obj_x, obj_y);
            }
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Scratch_Objects")
        {
            scratchobject col_object;
            col_object = collision.gameObject.GetComponent<scratchobject>();
            if (col_object.obj_disp == 1 && obj_disp == 1)
            {
                Debug.Log("接触！！！");
                Debug.Log(obj_mem + "の" + obj_id + "が" + col_object.obj_mem + "の" + col_object.obj_id + "と接触しています");
                if (col_object.obj_mem < obj_mem ||(col_object.obj_mem == obj_mem && col_object.obj_id < obj_id))
                {
                    sp_con.collision_on(obj_mem, obj_id, col_object.obj_mem, col_object.obj_id);
                    Debug.Log(obj_mem+"の"+ obj_id+"が"+ col_object.obj_mem+"の"+ col_object.obj_id+"と接触しています");
                }
            }
        }
    }
    void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Scratch_Objects")
        {
            scratchobject col_object;
            col_object = collision.gameObject.GetComponent<scratchobject>();
            if (col_object.obj_disp == 1 || obj_disp == 1)
            {
                if (col_object.obj_mem < obj_mem || (col_object.obj_mem == obj_mem && col_object.obj_id < obj_id))
                {
                    sp_con.collision_off(obj_mem, obj_id, col_object.obj_mem, col_object.obj_id);
                }
            }
        }
    }
    void setscreen(bool sw_screen)
    {
        if (sw_screen)
        {
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
                    screen[3] = sp_con.scsize_x;
                    break;
            }
            screen[0] = screen[0] * sp_con.disp_x / sp_con.scsize_x;    //左の限界座標 - Scratch系
            screen[1] = screen[1] * sp_con.disp_y / sp_con.scsize_y;    //上の限界座標 - Scratch系
            screen[2] = screen[2] * sp_con.disp_y / sp_con.scsize_y;    //下の限界座標 - Scratch系
            screen[3] = screen[3] * sp_con.disp_x / sp_con.scsize_x;    //右の限界座標 - Scratch系
        }
        else
        {
            screen[0] = sp_con.scsize_x * -1 * sp_con.disp_x / sp_con.scsize_x;
            screen[1] = sp_con.scsize_y * sp_con.disp_y / sp_con.scsize_y;
            screen[2] = sp_con.scsize_y * -1 * sp_con.disp_y / sp_con.scsize_y;
            screen[3] = sp_con.scsize_x * sp_con.disp_x / sp_con.scsize_x;
        }
    }
}
