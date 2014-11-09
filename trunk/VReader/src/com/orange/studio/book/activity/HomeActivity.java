package com.orange.studio.book.activity;

import android.app.ProgressDialog;
import android.os.Bundle;
import android.support.v4.app.Fragment;
import android.support.v4.app.FragmentManager;
import android.support.v4.app.FragmentManager.OnBackStackChangedListener;
import android.support.v4.app.FragmentTransaction;
import android.support.v4.widget.DrawerLayout;
import android.support.v7.app.ActionBar;
import android.support.v7.app.ActionBarActivity;
import android.view.View;
import android.view.View.OnClickListener;

import com.orange.studio.book.R;
import com.orange.studio.book.config.OrangeConfig.MENU_ID;
import com.orange.studio.book.dialog.ExitDialog;
import com.orange.studio.book.fragment.HomeFragment;
import com.orange.studio.book.fragment.NavigationDrawerFragment;

public class HomeActivity extends ActionBarActivity implements
NavigationDrawerFragment.NavigationDrawerCallbacks, OnClickListener {
	private NavigationDrawerFragment mNavigationDrawerFragment;
	private ActionBar mActionbar;
	private CharSequence mTitle;
	private ProgressDialog mProgressDialog = null;
	private ExitDialog mExitDialog=null;
	@Override
	protected void onCreate(Bundle savedInstanceState) {
		super.onCreate(savedInstanceState);
		setContentView(R.layout.activity_home);

		mNavigationDrawerFragment = (NavigationDrawerFragment) getSupportFragmentManager()
				.findFragmentById(R.id.navigation_drawer);
		mTitle = getTitle();

		// Set up the drawer.
		mNavigationDrawerFragment.setUp(R.id.navigation_drawer,
				(DrawerLayout) findViewById(R.id.drawer_layout));
		mActionbar = getSupportActionBar();
		mActionbar.hide();
		getSupportFragmentManager().addOnBackStackChangedListener(
				new OnBackStackChangedListener() {

					@Override
					public void onBackStackChanged() {
						Fragment f = getSupportFragmentManager()
								.findFragmentById(R.id.container);
						if (f != null) {
							updateTitleAndDrawer(f);
						}

					}
				});
		initView();
		initListener();
		initProgress(null);
	}
	private void initView(){
		mExitDialog = new ExitDialog(this);
	}
	private void initListener(){
		
	}
	private void updateTitleAndDrawer(Fragment fragment) {
		
	}
	private void replaceFragment(Fragment fragment) {
		if (fragment == null) {
			return;
		}
		String backStateName = fragment.getClass().getName();
		String fragmentTag = backStateName;
		FragmentManager fragmentManager = getSupportFragmentManager();
		boolean fragmentPopped = fragmentManager.popBackStackImmediate(
				backStateName, 0);

		if (!fragmentPopped
				&& fragmentManager.findFragmentByTag(fragmentTag) == null) {
			FragmentTransaction ft = fragmentManager.beginTransaction();
			ft.replace(R.id.container, fragment, fragmentTag);
			//ft.setTransition(FragmentTransaction.TRANSIT_FRAGMENT_FADE);
			ft.setCustomAnimations(R.anim.fragment_fade_in,
					 R.anim.fragment_fade_out);
			ft.addToBackStack(backStateName);
			ft.commit();
		}
	}
	@Override
	public void onNavigationDrawerItemSelected(int position) {
		Fragment mFragment = null;
		switch (position) {
		case MENU_ID.HOME_FRAGMENT:
			mFragment = HomeFragment.instantiate(getApplicationContext(),
					HomeFragment.class.getName());
			break;

		default:
			mFragment = HomeFragment.instantiate(getApplicationContext(),
					HomeFragment.class.getName());
			break;
		}
		replaceFragment(mFragment);
	}
	protected void initProgress(String message) {
		mProgressDialog = new ProgressDialog(HomeActivity.this);
		if (message != null) {
			mProgressDialog.setMessage(message);
		} else {
			mProgressDialog.setMessage(getString(R.string.waitting_message));
		}
	}
	@Override
	public void onClick(View arg0) {
		
	}

	
	@Override
	public void onBackPressed() {
		if (getSupportFragmentManager().getBackStackEntryCount() == 1) {
			mExitDialog.show();
			return;
		} else {
			super.onBackPressed();
		}
	}
}
