namespace RouteJs
{
    using System;
    using System.Collections.Generic;

    public sealed class WhiteLists
    {
        /// <summary>
        /// Gets or sets whitelist of controllers whose routes are always rendered
        /// </summary>
        public Dictionary<string, Type> ControllerWhitelist { get; set; }

        /// <summary>
        /// Gets or sets blacklist of controllers whose routes are never rendered
        /// </summary>
        public Dictionary<string, Type> ControllerBlacklist { get; set; }

        /// <summary>
        /// Gets or sets whitelist of areas whose default routes are always rendered. Areas as whitelisted if at
        /// least one controller in the area is whitelisted.
        /// </summary>
        public HashSet<string> AreaWhitelist { get; set; }
    }
}