using UnityEngine;
using SocketIO;
using System;
using System.Collections.Generic;

public class socketio : MonoBehaviour {
    SocketIOComponent socket;
    string id = "";
    public bool flame = true;
    public int maxmember = 4;
    public int Group = 0;
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
    private int count = 0;
    GameObject clone;
    GameObject line_clone;
    // Use this for initialization
    public class emit_dataset
    {
        public string emit;
        public int id;
        public int obj;
        public float num1;
        public float num2;
        public emit_dataset(string emit, int id, int obj, float num1, float num2)
        {
            this.emit = emit;
            this.id = id;
            this.obj = obj;
            this.num1 = num1;
            this.num2 = num2;
        }
        public override bool Equals(object obj)
        {
            // Accountクラスに型変換
            emit_dataset other = obj as emit_dataset;

            if (other == null)
            {
                // objがemit_datasetクラスでない場合、またはnullの場合は等しくないとする
                return false;
            }
            else
            {
                // objがAccountクラスの場合、かつ全ての値が等しいときTrueを返す
                bool ret = this.emit == other.emit && this.obj == other.obj && this.num1 == other.num1 && this.id == other.id;
                return ret;
            }
        }
    }
    public List<emit_dataset> emitdataset = new List<emit_dataset>();
    void Start () {
        Group = start.GROUP;
        Debug.Log("your Group is "+ Group);
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        socket.On("server/hello", hello);
        socket.On("unity/move", socket_move);
        socket.On("unity/rotate", socket_rotate);
        socket.On("unity/ang", socket_angle);
        socket.On("unity/movexy", socket_movexy);
        socket.On("unity/warp", socket_warp);
        socket.On("unity/center", socket_center);
        socket.On("unity/hide", socket_hide);
        socket.On("server/toggle_flame", socket_flame);
        socket.On("server/reset", socket_reset);
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
        emit_dataset dataclass;
        if(count++ > 2)
        {
            count = 0;
            for (int cnt = 0; cnt < emitdataset.Count; cnt++)
            {
                dataclass = emitdataset[cnt];
                switch (dataclass.emit)
                {
                    case "move":
                        unity_move(dataclass);
                        break;
                    case "rotate":
                        unity_rotate(dataclass);
                        break;
                    case "angle":
                        unity_angle(dataclass);
                        break;
                    case "movexy":
                        unity_movexy(dataclass);
                        break;
                    case "center":
                        unity_center(dataclass);
                        break;
                    case "warp":
                        unity_warp(dataclass);
                        break;
                    case "hide":
                        unity_hide(dataclass);
                        break;
                }
            }
        }
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
    public void unity_move(emit_dataset em)
    {
        obj_x[em.id, em.obj] += em.num1 * Mathf.Cos(obj_d[em.id, em.obj] * Mathf.Deg2Rad);
        obj_y[em.id, em.obj] += em.num1 * Mathf.Sin(obj_d[em.id, em.obj] * Mathf.Deg2Rad);
        if (obj_disp[em.id, em.obj] == -1)
        {
            obj_disp[em.id, em.obj] = 1;
        }
        //send_pos(socket_num, socket_obj, obj_x[socket_num, socket_obj], obj_y[socket_num, socket_obj]);
    }
    public void unity_rotate(emit_dataset em)  //n度回す
    {
        obj_r[em.id, em.obj] += em.num1;
        obj_d[em.id, em.obj] += em.num1;
        if (obj_disp[em.id, em.obj] == -1)
        {
            obj_disp[em.id, em.obj] = 1;
        }
    }
    public void unity_angle(emit_dataset em)   //角度をn度にする
    {
        obj_r[em.id, em.obj] = em.num1;
        obj_d[em.id, em.obj] = em.num1;
        if (obj_disp[em.id, em.obj] == -1)
        {
            obj_disp[em.id, em.obj] = 1;
        }
    }
    public void unity_movexy(emit_dataset em)   //x座標をnずつ動かす
    {
        obj_x[em.id, em.obj] += em.num1;
        obj_y[em.id, em.obj] += em.num2;
        if (obj_disp[em.id, em.obj] == -1)
        {
            obj_disp[em.id, em.obj] = 1;
        }
        //send_pos(socket_num, socket_obj, obj_x[socket_num, socket_obj], obj_y[socket_num, socket_obj]);
    }
    public void unity_center(emit_dataset em)  //表示する
    {
        if (flame)
        {
            switch (em.id)
            {
                case 0:
                    obj_x[em.id, em.obj] = disp_x / -2f;
                    obj_y[em.id, em.obj] = disp_y / 2f;
                    break;
                case 1:
                    obj_x[em.id, em.obj] = disp_x / 2f;
                    obj_y[em.id, em.obj] = disp_y / 2f;
                    break;
                case 2:
                    obj_x[em.id, em.obj] = disp_x / -2f;
                    obj_y[em.id, em.obj] = disp_y / -2f;
                    break;
                case 3:
                    obj_x[em.id, em.obj] = disp_x / 2f;
                    obj_y[em.id, em.obj] = disp_y / -2f;
                    break;
            }
        }
        else
        {
            obj_x[em.id, em.obj] = 0f;
            obj_y[em.id, em.obj] = 0f;
        }
    }
    public void unity_warp(emit_dataset em)    //画面の中央に移動する
    {
        obj_x[em.id, em.obj] = em.num1;
        obj_y[em.id, em.obj] = em.num2;
        if (obj_disp[em.id, em.obj] == -1)
        {
            obj_disp[em.id, em.obj] = 1;
        }
    }
    public void unity_hide(emit_dataset em)    //隠す
    {
        obj_disp[em.id, em.obj] = (int)em.num1;
    }
    public void socket_move(SocketIOEvent e)    //n歩進む
    {
        JSONObject obj = e.data;
        int socket_group = (int)obj.GetField("Group").n;
        if (socket_group == Group)
        {
            int socket_number = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            float socket_num1 = obj.GetField("Num1").n;
            float socket_num2 = obj.GetField("Num2").n;
            int socket_sw = (int)obj.GetField("Emitsw").n;
            emit_dataset nowdata = new emit_dataset("move",socket_number,socket_obj,socket_num1,socket_num2);
            if(socket_sw == 1)
            {
                emitdataset.Add(nowdata);
                Debug.Log(nowdata.emit + "を送信");
            }else
            {
                int remdata = checkArrayclass(nowdata, emitdataset);
                if (remdata >= 0)
                {
                    emitdataset.RemoveAt(remdata);
                    Debug.Log(nowdata.emit + "を削除");
                }
            }
        }
    }
    public void socket_rotate(SocketIOEvent e)  //n度回す
    {
        JSONObject obj = e.data;
        int socket_group = (int)obj.GetField("Group").n;
        if (socket_group == Group)
        {
            int socket_number = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            float socket_num1 = obj.GetField("Num1").n;
            float socket_num2 = obj.GetField("Num2").n;
            int socket_sw = (int)obj.GetField("Emitsw").n;
            emit_dataset nowdata = new emit_dataset("rotate", socket_number, socket_obj, socket_num1, socket_num2);
            if (socket_sw == 1)
            {
                emitdataset.Add(nowdata);
            }
            else
            {
                int remdata = checkArrayclass(nowdata, emitdataset);
                if (remdata >= 0)
                {
                    emitdataset.RemoveAt(remdata);
                }
            }
        }
    }
    public void socket_angle(SocketIOEvent e)   //角度をn度にする
    {
        JSONObject obj = e.data;
        int socket_group = (int)obj.GetField("Group").n;
        if (socket_group == Group)
        {
            int socket_number = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            float socket_num1 = obj.GetField("Num1").n;
            float socket_num2 = obj.GetField("Num2").n;
            int socket_sw = (int)obj.GetField("Emitsw").n;
            emit_dataset nowdata = new emit_dataset("angle", socket_number, socket_obj, socket_num1, socket_num2);
            if (socket_sw == 1)
            {
                emitdataset.Add(nowdata);
            }
            else
            {
                int remdata = checkArrayclass(nowdata, emitdataset);
                if (remdata >= 0)
                {
                    emitdataset.RemoveAt(remdata);
                }
            }
        }
    }
    public void socket_movexy(SocketIOEvent e)   //x座標をnずつ動かす
    {
        JSONObject obj = e.data;
        int socket_group = (int)obj.GetField("Group").n;
        if (socket_group == Group)
        {
            int socket_number = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            float socket_num1 = obj.GetField("Num1").n;
            float socket_num2 = obj.GetField("Num2").n;
            int socket_sw = (int)obj.GetField("Emitsw").n;
            emit_dataset nowdata = new emit_dataset("movexy", socket_number, socket_obj, socket_num1, socket_num2);
            if (socket_sw == 1)
            {
                emitdataset.Add(nowdata);
            }
            else
            {
                int remdata = checkArrayclass(nowdata, emitdataset);
                if (remdata >= 0)
                {
                    emitdataset.RemoveAt(remdata);
                }
            }
        }
    }
    public void socket_center(SocketIOEvent e)  //表示する
    {
        JSONObject obj = e.data;
        int socket_group = (int)obj.GetField("Group").n;
        if (socket_group == Group)
        {
            int socket_number = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            float socket_num1 = obj.GetField("Num1").n;
            float socket_num2 = obj.GetField("Num2").n;
            int socket_sw = (int)obj.GetField("Emitsw").n;
            emit_dataset nowdata = new emit_dataset("center", socket_number, socket_obj, socket_num1, socket_num2);
            if (socket_sw == 1)
            {
                emitdataset.Add(nowdata);
            }
            else
            {
                int remdata = checkArrayclass(nowdata, emitdataset);
                if (remdata >= 0)
                {
                    emitdataset.RemoveAt(remdata);
                }
            }
        }
    }
    public void socket_warp(SocketIOEvent e)    //画面の中央に移動する
    {
        JSONObject obj = e.data;
        int socket_group = (int)obj.GetField("Group").n;
        if (socket_group == Group)
        {
            int socket_number = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            float socket_num1 = obj.GetField("Num1").n;
            float socket_num2 = obj.GetField("Num2").n;
            int socket_sw = (int)obj.GetField("Emitsw").n;
            emit_dataset nowdata = new emit_dataset("warp", socket_number, socket_obj, socket_num1, socket_num2);
            if (socket_sw == 1)
            {
                emitdataset.Add(nowdata);
            }
            else
            {
                int remdata = checkArrayclass(nowdata, emitdataset);
                if (remdata >= 0)
                {
                    emitdataset.RemoveAt(remdata);
                }
            }
        }
    }
    public void socket_hide(SocketIOEvent e)    //隠す
    {
        JSONObject obj = e.data;
        int socket_group = (int)obj.GetField("Group").n;
        if (socket_group == Group)
        {
            int socket_number = (int)obj.GetField("Number").n;
            int socket_obj = (int)obj.GetField("Obj").n;
            float socket_num1 = obj.GetField("Num1").n;
            float socket_num2 = obj.GetField("Num2").n;
            int socket_sw = (int)obj.GetField("Emitsw").n;
            emit_dataset nowdata = new emit_dataset("hide", socket_number, socket_obj, socket_num1, socket_num2);
            if (socket_sw == 1)
            {
                emitdataset.Add(nowdata);
            }
            else
            {
                int remdata = checkArrayclass(nowdata, emitdataset);
                if (remdata >= 0)
                {
                    emitdataset.RemoveAt(remdata);
                }
            }
        }
    }
    public void socket_flame(SocketIOEvent e)
    {
        JSONObject obj = e.data;
        int socket_group = (int)obj.GetField("Group").n;
        if (socket_group == Group)
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
    public void socket_reset(SocketIOEvent e)
    {
        JSONObject obj = e.data;
        int socket_group = (int)obj.GetField("Group").n;
        if (socket_group == Group)
        {
            emitdataset = new List<emit_dataset>();
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
    int checkArrayclass(emit_dataset em ,List<emit_dataset> li)
    {
        emit_dataset _em;
        for (int i = 0; i < li.Count; i++)
        {
            _em = li[i];
            if (em.Equals(_em))
            {
                return i;
            }
        }
        return -1;
    }
}
