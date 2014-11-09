package com.orange.studio.book.fragment;

import android.os.Bundle;
import android.support.annotation.Nullable;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.Button;

import com.orange.studio.book.R;

public class HomeFragment extends BaseFragment {
	
	private Button mBtn=null;
	@Override
	public View onCreateView(LayoutInflater inflater,
			@Nullable ViewGroup container, @Nullable Bundle savedInstanceState) {
		if (mView == null) {
			mView = inflater.inflate(R.layout.fragment_home, container, false);
			initView();
			initListener();
		} else {
			((ViewGroup) mView.getParent()).removeView(mView);
		}
		return mView;
	}
	private void initView(){
		mHomeActivity=getHomeActivity();
		mBtn=(Button)mView.findViewById(R.id.selectFileBtn);
	}
	private void initListener(){
		mBtn.setOnClickListener(this);
	}
	@Override
	public void onClick(View v) {
		switch (v.getId()) {
		case R.id.selectFileBtn:
			mHomeActivity.showSelectFileDialog();
			break;

		default:
			break;
		}
		super.onClick(v);		
	}
}
