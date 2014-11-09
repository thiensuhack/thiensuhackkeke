package com.orange.studio.book.fragment;

import android.graphics.Bitmap;
import android.os.Bundle;
import android.os.Handler;
import android.support.annotation.Nullable;
import android.support.v4.app.Fragment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;

import com.nostra13.universalimageloader.core.DisplayImageOptions;
import com.nostra13.universalimageloader.core.assist.ImageScaleType;
import com.nostra13.universalimageloader.core.display.FadeInBitmapDisplayer;
import com.orange.studio.book.R;
import com.todddavies.components.progressbar.ProgressWheel;

public class BaseFragment extends Fragment{
	protected View mView = null;
	protected DisplayImageOptions options;
	
	protected View mNotFoundView=null;
	protected View mLoadingView=null;
	protected ProgressWheel mProgress=null;

	
	@Override
	public View onCreateView(LayoutInflater inflater,
			@Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		return super.onCreateView(inflater, container, savedInstanceState);
	}
	protected void initNotFoundView(){
		if(mView!=null){
			//mNotFoundView=(RelativeLayout)mView.findViewById(R.id.notFoundContainer);	
		}		
	}
	protected void initLoadingView(){
		if(mView!=null){
			//mLoadingView=(RelativeLayout)mView.findViewById(R.id.loadingContainer);
			mProgress=(ProgressWheel)mLoadingView.findViewById(R.id.progressWheel);
		}
	}
	protected void switchView(boolean isShowNotFound, boolean isShowLoading){
		if(mNotFoundView!=null){
			mNotFoundView.setVisibility(isShowNotFound?View.VISIBLE:View.GONE);
		}
		if(mLoadingView!=null){
			mLoadingView.setVisibility(isShowLoading?View.VISIBLE:View.GONE);
			if(mProgress!=null){
				if(isShowLoading){
					mProgress.spin();
				}else{
					mProgress.stopSpinning();
				}
			}
		}
	}
	protected void createImageLoader() {
		options = new DisplayImageOptions.Builder()
				.showImageOnLoading(R.drawable.not_found_icon)
				.showImageForEmptyUri(R.drawable.not_found_icon)
				.showImageOnFail(R.drawable.not_found_icon)
				.resetViewBeforeLoading(true).cacheOnDisk(true)
				.imageScaleType(ImageScaleType.IN_SAMPLE_POWER_OF_2)
				.cacheInMemory(true).considerExifParams(true)
				.bitmapConfig(Bitmap.Config.RGB_565).considerExifParams(true)
				.displayer(new FadeInBitmapDisplayer(300)).handler(new Handler()).build();
	}

	protected void createImageLoader(int imageLoadingResId,int emptyImageResId,
			int failedLoadedImageResId) {
		options = new DisplayImageOptions.Builder()
				.showImageOnLoading(imageLoadingResId)
				.showImageForEmptyUri(emptyImageResId)
				.showImageOnFail(failedLoadedImageResId)
				.resetViewBeforeLoading(true).cacheOnDisk(true)
				.imageScaleType(ImageScaleType.IN_SAMPLE_POWER_OF_2)
				.cacheInMemory(true).considerExifParams(true)
				.bitmapConfig(Bitmap.Config.RGB_565).considerExifParams(true)
				.displayer(new FadeInBitmapDisplayer(300)).handler(new Handler()).build();
	}
}
