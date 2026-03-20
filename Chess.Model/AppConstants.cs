namespace Chess.Model
{
    public static class AppConstants
    {
        // Users & Stats
        public const int MAX_USERNAME_LENGTH = 20;
        public const int MIN_USERNAME_LENGTH = 5;
        public const int MAX_ELO = 3300;
        public const int DEFAULT_ELO = 500;
        public const UserRole DEFAULT_ROLE = UserRole.Guest;


        // Email
        public const string APP_RECEIVER_EMAIL = "uribitmap2010@gmail.com";

        // Domain
        public const int BOARD_SIZE = 8; // 8*8
    }
}
