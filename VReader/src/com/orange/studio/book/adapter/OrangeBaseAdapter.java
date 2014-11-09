package com.orange.studio.book.adapter;

import android.graphics.Bitmap;
import android.os.Handler;
import android.view.View;
import android.view.ViewGroup;
import android.widget.BaseAdapter;

import com.nostra13.universalimageloader.core.DisplayImageOptions;
import com.nostra13.universalimageloader.core.assist.ImageScaleType;
import com.nostra13.universalimageloader.core.display.FadeInBitmapDisplayer;
import com.orange.studio.book.R;

public class OrangeBaseAdapter extends BaseAdapter {

	protected DisplayImageOptions options;

	public OrangeBaseAdapter() {
	}

	@Override
	public int getCount() {
		return 0;
	}

	@Override
	public Object getItem(int arg0) {
		return null;
	}

	@Override
	public long getItemId(int arg0) {
		return 0;
	}

	@Override
	public View getView(int arg0, View arg1, ViewGroup arg2) {
		return null;
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
