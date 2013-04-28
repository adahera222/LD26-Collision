using UnityEngine;
using System.Collections;

public class CenterAtom : MonoBehaviour {
	public int avalue = 21;
	public int Value
	{
		get
		{
			return avalue;
		}
		protected set
		{
			avalue = value;
			
			transform.localScale = new Vector3(avalue + 9, avalue + 9, avalue + 9);
			
			Main.Instance.ChangeHP(avalue);
		}
	}
	
	public TextMesh hpText;
	
	void Start()
	{
		Init();
	}
	
	public void Init()
	{
		float hue = Random.Range(0, 360f);
		renderer.material.color = new HSBColor(hue / 360f, 0.8f, 0.8f).ToColor();
				
		Value = 20;
	}
	
	public void ChangeColor(Color pNewColor)
	{
		renderer.material.color = pNewColor;
	}
	
	protected virtual void SelfDetroy()
	{
		Main.Instance.GameEnd();
		Destroy(gameObject);
	}
	
	void OnCollisionEnter(Collision collision)
	{
		Value = Value - 1;		
		D.log("Center Atom collided {0}", Value);
		if (Value == 0)
		{
			SelfDetroy();
		}
		
		Main.Instance.PlayLoseLife();
	}
}
