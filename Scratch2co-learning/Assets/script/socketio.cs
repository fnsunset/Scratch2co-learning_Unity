using UnityEngine;
using SocketIO;
using System.Collections;

public class socketio : MonoBehaviour {
    SocketIOComponent socket;
    string id = "";
    // Use this for initialization
    void Start () {
        GameObject go = GameObject.Find("SocketIO");
        socket = go.GetComponent<SocketIOComponent>();
        socket.On("server/hello", hello);
        Debug.Log("Send first message");
    }
	
	// Update is called once per frame
	void Update () {
	
	}
    public void hello(SocketIOEvent e) {
        JSONObject obj = e.data;
        string from = obj.GetField("from").str;
        id = obj.GetField("id").str;
        Debug.Log("response from: " + from + "your ID:"+id);

        //ちょっと変えるよここは最終的に
        JSONObject jsonObject = new JSONObject(JSONObject.Type.OBJECT);
        jsonObject.AddField("id", id);
        socket.Emit("scratch/hello", jsonObject);
    }
}
