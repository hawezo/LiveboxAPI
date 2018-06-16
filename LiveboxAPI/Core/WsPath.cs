using System.Collections.Generic;

namespace Livebox.Core
{

    /// <summary>
    /// An object representig a possible WS path, which can be a path and multiple commands, or a path alone.
    /// </summary>
    public class WsPath
    {
        /// <summary>
        /// Actual path of the WS API request.
        /// </summary>
        public string Path { get; set; }

        /// <summary>
        /// List of all possible commands.
        /// </summary>
        public List<string> Methods { get; set; }

        /// <summary>
        /// Displays the path and all of its methods.
        /// </summary>
        public override string ToString()
        {
            return $"{Path} + [ {string.Join(", ", this.Methods.ToArray())} ]";
        }
    }
}
