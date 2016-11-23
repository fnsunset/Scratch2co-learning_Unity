using UnityEngine;
using SocketIO;
using System;

public class socketio : MonoBehaviour {
    SocketIOComponent socket;
    string id = "";
    public bool flame = true;
    public int maxmember = 4;
    public int Group = start.getGROUP();
    public int disp_x;
    public int disp_y;
    public float scsize_x;
    public float scsize_y;
    public GameObject camerapos;
    public GameObject addobject;
    public GameObject obj_parent;
    public GameObject scr_line;
    public GameObject line_parent;
    public Sprite[] Sprites;
    public float[] Sprite_Size;
    public float[,] obj_x = new float[10,50];
    public float[,] obj_y = new float[10,50];
    public float[,] obj_z = new float[10,50]; //size
    public float[,] obj_r = new float[10,50]; //round
    public float[,] obj_d = new float[10,50]; //drection
    public int[,] obj_disp = new int[10,50];
    GameObject clone;
    GameObject line_clone;
    // Use this for initialization
    void Start () {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        socket.On("server/hello", hello);
        socket.On("unity/move", socket_move);
        socket.On("unity/rotate", socket_rotate);
        socket.On("unity/ang", socket_angle);
        socket.On("unity/movex", socket_movex);
        socket.On("unity/movey", socket_movey);
        socket.On("unity/warp", socket_warp);
        socket.On("unity/appear", socket_appear);
        socket.On("unity/hide", socket_hide);
        socket.On("server/toggle_flame", socket_flame);
        Debug.Log("Send first message");
        get_cam_pos campos = camerapos.GetComponent<get_cam_pos>();
        scsize_x = campos.ScreenBottomRight.x;
        scsize_y = campos.ScreenTopLeft.y;
        for (int cnt = 0; cnt < 6; cnt++)
        {
            line_clone = (GameObject)Instantiate(scr_line);
            line_clone.transform.parent = line_parent.transform;
            Line lines = line_clone.GetComponent<Line>();
            lines.line_id = cnt;
            switch (cnt)
            {
                case 0:
                    lines.line0 = new Vector3(campos.ScreenTopLeft.x, campos.ScreenTopLeft.y, 0f);
                    lines.line1 = new Vector3(campos.ScreenTopLeft.x * -1f, campos.ScreenTopLeft.y, 0f);
                    line_clone.tag = "LineX";
                    break;
                case 1:
                    lines.line0 = new Vector3(campos.ScreenTopLeft.x, 0f, 0f);
                    lines.line1 = new Vector3(campos.ScreenTopLeft.x * -1f, 0f, 0f);
                    line_clone.tag = "LineX";
                    break;
                case 2:
                    lines.line0 = new Vector3(campos.ScreenTopLeft.x, campos.ScreenTopLeft.y * -1f, 0f);
                    lines.line1 = new Vector3(campos.ScreenTopLeft.x * -1f, campos.ScreenTopLeft.y * -1f, 0f);
                    line_clone.tag = "LineX";
                    break;
                case 3:
                    lines.line0 = new Vector3(campos.ScreenTopLeft.x, campos.ScreenTopLeft.y * -1f, 0f);
                    lines.line1 = new Vector3(campos.ScreenTopLeft.x, campos.ScreenTopLeft.y,0f);
                    line_clone.tag = "LineY";
                    break;
                case 4:
                    lines.line0 = new Vector3(0f, campos.ScreenTopLeft.y * -1f, 0f);
                    lines.line1 = new Vector3(0f, campos.ScreenTopLeft.y, 0f);
                    line_clone.tag = "LineY";
                    break;
                case 5:
                    lines.line0 = new Vector3(campos.ScreenTopLeft.x * -1f, campos.ScreenTopLeft.y * -1f, 0f);
                    lines.line1 = new Vector3(campos.ScreenTopLeft.x * -1f, campos.ScreenTopLeft.y, 0f);
                    line_clone.tag = "LineY";
                    break;

            }
        }
        for (int cnta = 0; cnta < Sprites.Length; cnta++)
        {
            for(int cntb = 0; cntb < maxmember; cntb++)
            {
                clone = (GameObject)Instantiate(addobject);
                clone.transform.parent = obj_parent.transform;
                scratchobject sc_obj = clone.GetComponent<scratchobject>();
                sc_obj.obj_id = cnta;
                sc_obj.obj_mem = cntb;
                if(maxmember == 4)
                {
                    switch (cntb)
                    {
                        case 0:
                            obj_x[cntb,cnta] = disp_x /-2f;
                            obj_y[cntb, cnta] = disp_y /2f;
                            break;
                        case 1:
                            obj_x[cntb, cnta] = disp_x / 2f;
                            obj_y[cntb, cnta] = disp_y / 2f;
                            break;
                        case 2:
                            obj_x[cntb, cnta] = disp_x / -2f;
                            obj_y[cntb, cnta] = disp_y / -2f;
                            break;
                        case 3:
                            obj_x[cntb, cnta] = disp_x / 2f;
                            obj_y[cntb, cnta] = disp_y / -2f;
                            break;
                    }
                }
                obj_z[cntb, cnta] = Sprite_Size[cnta];
                obj_r[cntb, cnta] = 0f;
                obj_d[cntb, cnta] = 0f;
                obj_disp[cntb, cnta] = -1;
                sc_obj.obj_sprite = Sprites[cnta];
            }
        }
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void hello(SocketIOEvent e)          //サーバに接続時の確認みたいなやつ 
    {
        JSONObject obj = e.data;
        string from = obj.GetField("from").str;
        id = obj.GetField("id").str;

        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("id", id);
        jsonObject.AddField("group", Group);
        socket.Emit("unity/hello", jsonObject);
    }
    public void socket_move(SocketIOEvent e)    //n歩進む
    {
        JSONObject obj = e.data;
        float socket_group = obj.GetField("Group").n;
        if(socket_group == (float)Group)
        {
            float socket_move = obj.GetField("Move").n;
            int socket_num = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            obj_x[socket_num, socket_obj] += socket_move * Mathf.Cos(obj_d[socket_num, socket_obj]*Mathf.Deg2Rad);
            obj_y[socket_num, socket_obj] += socket_move * Mathf.Sin(obj_d[socket_num, socket_obj] * Mathf.Deg2Rad);
            if(obj_disp[socket_num, socket_obj] == -1)
            {
                obj_disp[socket_num, socket_obj] = 1;
            }
            send_pos(socket_num, socket_obj, obj_x[socket_num, socket_obj], obj_y[socket_num, socket_obj]);
        }
    }
    public void socket_rotate(SocketIOEvent e)  //n度回す
    {
        JSONObject obj = e.data;
        float socket_group = obj.GetField("Group").n;
        if (socket_group == (float)Group)
        {
            float socket_rotate = obj.GetField("Rotate").n;
            int socket_num = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            obj_r[socket_num, socket_obj] += socket_rotate;
            obj_d[socket_num, socket_obj] += socket_rotate;
            if (obj_disp[socket_num, socket_obj] == -1)
            {
                obj_disp[socket_num, socket_obj] = 1;
            }
        }
    }
    public void socket_angle(SocketIOEvent e)   //角度をn度にする
    {
        JSONObject obj = e.data;
        float socket_group = obj.GetField("Group").n;
        if (socket_group == (float)Group)
        {
            float socket_angle = obj.GetField("Angle").n;
            int socket_num = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            obj_r[socket_num, socket_obj] = socket_angle;
            obj_d[socket_num, socket_obj] = socket_angle;
            if (obj_disp[socket_num, socket_obj] == -1)
            {
                obj_disp[socket_num, socket_obj] = 1;
            }
        }
    }
    public void socket_movex(SocketIOEvent e)   //x座標をnずつ動かす
    {
        JSONObject obj = e.data;
        float socket_group = obj.GetField("Group").n;
        if (socket_group == (float)Group)
        {
            float socket_movex = obj.GetField("Movex").n;
            int socket_num = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            obj_x[socket_num, socket_obj] += socket_movex;
            if (obj_disp[socket_num, socket_obj] == -1)
            {
                obj_disp[socket_num, socket_obj] = 1;
            }
            send_pos(socket_num, socket_obj, obj_x[socket_num, socket_obj], obj_y[socket_num, socket_obj]);
        }
    }
    public void socket_movey(SocketIOEvent e)   //y座標をnずつ動かす
    {
        JSONObject obj = e.data;
        float socket_group = obj.GetField("Group").n;
        if (socket_group == (float)Group)
        {
            float socket_movey = obj.GetField("Movey").n;
            int socket_num = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            obj_y[socket_num, socket_obj] += socket_movey;
            if (obj_disp[socket_num, socket_obj] == -1)
            {
                obj_disp[socket_num, socket_obj] = 1;
            }
            send_pos(socket_num, socket_obj, obj_x[socket_num, socket_obj], obj_y[socket_num, socket_obj]);
        }
    }
    public void socket_warp(SocketIOEvent e)    //画面の中央に移動する
    {
        JSONObject obj = e.data;
        float socket_group = obj.GetField("Group").n;
        if (socket_group == (float)Group)
        {
            int socket_num = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            switch (socket_num)
            {
                case 0:
                    obj_x[socket_num, socket_obj] = disp_x / -2f;
                    obj_y[socket_num, socket_obj] = disp_y / 2f;
                    break;
                case 1:
                    obj_x[socket_num, socket_obj] = disp_x / 2f;
                    obj_y[socket_num, socket_obj] = disp_y / 2f;
                    break;
                case 2:
                    obj_x[socket_num, socket_obj] = disp_x / -2f;
                    obj_y[socket_num, socket_obj] = disp_y / -2f;
                    break;
                case 3:
                    obj_x[socket_num, socket_obj] = disp_x / 2f;
                    obj_y[socket_num, socket_obj] = disp_y / -2f;
                    break;
            }
            if (obj_disp[socket_num, socket_obj] == -1)
            {
                obj_disp[socket_num, socket_obj] = 1;
            }
        }
    }
    public void socket_appear(SocketIOEvent e)  //表示する
    {
        JSONObject obj = e.data;
        float socket_group = obj.GetField("Group").n;
        if (socket_group == (float)Group)
        {
            int socket_num = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            obj_disp[socket_num, socket_obj] = 1;
        }
    }
    public void socket_hide(SocketIOEvent e)    //隠す
    {
        JSONObject obj = e.data;
        float socket_group = obj.GetField("Group").n;
        if (socket_group == (float)Group)
        {
            int socket_num = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            obj_disp[socket_num, socket_obj] = 0;
        }
    }
    public void socket_flame(SocketIOEvent e)
    {
        JSONObject obj = e.data;
        float socket_group = obj.GetField("Group").n;
        if (socket_group == (float)Group)
        {
            if (flame)
            {
                flame = false;
            }
            else
            {
                flame = true;
            }
        }
    }
    public void collision_on(int mem1, int obj1, int mem2, int obj2)
    {
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("id", id);
        jsonObject.AddField("group", Group);
        jsonObject.AddField("mem1", mem1);
        jsonObject.AddField("obj1", obj1);
        jsonObject.AddField("mem2", mem2);
        jsonObject.AddField("obj2", obj2);
        socket.Emit("unity/collision_on", jsonObject);
    }
    public void collision_off(int mem1, int obj1, int mem2, int obj2)
    {
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("id", id);
        jsonObject.AddField("group", Group);
        jsonObject.AddField("mem1", mem1);
        jsonObject.AddField("obj1", obj1);
        jsonObject.AddField("mem2", mem2);
        jsonObject.AddField("obj2", obj2);
        socket.Emit("unity/collision_off", jsonObject);
    }
    void send_pos(int mem, int obj, float objx, float objy)
    {
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("id", id);
        jsonObject.AddField("group", Group);
        jsonObject.AddField("no", mem);
        jsonObject.AddField("obj", obj);
        jsonObject.AddField("objx", objx);
        jsonObject.AddField("objy", objy);
        socket.Emit("unity/objupdate", jsonObject);
    }
}
