using UnityEngine;
using SocketIO;
using System;

public class socketio : MonoBehaviour {
    SocketIOComponent socket;
    string id = "";
    bool flame = true;
    public int maxmember = 4;
    public int Group;
    public int disp_x;
    public int disp_y;
    public float scsize_x;
    public float scsize_y;
    public GameObject camerapos;
    public GameObject addobject;
    public GameObject obj_parent;
    public Sprite[] Sprites;
    public float[] Sprite_Size;
    public float[,] obj_x = new float[10,50];
    public float[,] obj_y = new float[10,50];
    public float[,] obj_z = new float[10,50]; //size
    public float[,] obj_r = new float[10,50]; //round
    public float[,] obj_d = new float[10,50]; //drection
    public int[,] obj_disp = new int[10,50];
    GameObject clone;
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
        Debug.Log("Send first message");
        get_cam_pos campos = camerapos.GetComponent<get_cam_pos>();
        scsize_x = campos.ScreenBottomRight.x;
        scsize_y = campos.ScreenTopLeft.y;
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
    public void hello(SocketIOEvent e) {
        JSONObject obj = e.data;
        string from = obj.GetField("from").str;
        id = obj.GetField("id").str;
        Debug.Log("response from: " + from + "your ID:"+id);

        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("id", id);
        jsonObject.AddField("group", Group);
        socket.Emit("unity/hello", jsonObject);
    }
    public void socket_move(SocketIOEvent e)
    {
        Debug.Log("moveを受信しました ");
        JSONObject obj = e.data;
        float socket_group = obj.GetField("Group").n;
        if(socket_group == (float)Group)
        {
            float socket_move = obj.GetField("Move").n;
            int socket_num = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            obj_x[socket_num, socket_obj] += socket_move * Mathf.Cos(obj_r[socket_num, socket_obj]*Mathf.Deg2Rad);
            obj_y[socket_num, socket_obj] += socket_move * Mathf.Sin(obj_r[socket_num, socket_obj] * Mathf.Deg2Rad);
            if(obj_disp[socket_num, socket_obj] == -1)
            {
                obj_disp[socket_num, socket_obj] = 1;
            }
            Debug.Log("moveを受信した結果、"+socket_num+"の"+socket_obj+"を"+socket_move+"歩動かしました");
        }
    }
    public void socket_rotate(SocketIOEvent e)
    {
        Debug.Log("rotateを受信しました ");
        JSONObject obj = e.data;
        float socket_group = obj.GetField("Group").n;
        if (socket_group == (float)Group)
        {
            float socket_rotate = obj.GetField("Rotate").n;
            int socket_num = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            obj_r[socket_num, socket_obj] += socket_rotate;
            if (obj_disp[socket_num, socket_obj] == -1)
            {
                obj_disp[socket_num, socket_obj] = 1;
            }
        }
    }
    public void socket_angle(SocketIOEvent e)
    {
        Debug.Log("angleを受信しました ");
        JSONObject obj = e.data;
        float socket_group = obj.GetField("Group").n;
        if (socket_group == (float)Group)
        {
            float socket_angle = obj.GetField("Angle").n;
            int socket_num = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            obj_r[socket_num, socket_obj] = socket_angle;
            if (obj_disp[socket_num, socket_obj] == -1)
            {
                obj_disp[socket_num, socket_obj] = 1;
            }
        }
    }
    public void socket_movex(SocketIOEvent e)
    {
        Debug.Log("movexを受信しました ");
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
        }
    }
    public void socket_movey(SocketIOEvent e)
    {
        Debug.Log("moveyを受信しました ");
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
        }
    }
    public void socket_warp(SocketIOEvent e)
    {
        Debug.Log("warpを受信しました ");
        JSONObject obj = e.data;
        float socket_group = obj.GetField("Group").n;
        if (socket_group == (float)Group)
        {
            float socket_warpx = obj.GetField("Warpx").n;
            float socket_warpy = obj.GetField("Warpy").n;
            int socket_num = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            obj_x[socket_num, socket_obj] = socket_warpx;
            obj_y[socket_num, socket_obj] = socket_warpy;
            if (obj_disp[socket_num, socket_obj] == -1)
            {
                obj_disp[socket_num, socket_obj] = 1;
            }
        }
    }
}
