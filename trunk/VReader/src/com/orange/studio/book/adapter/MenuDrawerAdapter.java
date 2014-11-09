package com.orange.studio.book.adapter;

import java.util.ArrayList;
import java.util.List;

import android.app.Activity;
import android.content.Context;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.ImageView;
import android.widget.TextView;

import com.orange.studio.book.R;
import com.orange.studio.book.object.MenuItemDTO;

public class MenuDrawerAdapter extends OrangeBaseAdapter {

	private class CustomViewHolder {
		public TextView menuDrawerName;
		public TextView menuDrawerTotal;
		public ImageView menuDrawerIcon;
	}

	private Activity mActivity;
	private List<MenuItemDTO> mListData;
	private LayoutInflater mInflater = null;

	public MenuDrawerAdapter(Activity _mActivity) {
		super();
		mActivity = _mActivity;
		mListData = new ArrayList<MenuItemDTO>();
		mInflater = (LayoutInflater) mActivity
				.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
	}

	public void updateDataList(List<MenuItemDTO> _mListData) {
		mListData.clear();
		mListData.addAll(_mListData);
		notifyDataSetChanged();
	}

	public void insertListData(List<MenuItemDTO> _mListData) {
		mListData.addAll(_mListData);
		notifyDataSetChanged();
	}

	@Override
	public int getCount() {
		return mListData.size();
	}

	@Override
	public MenuItemDTO getItem(int arg0) {
		if (mListData == null) {
			return null;
		}
		return mListData.get(arg0);
	}

	@Override
	public long getItemId(int arg0) {
		return arg0;
	}

	@Override
	public View getView(int position, View convertView, ViewGroup parent) {
		CustomViewHolder viewHolder;
		if (convertView == null) {
			convertView = mInflater.inflate(R.layout.layout_item_menu_drawer,
					parent, false);
			viewHolder = new CustomViewHolder();
			viewHolder.menuDrawerIcon = (ImageView) convertView
					.findViewById(R.id.menuDrawerIcon);
			viewHolder.menuDrawerName = (TextView) convertView
					.findViewById(R.id.menuDrawerName);
			viewHolder.menuDrawerTotal = (TextView) convertView
					.findViewById(R.id.menuDrawerTotal);
			convertView.setTag(viewHolder);
		} else {
			viewHolder = (CustomViewHolder) convertView.getTag();
		}
		MenuItemDTO mData = mListData.get(position);
		viewHolder.menuDrawerName.setText(mData.name);
		if (mData.total > 0) {
			viewHolder.menuDrawerTotal.setText(String.valueOf(mData.total));
		} else {
			viewHolder.menuDrawerTotal.setText("");
		}
		viewHolder.menuDrawerIcon.setBackgroundResource(mData.resId);
		return convertView;
	}

}
