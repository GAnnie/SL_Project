using com.nucleus.h1.logic.whole.modules.trade.data;
public class RankingTypeCellController : MonoBehaviourBase, IViewController
{
    private RankingTypeCell _view;

    public void InitView()
    {
        _view = gameObject.GetMissingComponent<RankingTypeCell>();
        _view.Setup(this.transform);
    }

    public void RegisterEvent()
    {

    }

    public void SetData(TradeMenu menu)
    {
        _view.NameLabel.text = menu.name;
    }

    public void Dispose()
    {

    }
}