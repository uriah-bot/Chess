namespace Chess.Model
{
    public static class AppConstants
    {
        // Users & Stats
        public const int MAX_USERNAME_LENGTH = 20;
        public const int MIN_USERNAME_LENGTH = 5;
        public const int MIN_PASSWORD_LENGTH = 6;
        public const int MAX_PASSWORD_LENGTH = 64; // cryptography mustnt break in DB, never tested tho

        public const int MAX_ELO = 3300;
        public const int DEFAULT_ELO = 500;

        public const int MY_USER_ID = 3;
        public const int PAPA_MOR_USER_ID = 4;

        // Email
        public const string APP_EMAIL = "uribitmap2010@gmail.com";
        public const string APP_KEY = "upbz xvsd rike fspc";

        // Domain
        public const int BOARD_SIZE = 8; // 8*8
    }
}
