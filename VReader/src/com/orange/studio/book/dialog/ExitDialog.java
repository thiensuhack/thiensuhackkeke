package com.orange.studio.book.dialog;

import android.content.Context;

import com.orange.studio.book.R;
import com.zuzu.dialogs.BaseDialog;

public class ExitDialog extends BaseDialog {

	public ExitDialog(Context context){
		super(context, context.getString(R.string.app_name),
				TYPE_2_BUTTON, R.layout.dialog_exit_confirm_layout);
		setNegativeButtonTitle(R.string.dialog_exit_app_btn_Cancel);
		setPositiveButtonTitle(R.string.dialog_exit_app_btn_Ok);
		setDefaultButtonTitle(R.string.dialog_exit_app_btn_Ok);
		setOnDialogListener(new OnDialogListener() {
			@Override
			public void onPositiveButtonClicked() {
				dismiss();
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
