using UnityEngine;
using System.Collections;

public class RotateObject : MonoBehaviour {

	//宣告:
	//旋轉速度、X軸、Y軸、Z軸，是一個帶有小數點的數值
	public float rotatespeed = 50.0f;
	public float X = 0f;
	public float Y = 0f;
	public float Z = 0f;

	//宣告三軸的旋轉量，是時間乘以旋轉速度
	//旋轉 : XRotate, YRotate, ZRotate
	void Update () {
	
		var XRotate = X * Time.deltaTime * rotatespeed;
		var YRotate = Y * Time.deltaTime * rotatespeed;
		var ZRotate = Z* Time.deltaTime * rotatespeed;

		transform.Rotate(XRotate, YRotate, ZRotate);
	}
}
