package com.orange.studio.book.dialog;

import java.io.File;
import java.util.ArrayList;
import java.util.Collections;
import java.util.Comparator;
import java.util.List;

import android.app.AlertDialog;
import android.content.Context;
import android.content.DialogInterface;
import android.content.DialogInterface.OnClickListener;
import android.os.Environment;
import android.view.LayoutInflater;
import android.view.View;
import android.view.ViewGroup;
import android.widget.AdapterView;
import android.widget.AdapterView.OnItemClickListener;
import android.widget.BaseAdapter;
import android.widget.ListView;
import android.widget.TextView;

import com.orange.studio.book.R;

public class DialogChooseDirectory implements OnItemClickListener,
		OnClickListener {
	public interface Result {
		void onChooseDirectory(String dir);
	}
	List<File> m_entries = new ArrayList<File>();
	File m_currentDir;
	Context m_context;
	AlertDialog m_alertDialog;
	ListView m_list;
	Result m_result = null;
	DirAdapter adapter = null;

	class ItemViewHolder {
		public TextView fileName;
		public TextView fileSize;
	}

	private void listDirs() {
		m_entries.clear();

		// Get files
		File[] files = m_currentDir.listFiles();

		// Add the ".." entry
		if (m_currentDir.getParent() != null)
			m_entries.add(new File(".."));

		if (files != null) {
			for (File file : files) {
				// if ( !file.isDirectory() )
				// continue;

				m_entries.add(file);
			}
		}

		Collections.sort(m_entries, new Comparator<File>() {
			public int compare(File f1, File f2) {
				return f1.getName().toLowerCase()
						.compareTo(f2.getName().toLowerCase());
			}
		});

	}

	public DialogChooseDirectory(Context ctx, Result res, String startDir) {
		m_context = ctx;
		m_result = res;

		if (startDir != null)
			m_currentDir = new File(startDir);
		else
			m_currentDir = Environment.getExternalStorageDirectory();

		listDirs();
		adapter = new DirAdapter(m_context);
		adapter.updateData(m_entries);

		AlertDialog.Builder builder = new AlertDialog.Builder(ctx);
		builder.setTitle(R.string.app_name);
		builder.setAdapter(adapter, this);

		// builder.setPositiveButton("Ok", new DialogInterface.OnClickListener()
		// {
		// public void onClick(DialogInterface dialog, int id) {
		// if (m_result != null)
		// m_result.onChooseDirectory(m_currentDir.getAbsolutePath());
		// dialog.dismiss();
		// }
		// });

		builder.setNegativeButton("Cancel",
				new DialogInterface.OnClickListener() {
					public void onClick(DialogInterface dialog, int id) {
						dialog.cancel();
					}
				});

		m_alertDialog = builder.create();
		m_list = m_alertDialog.getListView();
		m_list.setOnItemClickListener(this);
		m_alertDialog.show();
	}

	@Override
	public void onItemClick(AdapterView<?> arg0, View list, int pos, long id) {
		if (pos < 0 || pos >= m_entries.size())
			return;

		if (m_entries.get(pos).getName().equals(".."))
			m_currentDir = m_currentDir.getParentFile();
		else
			m_currentDir = m_entries.get(pos);
		if (m_currentDir.isFile()) {
			m_result.onChooseDirectory(m_currentDir.getAbsolutePath());
			m_alertDialog.dismiss();
			return;
		}
		listDirs();
		adapter.updateData(m_entries);
	}

	public void onClick(DialogInterface dialog, int which) {
	}

	public class DirAdapter extends BaseAdapter {
		private List<File> mData = null;
		private Context mContext = null;
		private LayoutInflater inflater = null;

		public DirAdapter(Context _context) {
			mContext = _context;
			mData = new ArrayList<File>();
			inflater = (LayoutInflater) mContext
					.getSystemService(Context.LAYOUT_INFLATER_SERVICE);
		}

		public void updateData(List<File> _data) {
			if (mData == null) {
				mData = new ArrayList<File>();
			}
			mData.clear();
			mData.addAll(_data);
			notifyDataSetChanged();
		}

		// This function is called to show each view item
		@Override
		public View getView(int position, View convertView, ViewGroup parent) {
			ItemViewHolder viewHolder;
			if (convertView == null) {
				convertView = inflater.inflate(R.layout.layout_item_directory_row,
						parent, false);
				viewHolder = new ItemViewHolder();
				viewHolder.fileName = (TextView) convertView
						.findViewById(R.id.folder_to_choose);
				viewHolder.fileSize = (TextView) convertView
						.findViewById(R.id.fileSize);
				convertView.setTag(viewHolder);
			} else {
				viewHolder = (ItemViewHolder) convertView.getTag();
			}

			if (m_entries.get(position) == null) {
				viewHolder.fileName.setText("..");
				viewHolder.fileName.setCompoundDrawablesWithIntrinsicBounds(
						m_context.getResources().getDrawable(
								R.drawable.ic_parent_dir), null, null, null);
			} else {

				viewHolder.fileName.setText(m_entries.get(position).getName());
				if (m_entries.get(position).isFile()) {
					viewHolder.fileName
							.setCompoundDrawablesWithIntrinsicBounds(
									m_context.getResources().getDrawable(
											R.drawable.ic_launcher), null,
									null, null);
					String strFileSize = String.valueOf(m_entries.get(position)
							.length() / 1024);
					viewHolder.fileSize.setText(strFileSize + "Kb");
				} else {
					viewHolder.fileName
							.setCompoundDrawablesWithIntrinsicBounds(
									m_context.getResources().getDrawable(
											R.drawable.ic_file_dir), null,
									null, null);
					viewHolder.fileSize.setText("");
				}
			}

			return convertView;
		}

		@Override
		public int getCount() {
			if (mData == null) {
				return 0;
			}
			return mData.size();
		}

		@Override
		public Object getItem(int arg0) {
			return mData.get(arg0);
		}

		@Override
		public long getItemId(int arg0) {
			return 0;
		}
	}
}