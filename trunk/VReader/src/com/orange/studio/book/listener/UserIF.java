package com.orange.studio.book.listener;

import java.util.List;

import android.os.Bundle;

import com.orange.studio.book.object.BookDTO;
import com.orange.studio.book.object.UserDTO;

public interface UserIF {
	public UserDTO getUserInfo(String url,String uId);
	public UserDTO loginUser(String url,Bundle params);
	public List<BookDTO> getUserBook(String url);
}
