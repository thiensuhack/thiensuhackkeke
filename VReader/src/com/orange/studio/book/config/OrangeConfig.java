package com.orange.studio.book.config;

public class OrangeConfig {
	
	public static int DBVERSION = 1;
	public static String LANGUAGE_DEFAULT = "1";
	public static String ITEMS_PAGE = "20";
	
	public static class Config {
		public static final boolean DEVELOPER_MODE = false;
	}
	public static class MENU_ID{
		public static final int HOME_FRAGMENT=1;
		public static final int DETAIL_FRAGMENT=2;
	}
	public static class BUNDLE_KEY{
		public static final String EBOOK_FILE="ebookfile";
	}
}
