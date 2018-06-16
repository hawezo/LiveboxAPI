namespace Livebox.Core
{
    /// <summary>
    /// Represents a couple of username and password for authentication purposes
    /// </summary>
    public class Credentials
    {
        /// <summary>
        /// An username.
        /// </summary>
        public string Username { get; set; }

        /// <summary>
        /// A password.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Instanciate a new <see cref="Credentials"/> object.
        /// </summary>
        /// <param name="username">An username.</param>
        /// <param name="password">A password.</param>
        public Credentials(string username, string password)
        {
            this.Username = username;
            this.Password = password;
        }
    }

}
