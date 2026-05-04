namespace Chess.Model
{
    public static class AppConstants
    {
        // Users & Stats
        public const int MAX_USERNAME_LENGTH = 20;
        public const int MIN_USERNAME_LENGTH = 5;
        public const int MIN_PASSWORD_LENGTH = 6;
        public const int MAX_PASSWORD_LENGTH = 64; // cryptography mustnt break in DB, never tested tho

        public const double DEFAULT_VOLUME = 0.8;

        public const int MAX_ELO = 3300;
        public const int DEFAULT_ELO = 500;

        public const int MY_USER_ID = 3;
        public const int PAPA_MOR_USER_ID = 4;

        public static string BASE_PATH = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Assets", "Music");

        // Email
        public const string APP_EMAIL = "uribitmap2010@gmail.com";
        public const string APP_KEY = "upbz xvsd rike fspc";

        // Domain
        public const int BOARD_SIZE = 8; // 8*8

        public static string STOCKFISH_PATH_TO_EXE = Path.Combine(AppDomain.CurrentDomain.BaseDirectory, "Engines" , "stockfish-windows-x86-64-avx2.exe");
    }
}
