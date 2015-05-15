using UnityEngine;
using System.Collections;
using com.nucleus.h1.logic.services;
using com.nucleus.h1.logic.core.modules.question.dto;
using com.nucleus.h1.logic.core.modules.question.data;
using System;

public class ProxyGuildModule
{
    private const string NAME_GuildInfoList = "Prefabs/Module/GuildModule/GuildsInfoWinUI";
	
	public static void OpenGuildInfoList()
	{
        GuildModel.Instance.GetAllGuild(1, (guildListDto) =>
        {
            GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME_GuildInfoList, UILayerType.DefaultModule, false);
            var controller = ui.GetMissingComponent<GuildsInfoWinUIController>();
            controller.InitView();
            controller.SetData(guildListDto);
        });

	}

    public static void CloseGuildInfoList()
	{
		UIModuleManager.Instance.CloseModule (NAME_GuildInfoList);
	}
}

