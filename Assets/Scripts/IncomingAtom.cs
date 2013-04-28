using UnityEngine;
using System.Collections;

public class IncomingAtom : MonoBehaviour {	
	private Job automoveJob = null;
	
	private TKTapRecognizer recognizer;
	
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
			if (value == 0)
			{
				//SelfDetroy();
				return;
			}
			else if(value > 0)
			{
				Sign = true;
			}
			else
			{
				Sign = false;
			}
			
			value = Mathf.Abs(value);
			
			if(value > 41)
			{
				value = 41;
			}
			else if (value < 1)
			{
				value = 1;
			}
			
			float absValue = Mathf.Abs(avalue);
			transform.localScale = new Vector3(absValue + 29, absValue + 29, absValue + 29);
		}
	}
	
	public bool sign;
	public bool Sign
	{
		get
		{
			return sign;
		}
		set
		{
			sign = value;
			if(sign)
			{
				numberText.Text = "+";
			}
			else
			{
				numberText.Text = "-";
			}
		}
	}
	
	public Rigidbody RigidBody
	{
		get
		{
			return GetComponent<Rigidbody>();
		}
	}
	
	private NumberText numberText;
	
	private void Start ()
	{
		float hue = Random.Range(0, 360f);
		renderer.material.color = new HSBColor(hue / 360f, 0.8f, 0.8f).ToColor();
		
		numberText = NumberText.Create("+");
		numberText.transform.parent = transform;
		numberText.transform.position = transform.position;
		
		automoveJob = Job.make(AutoMove());
		
		Value = Random.Range (1, 42) * (ExtRandom<int>.SplitChance() ? 1 : -1);
		
		recognizer = new TKTapRecognizer();
		TouchKit.addGestureRecognizer(recognizer);
		recognizer.gestureRecognizedEvent += (r) =>
		{
			D.log("Touched one outer", "");
			Main.Instance.TappedOuter(gameObject);
		};
	}
	
	private IEnumerator AutoMove()
	{
		while (true)
		{
			Vector3 positon = transform.position;
			if(Mathf.Abs(positon.x) > (Screen.width / 2 + 100) || Mathf.Abs(positon.y) > (Screen.height / 2 + 100))
			{
				D.log("Out of boundary, self destruction", "");
				SelfDetroy();
			}
			
			Bounds rendererBounds = GetComponent<Renderer>().bounds;
			
			recognizer.boundaryFrame = new TKRect(rendererBounds.min.x + Screen.width / 2, rendererBounds.min.y + Screen.height / 2, rendererBounds.size.x, rendererBounds.size.y);
			
			yield return new WaitForSeconds(0.016f);
		}
	}
	
	public static IncomingAtom Create()
	{
		GameObject obj = Instantiate(Main.Instance.outerPrefab) as GameObject;
		IncomingAtom outer = obj.GetComponent<IncomingAtom>();
		
		float posX, posY;
		if(ExtRandom<int>.SplitChance())
		{
			posX = Random.Range(-Screen.width / 2 - 90, Screen.width / 2 + 90);
			posY = (Random.Range(0, 90) + Screen.height / 2) * (ExtRandom<int>.SplitChance() ? 1 : -1);
		}
		else
		{
			posX = (Random.Range(0, 90) + Screen.width / 2) * (ExtRandom<int>.SplitChance() ? 1 : -1);
			posY = Random.Range(-Screen.height / 2 - 90, Screen.height / 2 + 90);
		}
		
		outer.transform.position = new Vector3(posX, posY);
		
		outer.RigidBody.AddForce(-outer.transform.position.normalized * 1000);
		
		return outer;
	}
	
	void OnCollisionEnter(Collision collision)
	{	
		IncomingAtom collidedAtom = collision.gameObject.GetComponent<IncomingAtom>();
		if(collidedAtom != null)
		{
			//D.log("===Before Collide {0} and {1}", Value * (Sign ? 1 : -1), collidedAtom.Value * (collidedAtom.Sign ? 1 : -1)); 
			//D.log("Collision happened {0}", collision.ToString());
			bool oldSign = Sign;
			
			if(Sign == collidedAtom.Sign)
			{
				Value = (Mathf.Abs(Value) + 1) * (Sign ? 1 : -1);
			}
			else
			{
				Value = (Mathf.Abs(Value) - 10) * (Sign ? 1 : -1);
			}
			//D.log("===After Collide {0} and {1}", Value * (Sign ? 1 : -1), collidedAtom.Value * (collidedAtom.Sign ? 1 : -1));
			if (Value == 0 || oldSign != Sign)
			{
				SelfDetroy();
			}
			else
			{
				Main.Instance.PlayCollideSound();
			}
		}
		else
		{
			SelfDetroy();
		}
		
	}
	
	public void SelfDetroy()
	{		
		automoveJob.kill();
		
		if(sparkJob != null)
		{
			sparkJob.kill();
		}
		
		if(recognizer != null)
		{
			TouchKit.removeGestureRecognizer(recognizer);
		}
		
		Main.Instance.incoAtoms.Remove(this);
		
		Destroy(gameObject);
	}
	
	#region Highlight Spark
	private Color oldColor;
	private Job sparkJob;
	
	public void StartSpark()
	{
		sparkJob = Job.make(SparkLoop());
	}
	
	private IEnumerator SparkLoop()
	{
		oldColor = renderer.material.color;
		while(true)
		{
			yield return new WaitForSeconds(0.3f);
			
			if(renderer.material.color == Color.white)
			{
				renderer.material.color = oldColor;
			}
			else
			{
				renderer.material.color = Color.white;
			}
			
		}
	}
	
	public void StopSpark()
	{
		renderer.material.color = oldColor;
		sparkJob.kill();
		sparkJob = null;
	}
	#endregion
}
