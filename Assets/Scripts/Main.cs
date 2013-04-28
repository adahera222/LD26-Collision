using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class Main : MonoBehaviour {
	private static Main instance;
	public static Main Instance
	{
		get
		{
			if(instance == null)
			{
				instance = FindObjectOfType(typeof(Main)) as Main;
			}
			return instance;
		}
	}
	
	public List<IncomingAtom> incoAtoms = new List<IncomingAtom>();
	
	public GameObject numTextPrefab;
	public GameObject outerPrefab;
	public GameObject centerPrefab;
	
	public TextMesh counter;
	public TextMesh hp;
	private NumberText endText; 
	
	public AudioClip[] collidSounds;
	public AudioClip loseLifeSound;
	
	private bool isGameEnd;
	
	private Job counterJob;
	// Use this for initialization
	void Start () {
		TouchKit.instance.debugDrawBoundaryFrames = true;
						
		Job.make(SpawnOuters());
	
		Init();
	}
	
	void Update()
	{
		if(isGameEnd)
		{
			if(Input.anyKey)
			{
				Init();
				Destroy(endText.gameObject);
				isGameEnd = false;
			}
		}
	}
	
	private void Init()
	{
		for(int index = incoAtoms.Count - 1; index >= 0; index--)
		{
			incoAtoms[index].SelfDetroy();
		}
		
		tappedObj = null;
		
		lastCollideTime = Time.timeSinceLevelLoad;
				
		counterJob = Job.make(Counter());
		
		GameObject centerAtom = Instantiate(centerPrefab) as GameObject;
		
	}
	
	private IEnumerator Counter()
	{
		while(true)
		{
			yield return new WaitForSeconds(0.02f);
			counter.text = Time.timeSinceLevelLoad.ToString();
		}
	}
	
	private IEnumerator SpawnOuters()
	{
		while(true)
		{
			yield return new WaitForSeconds(2);
			
			IncomingAtom atom = IncomingAtom.Create();
			incoAtoms.Add(atom);
		}
	}
	
	public void ChangeHP(int pHP)
	{
		hp.text = "HP " + pHP.ToString();
	}
	
	public void GameEnd()
	{
		counterJob.kill();
		
		endText = NumberText.Create("Your point is " + counter.text + "s.\n Press ANY key to restart." );
		endText.transform.position = new Vector3(endText.transform.position.x, endText.transform.position.y, -9);
		endText.FontSize = 700;
		endText.Color = Color.white;
		
		isGameEnd = true;
	}
	
	private GameObject tappedObj = null;
	public void TappedOuter(GameObject pObj)
	{
		if(tappedObj == null)
		{
			tappedObj = pObj;
			tappedObj.GetComponent<IncomingAtom>().StartSpark();
		}
		else 
		{
			if(pObj == tappedObj)
			{
				tappedObj.GetComponent<IncomingAtom>().StopSpark();
				tappedObj = null;
			}
			else
			{
				Vector3 force1 = (pObj.transform.position - tappedObj.transform.position).normalized * 10000;
				Vector3 force2 = (tappedObj.transform.position - pObj.transform.position).normalized * 10000;
				
				tappedObj.rigidbody.AddForce(force1);
				pObj.rigidbody.AddForce(force2);
				
				tappedObj.GetComponent<IncomingAtom>().StopSpark();
				tappedObj = null;
			}
		}
	}
	
	float lastCollideTime;
	public void PlayCollideSound()
	{
		if((Time.timeSinceLevelLoad - lastCollideTime) > 0.05f)
		{
			lastCollideTime = Time.timeSinceLevelLoad;
			So.Instance.playSound(collidSounds[Random.Range(0, 5)], AudioRolloffMode.Linear, 0.2f);
		}
	}
	
	public void PlayLoseLife()
	{
		So.Instance.playSound(loseLifeSound, AudioRolloffMode.Linear, 0.5f);
	}
}
