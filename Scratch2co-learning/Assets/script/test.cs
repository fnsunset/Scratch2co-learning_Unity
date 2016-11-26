using UnityEngine;
using System.Collections.Generic;
using System.Collections;

public class test : MonoBehaviour {

	// Use this for initialization
    public string text = "";
    public class objecta
    {
        public int test1;
        public int test2;
        public objecta(int test1, int test2)
        {
            this.test1 = test1;
            this.test2 = test2;
        }
        public override bool Equals(object obj)
        {
            // Accountクラスに型変換
            objecta other = obj as objecta;

            if (other == null)
            {
                // objがAccountクラスでない場合、またはnullの場合は等しくないとする
                return false;
            }
            else
            {
                // objがAccountクラスの場合、かつフィールドidとnameの値が等しい場合は、
                // 2つのオブジェクトが等しいとする
                return this.test1 == other.test1 && this.test2 == other.test2;
            }
        }
    }
    void Start () {
        var strData = new List<objecta>();
        objecta testt;
        strData.Add(new objecta(1,2));
        strData.Add(new objecta(3, 4));
        strData.Add(new objecta(5, 6));
        for (int i = 0; i < strData.Count; i++)
        {
            testt = strData[i];
            text += testt.test1 + " " + testt.test2 + "\r\n";
        }
        Debug.Log(text);
        objecta testa = new objecta(1, 2);
        objecta testb;
        text = ""; for (int i = 0; i < strData.Count; i++)
        {
            testb = strData[i];
            if (testa.Equals(testb))
            {
                strData.RemoveAt(i);
            }
        }
        
        for (int i = 0; i < strData.Count; i++)
        {
            testt = strData[i];
            text += testt.test1 + " " + testt.test2 + "\r\n";
        }
        Debug.Log(text);
    }
	
	// Update is called once per frame
	void Update () {
	
	}
}
