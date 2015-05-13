using UnityEngine;
public class ProxyRankingModule
{
    private const string NAME = "Prefabs/Module/RankingModule/RankingWinUI";

    public static void Open()
    {
        GameObject ui = UIModuleManager.Instance.OpenFunModule(NAME, UILayerType.DefaultModule, false);
        var controller = ui.GetMissingComponent<RankingWinUIController>();
        controller.InitView();
        controller.SetData();
    }

    public static void Close()
    {
        UIModuleManager.Instance.CloseModule(NAME);
    }
}