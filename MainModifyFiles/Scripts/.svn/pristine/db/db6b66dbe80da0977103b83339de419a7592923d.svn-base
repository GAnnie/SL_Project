using com.nucleus.h1.logic.core.modules.guild.dto;
public class GuildInfoCellController : MonoBehaviourBase, IViewController
{
    private GuildInfoCell _view;
    private System.Action<GuildInfoCellController> _callBackFun;
    
    private GuildBaseDto _dto;

    public void InitView()
    {
        _view = gameObject.GetMissingComponent<GuildInfoCell>();
        _view.Setup(this.transform);

        _view.SelectSprite.gameObject.SetActive(false);
        RegisterEvent();
    }

    public void RegisterEvent()
    {
        //EventDelegate.Set(_view.GuildInfoCellBtn.onClick,OnClick);
    }

    public void SetData(GuildBaseDto dto,System.Action<GuildInfoCellController> callBackFun)
    {
        _dto = dto;
        _view.IdLabel.text = dto.id.ToString();
        _view.NameLabel.text = dto.name;
        _view.LvLabel.text = dto.grade.ToString();
        _view.CountLabel.text = dto.memberCount.ToString();
        _view.BossLabel.text = dto.bossNickName;

        _callBackFun = callBackFun;
    }

    public GuildBaseDto GetData()
    {
        return _dto;
    }

    public void SetActive(bool b)
    {
        gameObject.SetActive(b);
    }

    void OnClick()
    {
        if (_callBackFun != null)
            _callBackFun(this);
    }

    public bool IsSelect
    {
        get
        {
            return _view.SelectSprite.gameObject.activeSelf;
        }
        set
        {
            if (_view.SelectSprite.gameObject.activeSelf != value)
            {
                _view.SelectSprite.gameObject.SetActive(value);
            }
        }
    }

    public void Dispose()
    {

    }
}