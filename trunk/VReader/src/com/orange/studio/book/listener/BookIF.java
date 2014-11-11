package com.orange.studio.book.listener;

import java.util.List;

import android.os.Bundle;

import com.orange.studio.book.object.BookDTO;

public interface BookIF {
	public List<BookDTO> getListBook(String url,Bundle params);
	public BookDTO getBookDetail(String url,String id);
}
