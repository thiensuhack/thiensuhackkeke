package com.orange.studio.book.model;

import java.util.List;
import java.util.concurrent.locks.Lock;
import java.util.concurrent.locks.ReentrantLock;

import android.os.Bundle;

import com.orange.studio.book.OrangeApplicationContext;
import com.orange.studio.book.config.OrangeConfig.Cache;
import com.orange.studio.book.listener.BookIF;
import com.orange.studio.book.object.BookDTO;
import com.orange.studio.book.util.OrangeUtils;
import com.zuzu.db.store.SimpleStoreIF;

public class BookModel implements BookIF{
	private static BookIF _instance;
	private static final Lock createLock = new ReentrantLock();
	private static final int STORE_EXPIRE = 1*60; //3 minutes
	private static final int STORE_EXPIRE_FIVE = 5*60; //3 minutes
	private static final int STORE_EXPIRE_A_DAY = 24*60*60; //3 minutes
		
	public BookModel() {
	}

	public static BookIF getInstance() {
		if (_instance == null) {
			createLock.lock();
			if (_instance == null) {
				_instance = new BookModel();
			}
			createLock.unlock();
		}
		return _instance;
	}
	private SimpleStoreIF getStoreAdapter() {
		return OrangeUtils.getStoreAdapter(Cache.LIST_BOOK_CACHE_KEY,
				OrangeApplicationContext.getContext(), Cache.LIST_BOOK_CACHE_NUMBER);
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
	public List<BookDTO> getListBook(String url, Bundle params) {
		return null;
	}

	@Override
	public BookDTO getBookDetail(String url, String id) {
		return null;
	}

}
