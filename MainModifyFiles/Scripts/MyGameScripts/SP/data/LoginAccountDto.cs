using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class LoginAccountDto
{
	public int code;
	public string msg;
	public string token;
	public List<AccountPlayerDto> players;
}