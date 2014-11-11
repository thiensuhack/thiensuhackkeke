package com.orange.studio.book.listener;

import java.util.List;

import com.orange.studio.book.object.BookDTO;

public interface BookDetailIF {
	public List<BookDTO> getListBookDetail(String key,List<Integer> ids);
	public BookDTO getBookDetail(String url,String id);
}
