using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class BattleCommand
{
	public enum Command
	{
		HIDE_ALL_PANEL,
		HIDE_BATTLE_PANEL,
		SHOW_POP_UP_UI,
		ESCAPE_SUCCESS,
		ESCAPE_FAILED,
		ON_CAPTURE_SUCCESS,
		ON_CAPTURE_FAILED,
		BATTLE_WIN,
		BATTLE_LOST,
		BATTLE_DRAW,
		BATTLE_ERROR
	}
	
	private List< Command > commandList = new List<Command>();

	public void AddCommand( Command command )
	{
		commandList.Add( command );
	}
		
	public void ExecuteCommands( BattleController bc )
	{
		foreach ( Command command in commandList )
		{
//			if ( command == Command.HIDE_ALL_PANEL )
//			{
//				GameObject bct = GameObject.Find("BattleController");
//				if(bct == null)
//					GameDebuger.Log("Can't Find BattleController");
//				BattleController battleContrller = bct.GetComponent<BattleController>();
//				Utility.HideAllPanel(battleContrller.buttonPanel , battleContrller.anchorRight , false , false,battleContrller.skillPanel);
//			}			
//			else if ( command == Command.HIDE_BATTLE_PANEL )
//			{
//				GameObject bct = GameObject.Find("BattleController");
//				if(bct == null)
//					GameDebuger.Log("Can't Find BattleController");
//				BattleController battleContrller = bct.GetComponent<BattleController>();
//                Utility.HideAllPanel(battleContrller.buttonPanel, battleContrller.anchorRight, true, false, battleContrller.skillPanel);
//			}
//			else if ( command == Command.SHOW_POP_UP_UI )
//			{
//				GameObject anchorRight = GameObject.Find("Anchor-Right");
//				if(anchorRight == null)
//					continue;
//		
//				PopupScript popup = anchorRight.GetComponent<PopupScript>();
//				popup.ShowUP();
//			}
//			else if ( command == Command.ESCAPE_SUCCESS )
//			{
//				if ( bc != null )
//				{
//					GameDebuger.Log("UIShowUp showUp = UIHelper.ShowUpUISprite( Prefabs/BattleResult/EscapeSuccess );");
//					bc.SetBattleStat( BattleController.BattleSceneStat.ON_PROGRESS );
//					UIShowUp showUp = UIHelper.ShowUpUISprite( "Prefabs/BattleResult/EscapeSuccess" );
//					if ( showUp != null )
//						showUp.Show( bc.ResetEscapeSuccessState );
//				}
//			}
//			else if ( command == Command.ESCAPE_FAILED )
//			{
//				GameDebuger.Log("UIShowUp showUp = UIHelper.ShowUpUISprite( Prefabs/BattleResult/EscapeFailed );");
//				bc.SetBattleStat( BattleController.BattleSceneStat.ON_PROGRESS );
//				UIShowUp showUp = UIHelper.ShowUpUISprite( "Prefabs/BattleResult/EscapeFailed" );
//				if ( showUp != null )
//					showUp.Show( bc.ResetEscapeFailedState );
//			}
//			else if ( command == Command.BATTLE_WIN )
//			{
//				UIShowUp showUp = UIHelper.ShowUpUISprite( "Prefabs/BattleResult/Sprite (Success)" );
//				if ( showUp != null )
//					showUp.Show( bc.ShowBattleReport);
//				
//				UIShowUp showUp2 = UIHelper.ShowUpUISprite( "Prefabs/BattleResult/Sprite (Success)2" );
//				if ( showUp2 != null )
//					showUp2.Show( null );
//			}
//			else if ( command == Command.BATTLE_LOST )
//			{
//				UIShowUp showUp = UIHelper.ShowUpUISprite( "Prefabs/BattleResult/Sprite (Fail)" );
//				if ( showUp != null )
//                    showUp.Show(bc.ShowBattleReport);
//				
//				UIShowUp showUp2 = UIHelper.ShowUpUISprite( "Prefabs/BattleResult/Sprite (Fail)2" );
//				if ( showUp2 != null )
//					showUp2.Show( null );
//			}
//			else if ( command == Command.BATTLE_DRAW )
//			{
//				UIShowUp showUp = UIHelper.ShowUpUISprite( "Prefabs/BattleResult/SpriteBattleDrawGame" );
//				if ( showUp != null )
//                    showUp.Show(bc.ShowBattleReport);
//			}
//			else if ( command == Command.ON_CAPTURE_SUCCESS )
//			{
//				UIShowUp showUp = UIHelper.ShowUpUISprite( "Prefabs/BattleResult/CaptureSuccess" );
//				if ( showUp != null )
//					showUp.Show( bc.ResetControlStat );
//			}
//			else if ( command == Command.ON_CAPTURE_FAILED )
//			{
//				UIShowUp showUp = UIHelper.ShowUpUISprite( "Prefabs/BattleResult/CaptureFailed" );
//				if ( showUp != null )
//					showUp.Show( bc.ResetControlStat );
//			}

			if ( command == Command.BATTLE_WIN )
			{
				UIShowUp showUp = UIHelper.ShowUpUISprite( "Prefabs/Module/Battle/BattleResult/Sprite (Success)" );
				if ( showUp != null )
					showUp.Show( bc.ShowBattleReport);
				
				UIShowUp showUp2 = UIHelper.ShowUpUISprite( "Prefabs/Module/Battle/BattleResult/Sprite (Success)2" );
				if ( showUp2 != null )
					showUp2.Show( null );
			}
			else if ( command == Command.BATTLE_LOST )
			{
				UIShowUp showUp = UIHelper.ShowUpUISprite( "Prefabs/Module/Battle/BattleResult/Sprite (Fail)" );
				if ( showUp != null )
                    showUp.Show(bc.ShowBattleReport);
				
				UIShowUp showUp2 = UIHelper.ShowUpUISprite( "Prefabs/Module/Battle/BattleResult/Sprite (Fail)2" );
				if ( showUp2 != null )
					showUp2.Show( null );
			}
			else if ( command == Command.BATTLE_DRAW )
			{
				UIShowUp showUp = UIHelper.ShowUpUISprite( "Prefabs/Module/Battle/BattleResult/SpriteBattleDrawGame" );
				if ( showUp != null )
                    showUp.Show(bc.ShowBattleReport);
			}
		}
		
		if ( commandList.Count > 0 )
			commandList.Clear();
	}
}
