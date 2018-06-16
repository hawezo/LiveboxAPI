namespace Livebox
{
    public static class Settings
    {

        /// <summary>
        /// Generally 192.168.1.1
        /// </summary>
        public static string LiveboxUrl { get; set; } = "http://livebox.home/";

        /// <summary>
        /// Admin by default, can not be changed.
        /// </summary>
        public static string Username { get; set; } = "admin";

        /// <summary>
        /// By default, the first 8 characters from the authentication key.
        /// </summary>
        public static string Password { get; set; } = "admin";

        /// <summary>
        /// Context for this instance. See <see cref="API.Authenticate"/>.
        /// </summary>
        public static string Context { get; set; }

    }
}
