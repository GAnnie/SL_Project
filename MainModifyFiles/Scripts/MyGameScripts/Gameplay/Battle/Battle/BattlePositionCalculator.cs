using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using com.nucleus.h1.logic.core.modules.battle.dto;

public class BattlePositionCalculator
{
	private static string[] a_positions = null;
	private static string[] b_positions = null;

	private static void Setup()
	{
		if (a_positions == null)
		{
			a_positions = new string[14];
			b_positions = new string[14];

//			a_positions[0] = "-4.22:-0.20";
//			a_positions[1] = "-4.16:1.30";
//			a_positions[2] = "-4.26:-1.65";
//			a_positions[3] = "-4.06:2.75";
//			a_positions[4] = "-4.31:-3.19";
//			a_positions[5] = "-2.52:-0.41";
//			a_positions[6] = "-2.44:1.05";
//			a_positions[7] = "-2.58:-1.86";
//			a_positions[8] = "-2.40:2.47";
//			a_positions[9] = "-2.61:-3.34";
//			a_positions[10] = "-5.81:-0.05";
//			a_positions[11] = "-5.68:1.52";
//			a_positions[12] = "-5.94:-1.51";
//			a_positions[13] = "-7.26:0.03";
//			
//			b_positions[0] = "3.93:-1.27";
//			b_positions[1] = "3.90:-2.78";
//			b_positions[2] = "3.93:0.20";
//			b_positions[3] = "3.70:-4.23";
//			b_positions[4] = "4.03:1.71";
//			b_positions[5] = "2.38:-1.12";
//			b_positions[6] = "2.38:-2.64";
//			b_positions[7] = "2.41:0.34";
//			b_positions[8] = "2.31:-4.16";
//			b_positions[9] = "2.53:1.90";
//			b_positions[10] = "5.39:-1.41";
//			b_positions[11] = "5.33:-2.91";
//			b_positions[12] = "5.32:0.07";
//			b_positions[13] = "6.75:-1.51";

			a_positions[0] = "-3.41:-2.43";
			a_positions[1] = "-3.21:-0.84";
			a_positions[2] = "-3.59:-4.04";
			a_positions[3] = "-3.13:0.96";
			a_positions[4] = "-3.83:-5.98";
			a_positions[5] = "-1.58:-3.21";
			a_positions[6] = "-1.32:-1.59";
			a_positions[7] = "-1.82:-4.60";
			a_positions[8] = "-1.14:-0.10";
			a_positions[9] = "-2.06:-6.28";
			a_positions[10] = "-4.71:-2.16";
			a_positions[11] = "-4.51:-0.60";
			a_positions[12] = "-4.89:-3.74";
			a_positions[13] = "-5.96:-1.88";
			
			b_positions[0] = "3.72:-1.95";
			b_positions[1] = "3.48:-3.53";
			b_positions[2] = "4.04:-0.32";
			b_positions[3] = "3.20:-5.09";
			b_positions[4] = "4.36:1.31";
			b_positions[5] = "2.03:-1.32";
			b_positions[6] = "1.83:-2.88";
			b_positions[7] = "2.39:0.37";
			b_positions[8] = "1.58:-4.43";
			b_positions[9] = "2.61:1.94";
			b_positions[10] = "5.17:-2.38";
			b_positions[11] = "4.97:-3.98";
			b_positions[12] = "5.55:-0.68";
			b_positions[13] = "6.53:-2.78";
		}
	}

	static public Vector3 GetMonsterPosition( VideoSoldier soldier, MonsterController.MonsterSide side, float distance = 0)
	{
		Setup();

		bool isPlayer = (side == MonsterController.MonsterSide.Player);

		int positionIndex = soldier.position - 1;

		string[] positions = isPlayer?a_positions:b_positions;

		string position = "0:0";

		if (positionIndex >= 0)
		{
			position = positions[positionIndex];
		}
		else
		{
			position = positions[0];
		}

		string[] vec = position.Split(':');
		float x = float.Parse(vec[0]);
		float z = float.Parse(vec[1]);
		
		return new Vector3(x, 0, z);
	}
	
//	static public Vector3 GetMonsterPosition(MonsterController.MonsterSide side, int index, int count, int shape, float distance )
//	{
//		Vector3 position = new Vector3(0,0.1f,0);
//		
////		if (count == 2 )
////		{
////			position.z = distance;
////		
////			if ( side == MonsterController.MonsterSide.Player )
////				position.z = -position.z;
////			
////			float verticalDistance = 35 * 0.07f;
////			
////			if ( shape == 1 )
////				verticalDistance = 18 * 0.07f;
////			
////			if (index == 0){
////				position.x = -verticalDistance;
////			}else if(index == 1){
////				position.x = verticalDistance;
////			}
////		}
////		else if(count == 3)
////		{
//			if (index == 1)
//				position.z = distance;
//			else
//				position.z = distance + 7 * 0.07f;
//			
//			if ( side == MonsterController.MonsterSide.Player )
//				position.z = -position.z;
//			
//			if (index == 0){
//				position.x = -2.0f;//-25f * 0.07f;
//			}else if (index == 1){
//				position.x = 0f;
//			}else{
//				position.x = 2.0f;//25f * 0.07f;
//			}
////		}
////		else
////		{
////			position.z = distance;
////			if ( side == MonsterController.MonsterSide.Player )
////				position.z = -position.z;
////		}
//		
//		return position;
//	}

    static public Vector3 GetZonePosition(MonsterController.MonsterSide side, BattleController bc)
    {
//        int playerMaxUnit = GetMaxStrikerMonsterShape(bc, MonsterController.MonsterSide.Player);
//        int enemyMaxUnit = GetMaxStrikerMonsterShape(bc, MonsterController.MonsterSide.Enemy);
//        float distance = CalculateMonsterDistance(playerMaxUnit, enemyMaxUnit);
//
//        return new Vector3(0, 0, side == MonsterController.MonsterSide.Player ? -distance : distance);

		return Vector3.zero;
    }
}
