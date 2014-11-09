package com.orange.studio.book.dialog;

import com.orange.studio.book.R;
import com.orange.studio.book.activity.HomeActivity;
import com.zuzu.dialogs.BaseDialog;

public class ExitDialog extends BaseDialog {
	private HomeActivity mHomeActivity = null;

	public ExitDialog(HomeActivity _mHomeActivity) {
		super(_mHomeActivity, _mHomeActivity.getString(R.string.app_name), TYPE_2_BUTTON,
				R.layout.dialog_exit_confirm_layout);
		mHomeActivity=_mHomeActivity;
		setNegativeButtonTitle(R.string.dialog_exit_app_btn_Cancel);
		setPositiveButtonTitle(R.string.dialog_exit_app_btn_Ok);
		setDefaultButtonTitle(R.string.dialog_exit_app_btn_Ok);
		setOnDialogListener(new OnDialogListener() {
			@Override
			public void onPositiveButtonClicked() {
				dismiss();
				try {
					mHomeActivity.exitApplication();
				} catch (Exception e) {
				}
			}

			@Override
			public void onNegativeButtonClicked() {
				dismiss();
			}

			@Override
			public void onDefaultButtonClicked() {
				dismiss();
			}
		});
	}
}
