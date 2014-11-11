package com.orange.studio.book.model;

import java.lang.reflect.Type;
import java.util.ArrayList;
import java.util.Iterator;
import java.util.LinkedList;
import java.util.List;
import java.util.Map;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;

import org.json.JSONObject;

import com.google.gson.Gson;
import com.google.gson.reflect.TypeToken;
import com.orange.studio.book.OrangeApplicationContext;
import com.orange.studio.book.config.OrangeConfig.Cache;
import com.orange.studio.book.http.OrangeHttpRequest;
import com.orange.studio.book.listener.BookDetailIF;
import com.orange.studio.book.object.BookDTO;
import com.orange.studio.book.util.OrangeUtils;
import com.zuzu.db.store.SimpleStoreIF;

public class BookDetailModel implements BookDetailIF{
	private static BookDetailIF _instance;
	private static final Lock createLock = new ReentrantLock();
	private static final int STORE_EXPIRE = 5*60; //5 minutes
		
	public BookDetailModel() {
	}

	public static BookDetailIF getInstance() {
		if (_instance == null) {
			createLock.lock();
			if (_instance == null) {
				_instance = new BookDetailModel();
			}
			createLock.unlock();
		}
		return _instance;
	}
	private SimpleStoreIF getStoreAdapter() {
		return OrangeUtils.getStoreAdapter(Cache.LIST_BOOK_DETAIL_CACHE_KEY,
				OrangeApplicationContext.getContext(), Cache.LIST_BOOK_DETAIL_CACHE_NUMBER);
	}
	public void setStore(String key, String value) {
		try {
			this.getStoreAdapter().put(key, value, STORE_EXPIRE);
		} catch (Exception e) {
		}		
	}
	public void setStore(String key, String value,int expiredTime) {
		try {
			this.getStoreAdapter().put(key, value, expiredTime);
		} catch (Exception e) {
		}		
	}
	@Override
	public List<BookDTO> getListBookDetail(String url, List<Integer> ids) {
		if (ids == null || ids.isEmpty())
			return null;

		List<BookDTO> result = new ArrayList<BookDTO>();

		List<String> keys = new ArrayList<String>(ids.size());

		List<Integer> ids_miss = new LinkedList<Integer>();
		for (Integer myInt : ids) {
			keys.add(String.valueOf(myInt));
		}
		Map<String, String> result2 = this.getStoreAdapter().getMultiKeys(keys,
				null);

		if (result2 != null && result2.size() > 0) {
			Iterator<String> ii = keys.iterator();
			while (ii.hasNext()) {
				try {
					String key = ii.next();
					String val = result2.get(key);

					BookDTO comic = this.deserialize(val);

					if (comic != null)
						result.add(comic);
					else {
						try {
							ids_miss.add(Integer.valueOf(key));
						} catch (Exception ex) {
						}
					}
				} catch (Exception ex) {
				}
			}
		} else {
			ids_miss = ids;
		}
		if(ids_miss!=null && ids_miss.size()>0){
			
		}else{
			return result;
		}
		return null;
	}
	private List<BookDTO> getListBookDetailFromIds(String url, List<Integer> ids) {
		try {
			String data=OrangeHttpRequest.getInstance().getStringFromServer(url, null);
			
		} catch (Exception e) {
		}
		return null;
	}
 	private BookDTO deserialize(String json){
		BookDTO result = null;
		if (json == null || json.equals(""))
			return result;
		try {
			result = new BookDTO();
			Gson gson = new Gson();
			Type listType = new TypeToken<BookDTO>() {
			}.getType();
			result = (BookDTO) gson.fromJson(json, listType);
		} catch (Exception e) {
			return null;
		}
		return result;
	}
	@Override
	public BookDTO getBookDetail(String url, String id) {
		return null;
	}

}
