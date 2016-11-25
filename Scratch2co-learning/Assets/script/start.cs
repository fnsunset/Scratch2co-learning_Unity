using UnityEngine;
using System.Collections;
using UnityEngine.SceneManagement;

public class start : MonoBehaviour {
    public static string IP = "192.168.2.104";
    public static int GROUP = 0;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}
    public void gettext(string text)
    {
        IP =  text;
    }
    public void buttonA()
    {
        SocketIO.SocketIOComponent.url = getIP();
        GROUP = 1;
        SceneManager.LoadScene("main");
    }
    public void buttonB()
    {
        SocketIO.SocketIOComponent.url = getIP();
        GROUP = 2;
        SceneManager.LoadScene("main");
    }
    public void buttonC()
    {
        SocketIO.SocketIOComponent.url = getIP();
        GROUP = 3;
        SceneManager.LoadScene("main");
    }
    public void buttonD()
    {
        SocketIO.SocketIOComponent.url = getIP();
        GROUP = 4;
        SceneManager.LoadScene("main");
    }
    public static string getIP()
    {
        return ("ws://" + IP + ":8080/socket.io/?EIO=4&transport=websocket");
    }
    public static int getGROUP()
    {
        return (GROUP);
    }
}
