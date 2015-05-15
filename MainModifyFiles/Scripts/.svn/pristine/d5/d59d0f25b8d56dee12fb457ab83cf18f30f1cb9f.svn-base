using com.nucleus.h1.logic.core.modules.guild.dto;
using com.nucleus.h1.logic.services;
public class GuildModel
{
	private static readonly GuildModel instance = new GuildModel ();
	public static GuildModel Instance {
		get {
			return instance;
		}
	}

    private GuildModel()
	{
	}

    #region 帮会列表
    private int _pageSize;
    private int _pageCount;
    private int _pageIndex;

    private GuildListDto _guildListDto;

    public void GetAllGuild(int pageIndex, System.Action<GuildListDto> getGuildListCallBack)
    {
        ServiceRequestAction.requestServer(GuildService.getAllGuild(1,20),"getAllGuild",(e)=>
        {
            _guildListDto = e as GuildListDto;
            _pageCount = _guildListDto.pageCount;

            // 排序 等级、成员数、创建时间
            //_guildListDto.list.Sort(GuildCompare);

            if (getGuildListCallBack != null)
            {
                getGuildListCallBack(_guildListDto);
            }
        });
    }

    #endregion

    #region 我的帮会信息

    public event System.Action OnGuildInfoUpDate;

    private GuildDto _myGuildDto;
    public void UpDateMyGuildInfo(GuildDto dto)
    {
        _myGuildDto = dto;
        PlayerModel.Instance.UpDateGuild(dto);

        if (OnGuildInfoUpDate != null)
            OnGuildInfoUpDate();
    }

    #endregion
}