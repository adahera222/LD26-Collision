using UnityEngine;
using System.Collections;

public class NumberText : MonoBehaviour {
	public string Text {
		get
		{
			return mesh.text;
		}
		set
		{
			mesh.text = value;
		}
	}
		
	private TextMesh mesh;
	
	public Bounds Size
	{
		get
		{
			return GetComponent<Renderer>().bounds;	
		}
	}
	
	public int FontSize
	{
		set
		{
			mesh.fontSize = value;
		}
	}
	
	public Color Color
	{
		set
		{
			renderer.material.color = value;
		}
	}
	
	// Use this for initialization
	void Awake () {
		mesh = GetComponent<TextMesh>();
		renderer.material.color = new Color(0, 0, 0, 1);
	}
	
	// Update is called once per frame
	void Update () {
	
	}
	
	public static NumberText Create(string pInitText)
	{
		GameObject obj = Instantiate(Main.Instance.numTextPrefab) as GameObject;
		NumberText numText = obj.AddComponent<NumberText>();
		numText.Text = pInitText;
		
		return numText;
	}
}
