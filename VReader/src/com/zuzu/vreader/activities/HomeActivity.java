package com.zuzu.vreader.activities;

import org.coolreader.CoolReader;
import org.coolreader.crengine.FileInfo;
import org.coolreader.crengine.Services;

import android.os.Bundle;

public class HomeActivity extends CoolReader {

	String path;

	@Override
	public void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		//FileDownloadTask.isDownloading = false;
		// this.getWindow().addFlags(WindowManager.LayoutParams.FLAG_KEEP_SCREEN_ON);
		Bundle bundle = new Bundle();
		bundle = this.getIntent().getExtras();
//		if (bundle != null) {
//			path = bundle.getString("file:///mnt/sdcard/anhchiyeudau.epub");
//		} else {
//			return;
//		}
		path = "/mnt/sdcard/Dung-bao-gio-di-an-mot-minh-Keith-Ferrazzi.-Tahl-Raz_.epub";
		FileInfo dir = Services.getScanner().findParent(new FileInfo(path),
				Services.getScanner().getRoot());
		FileInfo bookInfo = dir.findItemByPathName(path);
		// setBookOpen(bookInfo);
		// bookInfo.parent=b
		// this.setLastBook(path);
		this.loadDocument(bookInfo);
		// this.showReader();
	}

	@Override
	public void onBackPressed() {
		super.onBackPressed();
	}

	@Override
	protected void onStart() {
		super.onStart();
		try {
			//EasyTracker.getInstance(this).activityStart(this);
		} catch (Exception e) {
			return;
		}
	}

	@Override
	protected void onStop() {
		super.onStop();
		try {
			//EasyTracker.getInstance(this).activityStop(this);
		} catch (Exception e) {
			return;
		}
	}

}