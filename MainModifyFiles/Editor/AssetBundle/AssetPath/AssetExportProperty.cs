using UnityEngine;
using System.Collections;

public class AssetExportProperty
{
    //是否进行数据关联
    public bool isPushDependencies = true;

    //是否进行合拼整包
    public bool isPushIntoOneAssetBundle = false;

    //是否删除动作模型
    public bool isDeleteAnimation = false;

    //是否进行本地压缩， 此选项会先对资源进行压缩
    //如果isNativeCompress 为true, isCompress为false， 则游戏里面可以实现同步加载操作， 同事可以减少资源的大小
    public bool isNativeCompress = false;

    //是否进行压缩( 默认进行压缩 )， 这个压缩和NativeCompress不同， 这选项是控制Unity打包成为
    //Assetbundle时，是否进行压缩
    public bool isCompress = true;

    public AssetExportProperty()
    { }
}
