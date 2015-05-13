using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using com.nucleus.h1.logic.core.modules.player.dto;
using com.nucleus.h1.logic.core.modules.scene.data;


public class RobotTool : MonoBehaviour {

	private float _scaleRate = 1f;

    private Rect windowRect = new Rect(10, 100, 300, 370);
	private int frames = 0;
	private float accum   = 0f;
	private float fps;
	private int RobotCount = 0;
	private LayerManager manager = null;
	private GameObject WorldActorsObj = null;
	private float timer = 0f;
	private float memoryTimer = 0f;
	List<string> totleCountList = new List<string>();
	List<string> robotCountList = new List<string>();
	List<GameObject> npcCountList = new List<GameObject>();
	List<string> teleportCountList = new List<string> ();
	List<string> otherPlayer = new List<string>();
	List<GameObject> robotObjList = new List<GameObject>();
	private bool toggle = false;
	private float robotMax = 0;
	private float robotWalkCoolDown = 2;
	private int showCount = 0;

	private float _buttonHeight = 30f;
	private float _buttonHeight2 = 35f;

    void Start () {

		//StartCoroutine( FPS() );

		windowRect = new Rect(windowRect.x, windowRect.y, windowRect.width*_scaleRate, windowRect.height*_scaleRate);
		_buttonHeight *= _scaleRate;
		_buttonHeight2 *= _scaleRate;
    }
    
    void OnGUI()    
    {
        
		if (RobotInfo.Instance.IsOPen ()) {
			windowRect = GUI.Window(10,windowRect,WindowDraw,"Tool");		
		}

    }

	void OnEnable(){
		StartCoroutine (FPS ());
	}

	void Update(){
		accum += Time.timeScale / Time.deltaTime;
		timer += Time.deltaTime;
		memoryTimer += Time.deltaTime;
		frames++;

		if (timer > 1f) {
			peopleCount();
			ShowCount();
			timer = 0;
		}

		if (memoryTimer > 2f) {
			GetFreeMemory();
			memoryTimer = 0;
		}
	}


	int index = 1;
	WorldView wv = null;
	navArea nav;
	void WindowDraw(int windowID){

		GUIStyle style = new GUIStyle();
		style.normal.textColor = Color.green;

		GUILayout.BeginHorizontal();       //--------------
		GUILayout.BeginVertical();

		GUILayout.Label (string.Format("Fps:{0:0.0}",fps) ,style,GUILayout.Height(_buttonHeight));

		GUILayout.Label (string.Format("总人数:{0}",totleCountList.Count - teleportCountList.Count - npcCountList.Count) ,style,GUILayout.Height(_buttonHeight));

		GUILayout.Label(string.Format("其他玩家:{0}",otherPlayer.Count) ,style,GUILayout.Height(_buttonHeight));

		GUILayout.Label (string.Format("机器人:{0}",robotCountList.Count) ,style,GUILayout.Height(_buttonHeight));

		GUILayout.Label (string.Format("Npc:{0}",npcCountList.Count) ,style,GUILayout.Height(_buttonHeight));

		GUILayout.Label (string.Format("传送点:{0}",teleportCountList.Count) ,style,GUILayout.Height(_buttonHeight));

		GUILayout.Label(string.Format("剩余内存:{0}",_freeMemory) ,style,GUILayout.Height(_buttonHeight));

		GUILayout.Label(string.Format("总共内存:{0}",_totalMemory) ,style,GUILayout.Height(_buttonHeight));

		GUILayout.Label (string.Format("要生成人数:{0:0}",robotMax) ,style,GUILayout.Height(_buttonHeight));

		GUILayout.Label (string.Format("移动CD:{0:0}",robotWalkCoolDown) ,style,GUILayout.Height(_buttonHeight));

		GUILayout.Label (string.Format("当前显示人数:{0:0}",showCount) ,style,GUILayout.Height(_buttonHeight));

		GUILayout.EndVertical();


		GUILayout.BeginVertical();
	
		//------------------------------

		robotMax=GUILayout.HorizontalSlider (robotMax, 0, 100,GUILayout.Height(_buttonHeight));
		toggle = GUILayout.Toggle (toggle,"是否当前位置",GUILayout.Height(_buttonHeight));
		//添加一个
		if (GUILayout.Button("AddOne",GUILayout.Height(_buttonHeight2))) {
				AddOne();
		}

		if(GUILayout.Button("AutoAdd",GUILayout.Height(_buttonHeight2))){
			if(toggle){
				for(int i = 0 ; i <robotMax; i++){
					AddOne();
				}
			}
			else
			for(int i = 0 ; i <robotMax; i++){
				AddOne();
			}
		}

		if (GUILayout.Button ("Per-Second",GUILayout.Height(_buttonHeight2))) {
			InvokeRepeating("Per_Second",1,1);
		}
		GUILayout.Space(20f);
		robotWalkCoolDown=GUILayout.HorizontalSlider (robotWalkCoolDown, 2, 100,GUILayout.Height(_buttonHeight));
		if(GUILayout.Button("change",GUILayout.Height(_buttonHeight2))){
			RobotInfo.Instance.SetCookDown((int)robotWalkCoolDown);
		}


		if (GUILayout.Button ("DeleteAll",GUILayout.Height(_buttonHeight2))) {
			wv.DelRobotPLayerList();
			index = 1;
			wv = null;
			nav = null;
			RobotInfo.Instance.DelShowObjList();
		}

		GUILayout.EndVertical();
		GUILayout.EndHorizontal();
		//---------------------------------------------
		GUI.DragWindow(new Rect(0, 0, Screen.width, Screen.height));
	}

