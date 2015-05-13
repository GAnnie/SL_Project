using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class UIPageGroup : MonoBehaviour
{
    public UIAtlas atlas;

    UIGrid mGrid; //当前页面管理的Grid, 这个Grid只是用于’点‘图片排版的//
    Transform mCachedTransform;
    List<UISprite> mPages; //所有的点图片//

    int mCurrentPage; //维护一个当前页的变量//

    void Awake()
    {
		mGrid = gameObject.GetMissingComponent<UIGrid>();
		mGrid.cellWidth = 25;
		mGrid.pivot = UIWidget.Pivot.Center;
        mCachedTransform = this.transform;
        mPages = new List<UISprite>();
        mCurrentPage = -1;
    }

    void OnDestroy()
    {
        ClearPage();
    }

    public void AddPage()
    {
        UISprite sprite = NGUITools.AddWidget<UISprite>(mCachedTransform.gameObject);
        sprite.atlas = atlas;
		sprite.spriteName = "page_under";
        //sprite.alpha = 0.5f;
        sprite.MakePixelPerfect();

        sprite.hideFlags = HideFlags.DontSave;

		sprite.depth = 5;

        //add the sprite into list.
        mPages.Add(sprite);

        //rename
        int pageCount = mPages.Count;
        sprite.cachedTransform.name = pageCount.ToString().PadLeft(10, '0');
    }

    public void ClearPage()
    {
        for (int i = 0, imax = mPages.Count; i < imax; ++i)
        {
            UISprite s = mPages[i];
            GameObject.DestroyImmediate(s.gameObject);
        }
        mPages.Clear();

        this.mCurrentPage = -1;
    }


    public void RefreshLayout()
    {
		mGrid.sorting = UIGrid.Sorting.Alphabetic;
        mGrid.Reposition();
        //Bounds b = NGUIMath.CalculateRelativeWidgetBounds(mCachedTransform);
        //Vector3 pos = mCachedTransform.localPosition;
        //pos.x = -b.size.x / 2f;
        //mCachedTransform.localPosition = pos;
    }

    public void RefreshLayoutEx()
    {
        if (mPages != null && mPages.Count > 0)
        {
			mGrid.sorting = UIGrid.Sorting.Alphabetic;
            mGrid.Reposition();
            Vector3 pos = mCachedTransform.localPosition;
            int x = (mPages.Count - 1) * 25 / 2;
            pos.x = -x;
            mCachedTransform.localPosition = pos;
        }
    }

    public void SetCurrentPage(int page_)
    {
        if (mCurrentPage == page_) return;

        int pageCount = mPages.Count;

        if (page_ < 0 || page_ >= pageCount) return;

        UISprite newSelected = mPages[page_];

        if (mCurrentPage >= 0 && mCurrentPage < pageCount)
        {
            UISprite oldSelected = mPages[mCurrentPage];
            //oldSelected.color = Color.white;
            //oldSelected.alpha = 0.5f;
			oldSelected.spriteName = "page_under";
        }

        //newSelected.color = Color.white;
        //newSelected.alpha = 1f;
		newSelected.spriteName = "page_choice";

        mCurrentPage = page_;
    }

    public int GetPageCount()
    {
        return mPages.Count;
    }

	public int GetCurrentPage()
	{
		return mCurrentPage;
	}
}