	List<GameObject> objList = new List<GameObject>();
	public void ShowCount(){
		objList = RobotInfo.Instance.GetShowObjList ();
		int count = 0;
		for (int i = 0; i < objList.Count; i++) {
			RobotAutoWalk waw = objList[i].GetComponent<RobotAutoWalk>();
			if(waw.show){
				count++;
			}		
		}
		showCount = count;
	}

	void OnBecameVisible() {
		enabled = true;
	}
	
	
	
	int i = 0;
	public void Per_Second(){
		if (i >= robotMax) {
			CancelInvoke("Per_Second");
			i = 0;
		}
		else{
			++i;
			AddOne();
		}
	}

	private long _freeMemory = 0;
	private long _totalMemory = 0;

	private void GetFreeMemory()
	{
		if (SystemSetting.IsMobileRuntime)
		{
			_freeMemory = BaoyugameSdk.getFreeMemory() / 1024;
			_totalMemory = BaoyugameSdk.getTotalMemory() / 1024;
		}
	}




	public void AddOne(){
		if(wv == null){
			wv = WorldManager.Instance.GetView(); 
		}

		if(nav == null){
			nav = new navArea();
			nav.CreateMoveNavigation();
			RobotInfo.Instance.SetMapPointList(nav.mapPointList);
		}


		SimplePlayerDto dto = new SimplePlayerDto();
		dto.grade =index;
		dto.id = index;
		dto.charactorId = Random.Range (1, 6);
		dto.nickname = index.ToString();
		dto.sceneId = 1001;

		if (toggle) {
			dto.x = WorldManager.Instance.GetHeroView().cachedTransform.position.x;
			dto.z = WorldManager.Instance.GetHeroView().cachedTransform.position.z;		
		}
		else{
			int i = Random.Range(0,nav.mapPointList.Count);
			dto.x = nav.mapPointList[i].x;
			dto.z = nav.mapPointList[i].z;
		}
		wv.addRobotPlayer(dto);
		
		index++;
		
		if(manager == null){
			manager = GameObject.Find ("Gameplay").GetComponent<LayerManager>();
			WorldActorsObj = manager.WorldActors;
		}

		ClearList ();

		foreach (Transform child in WorldActorsObj.transform)
		{
			totleCountList.Add(child.name);
			
			if(child.name.StartsWith("Robot")){
				robotCountList.Add(child.name);
			}
			else if(child.name.StartsWith("npc")){
				teleportCountList.Add(child.name);
			}
			else if(child.name.StartsWith("player")){
				otherPlayer.Add(child.name);
			}
			else {
				if(!child.name.StartsWith("hero"))
					npcCountList.Add(child.gameObject);
			}
		}	
	}

	public void ClearList(){

		totleCountList.Clear();
		robotCountList.Clear();
		npcCountList.Clear ();
		otherPlayer.Clear ();
		teleportCountList.Clear ();
	}


	IEnumerator FPS()
	{
		while( true )
		{
			fps = accum/frames;
			accum = 0.0F;
			frames = 0;
			yield return new WaitForSeconds( 0.5f );
		}
	}
	public void  peopleCount(){

		if(manager == null){
			manager = GameObject.Find ("Gameplay").GetComponent<LayerManager>();
			WorldActorsObj = manager.WorldActors;
		}
		ClearList ();
		foreach (Transform child in WorldActorsObj.transform)
		{
			totleCountList.Add(child.name);
			
			if(child.name.StartsWith("Robot")){
				robotCountList.Add(child.name);
			}
			else if(child.name.StartsWith("npc")){
				teleportCountList.Add(child.name);
			}
			else if(child.name.StartsWith("player")){
				otherPlayer.Add(child.name);
			}
			else {
				if(!child.name.StartsWith("hero"))
					npcCountList.Add(child.gameObject);
			}
		}
	}


	public class navArea{
		NavMeshTriangulation n;

		public List<Vector3> mapPointList = new List<Vector3>();
		public navArea()
		{ 
			n = NavMesh.CalculateTriangulation();
		}
		
		public void CreateMoveNavigation()
		{
			mapPointList.AddRange(n.vertices);
		}
	}

	public class NavPoint{

		private Vector3 _position;
		public NavPoint(Vector3 position)
		{		
			_position = position;
		}


	}

}